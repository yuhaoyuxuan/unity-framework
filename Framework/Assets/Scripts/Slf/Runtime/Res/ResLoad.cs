using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Slf
{

    //==========================
    // - Author:      slf         
    // - Date:        2022/10/30 11:20:13
    // - Description: 资源加载
    //==========================
    public class ResLoad
    {
        /// <summary>
        /// 失败重试最大加载次数
        /// </summary>
        private static int LoadMax = 3;
        /// <summary>
        /// 资源加载次数
        /// </summary>
        protected int LoadCount;
        /// <summary>
        /// 资源数据
        /// </summary>
        protected ResData Data;
        /// <summary>
        /// 持有者HashCode
        /// </summary>
        public int Owner;
        /// <summary>
        /// 回调函数
        /// </summary>
        public Action<object> Callback;

        public void Init(ResData data)
        {
            Data = data;
            LoadCount = 0;
            Load();
        }

        private void Load()
        {
            if (Data.IsRemote)
            {
                LoadRemot();
            }
            else
            {
                LoadLocal();
            }
        }
        /// <summary>
        /// 包内资源
        /// </summary>
        private void LoadLocal()
        {

            //addressables加载方式
            //Addressables.LoadAssetAsync<object>(Data.Path).Completed += LoadLocalComplete;


            ResourceRequest rRequest = Resources.LoadAsync(Data.Path);
            rRequest.completed += LoadLocalComplete;
        }

        //addressables加载方式
        //private void LoadLocalComplete(AsyncOperationHandle<object> ap)
        //{
        //    LoadComplete(ap.Result, ap);
        //}

        private void LoadLocalComplete(AsyncOperation ap)
        {
            LoadComplete(((ResourceRequest)ap).asset);
        }



        /// <summary>
        /// 远程资源
        /// </summary>
        private void LoadRemot()
        {
            LoadCount++;
            if (LoadCount > LoadMax)
            {
                Debug.LogError("Load Remot Resource Error URL =" + Data.Path);
                return;
            }
            ResManager.Instance.StartCoroutine(LoadFromWeb(Data.Path));
        }

        IEnumerator LoadFromWeb(string url)
        {
            UnityWebRequest wr = new UnityWebRequest(url);
            DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
            wr.downloadHandler = texDl;
            yield return wr.SendWebRequest();

            if (wr.result != UnityWebRequest.Result.Success)
            {
                LoadRemot();
            }
            else
            {
                LoadComplete(texDl.texture);
            }
        }

        /// <summary>
        /// 加载完成
        /// </summary>
        /// <param name="asset"></param>
        private void LoadComplete(object asset)
        {
            Data.Content = asset;
            Data.Finish();
            ResManager.Instance.AddCache(Data);
            Recycle();
        }

        /// <summary>
        /// 加载完成 addressables加载方式
        /// </summary>
        /// <param name="asset"></param>
        //private void LoadComplete(object asset, AsyncOperationHandle handle)
        //{
        //    Data.Handle = handle;
        //    LoadComplete(asset);
        //}

        private void Recycle()
        {
            Data = null;
            ResManager.Instance.LoadPool.Enqueue(this);
        }
    }
}