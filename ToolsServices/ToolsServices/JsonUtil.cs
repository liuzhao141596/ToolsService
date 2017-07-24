using System.Web.Script.Serialization;

namespace ToolsServices
{
    /// <summary>
    /// Json相关扩展方法。
    /// 
    /// 修改纪录
    /// 
    /// 2017-07-24 版本：1.0 liuzhaozhao 创建文件。
    /// 
    /// <author>
    ///     <name>liuzhaozhao</name>
    ///     <date>2017-07-24</date>
    /// </author>
    /// </summary>

    public static class JsonUtil
    {
        static JavaScriptSerializer serializer = new JavaScriptSerializer();
        #region Json扩展
        /// <summary>
        /// 将对象转换为Json字串。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象实例。</param>
        /// <returns>Json字串</returns>
        public static string ToJson<T>(this T obj)
        {
            string result = "";

            if (obj is string)
            {
                result = obj.ToString();
            }
            else
            {
                //StringExtensions.ToJson(obj); 这个不可以 解析嵌套类
                result = serializer.Serialize(obj);
            }

            return result;
        }

        /// <summary>
        /// 将Json字符串转换为Json对象。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">Json字符串。</param>
        /// <returns>Json对象</returns>
        public static T FromJson<T>(this string json)
        {
            //return StringExtensions.FromJson<T>(json);  这个不可以 解析嵌套类
            return serializer.Deserialize<T>(json);
        }
        #endregion

    }
}
