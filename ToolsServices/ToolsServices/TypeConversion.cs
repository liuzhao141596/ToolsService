using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsServices
{
    /// <summary>
    /// 类型转换相关扩展方法。
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

   public static class TypeConversion
    {
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
        /// string类型数组 转化为int类型数组 不能够正确转换的赋0
        /// </summary>
        /// <param name="beChangeArray"></param>
        /// <returns></returns>
        public static int[] ChangeStringArrayToIntArray(string[] beChangeArray)
        {
            int[] numArray = new int[beChangeArray.Length];
            int index = 0;
            foreach (string str in beChangeArray)
            {
                numArray[index] = 0;
                int result = 0;
                if (int.TryParse(str, out result))
                {
                    numArray[index] = result;
                }
                index++;
            }
            return numArray;
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
            foreach (string str in beChangeArray)
            {
                numArray[index] = 0M;
                decimal result = 0M;
                if (decimal.TryParse(str, out result))
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
            const string dic = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
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
                    s += j.ToString(CultureInfo.InvariantCulture);
                else
                    s += Convert.ToChar(j - 10 + 'a');
                number = number / 36;
            }
            if (number <= 9)
                s += number.ToString(CultureInfo.InvariantCulture);
            else
                s += Convert.ToChar(number - 10 + 'a');
            Char[] c = s.ToCharArray();
            Array.Reverse(c);
            return new string(c);
        }
    }
}
