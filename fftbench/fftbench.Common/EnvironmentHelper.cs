// From BenchmarkDotNet, https://github.com/PerfDotNet/BenchmarkDotNet/
// Copyright (c) 2013–2015 Andrey Akinshin, Jon Skeet, Matt Warren
// MIT License

namespace fftbench
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class EnvironmentHelper
    {
        public string OsVersion { get; set; }
        //public string ProcessorName { get; set; }
        public int ProcessorCount { get; set; }
        public string ClrVersion { get; set; }
        public string Architecture { get; set; }
        public bool HasAttachedDebugger { get; set; }
        public string Configuration { get; set; }

        public static EnvironmentHelper GetCurrentInfo()
        {
            return new EnvironmentHelper()
            {
                OsVersion = GetOsVersion(),
                ProcessorCount = GetProcessorCount(),
                ClrVersion = GetClrVersion(),
                Architecture = GetArchitecture(),
                HasAttachedDebugger = GetHasAttachedDebugger(),
                Configuration = GetConfiguration()
            };
        }

        public string ToFormattedString(string clrHint = "")
        {
            var sb = new StringBuilder();

            sb.AppendFormat("OS={0}", OsVersion);
            sb.AppendLine();
            sb.AppendFormat("{0}CLR={1}, Arch={2} {3}{4}", clrHint, ClrVersion, Architecture, Configuration, GetDebuggerFlag());
            sb.AppendLine();

            return sb.ToString();
        }

        private string GetDebuggerFlag()
        {
            return HasAttachedDebugger ? " [AttachedDebugger]" : "";
        }

        private static string GetOsVersion()
        {
            return Environment.OSVersion.ToString();
        }

        private static int GetProcessorCount()
        {
            return Environment.ProcessorCount;
        }

        private static string GetClrVersion()
        {
            return RuntimeInformation.FrameworkDescription;
        }

        private static string GetArchitecture()
        {
            return IntPtr.Size == 4 ? "32-bit" : "64-bit";
        }

        private static bool GetHasAttachedDebugger()
        {
            return Debugger.IsAttached;
        }

        private static string GetConfiguration()
        {
            string configuration = "RELEASE";
#if DEBUG
            configuration = "DEBUG";
#endif
            return configuration;
        }
    }
}