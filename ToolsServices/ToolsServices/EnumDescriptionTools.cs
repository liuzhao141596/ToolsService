using System;
using System.ComponentModel;
using System.Reflection;

namespace ToolsServices
{
    /// <summary>
    /// EnumDescriptionTools 获取枚举描述
    /// 
    /// 修改纪录
    /// 
    /// 2017.07.05  版本:1.0  create by liuzhaozhao 
    /// 
    /// <author>
    ///     <name>liuzhaozhao</name>
    ///     <date>2017-07-05</date>
    /// </author>
    /// </summary>
    public static class EnumDescriptionTools
    {
        public static string GetDescription(System.Enum value)
        {
            Type enumType = value.GetType();
            // 获取枚举常数名称。
            string name = System.Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}
