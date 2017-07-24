using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ToolsServices
{
    public static class ListDataTable
    {
        /// <summary>    
        /// 将集合类转换成DataTable    
        /// </summary>    
        /// <param name="list">集合</param>    
        /// <returns></returns>    
        public static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.PropertyType.Name != typeof(Nullable).Name && pi.PropertyType.Name != "Nullable`1")
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }
                foreach (object t in list)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (pi.PropertyType.Name != typeof(Nullable).Name && pi.PropertyType.Name != "Nullable`1")
                        {
                            object obj = pi.GetValue(t, null);
                            tempList.Add(obj);
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>    
        /// DataTable 转换为List 集合    
        /// </summary>    
        /// <typeparam name="TResult">类型</typeparam>    
        /// <param name="dt">DataTable</param>    
        /// <returns></returns>    
        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            //创建一个属性的列表    
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口    

            Type t = typeof(T);



            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表     
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });

            //创建返回的集合    

            List<T> oblist = new List<T>();

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
        /// datatable表转list集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt">datatable表</param>
        /// <param name="rowStart">表的第几行开始计算</param>
        /// <returns>集合List</returns>
        public static List<T> DataTableToList<T>(System.Data.DataTable dt, int rowStart) where T : new()
        {
            List<T> lists = null;
            if (dt.Rows.Count > 0)
            {
                lists = new List<T>();//定义集合
                string tempName = string.Empty;
                for (int i = rowStart; i < dt.Rows.Count; i++)
                {
                    DataRow myRow = dt.Rows[i];//获取当前行
                    T t = new T();
                    #region 反射入List
                    Type type = t.GetType();
                    foreach (var pi in type.GetProperties())//遍历公共属性
                    {
                        tempName = pi.Name;
                        if (dt.Columns.Contains(tempName))
                        {
                            if (!pi.CanWrite) continue;//该属性不可写，直接跳出
                            if (!pi.PropertyType.IsGenericType)//对属性中有DateTime进行区分对待
                            {
                                if (pi.Name == "Guid")//对属性为Guid进行特别处理
                                {
                                    Guid guid = Guid.Empty;
                                    if (!Guid.TryParse(myRow[tempName].ToString(), out guid)) { guid = Guid.NewGuid(); }
                                    pi.SetValue(t, Convert.ChangeType(guid, pi.PropertyType), null);
                                }
                                else
                                {
                                    pi.SetValue(t, Convert.ChangeType(myRow[tempName], pi.PropertyType), null);
                                }

                            }
                            else
                            {
                                pi.SetValue(t, Convert.ChangeType(myRow[tempName], Nullable.GetUnderlyingType(pi.PropertyType)), null);
                            }

                        }
                    #endregion
                    }
                    lists.Add(t);
                }
            }
            return lists;
        }


        #region DataTable转换成List
        /// <summary>
        /// DataTable转换成List
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dt">dataTable</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            var list = new List<T>();
            if (dt == null) return list;
            var len = dt.Rows.Count;

            for (var i = 0; i < len; i++)
            {
                var info = new T();
                foreach (DataColumn dc in dt.Rows[i].Table.Columns)
                {
                    var field = dc.ColumnName;
                    var value = dt.Rows[i][field].ToString();
                    if (string.IsNullOrEmpty(value)) continue;
                    if (IsDate(value))
                    {
                        value = DateTime.Parse(value).ToString();
                    }

                    var p = info.GetType().GetProperty(field);

                    try
                    {
                        if (p.PropertyType == typeof(Guid))
                        {
                            p.SetValue(info, new Guid(value), null);
                        }
                        else if (p.PropertyType == typeof(string))
                        {
                            if (value.Contains("\""))
                                value = value.Replace("\"", "");
                            p.SetValue(info, value, null);
                        }
                        else if (p.PropertyType == typeof(short))
                        {
                            p.SetValue(info, short.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(int))
                        {
                            p.SetValue(info, int.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(bool))
                        {
                            p.SetValue(info, bool.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                        {
                            if (!string.IsNullOrEmpty(value))
                                p.SetValue(info, DateTime.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(float))
                        {
                            p.SetValue(info, float.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(decimal))
                        {
                            p.SetValue(info, decimal.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(double))
                        {
                            p.SetValue(info, double.Parse(value), null);
                        }
                        //else if (p.PropertyType == typeof(Enum))
                        //{
                        //    p.SetValue(info, int.Parse(value), null);
                        //}
                        else
                        {
                            p.SetValue(info, int.Parse(value), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        //p.SetValue(info, ex.Message, null);
                    }
                }
                list.Add(info);
            }
            dt.Dispose();
            dt = null;
            return list;
        }

        public static T ToModel<T>(this DataTable dt) where T : new()
        {
            var info = new T();
            if (dt == null) return info;
            foreach (DataColumn dc in dt.Rows[0].Table.Columns)
            {
                var field = dc.ColumnName;
                var value = dt.Rows[0][field].ToString();
                if (string.IsNullOrEmpty(value)) continue;
                if (IsDate(value))
                {
                    value = DateTime.Parse(value).ToString();
                }

                var p = info.GetType().GetProperty(field);

                try
                {

                    if (p.PropertyType == typeof(string))
                    {
                        if (value.Contains("\""))
                            value = value.Replace("\"", "");
                        p.SetValue(info, value, null);
                    }
                    else if (p.PropertyType == typeof(Guid))
                    {
                        p.SetValue(info, new Guid(value), null);
                    }
                    else if (p.PropertyType == typeof(short))
                    {
                        p.SetValue(info, short.Parse(value), null);
                    }
                    else if (p.PropertyType == typeof(int))
                    {
                        p.SetValue(info, int.Parse(value), null);
                    }
                    else if (p.PropertyType == typeof(bool))
                    {
                        p.SetValue(info, bool.Parse(value), null);
                    }
                    else if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                    {
                        if (!string.IsNullOrEmpty(value))
                            p.SetValue(info, DateTime.Parse(value), null);
                    }
                    else if (p.PropertyType == typeof(float))
                    {
                        p.SetValue(info, float.Parse(value), null);
                    }
                    else if (p.PropertyType == typeof(decimal))
                    {
                        p.SetValue(info, decimal.Parse(value), null);
                    }
                    else if (p.PropertyType == typeof(double))
                    {
                        p.SetValue(info, double.Parse(value), null);
                    }
                    else
                    {
                        p.SetValue(info, value, null);
                    }
                }
                catch (Exception ex)
                {
                    //p.SetValue(info, ex.Message, null);
                }
            }

            dt.Dispose();
            dt = null;
            return info;
        }

        /// <summary>
        /// 按照属性顺序的列名集合
        /// </summary>
        public static IList<string> GetColumnNames(this DataTable dt)
        {
            DataColumnCollection dcc = dt.Columns;
            //由于集合中的元素是确定的，所以可以指定元素的个数，系统就不会分配多余的空间，效率会高点
            IList<string> list = new List<string>(dcc.Count);
            foreach (DataColumn dc in dcc)
            {
                list.Add(dc.ColumnName);
            }
            return list;
        }

        private static bool IsDate(string d)
        {
            DateTime d1;
            double d2;
            return !double.TryParse(d, out d2) && DateTime.TryParse(d, out d1);
        }
        #endregion
    }
}
