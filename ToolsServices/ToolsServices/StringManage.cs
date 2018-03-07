using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolsServices
{
    /// <summary>
    /// Json相关扩展方法。
    /// 
    /// 修改纪录
    /// 
    /// 2018-03-07 版本：1.2 liuzhaozhao 创建文件。
    /// 
    /// <author>
    ///     <name>liuzhaozhao</name>
    ///     <date>2018-03-07</date>
    /// </author>
    /// </summary>
    public static class StringManage
    {
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
        /// 移除Html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string str)
        {
            Regex regex = new Regex(@"<\/*[^<>]*>");
            return regex.Replace(str, string.Empty);
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
    }
}
