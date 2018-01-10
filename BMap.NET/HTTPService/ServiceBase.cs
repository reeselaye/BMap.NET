using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace BMap.NET.HTTPService
{
    class WebClientWithTimeout : WebClient {
        private int _timeout = 5000;
        public int Timeout {
            set {
                _timeout = value;
            }
            get {
                return _timeout;
            }
        }

        protected override WebRequest GetWebRequest(Uri address) {
            WebRequest request = base.GetWebRequest(address);
            request.Timeout = Timeout;
            if(request is HttpWebRequest) {
                ((HttpWebRequest)request).ReadWriteTimeout = Timeout;
            }
            return request;
        }
    }
    /// <summary>
    /// 服务基类
    /// </summary>
    public class ServiceBase
    {
        protected static string _ak = Properties.BMap.Default.ServiceAK;  //AK
        protected static string _sk = Properties.BMap.Default.ServiceSK;  //SK
        protected static VerificationMode _vm = (VerificationMode)Properties.BMap.Default.VerificationMode;  //校验方式 0表示IP白名单校验（忽略SK）   1表示SN校验（需要SK）

        private WebClient wc;

        private static int stopAfterExceptionCount = 10;
        public static int MaxAllowedExceptionCount {
            set {
                stopAfterExceptionCount = value;
            }
        }
        private static int exceptionCounter = 0;
        public static void NoticeInternetConnected() {
            exceptionCounter = 0;
        }

        public ServiceBase() {
            wc = new WebClientWithTimeout();
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            wc.Proxy = null;
        }

        /// <summary>
        /// 从服务器上下载字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string DownloadString(string url)
        {
            if (exceptionCounter > stopAfterExceptionCount) {
                return null;
            }
            try
            {
                string str = wc.DownloadString(url);
                return str;
            }
            catch
            {
                exceptionCounter++;
                return null;
            }
        }
        /// <summary>
        /// 从服务器上下载字节流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public byte[] DownloadData(string url)
        {
            if (exceptionCounter > stopAfterExceptionCount) {
                return null;
            }
            try
            {
                byte[] data = wc.DownloadData(url);
                return data;
            }
            catch
            {
                exceptionCounter++;
                return null;
            }
        }
    }
}
