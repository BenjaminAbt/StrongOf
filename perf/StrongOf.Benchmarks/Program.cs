// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Reflection;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);
