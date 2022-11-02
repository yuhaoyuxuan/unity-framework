using UnityEngine;
using System.Collections;
using System;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2022/08/09 17:02:23
    // - Description: 日期扩展
    //==========================
    public static class DateExtension
    {
        /// <summary>
        /// 当前时间戳 毫秒
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long TimeStamp(this object obj)
        {
            TimeSpan ts = System.DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (long)ts.TotalMilliseconds;
        }


        /// <summary>
        /// 获取datetime实例
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime DateTime(this long timeStamp)
        {
            DateTime st = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0), TimeZoneInfo.Local);
            DateTime dt = st.AddMilliseconds(timeStamp);
            return dt;
        }

        /// <summary>
        /// 时间戳转换 年月日
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static string StampToYMDcn(this long timeStamp)
        {
            return timeStamp.DateTime().ToString("yyyy年MM月dd日");
        }


        /// <summary>
        /// 时间戳转换 yyyy/MM/dd
        /// </summary>
        public static string StampToYMD(this long timeStamp)
        {
            return timeStamp.DateTime().ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// 时间戳转换 年月日 时:分:秒
        /// </summary>
        public static string StampToYMDHMScn(this long timeStamp)
        {
            return timeStamp.DateTime().ToString("yyyy年MM月dd日 HH时mm分ss秒");
        }

        /// <summary>
        /// 时间戳转换 yyyy/MM/dd HH:mm:ss
        /// </summary>
        public static string StampToYMDHMS(this long timeStamp)
        {
            return timeStamp.DateTime().ToString("yyyy/MM/dd HH:mm:ss");
        }



        /// <summary>
        /// 根据时间戳返回时间格式
        /// </summary>
        /// <param name="time"></param>单位：毫秒
        /// <param name="index"></param> 返回string类型
        /// <returns></returns>
        public static string GetTimeStrType(long time, int index = 0)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            DateTime dt = startTime.AddMilliseconds(time);
            switch (index)
            {
                case 0:
                    return dt.ToString("MM/dd HH:mm");
                case 1:
                    return dt.ToString("yyyy/MM/dd HH:mm:ss");
                default:
                    break;
            }
            return "";
        }


        //一天的秒数
        static int oneMinuteS = 60;
        static int oneHourS = 60 * 60;
        static int oneDayS = 60 * 60 * 24;
        /// <summary>
        /// 秒 转换 日时分秒
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string SecoundToDHMS(this int second)
        {
            int day = second / oneDayS;
            int offset = second - day * oneDayS;
            int hour = offset / oneHourS;
            offset = offset - hour * oneHourS;
            int m = offset / oneMinuteS;
            int s = offset % oneMinuteS;


            string str = "";
            if (day != 0)
            {
                str += $"{day}天";
            }
            if (hour != 0)
            {
                str += $"{Pad(hour)}时";
            }
            if (m != 0)
            {
                str += $"{Pad(m)}分";
            }
            str += $"{Pad(second)}秒";

            return str;
        }



        /// <summary>
        /// 数字补0
        /// </summary>
        /// <param name="num"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Pad(int num, int len = 2)
        {
            string temp = num.ToString();
            int numLen = temp.Length;
            while (numLen < len)
            {
                temp = "0" + temp;
                numLen++;
            }
            return temp;
        }
    }
}