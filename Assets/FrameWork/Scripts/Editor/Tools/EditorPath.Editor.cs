using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Honor.Editor
{
    public static partial class EditorPath
    {
        /// <summary>
        /// Editor编辑器相关路径信息
        /// </summary>
        public static class Editor
        {
            /// <summary>
            /// 获取项目设置文件夹的完整路径
            /// </summary>
            /// <returns></returns>
            public static string ProjectSettingFolderFullPath()
            {
                string settingFolderFullPath = $"{Application.dataPath}/../ProjectSettings/Honor";
                //判断是否有文件
                if (!Directory.Exists(settingFolderFullPath))
                {
                    Directory.CreateDirectory(settingFolderFullPath);
                    //刷新
                    AssetDatabase.Refresh();
                }

                return settingFolderFullPath;
            }

        }
    }
}