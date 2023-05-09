using System;
using UnityEngine.UI;

namespace Slf
{
    //按钮点击音效类型
    public enum ButtonSound
    {
        Empty,
        Open,
        Close
    }

    //==========================
    // - Author:      slf         
    // - Date:        2021/09/10 18:22:49	
    // - Description: 按钮扩展
    //==========================
    public class AButton : Button
    {
        private Action cb;             //回调方法 不带参
        private Action<object> cb1;    //回调方法 带参
        private object param;          //回调参数

        /// <summary>
        /// 点击音效
        /// </summary>
        public ButtonSound Sound = ButtonSound.Open;

        private void ResetData()
        {
            cb = null;
            cb1 = null;
            param = null;
            onClick.RemoveListener(OnClickFunction);
        }

        protected override void OnDestroy()
        {
            ResetData();
        }

        /// <summary>
        /// 设置点击回调 回调方法
        /// </summary>
        /// <param name="callback"></param>
        public void SetClickCallback(Action callback)
        {
            AddClick();
            cb = callback;
        }

        //热更域 不支持泛型回调
        //设置点击回调 回调方法  回调的数据
        public void SetClickCallback<T>(Action<T> callback, T data)
        {
            AddClick();
            cb1 = new Action<object>(o => callback((T)o));
            param = data;
        }

        /// <summary>
        /// 主动触发回调
        /// </summary>
        public void TouchCallback()
        {
            OnClickFunction();
        }

        private void AddClick()
        {
            ResetData();
            onClick.AddListener(OnClickFunction);
        }

        private void OnClickFunction()
        {
            if (Sound != ButtonSound.Empty)
            {
                AudioManager.Instance.PlayAudio("Audio/Button");
            }
            if (cb1 != null)
            {
                cb1(param);
            }
            else if (cb != null)
            {
                cb();
            }
        }
    }
}

