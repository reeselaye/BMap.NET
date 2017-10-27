using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BMap.NET.HTTPService;

namespace BMap.NET.WindowsForm {
    public class CoordinateConverter {
        public static LatLngPoint WGS84Raw2Baidu(LatLngPoint wgs84raw) {
            return t(WGS84RawToWGS84Standard(wgs84raw), 1, 5);
        }

        public static LatLngPoint WGS84RawToWGS84Standard(LatLngPoint wgs84) {
            return new LatLngPoint(t2(wgs84.Lng), t2(wgs84.Lat));
        }

        private static LatLngPoint t(LatLngPoint p, int from, int to) {

            CoordinateTransService service = new CoordinateTransService();
            JObject result = service.CoordinateTransform((p.Lng).ToString(), (p.Lat).ToString(), from, to);
            if(result == null) {
                throw new ApplicationException("API 木有响应");
            }
            if((int)result["status"] != 0) {
                throw new ApplicationException((string)result["message"]);
            }
            return new LatLngPoint((double)result["result"][0]["x"], (double)result["result"][0]["y"]);
        }

        private static double t2(double d) {
            string s = d.ToString();
            if (!s.Contains(".")) {
                s += ".0";
            }
            string[] split = s.Split('.');
            if (split.Length != 2) {
                throw new ApplicationException("WGS84 坐标转换错误");
            }
            return double.Parse(split[0]) + double.Parse("0." + split[1])*100.0/60.0;
        }
    }
}
