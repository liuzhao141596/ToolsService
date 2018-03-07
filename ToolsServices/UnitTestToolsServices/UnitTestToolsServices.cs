using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsServices;

namespace UnitTestToolsServices
{
    [TestClass]
    public class UnitTestToolsServices
    {
        [TestMethod]
        public void GetLeftStringTest()
        {
            string leftString = StringManage.GetLeftString("kjshdfuiyh223的客服接口", 10);
            string RightString = StringManage.GetRightString("kjshdfuiyh223的客服接口", 10);
        }
    }
}
