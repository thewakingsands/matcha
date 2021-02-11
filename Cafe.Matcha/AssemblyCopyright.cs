// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

[assembly: AssemblyTitle("Cafe.Matcha")]
[assembly: AssemblyDescription("Cafe.Matcha")]
[assembly: AssemblyCompany("FFCafe")]
#if GLOBAL
[assembly: AssemblyVersion("5.2.5.0")]
#else
[assembly: AssemblyVersion("5.3.5.0")]
#endif
[assembly: AssemblyCopyright("Copyright © FFCafe 2021")]

namespace Cafe.Matcha
{
    partial class Data {
#if GLOBAL
      public const string Version = "5.2.5.0";
#else
      public const string Version = "5.3.5.0";
#endif
    }
}