
using System;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/08/03 20:50:17	
    // - Description: 定时器数据结构
    //==========================
    public class TimerData
    {
        //执行间隔 秒
        public float Delay;
        //持有者id
        public int TargetId;
        //完成回调
        public Action Callback;
        //完成回调 回参
        public Action<object> Callback1;
        //是否循环
        public bool Loop;
        //回调参数
        public object Param;

        //随着时间而变化
        public float CurrTime;

        public void ResetData()
        {
            Delay = 0.0f;
            TargetId = 0;
            Callback = null;
            Callback1 = null;
            Loop = false;
            CurrTime = 0.0f;
            Param = null;
        }

        public void ResetData(float d1 = 0, int d2 = 0, Action d3 = null, bool d4 = false, object d5 = null)
        {
            Delay = d1;
            TargetId = d2;
            Callback = d3;
            Loop = d4;
            CurrTime = Delay;
            Param = d5;
        }

        public void ResetData(float d1 = 0, int d2 = 0, Action<object> d3 = null, bool d4 = false, object d5 = null)
        {
            Delay = d1;
            TargetId = d2;
            Callback1 = d3;
            Loop = d4;
            CurrTime = Delay;
            Param = d5;
        }

    }

}