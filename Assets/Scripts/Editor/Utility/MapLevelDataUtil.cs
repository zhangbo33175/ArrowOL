using System.IO;
using System.Text;
using Editor.MapEditor;
using GameLib;
using Honor.Runtime;

namespace Editor.Utility
{
    /// <summary>
    /// 地图/关卡数据工具类（编辑器专用）
    /// 负责将编辑好的地图数据导出为：
    /// 1. JSON 配置文件（编辑器用）
    /// 2. Lua 配置表（游戏运行时读取）
    /// </summary>
    public static class MapLevelDataUtil
    {
        /// <summary>
        /// 将关卡数据保存为 JSON 文件（编辑器存档用）
        /// </summary>
        /// <param name="data">地图章节数据</param>
        /// <param name="levelSavePath">保存完整路径</param>
        public static void SaveDataJson(RMapChapterTypeData data, string levelSavePath)
        {
            // 序列化为格式化 JSON
            var jsonStr = JsonHelper.ToJson(data, true);
            // UTF8 编码
            var writeBytes = Encoding.UTF8.GetBytes(jsonStr);

            // 确保目录存在
            var dir = Path.GetDirectoryName(levelSavePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // 写入文件
            File.WriteAllBytes(levelSavePath, writeBytes);
            Log.Info($"保存关卡 {data.ChapterId}_{data.LevelId} 文件：{levelSavePath}");
        }

        /// <summary>
        /// 将关卡数据导出为游戏运行时使用的 Lua 配置表
        /// 自动生成注释、结构、字段、数组
        /// </summary>
        /// <param name="data">地图章节数据</param>
        /// <param name="levelLuaSavePath">Lua 文件保存路径</param>
        public static void SaveDataLua(RMapChapterTypeData data, string levelLuaSavePath)
        {
            var dir = Path.GetDirectoryName(levelLuaSavePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // 生成 Lua 表名
            var luaFileName = AorTxt.Format($"TableLevelData_{data.ChapterId}_{data.LevelId}");
            var sb = new StringBuilder();

            // ===================== 头部注释 =====================
            sb.AppendLine(
                "--=====================================================================================================");
            sb.AppendLine("-- (c) copyright 2026 - 2030, Honor.Runtime");
            sb.AppendLine("-- All Rights Reserved.");
            sb.AppendLine(
                "-- ----------------------------------------------------------------------------------------------------");
            sb.AppendLine(AorTxt.Format($"-- filename:  {luaFileName}"));
            sb.AppendLine(AorTxt.Format($"-- descrip:   关卡配置信息表"));
            sb.AppendLine("-- notices:   该文件自动生成，请不要手动修改！");
            sb.AppendLine(
                "--=====================================================================================================");
            sb.AppendLine();

            // ===================== 数据主体 =====================
            sb.AppendLine("---@type Tables.LevelData_Item");
            sb.AppendLine(AorTxt.Format($"Tables.LevelData_{data.ChapterId}_{data.LevelId} = ") + "{ ");

            // 基础关卡配置
            sb.AppendLine($"    ChapterId = \"{data.ChapterId}\",");
            sb.AppendLine($"    LevelId= {data.LevelId},");
            sb.AppendLine($"    BackgroundPath = \"{data.m_BackgroundPath}\",");
            sb.AppendLine($"    Time = \"{data.m_CcreateTime}\",");
            sb.AppendLine($"    SavePath =\" {data.SavePath}\",");
            sb.AppendLine($"    MapWidth = {data.m_MapWidth},");
            sb.AppendLine($"    MapHeight = {data.m_MapHeight},");

            // 附加目标数据
            sb.AppendLine($"    MapData= {{");
            for (int idx = 0; idx < data.m_MapObjectData.Count; idx++)
            {
                var obj = data.m_MapObjectData[idx];
                string pos = obj.m_Position.ToString("F2"); // (x,y,z)
                string rot = obj.m_Rotation.ToString("F2");
                string scale = obj.m_Scale.ToString("F2");
                string size = obj.m_Size.ToString("F2");

                sb.AppendLine($"       [{idx + 1}] = {{Id = {obj.m_Id}, " +
                              $"Type = \"{obj.m_Type}\"," +
                              $"Position = \"{pos}\"," +
                              $"Rotation = \"{rot}\"," +
                              $"Scale = \"{scale}\"," +
                              $"Sprite = \"{obj.m_Sprite}\"," +
                              $"Name = \"{obj.m_Name}\"," +
                              $"Size = \"{size}\"," +
                              $"IsChoose = {obj.m_IsChoose.ToString().ToLower()}}},");
            }

            sb.AppendLine($"    }},");

            sb.AppendLine($" }}");
            
            // 写入 Lua 文件（UTF8 无BOM，确保 Lua 读取正常）
            File.WriteAllText(levelLuaSavePath, sb.ToString(), new UTF8Encoding(false));
        }
    }
}