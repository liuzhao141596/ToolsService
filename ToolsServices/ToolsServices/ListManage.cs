using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsServices
{
    /// <summary>
    /// List相关扩展方法。
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
    public static  class ListManage
    {
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
    }
}
