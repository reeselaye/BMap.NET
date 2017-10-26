using NUnit.Framework;
using BMap.NET.HTTPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BMap.NET.HTTPService.Tests {
    [TestFixture()]
    public class CoordinateTransServiceTests {
        [Test()]
        public void CoordinateTransformTest() {
            CoordinateTransService service = new CoordinateTransService();
            JObject jObject = service.CoordinateTransform("108.12345643267", "22.52346320", 1, 5);
            Assert.IsNotNull(jObject);
        }
    }
}