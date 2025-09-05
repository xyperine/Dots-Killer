using DotsKiller.Utility;
using UnityEngine;

namespace DotsKiller
{
    public class ScalableAreaProvider : MonoBehaviour, IAreaProvider
    {
        [SerializeField] private RectTransform rectTransform;

        public Vector3 Center => rectTransform.TransformPoint(rectTransform.rect.center);
        public Vector3 Extents => rectTransform.TransformVector( rectTransform.rect.size * 0.5f);
        public Vector3 RandomPoint => Center + Extents.RandomInsideCuboid();
    }
}