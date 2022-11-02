
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
        public string MsgId;
        public int TargetId;
        public Action<object> Callback0;
        public Action Callback1;
        public bool Once;

        public object Param;

        public void ResetData()
        {
            MsgId = null;
            TargetId = -1;
            Callback0 = null;
            Callback1 = null;
            Once = false;
            Param = null;
        }

        public void ResetData(string d1 = null, int d2 = 0, Action<object> d3 = null, bool d4 = false)
        {
            MsgId = d1;
            TargetId = d2;
            Callback0 = d3;
            Once = d4;
            Param = null;
        }
        public void ResetData(string d1 = null, int d2 = 0, Action d3 = null, bool d4 = false)
        {
            MsgId = d1;
            TargetId = d2;
            Callback1 = d3;
            Once = d4;
            Param = null;
        }
    }
}