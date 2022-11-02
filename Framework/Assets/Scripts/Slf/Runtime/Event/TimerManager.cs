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
    public class TimerManager : Singleton<TimerManager>
    {
        //数据队列池
        private MyQueue<TimerData> TimeQueue = new MyQueue<TimerData>();
        private List<TimerData> List = new List<TimerData>();
        private Dictionary<int, int> HasDic = new Dictionary<int, int>();

        public TimerManager()
        {
            GameObject go = new GameObject("TimerManagerMono");
            go.AddComponent<TimerManagerMono>();
            GameObject.DontDestroyOnLoad(go);
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="delay">间隔时间秒</param>
        /// <param name="targetId">目标id</param>
        /// <param name="callback">回调方法</param>
        /// <param name="loop">是否循环 默认flse</param>
        public void Register(float delay, int targetId, Action callback, bool loop = false)
        {
            UnRegister(targetId, callback);
            TimerData time = TimeQueue.Dequeue();
            time.ResetData(delay, targetId, callback, loop);
            AddTime(time);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="delay">间隔时间秒</param>
        /// <param name="targetId">目标id</param>
        /// <param name="callback">回调方法</param>
        /// <param name="loop">是否循环 默认flse</param>
        /// <param name="param">回调参数</param>
        public void Register(float delay, int targetId, Action<object> callback, bool loop = false, object param = null)
        {
            UnRegister(targetId, callback);
            TimerData time = TimeQueue.Dequeue();
            time.ResetData(delay, targetId, callback, loop, param);
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
                if (time.TargetId == targetId)
                {
                    if (cb == null && cb1 == null
                        || cb != null && cb == time.Callback
                        || cb1 != null && cb1 == time.Callback1)
                    {
                        RemoveTime(time);
                    }
                }
            }
        }

        //刷新
        public void Update()
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
                    cb1 = time.Callback1;
                    param = time.Param;
                    if (time.Loop)
                    {
                        time.CurrTime = time.Delay;
                    }
                    else
                    {
                        if (cb != null)
                        {
                            UnRegister(time.TargetId, cb);
                        }
                        else
                        {
                            UnRegister(time.TargetId, cb1);
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
            if (HasDic.ContainsKey(time.TargetId))
            {
                HasDic[time.TargetId]++;
            }
            else
            {
                HasDic[time.TargetId] = 1;
            }

            List.Add(time);
        }

        //移除注册
        private void RemoveTime(TimerData time)
        {

            if (HasDic.ContainsKey(time.TargetId))
            {
                HasDic[time.TargetId]--;
                if (HasDic[time.TargetId] <= 0)
                {
                    HasDic.Remove(time.TargetId);
                }
            }
            List.Remove(time);
            time.ResetData();
            TimeQueue.Enqueue(time);
        }
    }

    public class TimerManagerMono : MonoBehaviour
    {
        public void Update()
        {
            TimerManager.instance.Update();
        }
    }
}
