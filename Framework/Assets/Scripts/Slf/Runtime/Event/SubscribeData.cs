
using System;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/08/02 15:05:27		
    // - Description: 订阅数据结构
    //==========================
    public class SubscribeData
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public string MessageId;
        /// <summary>
        /// 持有者id
        /// </summary>
        public int OwnerId;
        /// <summary>
        /// 带参 回调
        /// </summary>
        public Action<object> CallbackParam;
        /// <summary>
        /// 无参 回调
        /// </summary>
        public Action Callback;

        public void ResetData()
        {
            MessageId = null;
            OwnerId = -1;
            CallbackParam = null;
            Callback = null;
        }

        public void ResetData(string d1 = null, int d2 = 0, Action<object> d3 = null)
        {
            MessageId = d1;
            OwnerId = d2;
            CallbackParam = d3;
        }
        public void ResetData(string d1 = null, int d2 = 0, Action d3 = null)
        {
            MessageId = d1;
            OwnerId = d2;
            Callback = d3;
        }
    }

    /// <summary>
    /// 发布回调数据结构 减少GC
    /// </summary>
    public class CallbackSubribeData
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public string MessageId;
        /// <summary>
        /// 回调数据
        /// </summary>
        public object Param;

        public CallbackSubribeData Init(string id, object param)
        {
            MessageId = id;
            Param = param;
            return this;
        }
    }
}