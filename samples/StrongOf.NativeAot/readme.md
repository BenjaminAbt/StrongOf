# NativeAOT example and smoke test

This executable uses `StrongOf`, `StrongOf.Domains`, and `StrongOf.Json`. It checks all eleven generated
primitive wrappers and JSON converters, static factories, parsing, collections, direct TypeConverter use,
domain validation, and preservation of DateTimeOffset offsets. JSON reflection is disabled.

The three library assemblies are also listed as `TrimmerRootAssembly` items so publishing analyzes unused
library code, not just the methods reached by this example. Compiler and linker warnings fail the build.

From the repository root, on a machine with the [NativeAOT prerequisites](https://learn.microsoft.com/dotnet/core/deploying/native-aot/#prerequisites):

```powershell
dotnet publish samples/StrongOf.NativeAot/StrongOf.NativeAot.csproj -c Release -f net10.0 -r win-x64 -o artifacts/native-aot
./artifacts/native-aot/StrongOf.NativeAot.exe --require-native
```

On Linux:

```bash
dotnet publish samples/StrongOf.NativeAot/StrongOf.NativeAot.csproj -c Release -f net10.0 -r linux-x64 -o artifacts/native-aot
./artifacts/native-aot/StrongOf.NativeAot --require-native
```

`--require-native` checks that dynamic code is unavailable, preventing an accidental managed test run from
being counted as NativeAOT coverage. The example also runs under the JIT without that flag.

CI publishes and executes this example for .NET 9, 10, and 11 on Windows x64 and Linux x64.
It does not claim coverage of MVC, EF Core database operations, FluentValidation, or other runtime identifiers.

See [Models.cs](Models.cs) for converter attributes on freshly generated strong types and a property-level
converter on a precompiled domain type. [Program.cs](Program.cs) uses the generated JSON context explicitly.
When consuming the NuGet package, StrongOf's generator is included automatically. Repository project references
need the explicit analyzer reference shown in the sample project because analyzer project references are not transitive.
