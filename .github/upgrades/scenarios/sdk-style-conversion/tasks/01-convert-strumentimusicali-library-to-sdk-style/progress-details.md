# Progress: Convert StrumentiMusicali.Library to SDK-style

## Files Modified
- `StrumentiMusicaliSql/StrumentiMusicali.Library.csproj` — converted to SDK-style format
- `StrumentiMusicaliSql/packages.config` — deleted (migrated to PackageReference)

## Build Result
- Errors: 0
- Warnings: 4 (pre-existing CS code warnings in business source files — NOT conversion-related; see Issues)
- FodyPackageReference warning: **fixed** (was 1 new, now 0)
- Projects built: StrumentiMusicali.Library (net481) → `Bin\StrumentiMusicali.Library.dll`

## Test Result
- No test project discovered for StrumentiMusicali.Library. Class library, no unit tests present.

## Changes Summary
Converted `StrumentiMusicaliSql/StrumentiMusicali.Library.csproj` from legacy non-SDK format to SDK-style (`Microsoft.NET.Sdk`, `net481`):

1. **Project element** now uses `Sdk="Microsoft.NET.Sdk"`.
2. **TargetFramework** = `net481` (unchanged from v4.8.1 — no TFM upgrade, per scenario constraint).
3. **packages.config → PackageReference** migrated with identical versions:
   - EntityFramework 6.5.2
   - Fody 6.5.2 (added `PrivateAssets="all"` — was `developmentDependency` in packages.config)
   - PropertyChanged.Fody 3.4.0 (added `PrivateAssets="all"`)
4. **Output path preserved**: added `<AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>` so the library outputs to `..\Bin\` root (matching original legacy behavior and other solution projects) instead of the SDK default `Bin\net481\`.
5. **Explicit file includes** largely removed; retained only necessary metadata (`Compile Update`/`EmbeddedResource Update` with `DependentUpon`/`AutoGen`/`CopyToOutputDirectory` for EF migration designer files and resource designer files).
6. **Fody weaving** confirmed working after conversion (PropertyChanged.Fody weaver active via FodyWeavers.xml).
7. **`GenerateAssemblyInfo` set to false** (preserves existing AssemblyInfo.cs attributes).
8. **DDT.cs / DDTRiga.cs** (`Entity\Fatture\)`: not in original project's compile list, so converter excluded them via `<Compile Remove>`. **User confirmed to keep them excluded** to preserve original build behavior. The `Label="Compile items now included by globbing..."` ItemGroup is retained intentionally.

## Issues Encountered
- **FodyPackageReference warning** after conversion: resolved by adding `PrivateAssets="all"` to the `Fody` and `PropertyChanged.Fody` PackageReferences (matches their `developmentDependency` status in the original packages.config).
- **Output path regression** (`Bin\net481\` vs original `..\Bin\`): resolved with `AppendTargetFrameworkToOutputPath=false`.
- **4 pre-existing CS warnings** remain (CS0108, CS0067, CS0169) in `Entity\` and `Core\Item\` business source files. These are NOT caused by the format conversion and were present before it; fixing them is a functional refactor outside the scope of this format-only conversion. Left as-is.

## Verification Notes
- Pre-existing solution-wide build issues (missing PrestaSharp reference in the .sln) are unrelated to this conversion and persist independently.
- The workspace also contained unrelated, uncommitted changes (package version bumps and v4.8→v4.8.1 target changes in other projects). These belong to a separate effort and were NOT touched by this task.
