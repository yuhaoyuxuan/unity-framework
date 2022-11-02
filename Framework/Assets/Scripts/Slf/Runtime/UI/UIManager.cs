using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slf
{

    //==========================
    // - Author:      slf         
    // - Date:        2021/07/12 14:23:20
    // - Description: UI管理类
    //==========================
    public partial class UIManager : Singleton<UIManager>
    {
        public UIController UIController;

        /// <summary>
        /// 预加载的预制体
        /// </summary>
        Dictionary<int, GameObject> preloadGO;
        /// <summary>
        /// 已经打开的界面
        /// </summary>
        Dictionary<int, UIBase> openMap;
        /// <summary>
        /// 界面弹出效果
        /// </summary>
        UIPopup uiPopup;
        /// <summary>
        /// 界面层级
        /// </summary>
        UILayer uiLayer;

        /// <summary>
        /// 初始化 传入ui界面根容器
        /// </summary>
        /// <param name="root"></param>
        public void Init(RectTransform root)
        {
            preloadGO = new Dictionary<int, GameObject>();
            openMap = new Dictionary<int, UIBase>();
            uiLayer = new UILayer();
            uiPopup = new UIPopup();
            UIController = new UIController();
            uiLayer.InitLayer(root);
            UIController.Init();
            var t = TimerManager.instance;
            var p = PubSubManager.instance;
        }

        /// <summary>
        /// ui是否已经添加到显示列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsShowView(int id)
        {
            if (openMap.ContainsKey(id))
            {
                try
                {
                    UIBase ui = openMap[id];
                    return ui.gameObject.activeSelf;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 预加载界面
        /// </summary>
        /// <param name="id">界面id</param>
        /// <param name="cb">完成回调</param>
        public void PreloadView(int id, Action cb = null)
        {
            if (!openMap.ContainsKey(id))
            {
                UIData uiData = UIController.GetUIData(id);
                if (uiData == null)
                {
                    return;
                }
                var opHandle = Resources.Load<GameObject>(uiData.PrefabPath);
                GameObject prefab = GameObject.Instantiate<GameObject>(opHandle);
                if (!preloadGO.ContainsKey(id))
                {
                    preloadGO.Add(id, prefab);
                }

                if (cb != null)
                {
                    cb();
                }
            }
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="data">透传参数</param>
        public virtual void ShowView(int id, object data = null)
        {
            UIData uiData = UIController.GetUIData(id);
            if (uiData == null)
            {
                return;
            }
            UIBase ui;

            uiData.Data = data;
            if (!openMap.ContainsKey(id))
            {
                GameObject prefab;
                if (preloadGO.ContainsKey(id))
                {
                    prefab = preloadGO[id];
                    preloadGO.Remove(id);
                }
                else
                {
                    var opHandle = Resources.Load<GameObject>(uiData.PrefabPath);
                    prefab = GameObject.Instantiate<GameObject>(opHandle);
                }

                prefab.gameObject.SetActive(false);

                Type type = Type.GetType(uiData.ClassName);
                ui = prefab.AddComponent(type) as UIBase;
                ui.UiData = uiData;
                prefab.gameObject.SetActive(true);
                openMap.Add(id, ui);
            }
            else
            {
                ui = openMap[id];
            }

            ui.Preload(() =>
            {

                Debug.Log("open = " + uiData.ClassName);
                if (!ui.transform.parent)
                {
                    uiLayer.AddLayer(ui);
                    uiPopup.AddPopup(ui);
                }
                ui.AddLayer();
            });
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="id">界面id</param>
        public void CloseView(int id)
        {
            if (openMap.ContainsKey(id))
            {
                UIBase ui = openMap[id];
                Debug.Log("close = " + ui.UiData.ClassName);
                uiLayer.RemoveLayer(ui);
                ui.RmoveLayer();
                //销毁
                if (ui.UiData.IsDestroy)
                {
                    DestroyView(ui);
                }
            }
        }

        /// <summary>
        /// 关闭所有层级界面
        /// </summary>
        /// <param name="ignoreId">过滤id</param>
        public void CloseAllView(int ignoreId = -1)
        {
            uiLayer.CloseLayerAll(ignoreId);
        }

        /// <summary>
        /// 销毁界面
        /// </summary>
        /// <param name="id">界面id</param>
        public void DestroyView(int id)
        {
            if (openMap.ContainsKey(id))
            {
                UIBase ui = openMap[id];
                DestroyView(ui);
            }
        }

        private void DestroyView(UIBase ui)
        {
            if (ui.transform.parent)
            {
                uiLayer.RemoveLayer(ui);
                ui.RmoveLayer();
            }
            openMap.Remove(ui.UiData.ID);
            GameObject.Destroy(ui.gameObject);
        }
    }
}
