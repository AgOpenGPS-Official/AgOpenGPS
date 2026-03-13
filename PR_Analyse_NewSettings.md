# PR Analyse: NewSettings branch

**Branch:** `NewSettings`
**Geanalyseerd op:** 2026-03-13
**Omvang:** 96 bestanden gewijzigd | +9.654 / -5.794 regels
**Commits na laatste merge (e73943c):** 17 commits

---

## Overzicht

De PR voegt twee grote, onafhankelijke features toe:

1. **Settings Split** — De monolithische `Settings.cs` is opgesplitst in drie aparte profielen: Vehicle, Tool en Environment
2. **Auto-Updater** — Een volledig nieuw project voor automatisch updaten via GitHub of USB

---

## Feature 1: Settings Split (Vehicle / Tool / Environment)

### Nieuwe bestanden

| Bestand | Inhoud |
|---|---|
| `GPS/Properties/VehicleSettings.cs` | Voertuigafmetingen, autosteer, IMU, GPS-bron |
| `GPS/Properties/ToolSettings.cs` | Werktuigbreedte, secties, relais, guidance-algoritme |
| `GPS/Properties/Settings.cs` | Omgeving: display, vensterposities, simulatie-instellingen |
| `GPS/Properties/SettingsLegacy.cs` | Alle oude velden gecombineerd (uitsluitend voor migratie) |
| `GPS/Classes/CSettingsMigration.cs` | Migratie-logica van oud naar nieuw formaat (402 regels) |

### Nieuwe profielbeheer UI

| Form | Functie |
|---|---|
| `FormLoadVehicleTool` | Laadt vehicle én tool profiel in één scherm. Kleurcodering: oranje = huidig actief, groen = geselecteerd |
| `FormConvertProfiles` | Converteert oude profielbestanden naar het nieuwe drievoudige formaat. Met toggle-knoppen per type (Vehicle/Tool/Environment) |

Beide forms ondersteunen onscreen keyboard en touch-vriendelijke toggle-knoppen (ter vervanging van checkboxen).

### Registry-wijzigingen

Nieuw toegevoegd naast het bestaande `vehicleFileName`:

```
toolFileName
environmentFileName
toolsDirectory
environmentDirectory
```

### Laad-flow bij opstart (`RegistrySettings.Load()`)

```
1. Settings.Default.Load()           → Environment (Default.xml)
2. VehicleSettings.Default.Load()    → base\Vehicles\<vehicleFileName>.xml
3. ToolSettings.Default.Load()       → base\Tools\<toolFileName>.xml
   (fallback: vehicleFileName als toolFileName leeg is)
```

### Migratie-flow (automatisch bij ontbrekend nieuw bestand)

```
VehicleSettings.Load() → bestand niet gevonden → MigrateVehicle()
  └─ Laadt oud XML via SettingsLegacy
  └─ Kopieert velden via CopyVehicleSettings()
  └─ Slaat op als nieuw VehicleSettings XML
```

---

## Feature 2: Auto-Updater (nieuw project)

Volledig nieuw project: `SourceCode/Updater/AgOpenGPS.Updater.csproj`

### Diensten

| Klasse | Functie |
|---|---|
| `UpdateService.cs` (729 regels) | Check, download en pas update toe; sluit AgOpenGPS/AgIO af vóór installatie |
| `GithubReleaseService.cs` (347 regels) | GitHub API-integratie, release-info ophalen met optionele auth-token |
| `UsbUpdateService.cs` (244 regels) | Offline update via USB-schijf |
| `SemanticVersion.cs` (223 regels) | SemVer-vergelijking (major.minor.patch) |
| `ReleaseInfo.cs` | Model voor release-metadata (versie, assets, changelog) |

### Forms

| Form | Functie |
|---|---|
| `FormUpdate` | Hoofd-UI voor update-flow: check → download → installeer |
| `FormReleaseNotes` | Toont release notes van de nieuwe versie |
| `FormFirmwareUpdate` | UI voor firmware-update van hardware modules |
| `FormDialog` | Herbruikbaar bevestigingsdialoog |

---

## FormAllSettings — volledig herschreven

Nieuw: DataGridView met 3 kolommen per tab:

```
Setting | Profile Value | Default
```

Tabbladen: Vehicle / Tool / Environment / System
Gewijzigde waarden worden highlighted in lichtgeel.

---

## Wheelbase: volledige flow-analyse

### Conversie (oud → nieuw)

**`CSettingsMigration.cs:116`**
```csharp
dest.setVehicle_wheelbase = source.setVehicle_wheelbase;
```
✅ Correct — veld aanwezig in `SettingsLegacy`, correct gekopieerd naar `VehicleSettings`.

### Laden bij opstart

**`VehicleSettings.cs:71-78`** — automatische migratie als nieuw bestand ontbreekt:
```csharp
var result = XmlSettingsHandler.LoadXMLFile(path, this);
if (result == LoadResult.MissingFile)
    return CSettingsMigration.MigrateVehicle(vehicleFileName, this);
```

**`CVehicle.cs:57`** — runtime waarde wordt gevuld:
```csharp
VehicleConfig.Wheelbase = Properties.VehicleSettings.Default.setVehicle_wheelbase;
```
✅ Correct.

### Opslaan bij gebruikerswijziging

**`FormSteerWiz.cs:1174-1176`**
```csharp
Properties.VehicleSettings.Default.setVehicle_wheelbase = (double)nudWheelbase.Value * mf.inchOrCm2m;
mf.vehicle.VehicleConfig.Wheelbase = Properties.VehicleSettings.Default.setVehicle_wheelbase;
Properties.VehicleSettings.Default.Save(); // direct opgeslagen
```
✅ Correct — zowel runtime als XML worden bijgewerkt.

### Opslaan bij afsluiten

**`FormGPS.cs:698`**
```csharp
Properties.VehicleSettings.Default.Save();
```
✅ Correct — altijd opgeslagen in het vehicle-profiel XML.

---

## Aandachtspunten voor review

### 1. Path-inconsistentie in migratie (laag risico)

`VehicleSettings.Load()` zoekt in `vehiclesDirectory`, maar `MigrateVehicle()` zoekt het oude bestand in `baseDirectory + "Vehicles"` (`CSettingsMigration.cs:66`).
Als deze directories ooit van elkaar afwijken, kan de automatische migratie stilletjes falen met `LoadResult.MissingFile`.

**Aanbeveling:** Gebruik `RegistrySettings.vehiclesDirectory` ook in `MigrateVehicle()`.

### 2. Code-duplicatie: SemanticVersion

Identieke klasse bestaat op twee plekken:
- `SourceCode/GPS/Classes/SemanticVersion.cs`
- `SourceCode/Updater/Classes/SemanticVersion.cs`

**Aanbeveling:** Verplaats naar gedeeld project (`AgOpenGPS.Core`).

### 3. Nederlandse comments in code

In `VehicleSettings.cs` en `CSettingsMigration.cs` staan comments in het Nederlands (bijv. `// zonder snapDistance ... - die zitten nu in Environment`).
Voor een open-source project is Engels de standaard.

### 4. Twee grote features in één PR

De Settings Split en de Auto-Updater zijn volledig onafhankelijk. Voor reviewbaarheid zouden dit twee aparte PRs zijn geweest.

### 5. Verwijdering CRecordedPath.cs

`CRecordedPath.cs` (180 regels) is verwijderd zonder duidelijke vermelding in de commit messages. Controleer of dit bewust is.

### 6. Fallback toolFileName

```csharp
// RegistrySettings.cs
string toolFile = !string.IsNullOrEmpty(toolFileName) ? toolFileName : vehicleFileName;
```
Handig als tijdelijke maatregel, maar kan onverwacht gedrag geven wanneer vehicle- en tool-profiel bewust verschillend zijn. Geen commentaar in de code legt dit uit.

---

## Samenvatting oordeel

De PR is architectureel goed doordacht. De settings-split is een logische verbetering die multi-vehicle setups mogelijk maakt. De migratiestrategie via `CSettingsMigration` en `SettingsLegacy` is netjes. De auto-updater is volledig functioneel opgezet met GitHub API + USB fallback.

De wheelbase (en alle andere vehicle-instellingen) worden **correct** geconverteerd, opgeslagen en terug geladen.
