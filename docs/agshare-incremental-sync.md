# AgShare: incremental sync and cloud field deletion

## Context

Every AgShare upload (when closing a field, when the app shuts down, and in the bulk uploader) used to
send the **complete** boundary plus **all** guidance lines to the server with one `PUT /api/fields/{id}`
call, whether or not anything had changed. There was no form of change detection (no hash, timestamp or
dirty flag) — see `GPS/Classes/AgShare/AgShareUploader.cs` and `AgOpenGPS.Core/AgShare/AgShareClient.cs`.
For large fields with many guidance lines this meant needless traffic and waiting time on every save,
even when the user had only added a single line.

Goal: send only the boundary and/or the guidance lines that actually changed, while keeping the existing
full upload as the fallback and first-time path, so that **older clients that only use the existing full
PUT keep working against the (unchanged) server**.

This change covers both the client (`AgOpenGPS`, this repo) and the server (`AgShare.Server`).

Also implemented:
- A newly added AB line or curve sends only that one line when the field is closed, not the whole set
  again.
- A track that was deleted locally (or a boundary that was removed completely) is also deleted in the
  cloud database when the field is closed.
- New action: deleting a field in AgOpenGPS also deletes it in the cloud database (if it was ever
  uploaded to AgShare).

Server analysis: `FieldEntity.Boundary` is a single `Polygon` column (no sub-row granularity) and
`FieldEntity.ABLinesGeoJson` is a single text blob holding a `FeatureCollection` (no table or ID of its
own per line — lines are already identified uniquely by `Name` elsewhere, in the John Deere sync). No
database migration is therefore needed: guidance lines are identified by their `Name` here as well (the
same convention as `ExternalLineIdsJson` in `JohnDeereExportService`), and the boundary is treated as one
whole that is replaced in its entirety as soon as it changes (independent of the guidance lines).

## Server (`AgShare.Server`) — new, additive endpoints next to the existing PUT

File: `AgShare.Server\Controllers\FieldsController.cs`. The existing `Upload(Guid id,
FieldUploadRequest dto)` (full PUT) stays **completely unchanged** — old clients keep working.

New actions, using the same ring/feature building logic as `Upload()` (extracted into reusable helpers):

- `PATCH api/fields/{id}/boundary` — body `PolygonDto`. Looks up the existing field (`404` if it does not
  exist yet — these actions cannot create a new field, that remains the exclusive path of `PUT`),
  replaces only `field.Boundary`, sets `field.UpdatedAt`, saves. An empty `PolygonDto` (boundary deleted
  locally) results in `field.Boundary = null`, just like in `Upload()`.
- `PUT api/fields/{id}/ablines` — body `AbLineUploadDto {Name, Type, Coords}`. Upsert by name: new or
  changed, in both cases only this one line is sent.
- `DELETE api/fields/{id}/ablines?name={name}` — removes the feature with that name from the blob (the
  name is a query parameter, not a route segment, to avoid URL-encoding problems with free text).
- `DELETE api/fields/{id}` — deletes the whole field, following the same pattern as the existing
  `WebIsoXmlFieldsController.DeleteField`, but under ApiKey auth for the desktop client.

All actions use the same auth pattern as the rest of `FieldsController` (ApiKey scheme + owner check)
and set `field.JdSyncPending = true` where applicable.

Known, accepted risk: as with the existing John Deere sync coupling, `Name` is not a guaranteed unique
key for guidance lines.

## Client — new HTTP methods

File: `AgOpenGPS.Core\AgShare\AgShareClient.cs`:

- `PatchBoundaryAsync(Guid fieldId, PolygonDto boundary)` → `PATCH /api/fields/{id}/boundary`
- `UpsertGuidanceLineAsync(Guid fieldId, GuidanceTrackDto line)` → `PUT /api/fields/{id}/ablines`
- `DeleteGuidanceLineAsync(Guid fieldId, string name)` → `DELETE /api/fields/{id}/ablines?name=...`
- `DeleteFieldAsync(Guid fieldId)` → `DELETE /api/fields/{id}`

A `404` on one of the incremental endpoints (an old server without these routes) is the signal to fall
back to the full `PUT`.

## Client — change detection and incremental upload

`GPS\Classes\AgShare\AgShareSyncStore.cs` reads/writes a plain-text sidecar file `agshare_sync.txt` in
the field folder:

```
$AgShareSync
<fieldId guid>
<boundaryHash>
<number of lines>
<name>\t<hash>
...
```

Hash = SHA-256 over a deterministic text representation of the **local (pre-conversion)**
easting/northing/heading values — that is, before the WGS84 conversion — rounded with the same
`FileIoUtils.FormatDouble(value, 3)` that is already used when writing `TrackLines.txt`/`Boundary.txt`.
Deliberately not a hash over the WGS84-converted coordinates: that conversion depends on the field
origin, which is itself written to and read from `Field.txt` as text. A difference of a few decimals in
the re-read origin (rounding loss when serializing) would otherwise shift all converted points and cause
a false "everything changed" detection in every session, even when the user did not change anything. By
hashing the local values, which are already rounded to 3 decimals as they go to disk, the hash is exactly
reproducible as long as the user has not changed the geometry — independent of origin or conversion
precision.

`GPS\Classes\AgShare\AgShareUploader.cs`:

- Without existing sync state (or with a changed `FieldId`) → full `UploadFullAsync` (existing behavior),
  then write the new sync state.
- With existing sync state → compare per section:
  - Boundary changed (including removed completely) → `PatchBoundaryAsync`.
  - Line new or changed → `UpsertGuidanceLineAsync` (only that one line).
  - Line deleted locally → `DeleteGuidanceLineAsync` (only that one line).
  - Nothing changed → no network call, result `Skipped`.
  - On a `404` from one of these calls → full fallback via `UploadFullAsync`.

Known, deliberate limitation: metadata-only changes (field name, `IsPublic`) made without a
boundary/track change are not detected by this mechanism.

## Deleting a field in AgOpenGPS also deletes it in the cloud

`GPS\Forms\Pickers\FormFilePicker.cs`, `btnDeleteField_Click`: before the local `Directory.Delete`, if
AgShare is enabled and the field has an `agshare.txt`, `AgShareClient.DeleteFieldAsync` is called. If this
fails (for example because the server is unreachable), the local deletion still goes ahead, with a
warning that the cloud side has to be cleaned up manually.
