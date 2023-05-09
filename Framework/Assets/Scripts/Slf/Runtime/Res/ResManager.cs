using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2022/10/30 15:20:13
    // - Description: 资源管理
    //==========================
    public class ResManager : SingletonComponent<ResManager>
    {
        /// <summary>
        /// 加载池
        /// </summary>
        public MyQueue<ResLoad> LoadPool = new MyQueue<ResLoad>();
        /// <summary>
        /// 资源池
        /// </summary>
        public MyQueue<ResData> DataPool = new MyQueue<ResData>();
        /// <summary>
        /// 资源缓存
        /// </summary>
        Dictionary<string, ResData> Cache = new Dictionary<string, ResData>();
        /// <summary>
        /// 记录key映射已加载资源列表
        /// </summary>
        Dictionary<int, List<string>> KeyToCachePathList = new Dictionary<int, List<string>>();


        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="owner"></param>
        /// <param name="cb"></param>
        public void Load<T>(string path, int owner, Action<T> cb = null, ResType type = ResType.Sprite)
        {
            Load(path, "", owner, cb, type);
        }

        public void Load<T>(string path, string key, int owner, Action<T> cb = null, ResType type = ResType.Sprite)
        {
            ResData resData = GetCache(path);
            if (resData != null)
            {
                resData.Init(path, key, owner, type, cb);
                resData.Finish();
            }
            else
            {
                resData = DataPool.Dequeue();
                resData.Init(path, key, owner, type, cb);
                LoadPool.Dequeue().Init(resData);
            }
        }

        /// <summary>
        /// 卸载持有者资源
        /// </summary>
        /// <param name="owner">持有者</param>
        public void UnLoad(int owner)
        {
            DelRefRecord(owner);
        }

        /// <summary>
        /// 删除持有者资源引用记录
        /// </summary>
        /// <param name="owner"></param>
        public void DelRefRecord(int owner)
        {
            if (KeyToCachePathList.ContainsKey(owner))
            {
                List<string> pathList = KeyToCachePathList[owner];
                for (int i = 0; i < pathList.Count; i++)
                {
                    if (Cache.ContainsKey(pathList[i]))
                    {
                        Cache[pathList[i]].DelRef();
                    }
                }
                KeyToCachePathList.Remove(owner);
            }
        }

        /// <summary>
        /// 添加持有者资源引用记录
        /// </summary>
        /// <param name="rData"></param>
        public void AddRefRecord(ResData rData)
        {
            List<string> lists;
            if (KeyToCachePathList.ContainsKey(rData.Owner))
            {
                KeyToCachePathList[rData.Owner].Add(rData.Path);
            }
            else
            {
                lists = new List<string>() { rData.Path };
                KeyToCachePathList.Add(rData.Owner, lists);
            }
        }

        /// <summary>
        /// 获取缓存资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ResData GetCache(string path)
        {
            if (Cache.ContainsKey(path))
            {
                return Cache[path];
            }
            return null;
        }

        /// <summary>
        /// 添加资源缓存
        /// </summary>
        /// <param name="rData"></param>
        public void AddCache(ResData rData)
        {
            if (!Cache.ContainsKey(rData.Path))
            {
                Cache.Add(rData.Path, rData);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="rData"></param>
        public void DelCache(ResData rData)
        {
            if (Cache.ContainsKey(rData.Path))
            {
                Cache.Remove(rData.Path);
            }
            DataPool.Enqueue(rData);
        }
    }


}
