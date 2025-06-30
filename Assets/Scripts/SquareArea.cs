using UnityEngine;

namespace DotsKiller
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SquareArea : MonoBehaviour, IAreaProvider
    {
        [SerializeField] private BoxCollider2D collider;

        public Vector3 Center => collider.bounds.center;
        public Vector3 Extents => collider.bounds.extents;
    }
}