# Plan: SDK-style Conversion

**Scenario:** Convert StrumentiMusicali.Library to SDK-style project format

**Flow Mode:** Automatic

**Target Framework:** No change — will remain at .NET Framework 4.8.1

---

## Execution Strategy

The plan uses a single-task approach focused on converting the requested project (StrumentiMusicali.Library). SDK-style conversion is a structural change that does not require topological ordering of dependencies — each project can be converted independently.

---

## Task Breakdown

### Task 01: Convert StrumentiMusicali.Library to SDK-style

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

---

## Definition of Done

1. ✅ Project file converted to SDK-style format (Sdk attribute present, no legacy structure)
2. ✅ All NuGet packages from packages.config migrated to PackageReference
3. ✅ packages.config file removed
4. ✅ Explicit file includes removed (SDK globbing active)
5. ✅ Custom package imports (Fody, EF6) resolved via standard targets
6. ✅ TargetFrameworkVersion remains v4.8.1
7. ✅ Project loads in Visual Studio 2026
8. ✅ Test build completes (allows pre-existing solution-level errors)
9. ✅ All file changes committed to working branch

---

## Success Criteria

- **Structural:** Project file is valid XML, loads in Visual Studio, and appears in Solution Explorer without errors
- **Functional:** Builds successfully with same output; no new warnings or errors related to conversion
- **Dependencies:** No impact on projects that depend on StrumentiMusicali.Library
- **Source Control:** All changes committed with clear message

---

## Notes

- The solution currently has pre-existing build issues (missing PrestaSharp reference) that are NOT caused by non-SDK-style format and will persist through conversion. These are out of scope for this scenario.
- If the project fails to build post-conversion due to pre-existing issues, this is expected and does not indicate conversion failure.
- Fody and EF6 compatibility will be verified during the build validation phase.
