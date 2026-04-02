using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Honor.Runtime;

namespace Editor.MapEditor
{
    public static class UtilDataSet
    {
        /// <summary>
        /// DataSet转换为对象
        /// </summary>
        /// <param name="dataSet"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataSet dataSet) where T : class, new()
        {
            var list = new List<T>();

            if (dataSet == null || dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count < 4) return list;

            int columns = dataSet.Tables[0].Columns.Count; //列
            int rows = dataSet.Tables[0].Rows.Count; //行
            string[] cellKey = new string[columns]; //字段名字
            string[] typeList = new string[columns]; //字段类型
            for (int keyi = 0; keyi < columns; keyi++)
            {
                cellKey[keyi] = dataSet.Tables[0].Rows[1][keyi].ToString();
                typeList[keyi] = dataSet.Tables[0].Rows[2][keyi].ToString();
                if (cellKey[keyi] != string.Empty & typeList[keyi] == string.Empty)
                {
                    Log.Error("dataSet 错误, " + cellKey[keyi] + " 没有类型");
                    return list;
                }
            }

            for (int i = 4; i < rows; i++) //数据
            {
                var t = new T();
                for (int j = 0; j < columns; j++)
                {
                    string key = cellKey[j];
                    string type = typeList[j];

                    FieldInfo field = typeof(T).GetField(key);
                    if (field == null) continue;

                    if (key != string.Empty)
                    {
                        string value = dataSet.Tables[0].Rows[i][j].ToString();
                        if ("string".Equals(type))
                        {
                            field.SetValue(t, value);
                        }
                        else if ("number".Equals(type))
                        {
                            field.SetValue(t, int.Parse(value));
                        }
                        else if ("int".Equals(type))
                        {
                            field.SetValue(t, int.Parse(value));
                        }
                        else
                        {
                            field.SetValue(t, value);
                        }
                    }
                }

                list.Add(t);
            }

            return list;
        }
        
    }
}