# Cirreum.Exceptions 1.1.0

## Summary

This release lets `NotFoundException` carry its lookup keys across a serializing
`Result` round-trip. It is purely additive — no existing API changes.

## Why

`Cirreum.Result` 2.0.0 added a System.Text.Json converter so `Result`/`Result<T>`
round-trip correctly through serializing caches and transports. Because
exceptions are not serializable, a deserialized failure carries a
`SurrogateResultException` that preserves the original error's **type name and
message only** — any structured state (such as `NotFoundException.Keys`) is lost.

`Cirreum.Result` 2.0.0 also introduced the opt-in `IErrorState` contract: an
exception that implements it has its `string`→`string` state captured into the
error wire-format and round-tripped onto `SurrogateResultException.State`.

## What changed

- **`NotFoundException` implements `Cirreum.IErrorState`.** Its keys are exposed
  under a single `"keys"` entry (`string.Join(", ", Keys)`). A `NotFoundException`
  failure that travels through a distributed query cache now arrives with its
  keys intact on `SurrogateResultException.State["keys"]`.
- **Empty keys write nothing.** When `Keys` is empty the state map is empty and
  the converter omits it from the wire — no bloat for the common case.
- **Added a `Cirreum.Result 2.0.0` package reference** (Foundation/L0). The
  package remains AOT-compatible: referencing `Cirreum.Result` is non-infectious,
  and the `IErrorState` implementation is reflection-free `string`→`string` state.

## Compatibility

- No breaking changes; this is a minor release.
- The `IErrorState.State` member is an **explicit** interface implementation, so
  it does not alter the public surface of `NotFoundException` (in-process callers
  continue to read the strongly-typed `Keys` property).
- Multi-targets net8.0 / net9.0 / net10.0, unchanged.

## Upgrade

Update the package reference to `1.1.0`. No code changes required.
