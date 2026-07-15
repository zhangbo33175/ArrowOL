using UnityEditor;
using TMPro.EditorUtilities;
using UnityEngine;

public class TMPOverlayShaderGUI : TMP_SDFShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        base.OnGUI(materialEditor, props);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Gradient Settings", EditorStyles.boldLabel);

        materialEditor.ShaderProperty(FindProperty("_TopGold", props), "Top Color");
        materialEditor.ShaderProperty(FindProperty("_BottomGold", props), "Bottom Color");

        // 强制 Unity 刷新材质，解决不实时更新
        if (GUI.changed)
        {
            materialEditor.PropertiesChanged();
        }
    }
}