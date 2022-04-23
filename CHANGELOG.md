Changelog
=========

3.3.0
-----

### Added

* Added async overloads of a number of methods which accepted a `Func`. Since these methods rely on `ValueTask`, they are not available on anything older than .NET Standard 2.1.

### Changed

* A new CI build system based on GitHub Actions.

3.2.1
-----

### Changed

* Removed an unused dependency on `System.Reflection.Emit`.

3.2.0
-----

### Changed

* Sawmill and its various sibling packages are now strong named.

3.1.0
-----

### Changed

* Provided implementations of `IRewriter` are no longer sealed.
* Symbol packages are now shipped as `snupkg` files.


3.0.1
-----

### Changed

* Fixed a bug in `SpanFactory`


3.0.0
-----

### Changed

* `IRewriter` and `IRewritable` have been substantially redesigned, as outlined in https://www.benjamin.pizza/posts/2019-10-04-rewriting-irewritable.html 
    * Removed `RewriteChildren` from the interface
    * Added `CountChildren`
    * Changed signature of `GetChildren`. It now takes a `Span` to copy the children into (and returns `void`)
    * Changed signature of `SetChildren`. It now takes a `ReadOnlySpan` to copy the children from (instead of a `Children<T>`)
* Renamed `DefaultRewriteChildren` to `RewriteChildren`
* `ChildrenInContext` now returns an array (instead of a `Children<T>`)
* The function passed to `Fold` now takes a `Span` of children instead of an array and its parameters have been switched
* Added C#8 nullability checks
* Updated dependencies for various subpackages (`Sawmill.Microsoft.CodeAnalysis` etc)
* Performance improvements across the board

### Added

* `GetChildren` extension method which returns an array (to replace the `GetChildren` formerly on the interface)
* `SpanFunc` delegate type declaration

### Removed

* `Children<T>` and `NumberOfChildren`
* `Sawmill.Analyzers`
