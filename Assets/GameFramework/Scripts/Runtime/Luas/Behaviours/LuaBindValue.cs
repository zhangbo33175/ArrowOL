namespace Honor.Runtime
{
    /// <summary>
    /// Lua 绑定数据（序列化类，用于 Inspector 配置）
    /// 自定义序列化类必须添加 [System.Serializable]
    /// </summary>
    [System.Serializable]
    public class LuaBindValue
    {
        /// <summary>
        /// 绑定数据类型
        /// </summary>
        public enum BindValueType
        {
            Int32       = 0,
            Float       = 1,
            String      = 2,
            Boolean     = 3,
            Table       = 4,
            Array       = 5,
            Any         = 6,
            Trigger     = 7,
        }

        /// <summary>
        /// 类型对应的 Lua 类型字符串
        /// </summary>
        public static string[] LuaBindValueType =
        {
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
        /// 注释说明（仅编辑用）
        /// </summary>
        public string Comment;

        /// <summary>
        /// 绑定数据类型
        /// </summary>
        public BindValueType BindValueTypeName;

        /// <summary>
        /// 变量名称（Lua 层使用）
        /// </summary>
        public string Name;

        /// <summary>
        /// 变量值（基础类型使用字符串存储）
        /// </summary>
        public string Variant;

        /// <summary>
        /// 关联的注入节点名称（逗号分隔）
        /// 格式：xxx,yyy,zzz
        /// </summary>
        public string OnInjections;
    }
}