using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/08/02 15:05:27		
    // - Description: 发布/订阅
    //==========================
    public class PubSubManager : SingletonComponent<PubSubManager>
    {
        //订阅消息map
        private Dictionary<string, List<SubscribeData>> MsgMap = new Dictionary<string, List<SubscribeData>>();
        //订阅者的id对应的订阅消息列表
        private Dictionary<int, List<string>> IdToMsgs = new Dictionary<int, List<string>>();
        //数据队列
        private MyQueue<SubscribeData> SDQueue = new MyQueue<SubscribeData>();
      
        /// <summary>
        /// 回调队列(对象池)
        /// </summary>
        private MyQueue<CallbackSubribeData> callbackQueuePool = new MyQueue<CallbackSubribeData>();
        /// <summary>
        /// 等待主线程回调数据列表 
        /// </summary>
        private List<CallbackSubribeData> waitCallbackList = new List<CallbackSubribeData>();

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="msgId">消息</param>
        /// <param name="ownerId">持有者id</param>
        /// <param name="callback">回调方法</param>
        public void Register(string msgId, int ownerId, Action<object> callback)
        {
            SubscribeData temp = SDQueue.Dequeue();
            temp.ResetData(msgId, ownerId, callback);
            AddSub(temp);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="msgId">消息</param>
        /// <param name="ownerId">持有者id</param>
        /// <param name="callback">回调方法</param>
        public void Register(string msgId, int ownerId, Action callback)
        {
            SubscribeData temp = SDQueue.Dequeue();
            temp.ResetData(msgId, ownerId, callback);
            AddSub(temp);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="msgId">消息id</param>
        /// <param name="param">透传参数</param>
        public void Publish(string msgId, object param = null)
        {
            waitCallbackList.Add(callbackQueuePool.Dequeue().Init(msgId, param));
        }

        /// <summary>
        /// 删除全部目标id的所有消息订阅
        /// </summary>
        /// <param name="ownerId"></param>
        public void unRegister(int ownerId)
        {
            if (IdToMsgs.ContainsKey(ownerId))
            {
                List<string> msgIds = IdToMsgs[ownerId];
                for (int i = 0; i < msgIds.Count; i++)
                {
                    unRegister(msgIds[i], ownerId);
                }
                msgIds.Clear();
                IdToMsgs.Remove(ownerId);
            }
        }

        /// <summary>
        /// 删除目标id的单个消息订阅
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="ownerId"></param>
        public void unRegister(string msgId, int ownerId)
        {
            List<SubscribeData> temps;
            SubscribeData temp;
            if (MsgMap.ContainsKey(msgId))
            {
                temps = MsgMap[msgId];
                for (int i = 0; i < temps.Count; i++)
                {
                    temp = temps[i];
                    if (temp.OwnerId == ownerId)
                    {
                        temps.Remove(temp);
                        temp.ResetData();
                        SDQueue.Enqueue(temp);
                    }
                }
            }
        }
      

        private void AddSub(SubscribeData temp)
        {
            List<SubscribeData> temps;
            if (!MsgMap.ContainsKey(temp.MessageId))
            {
                temps = new List<SubscribeData>();
                temps.Add(temp);
                MsgMap.Add(temp.MessageId, temps);
            }
            else
            {
                temps = MsgMap[temp.MessageId];
                SubscribeData oldTemp;
                for (int i = 0; i < temps.Count; i++)
                {
                    oldTemp = temps[i];
                    /// 是否有相同消息 id targetid 和cb
                    if (temp.MessageId == oldTemp.MessageId && temp.OwnerId == oldTemp.OwnerId && (temp.CallbackParam == oldTemp.CallbackParam || temp.Callback == oldTemp.Callback))
                    {
                        Debug.LogError("相同订阅=====" + temp.MessageId);
                        temp.ResetData();
                        SDQueue.Enqueue(temp);
                        return;
                    }
                }


                temps.Add(temp);
            }
            List<string> msgs;
            if (!IdToMsgs.ContainsKey(temp.OwnerId))
            {
                msgs = new List<string>();
                msgs.Add(temp.MessageId);
                IdToMsgs.Add(temp.OwnerId, msgs);
            }
            else
            {
                msgs = IdToMsgs[temp.OwnerId];
                msgs.Add(temp.MessageId);
            }
        }


        void Update()
        {
            if (waitCallbackList.Count < 1)
            {
                return;
            }
            while (waitCallbackList.Count > 0)
            {
                Notice(waitCallbackList[0]);
                waitCallbackList.RemoveAt(0);
            }
        }

        //主线程调用
        private void Notice(CallbackSubribeData data)
        {
            if (MsgMap.ContainsKey(data.MessageId))
            {
                List<SubscribeData> temps = MsgMap[data.MessageId];
                SubscribeData temp;
                if (temps != null && temps.Count > 0)
                {
                    for (int i = 0; i < temps.Count; i++)
                    {
                        temp = temps[i];
                        if(temp.CallbackParam != null)
                        {
                            temp.CallbackParam(data.Param);
                        }
                        else
                        {
                            temp.Callback();
                        }
                    }
                }
            }
            callbackQueuePool.Enqueue(data);
        }
    }
}