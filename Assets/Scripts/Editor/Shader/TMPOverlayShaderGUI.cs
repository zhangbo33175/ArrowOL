/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TMPOverlayShaderGUI.cs
 * author:    云毅
 * created:   2026
 * descrip:   TMP 叠加材质 Shader 自定义检视面板 GUI（Gold 渐变属性）
 *            注：ShaderGUI 属于编辑器专属类型（引用 UnityEditor / TMPro.EditorUtilities），
 *            必须编译进 Editor 程序集，故本文件置于 Assets/Scripts/Editor/Shader 目录
 ***************************************************************/

#if UNITY_EDITOR
using UnityEditor;
using TMPro.EditorUtilities;
using UnityEngine;

/// <summary>
/// TMP Overlay Shader 的自定义材质检视面板
/// 在默认 TMP SDF 材质面板基础上追加 Gold 渐变（顶部/底部颜色）设置项
/// </summary>
public class TMPOverlayShaderGUI : TMP_SDFShaderGUI
{
    /// <summary>
    /// 绘制材质属性面板：先绘制默认 TMP 属性，再追加渐变设置区
    /// </summary>
    /// <param name="materialEditor">材质编辑器</param>
    /// <param name="props">当前材质的属性数组</param>
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
#endif // UNITY_EDITOR
