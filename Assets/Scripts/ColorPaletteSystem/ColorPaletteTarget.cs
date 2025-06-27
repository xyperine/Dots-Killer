#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DotsKiller.ColorPaletteSystem
{
    public class ColorPaletteTarget : MonoBehaviour
    {
        [field: SerializeField] public ColorPaletteTargetID ID { get; private set; }

        
        
#if UNITY_EDITOR
        public void SetID(ColorPaletteTargetID id)
        {
            ID = id;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}