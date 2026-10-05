# Known Issues

Open issues that are known but not yet fixed. Each entry describes the symptom, the cause in the code, a workaround and possible fixes.

---

## AgShare auto-upload: Field Close waits, and a second Field Close closes the field

### Symptom

- AgShare **Auto-Upload** is on, and the AgShare server cannot be reached.
- The user closes the field. The field stays open.
- The user presses **Field Close** again. The field closes.

### Cause

The close path is `FileSaveEverythingBeforeClosingField()` in `GPS/Forms/Controls.Designer.cs`.

1. **The first close waits for the upload.** When auto-upload is on, the close starts `AgShareUploader.UploadAsync` and then waits with `await agShareUploadTask` (around line 789). `AgShareClient` has a 5 second HTTP timeout per request (`AgOpenGPS.Core/AgShare/AgShareClient.cs`, line 42). `UploadAsync` makes a download and then an upload request, so an unreachable server holds the close for about 10 seconds. No progress is shown on this path, so the field looks like it does not close.

2. **The upload task never reports failure.** `UploadAsync` catches every exception (`GPS/Classes/AgShare/AgShareUploader.cs`, line 136) and only writes a log line. The awaited task therefore always completes as "successful", and the failure is never shown to the user.

3. **A second close overwrites the running task.** `isAgShareUploadStarted` is set to `true` when the first close starts the upload, and it is only reset in `FieldMenuButtonEnableDisable` / `JobClose`. The second close sees the flag and skips the upload start, but before that it runs `agShareUploadTask = Task.CompletedTask;` (line 743). It then awaits that finished task, so it closes immediately.

4. **The first close finishes later.** When the first close's await returns, it runs the same closing steps again. The field closes twice, and the field save and export run twice, possibly at the same time.

### How to confirm in the log

`AgOpenGPS_Events_Log.txt` shows:

- `Error uploading field to AgShare: ...` or `Failed to check field visibility on AgShare, defaulting to private.`
- two `** Field closed **` lines for one close action.

### Workaround

Press **Field Close** a second time, or turn off AgShare Auto-Upload while the server is unreachable.

### Possible fixes

- **Do not overwrite a running task.** Only assign `agShareUploadTask` when a new upload is started. Ignore a second close while a close is already in progress (a "closing" flag).
- **Do not block the close on the upload.** The snapshot is already saved, so the upload can run in the background, and a failure can be reported with a message. Alternatively, keep the wait but show a progress step in `FormSaving`.
- **Make upload failures visible.** Have `UploadAsync` return or throw a result, so the close can show "Upload failed" instead of "Completed".
- **Reduce the wait.** The download is only used to read the field's `IsPublic` flag, so it can be skipped or run with a shorter timeout.
