# SDK-style Conversion

## Preferences
- **Flow Mode**: Automatic

## Scenario-specific parameters
- **Project to convert (repo-relative)**: StrumentiMusicaliSql\StrumentiMusicali.Library.csproj

## Source Control
- **Source Branch**: NegozioLuca
- **Working Branch**: sdk-style-conversion-negozioluca
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Key Decisions Log
- **DDT.cs / DDTRiga.cs exclusion (2026-08-02):** User opted to KEEP `Entity\Fatture\DDT.cs` and `Entity\Fatture\DDTRiga.cs` excluded from compilation (preserve original build) rather than including them via SDK globbing. The `Label="Compile items now included by globbing..."` ItemGroup in StrumentiMusicali.Library.csproj is intentionally retained.

## Notes
- Pending changes detected at initialization were committed with message: "Save work before starting sdk-style-conversion".
- Conversion will NOT change TargetFramework values; only project file format will be updated to SDK-style.
- **Build tool decision:** StrumentiMusicali.Library uses `msbuild.exe` (net481 TFM + .resx files + Fody weaving). Cached per building-projects skill.
- **Preserve output path:** `<AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>` is required so StrumentiMusicali.Library outputs to `..\Bin\` root (matches original legacy behavior and other solution projects).
