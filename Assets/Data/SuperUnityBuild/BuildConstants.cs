using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        Regular,
    }

    public enum Platform
    {
        None,
        WebGL,
    }

    public enum ScriptingBackend
    {
        None,
        IL2CPP,
    }

    public enum Architecture
    {
        None,
        WebGL,
    }

    public enum Distribution
    {
        None,
        Itch,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638871798336780460);
        public const string version = "0.0.0.1";
        public const ReleaseType releaseType = ReleaseType.Regular;
        public const Platform platform = Platform.WebGL;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.IL2CPP;
        public const Architecture architecture = Architecture.WebGL;
        public const Distribution distribution = Distribution.Itch;
    }
}

