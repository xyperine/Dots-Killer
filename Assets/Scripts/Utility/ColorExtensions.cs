using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace DotsKiller.Utility
{
    public static class ColorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 ToFloat4(this Color color)
        {
            return new float4(color.r, color.g, color.b, color.a);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithAlpha(this Color color, float a)
        {
            color.a = a;
            return color;
        }
    }
}