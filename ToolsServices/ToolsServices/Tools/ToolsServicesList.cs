//************************************************************************
// 系统名				通用方法
// 作者				    liuzhaozhao
// 创建日期				2016.5.24
//************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace ToolsServices.Tools
{
    /// <summary>
    /// 工具类 
    /// create by liuzhaozhao 2016.5.24
    /// </summary>
    public static class ToolsServicesList
    {

        #region 类型转换类

        /// <summary>
        /// 将携带泛型集合信息的Json字符串转化为泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<T> JsonStringToList<T>(string jsonString)
        {
            var js = new JavaScriptSerializer();
            var list = js.Deserialize<List<T>>(jsonString);
            return list;
        }

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
        /// <summary>    
        /// 将集合类转换成DataTable    
        /// </summary>    
        /// <param name="list">集合</param>    
        /// <returns></returns>    
        public static DataTable ListToDataTable(IList list)
        {
            var result = new DataTable();
            if (list.Count <= 0) return result;
            var propertys = list[0].GetType().GetProperties();

            foreach (var pi in propertys)
            {
                result.Columns.Add(pi.Name, pi.PropertyType);
            }
            foreach (var t in list)
            {
                var tempList = new ArrayList();
                foreach (var pi in propertys)
                {
                    var obj = pi.GetValue(t, null);
                    tempList.Add(obj);
                }
                var array = tempList.ToArray();
                result.LoadDataRow(array, true);
            }
            return result;
        }

        /// <summary>    
        /// DataTable 转换为List<T> 集合    
        /// </summary>    
        /// <param name="dt">DataTable</param>    
        /// <returns></returns>    
        public static List<T> DataTableToList<T>(this DataTable dt) where T : class, new()
        {
            //创建一个属性的列表    
            var prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口    
            Type t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表     
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合    
            var oblist = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例    
                T ob = new T();
                //找到对应的数据  并赋值    
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.    
                oblist.Add(ob);
            }
            return oblist;
        }
        /// <summary>
        /// int类型数组转换为string类型的数组
        /// </summary>
        /// <param name="beChangeArray"></param>
        /// <returns></returns>
        public static string[] ChangeIntArrayToStringArray(int[] beChangeArray)
        {
            string[] strArray = new string[beChangeArray.Length];
            int index = 0;
            foreach (int num2 in beChangeArray)
            {
                strArray[index] = num2.ToString();
                index++;
            }
            return strArray;
        }
        /// <summary>
        /// string类型数组转换为decimal类型数组
        /// </summary>
        /// <param name="beChangeArray"></param>
        /// <returns></returns>
        public static decimal[] ChangeStringArrayToDecimalArray(string[] beChangeArray)
        {
            decimal[] numArray = new decimal[beChangeArray.Length];
            int index = 0;
            decimal result = 0M;
            foreach (string str in beChangeArray)
            {
                numArray[index] = 0M;
                result = 0M;
                if (decimal.TryParse(str, out result))
                {
                    numArray[index] = result;
                }
                index++;
            }
            return numArray;
        }
        /// <summary>
        /// string类型数组 转化为int类型数组 不能够正确转换的赋0
        /// </summary>
        /// <param name="beChangeArray"></param>
        /// <returns></returns>
        public static int[] ChangeStringArrayToIntArray(string[] beChangeArray)
        {
            int[] numArray = new int[beChangeArray.Length];
            int index = 0;
            int result = 0;
            foreach (string str in beChangeArray)
            {
                numArray[index] = 0;
                result = 0;
                if (int.TryParse(str, out result))
                {
                    numArray[index] = result;
                }
                index++;
            }
            return numArray;
        }
        /// <summary>
        /// 36进制转换为10进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Convert36To10(string value)
        {
            string dic = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = value.Aggregate(0L, (sum, c) => sum * 36 + dic.IndexOf(c));
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// 10进制转换为36进制
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConvertTo36(int number)
        {
            string s = "";
            int j = 0;
            while (number > 36)
            {
                j = number % 36;
                if (j <= 9)
                    s += j.ToString();
                else
                    s += Convert.ToChar(j - 10 + 'a');
                number = number / 36;
            }
            if (number <= 9)
                s += number.ToString();
            else
                s += Convert.ToChar(number - 10 + 'a');
            Char[] c = s.ToCharArray();
            Array.Reverse(c);
            return new string(c);
        }
        #endregion

        #region 加密类

        #endregion

        #region 数值转换类
        /// <summary>
        /// decimal 剩余两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? Round(this decimal? value, int len)
        {
            if (value.HasValue)
            {
                return Decimal.Round(value.Value, len);
            }
            return null;
        }

        /// <summary>
        /// 截取小数点后位数 无四舍五入
        /// </summary>
        /// <param name="source">源数字</param>
        /// <param name="len">小数点位数</param>
        /// <returns></returns>
        public static string TruncateDecimal(this decimal source, int len)
        {

            string destination;
            int i = source.ToString(CultureInfo.InvariantCulture).IndexOf(".", StringComparison.Ordinal);
            if (i < 0)
            {
                destination = source.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                destination = source.ToString(CultureInfo.InvariantCulture).Length >= (i + len + 1)
                    ? source.ToString(CultureInfo.InvariantCulture).Substring(0, i + len + 1)
                    : source.ToString(CultureInfo.InvariantCulture);
            }
            return destination;

        }

        /// <summary>
        /// 截取小数点后位数 无四舍五入
        /// </summary>
        /// <param name="source">源数字</param>
        /// <param name="len">小数点位数</param>
        /// <returns></returns>
        public static string TruncateDecimal(this decimal? source, int len)
        {
            return !source.HasValue ? string.Empty : TruncateDecimal(source.Value, len);
        }

        #endregion

        #region 字符串处理类
        /// <summary>
        /// 截取字符串左边指定位数，字符串长度小于截取位数时返回字符串本身
        /// </summary>
        /// <param name="description"></param>
        /// <param name="leftLength"></param>
        /// <returns></returns>
        public static string GetLeftString(string description, int leftLength)
        {
            if (!string.IsNullOrEmpty(description) && (description.Length > leftLength))
            {
                return description.Substring(0, leftLength);
            }
            return description;
        }
        /// <summary>
        /// 截取字符串右边指定位数，字符串长度小于截取位数时返回字符串本身
        /// </summary>
        /// <param name="description"></param>
        /// <param name="rightLength"></param>
        /// <returns></returns>
        public static string GetRightString(string description, int rightLength)
        {
            if (!string.IsNullOrEmpty(description) && (description.Length > rightLength))
            {
                return description.Substring(description.Length - rightLength);
            }
            return description;
        }
        /// <summary>
        /// 产生随机数 随机数包含数字和大小写的英文字母
        /// </summary>
        /// <param name="strLength">随机数的长度</param>
        /// <returns></returns>
        public static string GetRandomStr(int strLength)
        {
            return GetRandomStr(strLength, 0);
        }
        public static string GetRandomStr(int strLength, int randomSeed)
        {
            string[] strArray = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(new char[] { ',' });
            Random random = null;
            if (randomSeed > 0)
            {
                random = new Random(randomSeed);
            }
            else
            {
                random = new Random();
            }
            string str2 = "";
            while (str2.Length < strLength)
            {
                str2 = str2 + strArray[random.Next(strArray.Length)];
            }
            return str2;
        }
        /// <summary>
        /// 判断是否包含 "<'或者">"字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckHtml(string str)
        {
            bool rtn = str.IndexOf('<') > 0 || str.IndexOf('>') > 0;

            return rtn;
        }
        /// <summary>
        /// 判断字符串中是否有包含列表的字符串
        /// </summary>
        /// <param name="str">待验证字符串</param>
        /// <param name="taget">字符串列表</param>
        /// <returns>如果有包含返回真，否则返回假</returns>
        public static bool IsContains(this string str, List<string> taget)
        {
            const bool result = false;
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }

            return taget.Any(str.Contains);
        }
        /// <summary>
        /// 判断字符串是否存在于字符串列表中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="taget"></param>
        /// <returns></returns>
        public static bool IsEquals(this string str, List<string> taget)
        {
            const bool result = false;
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }
            //精确匹配
            return taget.Any(s => str.Trim().Equals(s));
        }
        #endregion

        #region List集合处理类
        /// <summary>
        /// 移除List<typeparamref name="T"/>中的重复实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparison">自定义重复实体的具体含义</param>
        public static void RemoveRepeatEntity<T>(List<T> list, Comparison<T> comparison)
        {
            if (list != null & list.Count > 0)
            {
                if (comparison != null)
                {
                    list.Sort(comparison);
                }
                else
                {
                    if (list[0] is IComparable<T>)
                    {
                        list.Sort();
                    }
                    else
                    {
                        return;
                    }
                }

                T t = list[list.Count - 1];
                for (int i = list.Count - 2; i >= 0; i--)
                {
                    if (comparison(t, list[i]) == 0)
                    {
                        list.RemoveAt(i);
                    }
                    else
                    {
                        t = list[i];
                    }
                }
            }
        }
        #endregion

        #region 序列化
        public static string ToJsonString(this object serialObject)
        {
            return JsonSerialize(serialObject);
        }

        public static string JsonSerialize(object serialObject)
        {
            if (serialObject == null)
            {
                return string.Empty;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(serialObject.GetType());
                serializer.WriteObject(stream, serialObject);
                byte[] s = stream.ToArray();
                return Encoding.UTF8.GetString(s, 0, s.Length);
            }
        }

        public static object JsonDeserialize(string str, Type type)
        {
            if (str == null || str.Trim().Length <= 0)
            {
                return null;
            }
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
                return serializer.ReadObject(stream);
            }
        }

        public static T JsonDeserialize<T>(string str)
        {
            return (T)JsonDeserialize(str, typeof(T));
        }
        #endregion

        #region 正则匹配类
        /// <summary>
        /// 正则匹配手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseMobileNumber(string input)
        {
            return Regex.IsMatch(input, "^(13[0-9]|15[0|3|6|8|9])[0-9]{8}$");
        }
        /// <summary>
        /// 正则匹配电话
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChinesePhoneNumber(string input)
        {
            return Regex.IsMatch(input, @"^[0-9\-]{0,20}$");
        }
        /// <summary>
        /// 是否是decimal类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDecimal(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, @"^\d+[.]?\d*$");
        }
        /// <summary>
        /// 是否是邮箱地址格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmailAddress(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        /// <summary>
        /// 是否是Int类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInteger(string input)
        {
            int num;
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return int.TryParse(input, out num);
        }
        /// <summary>
        /// 是否是Ipv4 地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIpv4Address(string input)
        {
            return Regex.IsMatch(input, @"^((?:2[0-5]{2}|1\d{2}|[1-9]\d|[1-9])\.(?:(?:2[0-5]{2}|1\d{2}|[1-9]\d|\d)\.){2}(?:2[0-5]{2}|1\d{2}|[1-9]\d|\d)):(\d|[1-9]\d|[1-9]\d{2,3}|[1-5]\d{4}|6[0-4]\d{3}|654\d{2}|655[0-2]\d|6553[0-5])$");
        }
        /// <summary>
        /// 是否是Ipv6地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIpv6Address(string input)
        {
            return Regex.IsMatch(input, "^([0-9A-Fa-f]{1,4}:){7}[0-9A-Fa-f]{1,4}$");
        }
        /// <summary>
        /// 是否是金额类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMoney(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, "^([0-9]+|[0-9]{1,3}(,[0-9]{3})*)(.[0-9]{1,2})?$");
        }
        /// <summary>
        /// 是否是Url 地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, @"(mailto\:|(news|(ht|f)tp(s?))\://)(([^[:space:]]+)|([^[:space:]]+)( #([^#]+)#)?) ");
        }
        /// <summary>
        /// 移除Html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string str)
        {
            Regex regex = new Regex(@"<\/*[^<>]*>");
            return regex.Replace(str, string.Empty);
        }

        #endregion




    }

}
