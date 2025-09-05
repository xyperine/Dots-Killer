using System;
using DotsKiller.ColorPaletteSystem;
using UnityEditor;
using UnityEngine;

namespace DotsKiller.Editor
{
    public static class ColorPaletteContextMenu
    {
        private const string COMPONENT_CONTEXT_PATH = "CONTEXT/Component/Set Color Palette ID/";
        private const string GAME_OBJECT_CONTEXT_PATH = "GameObject/Set Color Palette ID/";

        private const string BACKGROUND_NAME = nameof(ColorPaletteTargetID.Background);
        private const string DOT_NAME = nameof(ColorPaletteTargetID.Dot);
        private const string PRIMARY_NAME = nameof(ColorPaletteTargetID.Primary);
        private const string SECONDARY_NAME = nameof(ColorPaletteTargetID.Secondary);
        private const string DARK_TEXT_NAME = nameof(ColorPaletteTargetID.DarkText);
        private const string LIGHT_TEXT_NAME = nameof(ColorPaletteTargetID.LightText);
        private const string ACCENT_TEXT_NAME = nameof(ColorPaletteTargetID.AccentText);


        [MenuItem("Tools/Color Palette/Try Apply Colors")]
        private static void TryApplyColors()
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length == 0)
            {
                throw new NullReferenceException("Select game objects first!");
            }
            
            ColorPaletteApplier applier = GameObject.FindFirstObjectByType<ColorPaletteApplier>(FindObjectsInactive.Include);
            if (applier == null)
            {
                throw new NullReferenceException($"No {nameof(ColorPaletteApplier)} was found!");
            }
            
            foreach (GameObject gameObject in selectedGameObjects)
            {
                if (gameObject.TryGetComponent(out ColorPaletteTarget target))
                {
                    applier.ApplyTo(target);
                    
                    EditorUtility.SetDirty(target);
                }
                else
                {
                    Debug.LogWarning($"No {nameof(ColorPaletteTarget)} component was found on {gameObject.name}!");
                }
            }
        }
        
        
        private static void AddIDScript(ColorPaletteTargetID id)
        {
            Transform[] selectedTransforms = Selection.transforms;
            foreach (Transform transform in selectedTransforms)
            {
                if (!transform.TryGetComponent(out ColorPaletteTarget target))
                {
                    target = Undo.AddComponent<ColorPaletteTarget>(transform.gameObject);
                }

                Undo.RecordObject(target, "Set Color Palette ID");
                target.SetID(id);
            }
        }
        
        
        [MenuItem(COMPONENT_CONTEXT_PATH + BACKGROUND_NAME, false, 10)]
        [MenuItem(GAME_OBJECT_CONTEXT_PATH + BACKGROUND_NAME, false, 10)]
        private static void AddIDScriptBackground()
        {
            AddIDScript(ColorPaletteTargetID.Background);
        }
        
        
        [MenuItem(COMPONENT_CONTEXT_PATH + DOT_NAME, false, 10)]
        [MenuItem(GAME_OBJECT_CONTEXT_PATH + DOT_NAME, false, 10)]
        private static void AddIDScriptDot()
        {
            AddIDScript(ColorPaletteTargetID.Dot);
        }
        
        
        [MenuItem(COMPONENT_CONTEXT_PATH + PRIMARY_NAME, false, 10)]
        [MenuItem(GAME_OBJECT_CONTEXT_PATH + PRIMARY_NAME, false, 10)]
        private static void AddIDScriptPrimary()
        {
            AddIDScript(ColorPaletteTargetID.Primary);
        }
        
        
        [MenuItem(COMPONENT_CONTEXT_PATH + SECONDARY_NAME, false, 10)]
        [MenuItem(GAME_OBJECT_CONTEXT_PATH + SECONDARY_NAME, false, 10)]
        private static void AddIDScriptSecondary()
        {
            AddIDScript(ColorPaletteTargetID.Secondary);
        }
        
        
        [MenuItem(COMPONENT_CONTEXT_PATH + DARK_TEXT_NAME, false, 10)]
        [MenuItem(GAME_OBJECT_CONTEXT_PATH + DARK_TEXT_NAME, false, 10)]
        private static void AddIDScriptDarkText()
        {
            AddIDScript(ColorPaletteTargetID.DarkText);
        }
        
        
        [MenuItem(COMPONENT_CONTEXT_PATH + LIGHT_TEXT_NAME, false, 10)]
        [MenuItem(GAME_OBJECT_CONTEXT_PATH + LIGHT_TEXT_NAME, false, 10)]
        private static void AddIDScriptLightText()
        {
            AddIDScript(ColorPaletteTargetID.LightText);
        }
        
        [MenuItem(COMPONENT_CONTEXT_PATH + ACCENT_TEXT_NAME, false, 10)]
        [MenuItem(GAME_OBJECT_CONTEXT_PATH + ACCENT_TEXT_NAME, false, 10)]
        private static void AddIDScriptAccentText()
        {
            AddIDScript(ColorPaletteTargetID.AccentText);
        }
    }
}