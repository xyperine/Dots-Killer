using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace DotsKiller.ColorPaletteSystem
{
    public class ColorPaletteApplier : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ColorPaletteSO defaultPaletteSO;
        [SerializeField] private ColorPaletteSO activePaletteSO;


        private void OnValidate()
        {
            Apply();
        }


        private void Start()
        {
            Apply();
        }

        
        [Button]
        private void Apply()
        {
            Apply(activePaletteSO);
        }
        
        
        private void Apply(ColorPaletteSO paletteSO)
        {
            mainCamera.backgroundColor = paletteSO.GetColorFor(ColorPaletteTargetID.Background);
            
            ColorPaletteTarget[] targets =
                FindObjectsByType<ColorPaletteTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (ColorPaletteTarget target in targets)
            {
                Color color = paletteSO.GetColorFor(target.ID);
                
                if (target.TryGetComponent(out Graphic graphic))
                {
                    graphic.color = color;
                }

                if (target.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.color = color;
                }
            }
        }


        [Button]
        private void Default()
        {
            Apply(defaultPaletteSO);
        }
    }
}