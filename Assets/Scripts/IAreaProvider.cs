using UnityEngine;

namespace DotsKiller
{
    public interface IAreaProvider
    {
        Vector3 Center { get; }
        Vector3 Extents { get; }
        Vector3 RandomPoint { get; }
    }
}