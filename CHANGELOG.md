# Splunk SDK for C# PCL

## Version 2.1.0

### New Features
* Add the GitHub commits modular input example.
* Add support for Xamarin Forms.
* Add support for remotely debugging modular inputs with Visual Studio.

### Minor changes
* Building a modular input example no longer creates its `.spl` file.
* `Job` returns all available results by default if no arguments are passed to `Job.GetSearchResultsAsync()`, `Job.GetSearchPreviewAsync()`, and `Job.GetSearchEventsAsync()`.
* `Service` now uses interfaces in place of implementations in order to improve testability.
* Add a `ModularInputs/EventWriter.LogAsync()` overload that takes a `Severity` enum as a parameter.
* Add a `Service` constructor that takes a `Uri` as a parameter.

## Version 2.0.0
* Add ConfigureAwait(false) to all await calls in the library to prevent some deadlocks in user code.
* Add User-Agent string to HTTP requests.

## Version 2.0.0 pre-release

* Initial release.