using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsServices
{
    /// <summary>
    /// Json相关扩展方法。
    /// 
    /// 修改纪录
    /// 
    /// 2018-03-03 版本：1.2 liuzhaozhao 创建文件。
    /// 
    /// <author>
    ///     <name>liuzhaozhao</name>
    ///     <date>2018-03-03</date>
    /// </author>
    /// </summary>
    public static class NumericConversion
    {
        /// <summary>
        /// decimal 剩余两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="len"></param>
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
    }
}
