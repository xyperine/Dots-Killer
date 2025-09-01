# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!-- ## [Unreleased] -->

## [8.0.0-pre.1] - 2025-01-07

### Added

- Added `BuildConstants.buildCounter` constant (by [@magicjar](https://github.com/magicjar))
- Added Dedicated Server support for Windows, Mac and Linux. (with contribution from [@firdiar-mita](https://github.com/firdiar-mita))
- Added support for building for ARM64 on Windows in Unity 2023.1+
- Added support for `BuildPlatform`s to draw additional option properties in `BuildPlatformListDrawer`

### Changed

- **Breaking change:** Solution for setting Android API levels that `UnityEditor.AndroidSdkVersions` does not include, which was preventing building Meta Quest Store-compatible apps in certain Unity versions.
- **Breaking change:** Raised minimum supported Unity version to Unity 2021.3 LTS.
- **Breaking change:** Refactored `BuildPlatform`s and renamed `BuildPC` to `BuildWindows` and `BuildOSX` to `BuildMac` to match Unity platform naming conventions.

### Fixed

- Fix `$DAYSSINCE` build version template token being mangled by `$DAY` token.
- Fix `$NOUN` and `$ADJECTIVE` build version template tokens not working since conversion to UPM package.
- Fix 'Error building Player: Incompatible color space with graphics API' when building for Linear color space on Android. (by [@MatthieuG9](https://github.com/MatthieuG9))
- Fix Android split APK setting usage in Unity 2023.1 (by [@Brian-Jiang](https://github.com/Brian-Jiang))
- Fix build scripting defines not being culture-agnostic (by [@hilminamli](https://github.com/hilminamli))

## [7.0.0] - 2023-10-03

### Added

- Double-click settings assets to assign them as the active settings in the SuperUnityBuild window. (by [@RobProductions](https://github.com/RobProductions))
- Customize app build name. (by [@RobProductions](https://github.com/RobProductions))
- Customize `BuildConstants` file path. (by [@RobProductions](https://github.com/RobProductions))

### Changed

- **Breaking change:** Namespaced generated `BuildConstants` class and enums, added `BuildConstants.scriptingBackend` constant.
- Remove redundant `dataDirNameFormat` variable from Build Platforms.
- Renamed 'Per-Platform' Build Action type to 'Per-Build' to reflect when they are run.
- Enhanced support for string token usage in Build Action configurations. Per-Build actions have gained support for `$BASEPATH` and `$BUILDPATH`, whilst Single Run actions can now use `$VERSION`, `$BUILD`, `$YEAR`, `$MONTH`, `$DAY` and `$TIME`, with the time-based tokens corresponding to the time at which the action was run.
- Scene list UI overhaul. (by [@RobProductions](https://github.com/RobProductions))
- Release list UI overhaul. (by [@RobProductions](https://github.com/RobProductions))
- Streamline UI colors. (by [@RobProductions](https://github.com/RobProductions))
- Replaced the 'Open SuperUnityBuild' button shown in the Inspector for settings assets with new 'Open in SuperUnityBuild' button to assign them as the active settings in the SuperUnityBuild window. (by [@RobProductions](https://github.com/RobProductions))

### Fixed

- Remove dependency on .NET Standard 2.1. (by [@RobProductions](https://github.com/RobProductions))
- Fix scripting backend not being restored after build. (by [@RobProductions](https://github.com/RobProductions))

## [6.0.1] - 2023-05-26

### Fixed

- Fix help button text wrapping in certain situations.
- Fix regression in 6.0.0 that broke macOS builds in Unity 2022.1+.

## [6.0.0] - 2023-05-16

### Added

- Support for configuring `symbols.zip` generation on Android platform.
- **Breaking change:** Added support for selecting scripting backend per platform. (by [@RobProductions](https://github.com/RobProductions))
- Added documentation, changelog and license URLs to package manifest.
- Support for selecting multiple device type values on Android platform. (by [@MatthieuG9](https://github.com/MatthieuG9))

### Changed

- Code refactoring for improved developer experience.
- **Breaking change:** Drop support for versions of Unity older than 2020.3 LTS.
- Only open build output folder if at least one build has succeeded.

### Fixed

- Correctly handle inconsistent behaviour of different Unity Editor versions when deleting elements in Inspector lists.
- Fixed macOS Xcode project output path.
- Fixed Build Platform names not being set before serialization.
- iOSBuildType obsolete in U2021.2+ (by [@nickfourtimes](https://github.com/nickfourtimes))
- Open build folder fails w/bad slashes (by [@nickfourtimes](https://github.com/nickfourtimes))

## [5.0.4] - 2022-02-03

### Fixed

- Properly check output folder is valid before trying to open it after a build.

## [5.0.3] - 2022-02-03

### Fixed

- Fixed distribution names not displaying in build configuration list.
- Check output folder is valid before trying to open it after a build.

## [5.0.2] - 2022-01-04

### Fixed

- Fixed incorrect label length calculation.
- Fixed default user Build Actions path for new installations (should be `Assets/SuperUnityBuild/Editor/BuildActions`).

## [5.0.1] - 2021-11-25

### Changed

- Improved appearance of long UI button labels.

## [5.0.0] - 2021-11-24

### Added

- **Breaking change:** Added support for building Gradle projects for Android.

## [4.0.1] - 2021-11-24

### Fixed

- Fixed Android target SDK version not being set.

## [4.0.0] - 2021-11-19

### Added

- Added support for Split App Binary (generates APK + OBB/AAB + asset packs when enabled) for Android.
- Added support for building Xcode projects for macOS.
- Added support for building Visual Studio Solutions for PC.
- Added new $VARIANTS build path token to give greater control over build path generation.
- Added support for setting Android target SDK version.

### Changed

- More sensible default build path.
- Show notes in Build Actions list view.
- Show selected architectures and varients in Build Platforms list view.
- **Breaking change:** Simplified $ARCHITECTURE build path token by removing variants list.
- Remove empty build tokens from build path, rather than substituting 'None'.
- Allow BuildConstants.cs to be moved from default location.

## [3.0.0] - 2021-10-19

### Added

- Added ability to choose between APK, split APK or Android App Bundle output for Android builds.

### Changed

- **Breaking change:** Removed standalone 'split APKs' variant for Android in favour of new build output type variant.
- **Breaking change:** Removed 'build system' variant for Android now that Gradle is the only supported build system.
- Set Android architecture binary name according to build output type variant being built.
- Stop using deprecated `BuildPipeline.BuildPlayer` overload.

## [2.2.0] - 2021-10-07

### Added

- Added UWP platform.

### Changed

- Don't create extra output directory for iOS builds.

## [2.1.0] - 2021-08-24

### Added

- Allow certain pre-build per-platform actions to optionally configure Editor.

### Changed

- Removed pre-Unity 2019.1 code.

## [2.0.0] - 2021-05-13

### Changed

- Increased minimum supported Unity version to 2019.1.

## [1.3.0] - 2021-05-11

### Fixed

- Fixed compilation errors on Unity 2018.1 - 2018.3. [Issue #74](https://github.com/superunitybuild/buildtool/issues/74)

## [1.2.0] - 2021-05-10

### Changed

- Revised Product Parameters UI to better denote how the various options for setting a build version work and interact with each other.
- Deprecated `BuildSettings.productParameters.lastGeneratedVersion` property. BuildActions that previously referenced this should now use `BuildSettings.productParameters.buildVersion` instead.
- Deprecated `BuildSettings.productParameters.version` property. This has been replaced by `BuildSettings.productParameters.versionTemplate`.

### Fixed

- Fixed correct build version string not being available for BuildActions to use when 'Sync Version with Player Settings' option was enabled. [Issue #73](https://github.com/superunitybuild/buildtool/issues/73)

## [1.1.0] - 2021-04-29

### Added

- Increment build numbers for supported platforms when generating version string. [Issue #71](https://github.com/superunitybuild/buildtool/issues/71)
- Allow BuildActions to draw custom serialized property fields

### Changed

- Use `EditorGUILayout.DropdownButton` for UI dropdown buttons

## [1.0.0] - 2021-04-15

This release includes all changes from 1.0.0 pre-releases ([1.0.0-pre.1](#100-pre1---2021-04-09), [1.0.0-pre.2](#100-pre2---2021-04-13)), plus:

### Changed

- No longer uneccessarily update Editor Build Settings scene list during build (behaviour introduced in 1.0.0-pre.2).

## [1.0.0-pre.2] - 2021-04-13

### Added

- Added 'macOS Architecture' variant on Unity 2020.2+ to support building for Apple Silicon. [Issue #70](https://github.com/superunitybuild/buildtool/issues/70)
- Added ability to set release type scene list from Build Settings. [Issue #27](https://github.com/superunitybuild/buildtool/issues/27)
- Set Build Settings scene list from release type when 'Refresh BuildConstants and Apply Defines' is clicked. [Issue #16](https://github.com/superunitybuild/buildtool/issues/16)

### Changed

- Made $ARCHITECTURE build path token more human-readable.
- 'Refresh BuildConstants...' button now configures Editor environment settings in an identical way to performing a build, so you can match your Editor settings to a given build configuration.
- **Breaking change:** Improved generation of build variant values from enum names.
- **Breaking change:** Renamed 'OSX' build platform to 'macOS'.
- Removed pre-Unity 2018.1 code.

### Fixed

- Only enable Android split APK variant on supported Unity versions.
- Fixed Build Action filter UI shown for 'Single Run' actions. [Issue #69](https://github.com/superunitybuild/buildtool/issues/69)

## [1.0.0-pre.1] - 2021-04-09

### Added

- Added product icon.
- Added UPM support. [PR #65](https://github.com/superunitybuild/buildtool/pull/65/commits/6b90791566a771bf189ed6272d3005b4d1933ca1)
- Added support for setting iOS device type and SDK version. [Issue #61](https://github.com/superunitybuild/buildtool/issues/61)
- Added support for setting Android min SDK version.
- Added ability to select which BuildSettings to use. [PR #60](https://github.com/superunitybuild/buildtool/pull/60)
- Added support for Android ARM64 architecture. [PR #56](https://github.com/superunitybuild/buildtool/pull/56)
- Added support for APK splitting in Android. [PR #58](https://github.com/superunitybuild/buildtool/pull/58/commits/30c8959cd670bb68c3cd70a6728644df25055fc5)
- Added option to sync version with PlayerSettings. [PR #53](https://github.com/superunitybuild/buildtool/pull/53)

### Changed

- Changed default build version format.
- Changed default build folder and output path options. The new default build folder name `"Builds"` follows the convention documented in the _de facto_ standard [GitHub Unity .gitignore](https://github.com/github/gitignore/blob/master/Unity.gitignore).
- Standardised code style.
- Minor UI tweaks to improve layout and better match Editor style.
- Improved default assets management.
- **Breaking change:** Updated namespace.
- Prefill company name and bundle identifier when creating new Release Types.
- Improve handling of build options. [PR #65](https://github.com/superunitybuild/buildtool/pull/65/commits/e7cfee053255e5248784a6da96a36e89506ccf9f).
- Updated to iOS build platform work by [@chloethompson](https://github.com/chloethompson) to support changes. [PR #52](https://github.com/superunitybuild/buildtool/pull/52)

### Removed

- Removed support for Unity versions older than 2018.1.
- Removed obsolete code in `BuildProject` and `BuildReleaseTypeDrawer`.
- Removed obsolete Linux build architectures on Unity 2019.2+. [PR #50](https://github.com/superunitybuild/buildtool/pull/50)

### Fixed

- `BuildPlatformListDrawer` performance improvement. [Issue #66](https://github.com/superunitybuild/buildtool/issues/66)
- **Breaking change:** Fixed platform instances not calling overridden ApplyVariant methods on build. [Issue #62](https://github.com/superunitybuild/buildtool/issues/62)
- Fixed iOS build variant options not being applied. [Issue #61](https://github.com/superunitybuild/buildtool/issues/61)
- Fixed typos [PR #59](https://github.com/superunitybuild/buildtool/pull/59)
- Fixed Android device type setting being ignored. [PR #58](https://github.com/superunitybuild/buildtool/pull/58/commits/15b96e9e9777ef500b6bfa6d9db800a17dab9273)
- Updated BuildActions wiki link. [PR #51](https://github.com/superunitybuild/buildtool/pull/51)
- Restored pre-build player settings after build has finished. [PR #49](https://github.com/superunitybuild/buildtool/pull/49)
- Fixed UI spacing for Unity 2019.3x [Issue #43](https://github.com/superunitybuild/buildtool/issues/43)
- Fixed issue in Linux build name moving `binaryName` to `BuildArchitecture`. [PR #41](https://github.com/superunitybuild/buildtool/pull/41)
- Fixed an issue where custom defines were overwrite when build was finished. [Issue #36](https://github.com/superunitybuild/buildtool/issues/36)

[unreleased]: https://github.com/superunitybuild/buildtool/compare/v8.0.0-pre.1...HEAD
[8.0.0-pre.1]: https://github.com/superunitybuild/buildtool/compare/v8.0.0...v8.0.0-pre.1
[7.0.0]: https://github.com/superunitybuild/buildtool/compare/v6.0.1...v7.0.0
[6.0.1]: https://github.com/superunitybuild/buildtool/compare/v6.0.0...v6.0.1
[6.0.0]: https://github.com/superunitybuild/buildtool/compare/v5.0.4...v6.0.0
[5.0.4]: https://github.com/superunitybuild/buildtool/compare/v5.0.3...v5.0.4
[5.0.3]: https://github.com/superunitybuild/buildtool/compare/v5.0.2...v5.0.3
[5.0.2]: https://github.com/superunitybuild/buildtool/compare/v5.0.1...v5.0.2
[5.0.1]: https://github.com/superunitybuild/buildtool/compare/v5.0.0...v5.0.1
[5.0.0]: https://github.com/superunitybuild/buildtool/compare/v4.0.1...v5.0.0
[4.0.1]: https://github.com/superunitybuild/buildtool/compare/v4.0.0...v4.0.1
[4.0.0]: https://github.com/superunitybuild/buildtool/compare/v3.0.0...v4.0.0
[3.0.0]: https://github.com/superunitybuild/buildtool/compare/v2.2.0...v3.0.0
[2.2.0]: https://github.com/superunitybuild/buildtool/compare/v2.1.0...v2.2.0
[2.1.0]: https://github.com/superunitybuild/buildtool/compare/v2.0.0...v2.1.0
[2.0.0]: https://github.com/superunitybuild/buildtool/compare/v1.3.0...v2.0.0
[1.3.0]: https://github.com/superunitybuild/buildtool/compare/v1.2.0...v1.3.0
[1.2.0]: https://github.com/superunitybuild/buildtool/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/superunitybuild/buildtool/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/superunitybuild/buildtool/compare/v1.0.0-pre.2...v1.0.0
[1.0.0-pre.2]: https://github.com/superunitybuild/buildtool/compare/v1.0.0-pre.1...v1.0.0-pre.2
[1.0.0-pre.1]: https://github.com/superunitybuild/buildtool/compare/v0.9.8...v1.0.0-pre.1
