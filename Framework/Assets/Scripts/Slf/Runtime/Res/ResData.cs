
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.U2D;

namespace Slf
{

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResType
    {
        Sprite,         //精灵
        SpriteAtlas,    //精灵图集
        GameObject,     //游戏对象
        Texture         //纹理
    }

    //==========================
    // - Author:      slf         
    // - Date:        2022/10/30 10:20:13
    // - Description: 资源数据
    //==========================
    public class ResData
    {
        /// <summary>
        /// 远程资源检测
        /// </summary>
        private static Regex RemoteReg = new Regex(@"http.?:\/\/");
        /// <summary>
        /// 资源类型
        /// </summary>
        public ResType ResType;
        /// <summary>
        /// 资源路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 只有资源是Atlas图集  key 
        /// </summary>
        public string Key;
        /// <summary>
        /// 持有者HashCode
        /// </summary>
        public int Owner;
        /// <summary>
        /// 回调函数
        /// </summary>
        public Action<object> Callback;
        /// <summary>
        /// 引用计数
        /// </summary>
        public int RefCount;
        /// <summary>
        /// 内容
        /// </summary>
        public object Content;

        /// <summary>
        /// 是否远程资源
        /// </summary>
        public bool IsRemote;

        /// <summary>
        /// 获取内容类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetContent<T>()
        {
            if (ResType == ResType.SpriteAtlas)
            {
                return (T)Content;
            }
            else if (ResType == ResType.Sprite)
            {
                object sp;
                if (!string.IsNullOrEmpty(Key))
                {
                    sp = ((SpriteAtlas)Content).GetSprite(Key);
                    return (T)sp;
                }

                Texture2D t2 = (Texture2D)Content;
                sp = Sprite.Create(t2, new Rect(0, 0, t2.width, t2.height), Vector2.zero);
                return (T)sp;
            }
            else if (ResType == ResType.GameObject)
            {
                object obj = GameObject.Instantiate((GameObject)Content);
                return (T)obj;
            }
            return (T)Content;
        }

        public void Init<T>(string path, string key, int owner, ResType type, Action<T> cb)
        {
            Path = path;
            Key = key;
            Owner = owner;
            ResType = type;
            IsRemote = RemoteReg.IsMatch(Path);
            if (cb != null)
            {
                Callback = new Action<object>(o => cb(GetContent<T>()));
            }
        }

        /// <summary>
        /// 添加引用计数
        /// </summary>
        public void AddRef()
        {
            RefCount++;
            ResManager.instance.AddRefRecord(this);
        }

        /// <summary>
        /// 删除引用计数
        /// </summary>
        public void DelRef()
        {
            RefCount--;
            if (RefCount <= 0)
            {
                Recycle();
            }
        }

        /// <summary>
        /// 完成
        /// </summary>
        public void Finish()
        {
            AddRef();
            if (Callback != null)
            {
                Callback(Content);
            }
            Callback = null;
        }

        /// <summary>
        /// 回收数据
        /// </summary>
        public void Recycle()
        {
            if (IsRemote)
            {
                GameObject.Destroy((UnityEngine.Object)Content);
            }
            else if (ResType != ResType.GameObject)
            {
                Resources.UnloadAsset((UnityEngine.Object)Content);
            }
            Callback = null;
            Content = null;
            ResManager.instance.DelCache(this);
        }
    }

}
