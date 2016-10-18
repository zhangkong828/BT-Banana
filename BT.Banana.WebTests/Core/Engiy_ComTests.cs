using Microsoft.VisualStudio.TestTools.UnitTesting;
using BT.Banana.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Banana.Web.Core.Tests
{
    [TestClass()]
    public class Engiy_ComTests
    {
        [TestMethod()]
        public void SearchTest()
        {
            var result = Engiy_Com.Search("我", 1);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void GetDetialTest()
        {
            var result = Engiy_Com.GetDetial("AVTUs7C7GP6CfE_FlZIv");
            Assert.IsNotNull(result);
        }
    }
}