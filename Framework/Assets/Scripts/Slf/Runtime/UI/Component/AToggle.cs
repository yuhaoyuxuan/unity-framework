using System;
using UnityEngine.UI;

namespace Slf
{

    //==========================
    // - Author:      slf         
    // - Date:        2021/11/03 16:05:08
    // - Description: Toggle扩展
    //==========================
    public class AToggle : Toggle
    {
        private Action cb;             //回调方法 不带参
        private Action<object> cb1;    //回调方法 带参
        private object param;          //回调参数

        public ButtonSound Sound = ButtonSound.Open;
        /// <summary>
        /// 未选中的时候 是否隐藏 graphic
        /// </summary>
        public bool HideGraphic;
        /// <summary>
        /// 是否可以重复点击 默认false
        /// </summary>
        public bool RepeatClick;


        private bool oldIsOn;//上一次状态
        private bool playSound = true;
        private void resetData()
        {
            cb = null;
            cb1 = null;
            param = null;
            onValueChanged.RemoveListener(OnClickFunction);
        }

        protected override void Awake()
        {
            base.Awake();
            if (HideGraphic)
            {
                graphic.gameObject.SetActive(false);
            }
        }



        protected override void OnDestroy()
        {
            resetData();
        }

        //设置点击回调 回调方法
        public void SetClickCallback(Action callback)
        {
            AddClick();
            cb = callback;
        }

        //设置点击回调 回调方法  回调的数据
        //public void setClickCallback(Action<object> callback, object data)
        //{
        //    addClick();
        //    cb1 = callback;
        //    param = data;
        //}

        ////热更域 不支持泛型回调
        ////设置点击回调 回调方法  回调的数据
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
            playSound = false;

            if (isOn == true)
            {
                OnClickFunction(true);
            }
            else
            {
                isOn = true;
            }

        }

        private void AddClick()
        {
            resetData();
            onValueChanged.AddListener(OnClickFunction);
        }

        private void OnClickFunction(bool boo)
        {
            if (HideGraphic)
            {
                graphic.gameObject.SetActive(boo);
            }

            if(oldIsOn == isOn && !RepeatClick) {
                return;
            }
            oldIsOn = isOn;

            if (Sound != ButtonSound.Empty && playSound)
            {
                AudioManager.Instance.PlayAudio("Sound/Button");
            }

            if (cb1 != null)
            {
                cb1(param);
            }
            else if (cb != null)
            {
                cb();
            }
            playSound = true;
        }
    }

}
