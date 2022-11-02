using UnityEngine;
using UnityEngine.UI;

namespace Slf
{
    /**层级枚举*/
    public enum LayerType
    {
        Scene,  //场景层
        Sole,   //唯一的  会把同层级的ui移除
        Panel,  //面板层
        Loading, //加载层
    }

    //==========================
    // - Author:      slf         
    // - Date:        2021/07/12 14:02:20
    // - Description: 层级管理
    //==========================
    public class UILayer
    {
        private GameObject sceneLayer;   //场景层
        private GameObject soleLayer;    //唯一层 会把同层级的view移除
        private GameObject panelLayer;   //面板层
        private GameObject loadingLayer; //loading层

        private GameObject soleBlack;  //唯一层黑底
        private GameObject panelBlack;  //面板层黑底

        /// <summary>
        /// 初始化层级
        /// </summary>
        /// <param name="root"></param>
        public void InitLayer(RectTransform root)
        {
            sceneLayer = GetGO("scene", root);
            soleLayer = GetGO("sole", root);
            panelLayer = GetGO("panel", root);
            loadingLayer = GetGO("loading", root);

            soleBlack = AddBlack((RectTransform)soleLayer.transform, "soleBlack");
            panelBlack = AddBlack((RectTransform)panelLayer.transform, "panelBlack");
        }

        private GameObject GetGO(string name, RectTransform parent)
        {
            GameObject temp = new GameObject(name);
            RectTransform tf = temp.AddComponent<RectTransform>();
            tf.sizeDelta = parent.sizeDelta;
            tf.SetParent(parent, false);

            tf.localPosition = Vector3.zero;
            tf.offsetMin = Vector2.zero;
            tf.anchorMin = Vector2.zero;
            tf.anchorMax = Vector2.one;
            tf.offsetMax = Vector2.zero;
            return temp;
        }

        /// <summary>
        /// 添加层遮挡黑底
        /// </summary>
        private GameObject AddBlack(RectTransform rt,string name)
        {
            GameObject go = GetGO(name, rt);
            Image img = go.AddComponent<Image>();
            img.color = new Color(0, 0, 0, 0.8f);
            go.SetActive(false);

            return go;
        }

        /// <summary>
        /// 刷新黑底
        /// </summary>
        private void FlushDarkLayer(LayerType type)
        {
            Transform layer = panelLayer.transform;
            GameObject black = panelBlack;

            if(type == LayerType.Sole)
            {
                layer = soleLayer.transform;
                black = soleBlack;
            }


            int chileLen = layer.childCount;
            Transform transform;
            UIBase ui;
            int layerIdx;
            int offsetNum = 0;
            for (int i = chileLen - 1; i >= 0; i--)
            {
                transform = layer.GetChild(i);
                ui = transform.GetComponent<UIBase>();
                if (ui == null)
                {
                    if (chileLen > 3 && i == chileLen - 1)
                    {
                        offsetNum = 1;
                    }
                    continue;
                }
                layerIdx = transform.GetSiblingIndex() - 1 + offsetNum;
                if (layerIdx < 0)
                {
                    layerIdx = 0;
                }
                if (ui.UiData.IsDarkRect)
                {
                    black.transform.SetSiblingIndex(layerIdx);
                    black.SetActive(true);
                    return;
                }
            }
            black.SetActive(false);
        }


        /// <summary>
        /// 添加面板
        /// </summary>
        /// <param name="ui"></param>
        public void AddLayer(UIBase ui)
        {
            LayerType layerType = ui.UiData.LayerType;
            switch (layerType)
            {
                case LayerType.Scene:
                    CloseLayerAll();
                    ui.transform.SetParent(sceneLayer.transform, false);
                    break;
                case LayerType.Sole:
                    CloseLayer(LayerType.Sole);
                    ui.transform.SetParent(soleLayer.transform, false);
                    break;
                case LayerType.Panel:
                    ui.transform.SetParent(panelLayer.transform, false);
                    break;
                case LayerType.Loading:
                    ui.transform.SetParent(loadingLayer.transform, false);
                    break;
            }
            if (ui.gameObject.activeSelf == false)
            {
                ui.gameObject.SetActive(true);
            }
            ui.transform.localPosition = new Vector3(0, 0, 0);
            ui.transform.localScale = new Vector3(1, 1, 1);

            if (layerType == LayerType.Panel || layerType == LayerType.Sole)
            {
                FlushDarkLayer(layerType);
            }
        }

        /// <summary>
        /// 移除面板
        /// </summary>
        /// <param name="layer"></param>
        public void RemoveLayer(UIBase layer)
        {
            LayerType layerType = layer.UiData.LayerType;
            if (layer && layer.transform && layer.transform.parent)
            {
                layer.gameObject.SetActive(false);
                layer.transform.SetParent(null, false);

                if (layerType == LayerType.Panel || layerType == LayerType.Sole)
                {
                    FlushDarkLayer(layerType);
                }
            }
        }

        /// <summary>
        /// 关闭所有层子项
        /// </summary>
        /// <param name="ignoreId">过滤界面id</param>
        public void CloseLayerAll(int ignoreId = -1)
        {
            CloseLayer(LayerType.Scene,ignoreId);
            CloseLayer(LayerType.Sole, ignoreId);
            CloseLayer(LayerType.Panel, ignoreId); 
            CloseLayer(LayerType.Loading, ignoreId); 
        }

        /// <summary>
        /// 关闭 一层ui子项
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ignoreId"> 过滤界面id</param>
        private void CloseLayer(LayerType type,int ignoreId = -1)
        {
            GameObject[] layerContainer = new GameObject[] {sceneLayer, soleLayer, panelLayer, loadingLayer };
            GameObject layer = layerContainer[(int)type];
            int chileLen = layer.transform.childCount;

            Transform transform;
            UIBase ui;
            while (--chileLen >= 0)
            {
                transform = layer.transform.GetChild(chileLen);
                ui = transform.GetComponent<UIBase>();
                if (ui != null)
                {
                    if(ui.UiData.ID != ignoreId)
                    {
                        ui.Close();
                    }
                }
            }
        }
    }
}
