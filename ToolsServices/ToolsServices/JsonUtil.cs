using System.Collections.Generic;
using System.Text;
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
        static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();
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
                result = Serializer.Serialize(obj);
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
            return Serializer.Deserialize<T>(json);
        }
        #endregion


        /// <summary>
        /// 集合Json字符串转化为字符串集合
        /// </summary>
        /// <param name="rawjson"></param>
        /// <returns></returns>
        public static List<string> JsonStringCollection(string rawjson)
        {
            var list = new List<string>();

            if (string.IsNullOrEmpty(rawjson))
                return list;

            rawjson = rawjson.Trim();
            var builder = new StringBuilder();
            int nestlevel = -1;
            int mnestlevel = -1;
            for (int i = 0; i < rawjson.Length; i++)
            {
                if (i == 0)
                    continue;
                if (i == rawjson.Length - 1)
                    continue;
                char jsonchar = rawjson[i];
                if (jsonchar == '{')
                    nestlevel++;
                if (jsonchar == '}')
                    nestlevel--;
                if (jsonchar == '[')
                    mnestlevel++;
                if (jsonchar == ']')
                    mnestlevel--;
                if (jsonchar == ',' && nestlevel == -1 && mnestlevel == -1)
                {
                    list.Add(builder.ToString());
                    builder = new StringBuilder();
                }
                else
                    builder.Append(jsonchar);
            }

            if (builder.Length > 0)
                list.Add(builder.ToString());

            return list;
        }
    }
}
