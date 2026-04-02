using System;

namespace Honor.Runtime
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GameHiddenPropertiesAttribute : Attribute
    {
        public string[] PropertiesNames;

        public GameHiddenPropertiesAttribute(params string[] propertiesNames)
        {
            PropertiesNames = propertiesNames;
        }
    }
}
