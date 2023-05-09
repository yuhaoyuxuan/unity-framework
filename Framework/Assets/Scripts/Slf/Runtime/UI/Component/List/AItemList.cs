using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Slf
{
    //列表接口
    public interface IAList
    {
        //刷新数据
        void RefreshData(object[] datas);
        //刷新显示
        void RefreshView();
    }

    //==========================
    // - Author:      slf         
    // - Date:        2021/09/15 11:06:03	
    // - Description: 单项渲染列表
    //==========================
    public class AItemList : AMonoBehaviour, IAList
    {
        //预制体 必须挂载组件AItemRenderer的派生类
        public GameObject itemGo;

        private List<AMonoBehaviour> itemGoPool;
        private List<AMonoBehaviour> itemGoList;
        //数据列表
        private object[] datas;
        /// <summary>
        /// 点击回调
        /// </summary>
        private Action<IAItemRenderer> cb;

        public override void InitEvent()
        {
            if (itemGoPool == null)
            {
                itemGoPool = new List<AMonoBehaviour>();
                itemGoList = new List<AMonoBehaviour>();
            }
        }

        public override void DestroyView()
        {
            DestroyChild();
            datas = null;
            cb = null;
            itemGoPool = null;
            itemGoList = null;

        }

        /// <summary>
        /// 销毁列表子项
        /// </summary>
        public void DestroyChild()
        {
            for (int i = 0; i < itemGoPool.Count; i++)
            {
                GameObject.Destroy(itemGoPool[i].gameObject);
            }
            for (int i = 0; i < itemGoList.Count; i++)
            {
                GameObject.Destroy(itemGoList[i].gameObject);
            }
            datas = null;
            itemGoPool.Clear();
            itemGoList.Clear();
        }

        /// <summary>
        /// 填充数据 刷新显示列表
        /// </summary>
        /// <param name="ds">数据数组</param>
        public void RefreshData<T>(List<T> ds)
        {
            if (ds == null)
            {
                return;
            }
            object[] ts = new object[ds.Count];
            for(int i = 0; i < ds.Count; i++)
            {
                ts[i] = ds[i];
            }
            RefreshData(ts);

        }

        /// <summary>
        /// 填充数据 刷新显示列表
        /// </summary>
        /// <param name="ds">数据数组</param>
        public void RefreshData(object[] ds)
        {
            if (ds == null)
            {
                return;
            }
            datas = ds;

            if (itemGoList == null)
            {
                InitEvent();
            }

            RefreshView();
        }

        /// <summary>
        /// 刷新显示列表
        /// </summary>
        public void RefreshView()
        {
            int maxLen = Mathf.Max(datas.Length, itemGoList.Count);
            AMonoBehaviour item;
            for (int i = 0; i < maxLen; i++)
            {
                if (datas.Length > i && itemGoList.Count <= i)
                {
                    itemGoList.Add(GetItem());
                }
                else if (datas.Length <= i && itemGoList.Count > i)
                {
                    item = itemGoList[i];
                    itemGoList[i] = null;
                    RemoveItem(item);
                }
            }
            itemGoList.RemoveAll(n => n == null);
            RefreshItemData();
        }

        private void RefreshItemData()
        {
            object data;
            AMonoBehaviour item;
            for (int i = 0; i < datas.Length; i++)
            {
                data = datas[i];
                item = itemGoList[i];
                IAItemRenderer temp = (IAItemRenderer)item;
                temp.SubData = data;
                if (cb != null)
                {
                    if (item.GetComponent<AButton>())
                    {
                        item.GetComponent<AButton>().SetClickCallback(OnItemTap, temp);
                    }
                    else if (item.GetComponent<AToggle>())
                    {
                        ToggleGroup tg = GetComponent<ToggleGroup>();
                        if (tg)
                        {
                            item.GetComponent<AToggle>().group = tg;
                            item.GetComponent<AToggle>().SetClickCallback(OnItemTap, temp);
                        }
                    }

                };
            }
        }

        private AMonoBehaviour GetItem()
        {
            AMonoBehaviour item;
            if (itemGoPool.Count > 0)
            {
                item = itemGoPool[0];
                itemGoPool.RemoveAt(0);
            }
            else
            {
                item = GetAItemRenderer();

            }
            item.gameObject.SetActive(true);
            item.transform.SetParent(transform, false);
            return item;
        }


        private AMonoBehaviour GetAItemRenderer()
        {
            GameObject go = Instantiate(itemGo);
            AMonoBehaviour[] games = go.GetComponents<AMonoBehaviour>();
            string name;
            for (int i = 0; i < games.Length; i++)
            {
                var ty = games[i].GetType();
                name = games[i].GetType().BaseType.ToString();
                if (name.IndexOf("AItemRenderer") != 0)
                {
                    return games[i];
                }
            }
            return null;
        }

        private void RemoveItem(AMonoBehaviour item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(null, false);
            itemGoPool.Add(item);
        }

        /// <summary>
        /// 设置子项点击回调 子项 必须有 AButton|AToggle 组件
        /// </summary>
        /// <param name="cb"></param>
        public void SetItemTap(Action<IAItemRenderer> cb)
        {
            this.cb = cb;
        }

        private void OnItemTap(IAItemRenderer dataO)
        {
            if (cb != null)
            {
                cb(dataO);
            }
        }

        /// <summary>
        /// 设置默认子项点击
        /// </summary>
        /// <param name="data"></param>
        public void SetDefaultItemTap(object data)
        {
            for (int i = 0; i < itemGoList.Count; i++)
            {
                IAItemRenderer temp = (IAItemRenderer)itemGoList[i];
                if (temp.SubData == data)
                {
                    SetDefaultItemTap(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 设置默认子项点击
        /// </summary>
        /// <param name="data"></param>
        public void SetDefaultItemTap(int idx)
        {
            if (itemGoList.Count > idx)
            {

                if (itemGoList[idx].GetComponent<AButton>())
                {
                    itemGoList[idx].GetComponent<AButton>().TouchCallback();
                }
                if (itemGoList[idx].GetComponent<AToggle>())
                {
                    itemGoList[idx].GetComponent<AToggle>().TouchCallback();
                }
            }
        }
    }

}
