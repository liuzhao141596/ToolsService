using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ToolsServices.Tools;

namespace ToolsServices
{
    class Program
    {
        static void Main(string[] args)
        {
            //            string test = "{'dataCode':1,'Message':'成功','houserInfo':'teygt'}";

            //List<string> ss=         ToolsServicesList.JsonStringCollection(test);

            // decimal? dd =(decimal?) 487.348738;
            // decimal? dds = null;
            //var ss= ToolsServicesList.TruncateDecimal(dd,2);
            //var s = ToolsServicesList.TruncateDecimal(dds, 2);
            // var rsu = ToolsServicesList.Round(dd);
            //string[] test = new string[] { "12", "34", "er", "3" };
            //int[] ss = ToolsServicesList.ChangeStringArrayToIntArray(test);

            //string ss = "eriuidjhfgj";
            //string resule=ToolsServicesList.GetRightString(ss, 4);

            //string ss= ToolsServicesList.GetRandomStr(4);
            //string s = ToolsServicesList.GetRandomStr(4,2);

            List<Person> personList = new List<Person>()
            {
                new Person{Id =1,Name="张三",Age=18},
                new Person{Id =2,Name="李四",Age=20},
            new Person{Id =1,Name="张三",Age=18}
            };
             
            //Comparison<Person> comparison=new Comparison<Person>{}
            ToolsServicesList.RemoveRepeatEntity<Person>(personList, null);

        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
