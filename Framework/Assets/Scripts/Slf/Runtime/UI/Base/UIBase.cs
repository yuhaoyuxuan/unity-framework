using System;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/12/29 17:17:23
    // - Description: ui基类
    //==========================
    public class UIBase : AMonoBehaviour, IUI, IPreload
    {
        /// <summary>
        /// ui界面数据
        /// </summary>
        public UIData UiData;

        /// <summary>
        /// 预加载完成回调
        /// </summary>
        public Action PreloadCb;

        //传参 ui数据
        protected object Data
        {
            get { return UiData.Data; }
            set { }
        }

        /// <summary>
        /// 添加显示列表后
        /// </summary>
        public void AddLayer()
        {
            InitView();
        }

        /// <summary>
        /// 移除显示列表后
        /// </summary>
        public void RmoveLayer()
        {
            PreloadCb = null;
            RemoveView();
        }

        public virtual void InitView() { }
        public virtual void RemoveView() { }


        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="cb"></param>
        public virtual void Preload(Action cb)
        {
            PreloadCb = cb;
            PreloadComplete();
        }

        /// <summary>
        /// 预加载完成
        /// </summary>
        public void PreloadComplete()
        {
            if (PreloadCb != null)
            {
                PreloadCb();
                PreloadCb = null;
            }
        }

        /// <summary>
        /// 预加载超时
        /// </summary>
        public void PreloadTimeout()
        {
            PreloadCb = null;
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        public void Close()
        {
            UIManager.instance.CloseView(UiData.ID);
        }
    }

}
