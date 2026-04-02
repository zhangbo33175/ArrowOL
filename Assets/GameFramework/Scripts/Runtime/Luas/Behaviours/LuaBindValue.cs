namespace Honor.Runtime
{
    /// <summary>
    /// Lua绑定数据
    /// 对于自定义的非Unity对象如果需要序列化处理则必须添加[System.Serializable]标签
    /// </summary>
    [System.Serializable]
    public class LuaBindValue
    {
        /// <summary>
        /// 绑定数据类型定义
        /// </summary>
        public enum BindValueType
        {
            Int32           =       0,
            Float           =       1,
            String          =       2,
            Boolean         =       3,
            Table           =       4,
            Array           =       5,
            Any             =       6,
            Trigger         =       7,
        }

        /// <summary>
        /// 绑定数据类型到Lua层的映射类型
        /// </summary>
        public static string[] LuaBindValueType = {
            "number",
            "number",
            "string",
            "boolean",
            "table",
            "array",
            "any",
            "trigger",
        };

        /// <summary>
        /// 绑定数据的注释信息
        /// </summary>
        public string Comment;

        /// <summary>
        /// 绑定数据的类型
        /// </summary>
        public BindValueType BindValueTypeName;

        /// <summary>
        /// 绑定数据的名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 绑定数据变量值（基础类型时有效）
        /// </summary>
        public string Variant;

        /// <summary>
        /// 绑定数据绑定到的注入信息名称集合
        /// 格式：xxx,yyy,zzz,....
        /// </summary>
        public string OnInjections;

    }

}


