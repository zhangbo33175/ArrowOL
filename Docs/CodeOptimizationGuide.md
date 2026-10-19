# Honor.Runtime 代码优化规范

## 1. 文件头规范

每个C#文件开头必须包含版权头：

```csharp
/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  XxxYyy.cs
 * author:    云毅
 * created:   2026
 * descrip:   文件功能简要描述
 ***************************************************************/
```

---

## 2. 命名规范

### 2.1 类名/方法名/属性名（PascalCase）
```csharp
// ✅ 正确
public class ObjectTemplateManager
public void Initialize()
public string TemplateId { get; }

// ❌ 错误
public void initObjectTemplate()
public string templateID { get; }
```

### 2.2 私有字段（_camelCase）
```csharp
// ✅ 正确（商业规范：下划线开头+小驼峰）
private readonly Dictionary<string, ObjectTemplate> _templates;
private ObjectTemplateManager _instance;

// ❌ 错误（旧规范：m_前缀）
private Dictionary<string, ObjectTemplate> m_Templates;
private ObjectTemplateManager m_Instance;
```

### 2.3 常量（PascalCase）
```csharp
// ✅ 正确
public const string TemplateIdColumnName = "TemplateId";
private const int DefaultGridSize = 32;

// ❌ 错误
private const string kTemplateIdColumnName = "TemplateId";
```

### 2.4 方法参数/局部变量（camelCase）
```csharp
// ✅ 正确
public void LoadFromFile(string filePath)
{
    var templateList = new List<ObjectTemplate>();
}
```

---

## 3. 注释规范

### 3.1 类/方法/属性必须有XML注释
```csharp
/// <summary>
/// 物体模板管理器（单例模式）
/// 统一管理建筑、人物、物件三类地图元素模板
/// </summary>
public class ObjectTemplateManager
{
    /// <summary>
    /// 初始化模板管理器
    /// </summary>
    /// <param name="configPath">配置文件路径</param>
    public void Initialize(string configPath)
    {
    }

    /// <summary>
    /// 模板ID字典
    /// </summary>
    public Dictionary<string, ObjectTemplate> Templates { get; }
}
```

### 3.2 复杂逻辑必须有行内注释
```csharp
// 根据资源名判断物体类型：包含Building→建筑，包含Character→人物
string resName = GetStringValue(dataRow, "ResName");
if (resName.Contains("Building", StringComparison.OrdinalIgnoreCase))
    return MapObjectType.Building;
```

---

## 4. 文件结构规范

### 4.1 使用#region分区
```csharp
public class ExampleClass
{
    #region 常量
    private const int MaxCount = 100;
    #endregion

    #region 单例
    private static ExampleClass _instance;
    public static ExampleClass Instance { get; }
    #endregion

    #region 私有字段
    private string _name;
    #endregion

    #region 公共属性
    public string Name { get; set; }
    #endregion

    #region 构造函数
    public ExampleClass() { }
    #endregion

    #region 公共方法
    public void DoSomething() { }
    #endregion

    #region 私有方法
    private void InternalLogic() { }
    #endregion
}
```

### 4.2 partial类拆分规范
- 超过500行的类必须拆分为partial类
- 按功能拆分：XxxModule.cs（主类）、XxxModule.Methods.cs（方法）、XxxModule.Data.cs（数据）、XxxModule.UI.cs（UI相关）

---

## 5. 健壮性规范

### 5.1 空值检查
```csharp
public void AddTemplate(ObjectTemplate template)
{
    // ✅ 正确：先检查空值
    if (template == null)
    {
        Debug.LogWarning("[ObjectTemplateManager] 模板对象为空，跳过添加");
        return;
    }

    if (string.IsNullOrEmpty(template.TemplateId))
    {
        Debug.LogWarning("[ObjectTemplateManager] 模板ID不能为空");
        return;
    }

    _templates[template.TemplateId] = template;
}
```

### 5.2 异常处理
```csharp
public List<ObjectTemplate> LoadFromFile(string filePath)
{
    try
    {
        // 业务逻辑
    }
    catch (Exception e)
    {
        Debug.LogError($"[TemplateConfigReader] 读取Excel异常: {e.Message}");
        return new List<ObjectTemplate>();
    }
}
```

---

## 6. 代码简洁性规范

### 6.1 提取公共方法
- 重复出现3次以上的代码必须提取为公共方法
- 公共方法放到工具类（如MapToolEditorUtils）中

### 6.2 单个方法不超过50行
- 超过50行的方法必须拆分为多个小方法
- 每个方法只做一件事

---

## 7. 日志规范

### 7.1 统一日志前缀
```csharp
// ✅ 正确
Debug.Log($"[ObjectTemplateManager] 初始化完成，共加载 {count} 个模板");
Debug.LogWarning($"[ObjectTemplateManager] 模板不存在: {templateId}");
Debug.LogError($"[ObjectTemplateManager] 读取配置文件失败: {filePath}");
```

---

## 8. 优化优先级

1. **P0 - 核心框架**：Bases基础类、单例管理器、事件系统
2. **P1 - UI框架**：UIManager、UI基类、遮罩系统
3. **P2 - 工具类**：Utils目录下的所有工具类
4. **P3 - 业务模块**：MapTool地图编辑器、MapGrid地图网格
5. **P4 - Editor工具**：编辑器扩展工具
