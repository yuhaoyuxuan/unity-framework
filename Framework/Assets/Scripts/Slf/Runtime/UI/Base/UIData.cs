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
        public string ClassName;
        /// <summary>
        /// 预制体路径
        /// </summary>
        public string PrefabPath;
        /// <summary>
        /// 弹出效果
        /// </summary>
        public PopupType PopupType;
        /// <summary>
        /// 层级类型
        /// </summary>
        public LayerType LayerType;
        /// <summary>
        /// 关闭面板是否销毁
        /// </summary>
        public bool IsDestroy;
        /// <summary>
        /// 是否显示半透黑底
        /// </summary>
        public bool IsDarkRect;
        /// <summary>
        /// 是否同步显示加载loading  addLayer后移除loading 
        /// </summary>
        public bool IsSync;
        /// <summary>
        /// 透传数据
        /// </summary>
        public object Data;

        public UIData(params object[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            ID = (int)args[0];
            ClassName = (string)args[1];
            PrefabPath = (string)args[2];
            PopupType = (PopupType)args[3];
            LayerType = (LayerType)args[4];
            IsDestroy = (bool)args[5];
            IsDarkRect = (bool)args[6];
            IsSync = (bool)args[7];
        }
    }

}