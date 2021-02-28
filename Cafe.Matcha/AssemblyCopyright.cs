using System;
using System.Reflection;

[assembly: AssemblyTitle("Cafe.Matcha")]
[assembly: AssemblyDescription("Cafe.Matcha")]
[assembly: AssemblyCompany("FFCafe")]
#if GLOBAL
[assembly: AssemblyVersion("5.4.0.0")]
#else
[assembly: AssemblyVersion("5.3.5.1")]
#endif
[assembly: AssemblyCopyright("Copyright © FFCafe 2021")]

namespace Cafe.Matcha
{
    partial class Data {
#if GLOBAL
      public const string Version = "5.4.0.0";
#else
      public const string Version = "5.3.5.1";
#endif
    }
}