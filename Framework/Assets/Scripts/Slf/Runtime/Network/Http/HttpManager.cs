using UnityEngine;
using UnityEngine.Networking;
using System;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/07/15 18:19:27	
    // - Description: http通信管理类
    //==========================
    public class HttpManager : SingletonComponent<HttpManager>
    {
        public MyQueue<Http> HttpQueue = new MyQueue<Http>();//http队列
        /// <summary>
        /// 发送get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cb"></param>
        public void SendGet(string url, Action<string> cb = null)//发送get请求
        {
            Http http = HttpQueue.Dequeue();
            http.Data.ResetData(url, null, UnityWebRequest.kHttpVerbGET, cb);
            http.Init();
        }

        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="cb"></param>
        /// <param name="time"></param>
        public void SendPost(string url, object param, Action<string> cb = null, int time = 5)//发送post请求
        {
            Http http = HttpQueue.Dequeue();
            http.Data.ResetData(url, param, UnityWebRequest.kHttpVerbPOST, cb, time);
            http.Init();
        }
    }
}



