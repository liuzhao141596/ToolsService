using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolsServices
{
    /// <summary>
    /// 正则匹配相关扩展方法。
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
    public static class RegularManage
    {
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
    }
}
