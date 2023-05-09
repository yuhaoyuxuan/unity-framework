using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2022/10/31 15:20:13
    // - Description: 虚拟列表
    //==========================
    public class AVirtualScrollView : ScrollRect, IAList
    {
        /// <summary>
        /// 预制体 必须挂载组件AItemRenderer的派生类
        /// </summary>
        [SerializeField]
        private GameObject ItemRenderer;

        /// <summary>
        /// 最大渲染预制体 垂直数量
        /// </summary>
        private int verticalCount;
        /// <summary>
        /// 最大渲染预制体 水平数量
        /// </summary>
        private int horizontalCount;
        /// <summary>
        /// 预制体宽高 添加间隔偏移后
        /// </summary>
        private float itemW;
        private float itemH;
        /// <summary>
        /// 开始坐标
        /// </summary>
        private Vector2 startPos;
        /// <summary>
        /// 内容坐标偏移
        /// </summary>
        private Vector2 offsetContentPos;
        /// <summary>
        /// 内容布局
        /// </summary>
        private LayoutGroup contentLayout;
        /// <summary>
        /// 布局类型 0horizontal(水平) 1vertical(垂直) 2grid(格子)
        /// </summary>
        private int LayoutGroupType;

        /// <summary>
        /// 修改数据 刷新显示列表  
        /// </summary>
        private bool dataRefresh;
        /// <summary>
        /// 滑动刷新 刷新显示列表
        /// </summary>
        private bool scrollMoveRefresh;

        private List<GameObject> itemPool;
        private List<GameObject> itemList;

        /**预制体渲染类列表 */
        private List<IAItemRenderer> itemRendererList;
        /// <summary>
        /// 数据列表
        /// </summary>
        private object[] datas;


        /// <summary>
        /// 点击回调
        /// </summary>
        private Action<IAItemRenderer> itemClickCb;

        private bool isInit;
        private bool isStart;
        protected override void Awake()
        {
            onValueChanged.AddListener(ScrollMove);
        }

        protected override void OnDestroy()
        {
            onValueChanged.RemoveListener(ScrollMove);
            DestroyChild();
            base.OnDestroy();
        }

        protected override void Start()
        {
            base.Start();
            InitData();
        }

        private void InitData()
        {
            itemPool = new List<GameObject>();
            itemList = new List<GameObject>();
            itemRendererList = new List<IAItemRenderer>();
            contentLayout = content.GetComponent<LayoutGroup>();
            contentLayout.enabled = false;
            ContentSizeFitter cs = content.GetComponent<ContentSizeFitter>();
            if (cs != null)
            {
                cs.enabled = false;
            }


            RectTransform itemRT = ItemRenderer.GetComponent<RectTransform>();
            offsetContentPos = new Vector2(-(viewport.sizeDelta.x / 2), viewport.sizeDelta.y / 2);
            //预制体宽高
            string name = contentLayout.ToString();
            bool isGrid = name.IndexOf("Grid") != -1;
            if (isGrid)
            {
                LayoutGroupType = 2;
                GridLayoutGroup gridLayout = (GridLayoutGroup)contentLayout;
                itemW = itemRT.sizeDelta.x + gridLayout.spacing.x;
                itemH = itemRT.sizeDelta.y + gridLayout.spacing.y;
            }
            else if (horizontal)
            {
                LayoutGroupType = 0;
                itemW = itemRT.sizeDelta.x + ((HorizontalLayoutGroup)contentLayout).spacing;
                itemH = itemRT.sizeDelta.y;
            }
            else if (vertical)
            {
                LayoutGroupType = 1;
                itemW = itemRT.sizeDelta.x;
                itemH = itemRT.sizeDelta.y + ((VerticalLayoutGroup)contentLayout).spacing;
            }

            //垂直、水平最大预制体数量
            RectTransform scrollRT = (RectTransform)gameObject.transform;
            horizontalCount = Mathf.CeilToInt(scrollRT.rect.width / itemW) + 1;
            verticalCount = Mathf.CeilToInt(scrollRT.rect.height / itemH) + 1;

            if (isGrid)
            {
                GridLayoutGroup grid = (GridLayoutGroup)contentLayout;

                if (grid.startAxis == GridLayoutGroup.Axis.Horizontal)
                {
                    horizontalCount = Mathf.FloorToInt(scrollRT.sizeDelta.x / itemW);
                }
                else
                {
                    verticalCount = Mathf.FloorToInt(scrollRT.sizeDelta.y / itemH);
                }
            }
            isInit = true;
            if (datas != null)
            {
                RefreshData(datas);
            }
        }

        /// <summary>
        /// 设置子项点击回调 子项 必须有 AButton 组件
        /// </summary>
        /// <param name="cb"></param>
        public void SetItemTap(Action<IAItemRenderer> cb)
        {
            itemClickCb = cb;
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
            for (int i = 0; i < ds.Count; i++)
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
            if (!isInit)
            {
                return;
            }
            isStart = true;
            RefreshContentSize();
            StartCoroutine(AddItem());
            dataRefresh = true;
        }

        private void Update()
        {
            if (!isStart || !scrollMoveRefresh)
            {
                return;
            }
            RefreshView();
        }


        WaitForSeconds wait = new WaitForSeconds(0.05f);
        /**添加预制体 */
        private IEnumerator AddItem()
        {
            int len = 0;
            switch (LayoutGroupType)
            {
                case 0:
                    len = horizontalCount;
                    break;
                case 1:
                    len = verticalCount;
                    break;
                case 2:
                    len = horizontalCount * verticalCount;
                    break;
            }
            len = Mathf.Min(len, datas.Length);
            int itemListLen = itemList.Count;
            GameObject child = null;
            if (itemListLen < len)
            {
                for (var i = itemListLen; i < len; i++)
                {
                    if (itemPool.Count > 0)
                    {
                        child = itemPool[0];
                        itemPool.RemoveAt(0);
                        child.transform.SetParent(content, false);
                        itemList.Add(child);
                        itemRendererList.Add(child.GetComponent<IAItemRenderer>());
                        //child.transform.localPosition = new Vector3(-3000f, 0);
                    }
                    else
                    {
                        child = GameObject.Instantiate(ItemRenderer);
                        child.transform.SetParent(content, false);
                        //child.transform.localPosition = new Vector3(-3000f, 0);
                        if (itemClickCb != null)
                        {
                            if (child.GetComponent<AButton>())
                            {
                                child.GetComponent<AButton>().SetClickCallback(itemClickCb, child.GetComponent<IAItemRenderer>());
                            }
                        };

                        itemList.Add(child);
                        itemRendererList.Add(child.GetComponent<IAItemRenderer>());
                        scrollMoveRefresh = true;
                        yield return wait;
                    }
                }
            }
            else
            {
                int cL = content.childCount;
                while (cL > len)
                {
                    child = itemList[cL - 1];
                    child.transform.SetParent(null, false);
                    child.transform.localPosition = Vector3.zero;
                    itemList.RemoveAt(cL - 1);
                    itemRendererList.RemoveAt(cL - 1);
                    itemPool.Add(child);
                    cL = content.childCount;
                }
            }
            scrollMoveRefresh = true;
            yield break;
        }

        /// <summary>
        /// 根据数据数量 改变content宽高
        /// </summary>
        private void RefreshContentSize()
        {
            Vector2 contentSize = content.sizeDelta;
            LayoutGroup contentLayout = content.GetComponent<LayoutGroup>();
            int dataLength = datas.Length;
            switch (LayoutGroupType)
            {
                case 0:
                    contentSize.x = contentLayout.padding.left + dataLength * itemW + contentLayout.padding.right;
                    //horizontalNormalizedPosition = 0;
                    break;
                case 1:
                    contentSize.y = contentLayout.padding.top + dataLength * itemH + contentLayout.padding.bottom;
                    //verticalNormalizedPosition = 0;
                    break;
                case 2:
                    GridLayoutGroup grid = (GridLayoutGroup)contentLayout;

                    if (grid.startAxis == GridLayoutGroup.Axis.Horizontal)
                    {
                        contentSize.y = contentLayout.padding.top + Mathf.CeilToInt((float)dataLength / (float)horizontalCount) * itemH + contentLayout.padding.bottom;
                    }
                    else if (grid.startAxis == GridLayoutGroup.Axis.Vertical)
                    {
                        contentSize.x = contentLayout.padding.left + Mathf.CeilToInt((float)dataLength / (float)verticalCount) * itemW + contentLayout.padding.right;
                    }
                    break;
            }
            content.sizeDelta = contentSize;
            RectTransform itemRT = ItemRenderer.GetComponent<RectTransform>();
            //起始位置
            startPos = new Vector2(itemRT.sizeDelta.x * itemRT.pivot.x + contentLayout.padding.left - contentSize.x / 2, contentSize.y / 2 - (itemRT.sizeDelta.y * itemRT.pivot.y + contentLayout.padding.top));
        }

        /// <summary>
        /// 刷新显示列表
        /// 刷新预制体位置 和 数据填充
        /// </summary>
        public void RefreshView()
        {
            switch (LayoutGroupType)
            {
                case 0:
                    RefreshHorizontal();
                    break;
                case 1:
                    RefreshVertical();
                    break;
                case 2:
                    RefreshGrid();
                    break;
            }
            scrollMoveRefresh = false;
            dataRefresh = false;
        }

        /// <summary>
        /// 刷新水平
        /// </summary>
        private void RefreshHorizontal()
        {
            int start = Mathf.FloorToInt(Mathf.Abs(content.localPosition.x - offsetContentPos.x) / itemW);
            if (start < 0 || content.localPosition.x > offsetContentPos.x)
            {
                start = 0;
            }
            int end = start + horizontalCount;
            if (end > datas.Length)
            {
                end = datas.Length;
                start = Mathf.Max(end - horizontalCount, 0);
            }
            float tempV = 0;
            int itemListLen = itemList.Count;
            int idx;
            float offsetX = content.sizeDelta.x / 2;
            GameObject item;

            Vector2 posV2;
            for (var i = 0; i < itemListLen; i++)
            {
                idx = (start + i) % itemListLen;
                item = itemList[idx];
                tempV = startPos.x + ((start + i) * itemW) + offsetX;
                if (item.transform.localPosition.x != tempV || dataRefresh)
                {
                    posV2 = item.transform.localPosition;
                    posV2.x = tempV;
                    item.transform.localPosition = posV2;
                    itemRendererList[idx].SubData = datas[start + i];
                }
            }
        }
        /// <summary>
        /// 刷新垂直
        /// </summary>
        private void RefreshVertical()
        {
            int start = Mathf.FloorToInt((content.localPosition.y - offsetContentPos.y) / itemH);
            if (start < 0 || content.localPosition.y < offsetContentPos.y)
            {
                start = 0;
            }

            int end = start + verticalCount;
            if (end > datas.Length)
            {
                end = datas.Length;
                start = Mathf.Max(end - verticalCount, 0);
            }
            float tempV = 0;
            int itemListLen = itemList.Count;
            int idx;
            float offsetY = content.sizeDelta.y / 2;
            GameObject item;

            Vector2 posV2;
            for (var i = 0; i < itemListLen; i++)
            {
                idx = (start + i) % itemListLen;
                item = itemList[idx];
                tempV = startPos.y + (-(start + i) * itemH) - offsetY;
                if (item.transform.localPosition.y != tempV || dataRefresh)
                {
                    posV2 = item.transform.localPosition;
                    posV2.y = tempV;
                    item.transform.localPosition = posV2;
                    itemRendererList[idx].SubData = datas[start + i];
                }
            }
        }
        /// <summary>
        /// 刷新网格
        /// </summary>
        private void RefreshGrid()
        {
            GridLayoutGroup grid = (GridLayoutGroup)contentLayout;

            //是否垂直方向 添加网格
            bool isVDirection = grid.startAxis == GridLayoutGroup.Axis.Vertical;

            int start = Mathf.FloorToInt((content.localPosition.y - offsetContentPos.y) / itemH) * horizontalCount;
            if (isVDirection)
            {
                start = Mathf.FloorToInt(Mathf.Abs(content.localPosition.x - offsetContentPos.x) / itemW) * verticalCount;
                if (content.localPosition.x > offsetContentPos.x)
                {
                    start = 0;
                }
            }
            else if (content.localPosition.y < offsetContentPos.y)
            {
                start = 0;
            }

            if (start < 0)
            {
                start = 0;
            }

            int end = start + horizontalCount * verticalCount;
            if (end > datas.Length)
            {
                end = datas.Length;
                start = Mathf.Max(end - horizontalCount * verticalCount, 0);
            }
            float tempX, tempY;
            int itemListLen = itemList.Count;
            float offsetX = content.sizeDelta.x / 2;
            float offsetY = content.sizeDelta.y / 2;
            Vector2 posV = Vector2.zero;
            GameObject item;
            int idx;
            for (var i = 0; i < itemListLen; i++)
            {
                idx = (start + i) % itemListLen;
                item = itemList[idx];
                if (isVDirection)
                {
                    tempX = startPos.x + (start + i) / verticalCount * itemW + offsetX;
                    tempY = startPos.y + -((start + i) % verticalCount) * itemH;
                }
                else
                {
                    tempX = startPos.x + ((start + i) % horizontalCount) * itemW;
                    tempY = startPos.y + -((start + i) / horizontalCount) * itemH - offsetY;
                }

                if (item.transform.localPosition.y != tempY || item.transform.localPosition.x != tempX || dataRefresh)
                {
                    posV.x = tempX;
                    posV.y = tempY;
                    item.transform.localPosition = posV;
                    itemRendererList[idx].SubData = datas[start + i];
                }
            }
        }

        /// <summary>
        /// 销毁列表子项
        /// </summary>
        public void DestroyChild()
        {
            if (itemPool != null)
            {
                for (int i = 0; i < itemPool.Count; i++)
                {
                    GameObject.Destroy(itemPool[i].gameObject);
                }
                itemPool.Clear();
            }

            if (itemList != null)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    GameObject.Destroy(itemList[i].gameObject);
                }
                itemList.Clear();
            }
        }

        private void ScrollMove(Vector2 v2)
        {
            scrollMoveRefresh = true;
        }
    }

}
