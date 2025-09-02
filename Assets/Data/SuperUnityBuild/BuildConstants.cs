using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        Test,
    }

    public enum Platform
    {
        None,
        WebGL,
        Android,
        Windows,
    }

    public enum ScriptingBackend
    {
        None,
        IL2CPP,
    }

    public enum Target
    {
        None,
        Player,
    }

    public enum Distribution
    {
        None,
        Itch,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638924479110245792);
        public const string version = "0.0.1.9376:5327";
        public const int buildCounter = 16;
        public const ReleaseType releaseType = ReleaseType.Test;
        public const Platform platform = Platform.Windows;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.IL2CPP;
        public const Target target = Target.Player;
        public const Distribution distribution = Distribution.None;
    }
}

