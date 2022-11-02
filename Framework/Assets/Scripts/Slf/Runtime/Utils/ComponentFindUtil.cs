using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2022/08/09 16:02:23
    // - Description: 查找预制体节点组件 并缓存
    //==========================
    public class ComponentFindUtil
    {
        private static Dictionary<int, Dictionary<string, Object>> cacheDic = new Dictionary<int, Dictionary<string, Object>>();

        /// <summary>
        /// 查找子项的组件并缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        static public T Find<T>(string path ,Transform transform) where T : Object
        {
            T temp = GetCache(path, transform) as T;
            if (temp != null)
            {
                return temp;
            }

            Transform tf = transform.Find(path);
            if (tf != null)
            {
                temp = tf.GetComponent<T>();
                if (temp != null)
                {
                    SetCache(path, transform, temp);
                }
                return temp;
            }
            return null;
        }


        /// <summary>
        /// 查找子项的gameObject并缓存
        /// </summary>
        /// <param name="path"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        static public GameObject Find(string path, Transform transform)
        {
            GameObject temp = GetCache(path, transform) as GameObject;
            if (temp != null)
            {
                return temp;
            }

            Transform tf = transform.Find(path);
            if (tf != null)
            {
                SetCache(path, transform, tf.gameObject);
                return tf.gameObject;
            }
            return null;
        }

        /// <summary>
        /// 销毁缓存
        /// </summary>
        /// <param name="Transform"></param>
        public static void Destroy(Transform transform) 
        {
            int code = transform.GetHashCode();
            if (cacheDic.ContainsKey(code)){
                cacheDic.Remove(code);
            }

        }

        /// <summary>
        /// 获取缓存的组件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        private static Object GetCache(string path, Transform transform)
        {
            int code = transform.GetHashCode();
            if (cacheDic.ContainsKey(code)){
                Dictionary<string, Object> tempMap = cacheDic[code];
                if (tempMap.ContainsKey(path))
                {
                    return tempMap[path];
                }
            }
            return null;
        }

        /// <summary>
        /// 缓存组件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="transform">预制体</param>
        /// <param name="obj">组件｜对象</param>
        private static void SetCache(string path, Transform transform , Object obj)
        {
            int code = transform.GetHashCode();
            Dictionary<string, Object> tempMap;
            if (!cacheDic.ContainsKey(code)){
                tempMap = new Dictionary<string, Object>();
                cacheDic.Add(code, tempMap);
            }else{
                tempMap = cacheDic[code];
            }

            if(!tempMap.ContainsKey(path)){
                tempMap.Add(path, obj);
            }
        }
    }
}
