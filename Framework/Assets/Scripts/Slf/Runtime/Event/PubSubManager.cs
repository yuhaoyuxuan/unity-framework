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
    public class PubSubManager : Singleton<PubSubManager>
    {
        //订阅消息map
        private Dictionary<string, List<SubscribeData>> MsgMap = new Dictionary<string, List<SubscribeData>>();
        //订阅者的id对应的订阅消息列表
        private Dictionary<int, List<string>> IdToMsgs = new Dictionary<int, List<string>>();
        //数据队列
        private MyQueue<SubscribeData> SDQueue = new MyQueue<SubscribeData>();
        /// <summary>
        /// 带回调的队列
        /// </summary>
        private List<object[]> cbList = new List<object[]>();

        public PubSubManager()
        {
            GameObject go = new GameObject("PubSubMono");
            go.AddComponent<PubSubMono>();
            GameObject.DontDestroyOnLoad(go);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="msgId">消息</param>
        /// <param name="targetId">持有者id</param>
        /// <param name="callback">回调方法</param>
        public void Register(string msgId, int targetId, Action<object> callback)
        {
            SubscribeData temp = SDQueue.Dequeue();
            temp.ResetData(msgId, targetId, callback);
            AddSub(temp);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="msgId">消息</param>
        /// <param name="targetId">持有者id</param>
        /// <param name="callback">回调方法</param>
        public void Register(string msgId, int targetId, Action callback)
        {
            SubscribeData temp = SDQueue.Dequeue();
            temp.ResetData(msgId, targetId, callback);
            AddSub(temp);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="msgId">消息id</param>
        /// <param name="param">透传参数</param>
        public void Publish(string msgId, object param = null)
        {
            cbList.Add(new object[] { msgId, param });
        }


        /// <summary>
        /// 删除全部目标id的所有消息订阅
        /// </summary>
        /// <param name="targetId"></param>
        public void unRegister(int targetId)
        {
            if (IdToMsgs.ContainsKey(targetId))
            {
                List<string> msgIds = IdToMsgs[targetId];
                for (int i = 0; i < msgIds.Count; i++)
                {
                    unRegister(msgIds[i], targetId);
                }
                msgIds.Clear();
                IdToMsgs.Remove(targetId);
            }
        }

        /// <summary>
        /// 删除目标id的单个消息订阅
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="targetId"></param>
        public void unRegister(string msgId, int targetId)
        {
            List<SubscribeData> temps;
            SubscribeData temp;
            if (MsgMap.ContainsKey(msgId))
            {
                temps = MsgMap[msgId];
                for (int i = 0; i < temps.Count; i++)
                {
                    temp = temps[i];
                    if (temp.TargetId == targetId)
                    {
                        temps.Remove(temp);
                        temp.ResetData();
                        SDQueue.Enqueue(temp);
                    }
                }
            }
        }


        public void Update()
        {
            if (cbList.Count < 1)
            {
                return;
            }
            object[] datas;
            while (cbList.Count > 0)
            {
                datas = cbList[0];
                cbList.RemoveAt(0);
                Notice(datas);
            }
        }
      

        private void AddSub(SubscribeData temp)
        {
            List<SubscribeData> temps;
            if (!MsgMap.ContainsKey(temp.MsgId))
            {
                temps = new List<SubscribeData>();
                temps.Add(temp);
                MsgMap.Add(temp.MsgId, temps);
            }
            else
            {
                temps = MsgMap[temp.MsgId];
                SubscribeData oldTemp;
                for (int i = 0; i < temps.Count; i++)
                {
                    oldTemp = temps[i];
                    /// 是否有相同消息 id targetid 和cb
                    if (temp.MsgId == oldTemp.MsgId && temp.TargetId == oldTemp.TargetId && (temp.Callback0 == oldTemp.Callback0 || temp.Callback1 == oldTemp.Callback1))
                    {
                        Debug.LogError("相同订阅=====" + temp.MsgId);
                        temp.ResetData();
                        SDQueue.Enqueue(temp);
                        return;
                    }
                }


                temps.Add(temp);
            }
            List<string> msgs;
            if (!IdToMsgs.ContainsKey(temp.TargetId))
            {
                msgs = new List<string>();
                msgs.Add(temp.MsgId);
                IdToMsgs.Add(temp.TargetId, msgs);
            }
            else
            {
                msgs = IdToMsgs[temp.TargetId];
                msgs.Add(temp.MsgId);
            }
        }


       

        //todo 以主线方式 调用 主线层的方法
        private void Notice(object[] datas)
        {
            string msgId = (string)datas[0];
            object param = datas[1];

            if (MsgMap.ContainsKey(msgId))
            {
                List<SubscribeData> temps = MsgMap[msgId];
                SubscribeData temp;
                if (temps != null && temps.Count > 0)
                {
                    for (int i = 0; i < temps.Count; i++)
                    {
                        temp = temps[i];
                        if (param != null && temp.Callback0 != null)
                        {
                            temp.Callback0(param);
                        }
                        else if (temp.Callback0 != null)
                        {
                            temp.Callback0(null);
                        }
                        else
                        {
                            temp.Callback1();
                        }
                    }
                }
            }
        }
    }

    public class PubSubMono : MonoBehaviour
    {
        public void Update()
        {
            PubSubManager.instance.Update();
        }
    }
}