// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved
// Polyfill so that records / init-only setters compile against netstandard2.0.

#pragma warning disable IDE0130 // Namespace does not match folder structure - intentional polyfill
namespace System.Runtime.CompilerServices;

internal static class IsExternalInit { }
