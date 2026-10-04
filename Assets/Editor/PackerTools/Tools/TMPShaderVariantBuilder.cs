/****************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TMPShaderVariantBuilder.cs
 * author:    云毅
 * created:   2026
 * descrip:   生成 TMP ShaderVariantCollection
 *            修复 AB 模式下 TMP 文字渲染为空（shader isSupported=False）问题。
 *            根因：AB 打包时按 "Strip Unused" 裁剪了 TMP shader 的渲染变体
 *            （如 OUTLINE_ON|UNDERLAY_ON），导致 AB 内材质加载后 shader
 *            无法编译该变体 → 网格已建但渲染不出文字。
 *            修复：把所有 TMP 材质实际用到的 keyword 变体收集进
 *            ShaderVariantCollection 并放入 Resources（始终在构建内，
 *            阻止该 shader 的变体被裁剪）。
 ****************************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Honor.Editor
{
    //=========================================================================
    // TMP ShaderVariantCollection 生成器
    // 扫描项目内所有使用 TMP shader 的材质，收集其 keyword 变体，
    // 生成 .shadervariants 并保存到 Assets/Resources 下
    //=========================================================================
    /// <summary>
    /// TMP ShaderVariantCollection 生成器
    /// 说明：生成后需重新打包 AB 才能生效（变体保留发生在打包期）
    /// </summary>
    public static class TMPShaderVariantBuilder
    {
        #region 常量

        /// <summary>TMP shader 源目录（相对 Assets）</summary>
        private const string TmpShaderDir = "Assets/Plugins/ThirdLibs/TextMesh Pro/Shaders";

        /// <summary>SVC 输出目录（必须放 Resources，确保始终在构建内）</summary>
        private const string OutputDir = "Assets/Resources/ShaderVariants";

        /// <summary>SVC 输出文件名</summary>
        private const string OutputName = "TMP_ShaderVariants.shadervariants";

        #endregion

        //=========================================================================
        // 入口（已从 Honor/工具 菜单移除，收口到 AB 打包工具界面的"生成TMP变体集合"按钮）
        //=========================================================================
        /// <summary>
        /// 生成 TMP ShaderVariantCollection
        /// 说明：原为 Honor/工具 菜单项；现收口到打包工具界面（BuildPackage_EditorWindow），
        ///       保留本方法供界面按钮与代码复用
        /// </summary>
        public static void GenerateTmpShaderVariants()
        {
            // 1. 收集所有 TMP shader
            List<Shader> tmpShaders = new List<Shader>();
            foreach (string guid in AssetDatabase.FindAssets("t:Shader", new[] { TmpShaderDir }))
            {
                Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath(guid));
                if (shader != null && !tmpShaders.Contains(shader))
                    tmpShaders.Add(shader);
            }

            if (tmpShaders.Count == 0)
            {
                Debug.LogWarning("[TMP SVC] 未找到任何 TMP shader，路径: " + TmpShaderDir);
                return;
            }

            // 2. 收集所有使用 TMP shader 的材质及其 keyword 变体
            //    key: shader, value: 该 shader 用到的 keyword 组合（去重）
            Dictionary<Shader, HashSet<string>> variantMap = new Dictionary<Shader, HashSet<string>>();
            foreach (string guid in AssetDatabase.FindAssets("t:Material"))
            {
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));
                if (mat == null || mat.shader == null) continue;
                if (!tmpShaders.Contains(mat.shader)) continue;

                string key = string.Join("|", mat.shaderKeywords);
                if (!variantMap.TryGetValue(mat.shader, out HashSet<string> set))
                {
                    set = new HashSet<string>();
                    variantMap[mat.shader] = set;
                }
                set.Add(key);
            }

            // 3. 构建 ShaderVariantCollection
            ShaderVariantCollection svc = new ShaderVariantCollection();
            int addedCount = 0;

            // 3.1 材质用到的 keyword 变体
            foreach (KeyValuePair<Shader, HashSet<string>> kv in variantMap)
            {
                foreach (string kws in kv.Value)
                {
                    string[] keywords = string.IsNullOrEmpty(kws) ? new string[0] : kws.Split('|');
                    AddVariantSafe(svc, kv.Key, keywords);
                    addedCount++;
                }
            }

            // 3.2 全部 TMP shader 的基准变体（无 keyword）
            foreach (Shader shader in tmpShaders)
            {
                AddVariantSafe(svc, shader, new string[0]);
                addedCount++;
            }

            // 4. 保存到 Resources
            if (!Directory.Exists(OutputDir))
                Directory.CreateDirectory(OutputDir);

            string path = string.Format("{0}/{1}", OutputDir, OutputName);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(svc, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(string.Format(
                "[TMP SVC] 生成完成：{0} 个 shader，共添加 {1} 个变体。\n保存路径：{2}\n" +
                "请重新打包 AB（打AB 或 打全部AB）后验证 TMP 文字渲染。",
                tmpShaders.Count, addedCount, path));
        }

        /// <summary>
        /// 安全添加变体（捕获重复/非法变体异常）
        /// TMP 文字走前向渲染 pass（PassType.Normal），故显式指定；
        /// 本 Unity 版本仅支持 string[] 关键字构造
        /// </summary>
        private static void AddVariantSafe(ShaderVariantCollection svc, Shader shader, string[] keywords)
        {
            try
            {
                svc.Add(new ShaderVariantCollection.ShaderVariant(shader, PassType.Normal, keywords));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(string.Format("[TMP SVC] 添加变体失败 shader={0} err={1}",
                    shader != null ? shader.name : "null", e.Message));
            }
        }
    }
}
