

using UnityEngine;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/08/05 17:01:40	
    // - Description: 抽象单例基类
    //==========================
    public abstract class Singleton<T> where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                     _instance = new T();
                return _instance;
            }
        }

        public virtual void Init() { }
    }
    //==========================
    // - Author:      slf         
    // - Date:        2021/08/05 17:12:23	
    // - Description: 抽象组件单例基类 放到不销毁场景
    //==========================
    public abstract class SingletonComponent<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// 关闭程序后，不必在创建新的单例组件
        /// </summary>
        static bool quitApplication;
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null && !quitApplication)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    GameObject.DontDestroyOnLoad(go);
                    _instance = go.AddComponent<T>();

                }
                return _instance;
            }
        }

        public virtual void Init() { }

        public virtual void OnDestroy(){
            quitApplication = true;
        }
    }
}
