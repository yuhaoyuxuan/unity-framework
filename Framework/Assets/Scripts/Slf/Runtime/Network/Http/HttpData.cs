
using System;

namespace Slf
{
    /// <summary>
    /// http通信数据
    /// </summary>
    public class HttpData
    {
        public string Url;
        public object Param;
        public string MethodType;
        public Action<string> Cb;
        public int Timeout;

        public void ResetData(string d1 = null, object d2 = null, string d3 = null, Action<string> d4 = null, int time = 5)
        {
            Url = d1;
            Param = d2;
            MethodType = d3;
            Cb = d4;
            Timeout = time;
        }
    }
}