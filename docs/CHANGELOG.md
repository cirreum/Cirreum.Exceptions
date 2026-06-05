# Changelog

All notable changes to **Cirreum.Exceptions** are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.1.0] - 2026-06-05

### Added

- `NotFoundException` now implements `Cirreum.IErrorState`, exposing its lookup
  keys as serializable `string`→`string` error state under a single `"keys"`
  entry. When a `Result` carrying a `NotFoundException` failure is serialized
  (e.g. by a distributed query cache), the keys survive the round-trip onto
  `SurrogateResultException.State` instead of being lost. Empty keys write
  nothing to the wire.
- Reference to `Cirreum.Result` `2.0.0` (Foundation/L0), which provides the
  `IErrorState` contract and the `Result` System.Text.Json converter.

## [1.0.4]

Baseline release. Prior history predates this changelog.
