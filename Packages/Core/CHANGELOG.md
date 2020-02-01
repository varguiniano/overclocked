# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.0.3] - 2019-10-29
### Fixed
- Language selector now sets the dropdown value to the current language.

## [0.0.2] - 2019-10-29
### Added
- Localization manager can now store the current language on a config file if that file is available.
- Changelog file.

### Changed
- The Logger now throws an error when there debug config file is missing instead of just not displaying anything.

### Fixed
- The localization manager editor no longer throws an exception when the language library reference is null.

## [0.0.1] - 2019-10-28
### Added
- Initial package version, see the Github project for more info on features.