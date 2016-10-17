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
            var result = Engiy_Com.Search("我", "1");
            Assert.IsNotNull(result);
        }
    }
}