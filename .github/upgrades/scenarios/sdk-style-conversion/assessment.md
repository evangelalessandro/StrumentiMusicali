# Assessment: SDK-style Conversion

## Projects in Solution

| Project | Path | Current Format | Status |
|---------|------|-----------------|--------|
| StrumentiMusicali.Library | StrumentiMusicaliSql/StrumentiMusicali.Library.csproj | Legacy | **TARGET** |
| StrumentiMusicali.Core | StrumentiMusicali.Core/StrumentiMusicali.Core.csproj | Legacy | Dependency |
| StrumentiMusicali.App | StrumentiMusicaliApp/StrumentiMusicali.App.csproj | Legacy | Dependency |
| StrumentiMusicali.ftpBackup | StrumentiMusicali.ftpBackup/StrumentiMusicali.ftpBackup.csproj | Legacy | Dependency |
| StrumentiMusicali.PrestaShopSyncro | StrumentiMusicali.PrestaShopSyncro/StrumentiMusicali.PrestaShopSyncro.csproj | Legacy | Dependency |
| StrumentiMusicali.Service | StrumentiMusicali.Service/StrumentiMusicali.Service.csproj | Legacy | Dependency |
| WooCommerce.NET | StrumentiMusicali.WooCommerce/WooCommerce.NET.csproj | SDK-style | Already modern |
| StrumentiMusicaliBackupFtp | StrumentiMusicaliBackupFtp/StrumentiMusicaliBackupFtp.csproj | SDK-style | Already modern |
| StrumentiMusicali.WOOCommerceSyncro | StrumentiMusicali.WOOCommerceSyncro/StrumentiMusicali.WOOCommerceSyncro.csproj | SDK-style | Already modern |
| PrestaSharp | PrestaSharp/PrestaSharp.csproj | SDK-style | Already modern (missing from disk) |

## Target Project Details

### StrumentiMusicali.Library (StrumentiMusicaliSql/StrumentiMusicali.Library.csproj)

**Current State:**
- Format: Legacy (.NET Framework 4.8.1)
- `TargetFrameworkVersion`: v4.8.1
- Package manager: `packages.config`
- Custom imports: 2 (EntityFramework.props, PropertyChanged.Fody.props)
- Explicit file includes: Extensive (~100+ Compile, Content, None items)
- Project type: Class Library

**Conversion Needs:**
- Remove `packages.config` and migrate to `PackageReference` in the project file
- Remove explicit file includes (rely on SDK-style globbing)
- Preserve custom import behavior or migrate to standard NuGet targets
- Flatten PropertyGroup entries and use SDK defaults where possible
- Migrate assembly attributes to SDK-style auto-generation

**Risk Level:** **MEDIUM**
- Entity Framework 6 usage requires careful migration (consider EF Core in future)
- Fody weaving requires special handling to ensure post-build targets remain functional
- Many explicit file references must be removed cleanly

---

## Dependency Analysis

**Scope Decision:** Focus on converting the target project (StrumentiMusicali.Library). Other legacy projects in the solution can be converted individually as separate future tasks. This is the only project explicitly requested for conversion.

---

## Baseline Build Status

**Before Conversion:**
- Solution: **Build FAILS** (missing PrestaSharp reference in solution, but PrestaSharp.csproj exists on disk as SDK-style)
- Compilation errors: Yes (missing project references)
- Solution file issue: PrestaSharp project reference points to non-existent path

**Notes:**
- The solution has structural issues that will need to be resolved before or after conversion
- These issues are NOT caused by non-SDK-style format, so they will persist through SDK-style conversion
- Recommend reviewing solution file (.sln) independently

---

## Conversion Approach

1. **Single Task Execution:** Convert StrumentiMusicali.Library to SDK-style format using the `convert_project_to_sdk_style` tool
2. **No Framework Change:** Target framework will remain `net481` (not changing to .NET Core/.NET modern)
3. **Post-Conversion Validation:** Verify the project structure and that it loads in Visual Studio
4. **Build Test:** Attempt a targeted build (may fail due to solution-level issues, which are pre-existing)

---

## Key Findings

- ✅ All 6 main projects in scope are legacy (non-SDK-style), except the SDK-style projects which are already modern
- ⚠️ The solution has pre-existing build issues (missing PrestaSharp reference in sln file)
- ⚠️ packages.config will be migrated to inline `PackageReference` entries
- ⚠️ Fody and EF6 targets require special attention during conversion
- ✅ C# Language Version (7.2) can remain as-is after conversion
- ✅ No ASP.NET Framework web projects in scope — only class libraries
