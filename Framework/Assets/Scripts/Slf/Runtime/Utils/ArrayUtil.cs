
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2022/08/09 16:37:51	
    // - Description: 数组工具
    //==========================
    public class ArrayUtil
    {
        /// <summary>
        /// list转数组
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="listObj">列表</param>
        /// <param name="reverse">是否反序</param>
        /// <returns></returns>
        public static T[] ListToArr<T>(object listObj, bool reverse = false)
        {
            IList<T> list = (IList<T>)listObj;

            if (list == null)
            {
                Debug.LogError("传入列表错误===" + listObj);
                return new T[0];
            }

            T[] arr = new T[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                arr[i] = list[i];
            }

            if (reverse)
            {
                Array.Reverse(arr);
            }

            return arr;
        }


    }
}
