using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/08/03 20:50:17	
    // - Description: 定时器管理单位秒  
    //==========================
    public class TimerManager : SingletonComponent<TimerManager>
    {
        //数据队列池
        private MyQueue<TimerData> TimeQueue = new MyQueue<TimerData>();

        private List<TimerData> List = new List<TimerData>();
        private Dictionary<int, int> HasDic = new Dictionary<int, int>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="delay">间隔时间秒</param>
        /// <param name="ownerId">注册着id</param>
        /// <param name="callback">回调方法</param>
        /// <param name="loop">是否循环 默认flse</param>
        public void Register(float delay, int ownerId, Action callback, bool loop = false)
        {
            UnRegister(ownerId, callback);
            TimerData time = TimeQueue.Dequeue();
            time.ResetData(delay, ownerId, callback, loop);
            AddTime(time);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="delay">间隔时间秒</param>
        /// <param name="ownerId">目标id</param>
        /// <param name="callback">回调方法</param>
        /// <param name="loop">是否循环 默认flse</param>
        /// <param name="param">回调参数</param>
        public void Register(float delay, int ownerId, Action<object> callback, bool loop = false, object param = null)
        {
            UnRegister(ownerId, callback);
            TimerData time = TimeQueue.Dequeue();
            time.ResetData(delay, ownerId, callback, loop, param);
            AddTime(time);
        }


        //移除全部
        public void UnRegister(int targetId)
        {
            Remove(targetId);
        }

        //移除单个
        public void UnRegister(int targetId, Action callback)
        {
            Remove(targetId, callback);
           
        }

        //移除单个
        public void UnRegister(int targetId, Action<object> callback)
        {
            Remove(targetId, null, callback);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="cb"></param>
        private void Remove(int targetId, Action cb = null, Action<object> cb1 = null)
        {
            if (!CanRemove(targetId))
            {
                return;
            }

            TimerData time;
            for (int i = List.Count - 1; i >= 0; i--)
            {
                time = List[i];
                if (time.OwnerId == targetId)
                {
                    if (cb == null && cb1 == null
                        || cb != null && cb == time.Callback
                        || cb1 != null && cb1 == time.CallbackParam)
                    {
                        RemoveTime(time);
                    }
                }
            }
        }

        //是否可以移除注册
        private bool CanRemove(int id)
        {
            if (List.Count < 1 || !HasDic.ContainsKey(id) || HasDic[id] < 1)
            {
                return false;
            }

            return true;
        }

        //添加注册
        private void AddTime(TimerData time)
        {
            if (HasDic.ContainsKey(time.OwnerId))
            {
                HasDic[time.OwnerId]++;
            }
            else
            {
                HasDic[time.OwnerId] = 1;
            }

            List.Add(time);
        }

        //移除注册
        private void RemoveTime(TimerData time)
        {

            if (HasDic.ContainsKey(time.OwnerId))
            {
                HasDic[time.OwnerId]--;
                if (HasDic[time.OwnerId] <= 0)
                {
                    HasDic.Remove(time.OwnerId);
                }
            }
            List.Remove(time);
            time.ResetData();
            TimeQueue.Enqueue(time);
        }



        void Update()
        {
            if (List.Count < 1)
            {
                return;
            }

            Action cb;
            Action<object> cb1;
            object param;
            for (int i = List.Count - 1; i >= 0; i--)
            {
                if (List.Count <= i)
                {
                    continue;
                }
                TimerData time = List[i];

                if (time == null)
                {
                    continue;
                }
                time.CurrTime -= Time.deltaTime;
                if (time.CurrTime <= 0)
                {
                    cb = time.Callback;
                    cb1 = time.CallbackParam;
                    param = time.Param;
                    if (time.Loop)
                    {
                        time.CurrTime = time.Delay;
                    }
                    else
                    {
                        if (cb != null)
                        {
                            UnRegister(time.OwnerId, cb);
                        }
                        else
                        {
                            UnRegister(time.OwnerId, cb1);
                        }
                    }

                    if (param != null && cb1 != null)
                    {
                        cb1(param);
                    }
                    else if (cb1 != null)
                    {
                        cb1(null);
                    }
                    else
                    {
                        cb();
                    }
                }
            }
        }
    }
}
