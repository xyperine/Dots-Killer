using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DotsKiller.ColorPaletteSystem
{
    [CreateAssetMenu(fileName = "Color_Palette", menuName = "Color Palette", order = 0)]
    public class ColorPaletteSO : ScriptableObject
    {
        [SerializeField, SerializedDictionary("ID", "Color")] private SerializedDictionary<ColorPaletteTargetID, Color> palette;


       public Color GetColorFor(ColorPaletteTargetID id)
       {
           if (!palette.TryGetValue(id, out Color color))
           {
               throw new KeyNotFoundException();
           }

           return color;
       }
    }
}