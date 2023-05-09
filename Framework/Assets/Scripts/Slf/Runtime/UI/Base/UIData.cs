using System;
using UnityEngine;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/12/29 17:19:51
    // - Description: 界面数据
    //==========================
    public class UIData
    {
        /// <summary>
        /// 唯一界面id
        /// </summary>
        public int ID;
        /// <summary>
        /// 类全路径
        /// </summary>
        public string ClassPath;
        /// <summary>
        /// 预制体路径
        /// </summary>
        public string PrefabPath;
        /// <summary>
        /// 层级类型
        /// </summary>
        public LayerType LayerType;
        /// <summary>
        /// 弹出效果
        /// </summary>
        public PopupType PopupType;
        /// <summary>
        /// 关闭面板是否销毁
        /// </summary>
        public bool IsDestroy;
        /// <summary>
        /// 是否显示半透黑底
        /// </summary>
        public bool IsBlackMask;
        /// <summary>
        /// 是否同步显示加载loading  addLayer后移除loading 
        /// </summary>
        public bool IsSync;
        /// <summary>
        /// 透传数据
        /// </summary>
        public object Data;

        /// <summary>
        /// ui数据
        /// </summary>
        /// <param name="uiId">界面唯一id</param>
        /// <param name="classPath">类路径</param>
        /// <param name="prefabPath">预制体路径</param>
        /// <param name="layer">层级</param>
        /// <param name="popup">弹出效果</param>
        /// <param name="destroy">关闭面板是否销毁</param>
        /// <param name="blackMask">半透明黑底</param>
        /// <param name="sync">是否同步显示</param>
        public UIData(int uiId,string classPath,string prefabPath,LayerType layer = LayerType.Panel,PopupType popup = PopupType.None,bool destroy = false ,bool blackMask = false,bool sync = false)
        {
            ID = uiId;
            ClassPath = classPath;
            PrefabPath = prefabPath;
            LayerType = layer;
            PopupType = popup;
            IsDestroy = destroy;
            IsBlackMask = blackMask;
            IsSync = sync;
        }
    }

}