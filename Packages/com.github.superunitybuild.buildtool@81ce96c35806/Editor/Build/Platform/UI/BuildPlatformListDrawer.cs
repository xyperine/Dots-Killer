using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SuperUnityBuild.BuildTool
{
    [CustomPropertyDrawer(typeof(BuildPlatformList))]
    public class BuildPlatformListDrawer : PropertyDrawer
    {
        private int index = 0;
        private SerializedProperty list = null;
        private BuildPlatformList platformList = null;

        private readonly List<string> availablePlatformNameList = new();
        private readonly List<Type> availablePlatformTypeList = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _ = EditorGUI.BeginProperty(position, label, property);

            _ = EditorGUILayout.BeginHorizontal();
            property.serializedObject.Update();

            bool show = property.isExpanded;
            UnityBuildGUIUtility.DropdownHeader("Build Platforms", ref show, false, GUILayout.ExpandWidth(true));
            property.isExpanded = show;

            UnityBuildGUIUtility.HelpButton("Parameter-Details#build-platforms");
            EditorGUILayout.EndHorizontal();

            platformList = fieldInfo.GetValue(property.serializedObject.targetObject) as BuildPlatformList;
            PopulateAvailablePlatforms();
            list = property.FindPropertyRelative("platforms");
            list.serializedObject.Update();

            if (show)
            {
                _ = EditorGUILayout.BeginVertical(UnityBuildGUIUtility.dropdownContentStyle);

                DrawPlatforms(property);

                if (list.arraySize > 0)
                    GUILayout.Space(20);

                // Draw all available platforms.
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                index = EditorGUILayout.Popup(index, availablePlatformNameList.ToArray(), UnityBuildGUIUtility.popupStyle, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(250));
                if (GUILayout.Button("Add Platform", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(150)))
                {
                    BuildPlatform addedBuildPlatform = ScriptableObject.CreateInstance(availablePlatformTypeList[index]) as BuildPlatform;
                    addedBuildPlatform.name = addedBuildPlatform.platformName;
                    platformList.platforms.Add(addedBuildPlatform);

                    AssetDatabase.AddObjectToAsset(addedBuildPlatform, BuildSettings.instance);
                    AssetDatabaseUtility.ImportAsset(AssetDatabase.GetAssetPath(BuildSettings.instance));

                    platformList.platforms[^1].enabled = true;

                    for (int i = platformList.platforms.Count - 1; i >= 0; i--)
                    {
                        if (!platformList.platforms[i].enabled)
                            platformList.platforms.RemoveAt(i);
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }

            EditorGUI.EndProperty();
        }

        private void DrawPlatforms(SerializedProperty property)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                SerializedProperty listEntry = list.GetArrayElementAtIndex(i);

                BuildPlatform platform = listEntry.objectReferenceValue as BuildPlatform;
                if (platform == null)
                {
                    list.SafeDeleteArrayElementAtIndex(i);
                    --i;
                    continue;
                }

                SerializedObject serializedBuildPlatform = new(platform);

                _ = EditorGUILayout.BeginHorizontal();
                bool show = listEntry.isExpanded;
                string tooltip = platform.ToString();
                string text = UnityBuildGUIUtility.ToLabel(tooltip);

                UnityBuildGUIUtility.DropdownHeader(new GUIContent(text, tooltip), ref show, false, GUILayout.ExpandWidth(true));

                listEntry.isExpanded = show;

                if (UnityBuildGUIUtility.DeleteButton())
                {
                    List<BuildPlatform> platforms = BuildSettings.platformList.platforms;

                    // Destroy underlying object
                    ScriptableObject.DestroyImmediate(platforms[i], true);
                    AssetDatabaseUtility.ImportAsset(AssetDatabase.GetAssetPath(BuildSettings.instance));

                    // Remove object reference from list
                    list.SafeDeleteArrayElementAtIndex(i);
                    show = false;
                }

                EditorGUILayout.EndHorizontal();

                if (show && platform.enabled)
                {
                    _ = EditorGUILayout.BeginVertical(UnityBuildGUIUtility.dropdownContentStyle);
                    platform.Draw(serializedBuildPlatform);
                    EditorGUILayout.EndVertical();
                }
            }
        }

        private void PopulateAvailablePlatforms()
        {
            if (availablePlatformTypeList.Count > 0)
                return;

            Type ti = typeof(BuildPlatform);

            availablePlatformNameList.Clear();
            availablePlatformTypeList.Clear();
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (ti.IsAssignableFrom(t) && ti != t)
                    {
                        BuildPlatform instance = ScriptableObject.CreateInstance(t) as BuildPlatform;
                        availablePlatformNameList.Add(instance.platformName);
                        availablePlatformTypeList.Add(t);
                    }
                }
            }
        }
    }
}
