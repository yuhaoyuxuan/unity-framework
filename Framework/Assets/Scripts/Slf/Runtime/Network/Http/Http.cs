using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Slf
{
    public class Http
    {
        protected UnityWebRequest URequest;
        public HttpData Data = new HttpData();

        public void Init()
        {
            if (URequest == null)
            {
                URequest = new UnityWebRequest();
            }

            URequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");
            URequest.downloadHandler = new DownloadHandlerBuffer();
            URequest.timeout = Data.Timeout;
            HttpManager.Instance.StartCoroutine(SendMsg());
        }

        // 	/**发送请求 */
        private IEnumerator SendMsg()
        {
            URequest.url = Data.Url;
            URequest.method = Data.MethodType;
            string json = "";
            if (Data.MethodType == UnityWebRequest.kHttpVerbPOST)
            {
                json = JsonUtility.ToJson(Data.Param);
                byte[] databyte = Encoding.UTF8.GetBytes(json);
                URequest.uploadHandler = new UploadHandlerRaw(databyte);
            }

            Debug.Log("api发送=" + URequest.url + "   " + json);
            yield return URequest.SendWebRequest();

            //异常处理
            if (URequest.result == UnityWebRequest.Result.ProtocolError)
            {
                IoError();
            }
            else
            {
                Success();
            }
        }

        private void IoError()
        {
            Debug.LogError("api异常=" + URequest.error);
            if (Data.Cb != null)
            {
                Data.Cb(null);
            }
            Reset();
        }

        private void Success()
        {
            Debug.Log("api收到=" + URequest.downloadHandler.text);
            if (Data.Cb != null)
            {
                Data.Cb(URequest.downloadHandler.text);
            }
            Reset();
        }

        private void Reset()
        {
            URequest.Abort();
            URequest.Dispose();
            Data.ResetData();
            URequest = null;
            HttpManager.Instance.HttpQueue.Enqueue(this);
        }
    }
}