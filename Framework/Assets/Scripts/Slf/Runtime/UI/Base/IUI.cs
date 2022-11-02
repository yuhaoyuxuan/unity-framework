using System;
namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/12/29 17:16:45
    // - Description: ui接口
    //==========================
    public interface IUI
    {
        /// <summary>
        /// 初始化事件 只会调用一次
        /// </summary>
        void InitEvent();
        /// <summary>
        /// 初始化视图 每次打开都会调用
        /// </summary>
        void InitView();
        /// <summary>
        /// 移除面板 每次移除都会调用
        /// </summary>
        void RemoveView();
        /// <summary>
        /// 销毁面板 只会调用一次
        /// </summary>
        void DestroyView();
    }

    //==========================
    // - Author:      slf         
    // - Date:        2021/12/29 17:19:34
    // - Description: 预加载接口
    //==========================
    public interface IPreload
    {
        /// <summary>
        /// 预加载 、数据通信 准备工作 子类 重写
        /// </summary>
        /// <param name="cb"></param>
        void Preload(Action cb);
        /// <summary>
        /// 预加载完成
        /// </summary>
        void PreloadComplete();
        /// <summary>
        /// 预加载超时
        /// </summary>
        void PreloadTimeout();
    }

}