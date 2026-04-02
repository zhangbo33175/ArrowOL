using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 编辑器模式下数据导出工具类
    /// </summary>
    public static class TableExportEditorUtilityTool
    {
        /// <summary>
        /// 使用table导出的string创建txt文件
        /// </summary>
        /// <param name="exportDirectory">创建asset文件需要存放的文件夹</param>
        /// <param name="mulToOne">是否将assetChars的内容导出到一个文件</param>
        /// <param name="exportOneFileName">将assetChars的内容导出到一个文件的文件名字</param>
        /// <param name="assetChars">Dictionary<string, List<string>> 要导出的文件名/导出文件内容</param>
        public static bool CreateTableExportTxt(string exportDirectory, bool mulToOne, string exportOneFileName,
            Dictionary<string, List<string>> assetChars)
        {
            // 检查传入路径是否是以.asset后缀名结尾
            if (string.IsNullOrEmpty(exportDirectory))
            {
                Log.Error("CreateTableExportTxt 时传入的路径为空");
                return false;
            }

            // 删除当前文件夹下面原有的asset文件
            string assetFilePath = exportDirectory.Substring(exportDirectory.IndexOf("/Assets") + 1);
            string oneFilePath = $"{assetFilePath}/{exportOneFileName}.txt";
            if (File.Exists(oneFilePath))
            {
                Log.Info($"CreateTableExportTxt 删除旧的TMP字符集文件{oneFilePath}");
                File.Delete(oneFilePath);
            }

            foreach (var item in assetChars)
            {
                string perFilePath = $"{assetFilePath}/{item.Key}.txt";
                if (File.Exists(perFilePath))
                {
                    Log.Info($"CreateTableExportTxt 删除旧的TMP字符集文件{perFilePath}");
                    File.Delete(perFilePath);
                }
            }

            if (mulToOne)
            {
                string tmpChars = "";
                foreach (var item in assetChars)
                {
                    // 对要制作字库的文字内容进行去重
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    tmpChars = $"{tmpChars}{string.Join("", assetStringArray)}";
                }

                var filePath = AorTxt.Format("{0}/{1}",
                    Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length), oneFilePath);
                File.WriteAllText(filePath, tmpChars, new UTF8Encoding(false));
                AssetDatabase.Refresh();
            }
            else
            {
                foreach (var item in assetChars)
                {
                    string perFilePath = $"{assetFilePath}/{item.Key}.txt";
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    string tempStr = string.Join("", assetStringArray);

                    var filePath = AorTxt.Format("{0}/{1}",
                        Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length), perFilePath);
                    File.WriteAllText(filePath, tempStr, new UTF8Encoding(false));
                }

                AssetDatabase.Refresh();
            }

            return true;
        }

        /// <summary>
        /// 使用table导出的string创建txt文件
        /// </summary>
        /// <param name="exportDirectory">创建asset文件需要存放的文件夹</param>
        /// <param name="mulToOne">是否将assetChars的内容导出到一个文件</param>
        /// <param name="exportOneFileName">是否将assetChars的内容导出到一个文件</param>
        /// <param name="assetChars">Dictionary<string, List<string>> 要导出的文件名/导出文件内容</param>
        public static bool CreateTableExportTextTxt(string exportDirectory, bool mulToOne, string exportOneFileName,
            Dictionary<string, List<string>> assetChars)
        {
            // 检查传入路径是否是以.asset后缀名结尾
            if (string.IsNullOrEmpty(exportDirectory))
            {
                Log.Error("CreateTableExportTextTxt 时传入的路径为空");
                return false;
            }

            // 删除当前文件夹下面原有的asset文件
            string assetFilePath = exportDirectory.Substring(exportDirectory.IndexOf("/Assets") + 1);
            string oneFilePath = $"{assetFilePath}/{exportOneFileName}.txt";
            if (File.Exists(oneFilePath))
            {
                Log.Info($"CreateTableExportTextTxt 删除旧的TMP字符集文件{oneFilePath}");
                File.Delete(oneFilePath);
            }

            foreach (var item in assetChars)
            {
                string perFilePath = $"{assetFilePath}/{item.Key}.txt";
                if (File.Exists(perFilePath))
                {
                    Log.Info($"CreateTableExportTextTxt 删除旧的TMP字符集文件{perFilePath}");
                    File.Delete(perFilePath);
                }
            }

            if (mulToOne)
            {
                string tmpChars = "";
                foreach (var item in assetChars)
                {
                    // 对要制作字库的文字内容进行去重
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    tmpChars = $"{tmpChars}{string.Join("", assetStringArray)}";
                }

                var filePath = AorTxt.Format("{0}/{1}",
                    Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length), oneFilePath);
                File.WriteAllText(filePath, tmpChars, new UTF8Encoding(false));
                AssetDatabase.Refresh();
            }
            else
            {
                foreach (var item in assetChars)
                {
                    string perFilePath = $"{assetFilePath}/{item.Key}.txt";
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    string tempStr = string.Join("", assetStringArray);

                    var filePath = AorTxt.Format("{0}/{1}",
                        Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length), perFilePath);
                    File.WriteAllText(filePath, tempStr, new UTF8Encoding(false));
                }

                AssetDatabase.Refresh();
            }

            return true;
        }
    }
}