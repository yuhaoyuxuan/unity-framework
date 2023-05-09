
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
        /// <summary>
        /// 执行间隔 秒
        /// </summary>
        public float Delay;
        /// <summary>
        /// 持有者id 
        /// </summary>
        public int OwnerId;
        /// <summary>
        /// 回调
        /// </summary>
        public Action Callback;
        /// <summary>
        /// 带参回调
        /// </summary>
        public Action<object> CallbackParam;
        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop;
        /// <summary>
        /// 回调参数
        /// </summary>
        public object Param;

        //随着时间而变化
        public float CurrTime;

        public void ResetData()
        {
            Delay = 0.0f;
            OwnerId = 0;
            Callback = null;
            CallbackParam = null;
            Loop = false;
            CurrTime = 0.0f;
            Param = null;
        }

        public void ResetData(float d1 = 0, int d2 = 0, Action d3 = null, bool d4 = false, object d5 = null)
        {
            Delay = d1;
            OwnerId = d2;
            Callback = d3;
            Loop = d4;
            CurrTime = Delay;
            Param = d5;
        }

        public void ResetData(float d1 = 0, int d2 = 0, Action<object> d3 = null, bool d4 = false, object d5 = null)
        {
            Delay = d1;
            OwnerId = d2;
            CallbackParam = d3;
            Loop = d4;
            CurrTime = Delay;
            Param = d5;
        }

    }

}