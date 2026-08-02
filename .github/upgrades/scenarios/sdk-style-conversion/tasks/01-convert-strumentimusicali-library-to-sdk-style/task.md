# 01-convert-strumentimusicali-library-to-sdk-style: Convert StrumentiMusicali.Library to SDK-style

**Objective:**
Convert `StrumentiMusicaliSql/StrumentiMusicali.Library.csproj` from legacy project format to modern SDK-style format.

**Scope:**
- Remove legacy property groups (Configuration/Platform conditionals)
- Migrate `packages.config` to inline `PackageReference` entries in the project file
- Remove explicit file includes (Compile, Content, None, EmbeddedResource)
- Preserve custom NuGet package imports (EntityFramework, Fody) as standard package targets
- Fold PropertyGroup entries into SDK defaults
- Update Project element to use `Sdk="Microsoft.NET.Sdk"`
- Preserve TargetFrameworkVersion at v4.8.1 (no TFM upgrade)

**Complications:**
- **packages.config migration:** Must extract all package references and convert to `PackageReference` items with correct versions
- **Fody weaving:**  PropertyChanged.Fody has build-time weaving requirements — conversion must preserve post-build targets or rely on Fody's standard targets
- **EF6 usage:** EntityFramework 6 is .NET Framework focused — conversion should not affect EF version (EF Core migration is separate scenario)
- **Explicit file includes:** ~100+ entries across Compile, Content, None, EmbeddedResource categories will be removed — relies on SDK globbing

**Expected Outcomes:**
- Project file size reduced significantly (~13 KB → ~2-3 KB typical)
- All PackageReferences preserved with same versions
- File structure unchanged (SDK-style globbing includes same files)
- Project loads and builds in Visual Studio 2026
- No breaking changes to dependent projects

**Build Validation:**
- ✅ Project builds to same output (bin/Debug, bin/Release)
- ✅ All NuGet packages resolved correctly
- ✅ No new compilation warnings introduced
- ✅ Fody post-build weaving runs as before

**Risk Level:** MEDIUM
- Primary risks: Fody tooling compatibility, missing file references if SDK globbing differs from explicit includes
- Mitigation: Use tool-based conversion (`convert_project_to_sdk_style`), test build immediately after, verify target assemblies produced
