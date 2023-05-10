using System.Collections.Generic;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/09/12 13:20:53	
    // - Description: 红点数据 id和pid的关系是  pid是id外层  id触发的时候 也会 触发pid
    //==========================
    public class RedDotData
    {
        public int ID;        //红点id
        public int ParentID;  //父id     

        public RedDotData(int id, int parentId = 0)
        {
            ID = id;
            ParentID = parentId;
        }
    }

    //==========================
    // - Author:      slf         
    // - Date:        2021/09/12 13:30:29	
    // - Description: 红点管理
    //==========================
    public class RedDotManager : Singleton<RedDotManager>
    {
        //红点的子项 
        private Dictionary<int, List<int>> parentToChildListDic = new Dictionary<int, List<int>>();
        //红点的父项
        private Dictionary<int, int> childToParentDic = new Dictionary<int, int>();
        //红点id 状态
        private Dictionary<int, bool> redDotStateDic = new Dictionary<int, bool>();
        //红点的组件
        private Dictionary<int, List<ARedDot>> idToComponentListDic = new Dictionary<int, List<ARedDot>>();


        public override void Init()
        {
            InitRegister();
        }

        //注册红点依赖
        private void InitRegister()
        {

            List<RedDotData> tempList = new List<RedDotData>()
            {
                new RedDotData(1),
                new RedDotData(2),
                new RedDotData(3,1),
            };

            CreateRegister(tempList);
        }

        public void CreateRegister(List<RedDotData> tempList)
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                CreateRegister(tempList[i]);
            }
        }

        /// <summary>
        /// 创建红点数据
        /// </summary>
        /// <param name="temp"></param>
        public void CreateRegister(RedDotData temp)
        {
            if (temp.ParentID == 0) { return; }

            childToParentDic.Add(temp.ID, temp.ParentID);
            if (!parentToChildListDic.ContainsKey(temp.ParentID))
            {
                parentToChildListDic.Add(temp.ParentID, new List<int>());
            }
            parentToChildListDic[temp.ParentID].Add(temp.ID);
        }



        //注册
        public void RegisterRedDot(int dotId, ARedDot reddot)
        {
            if (dotId == 0)
            {
                reddot.RefreshActive();
                return;
            }

            UnRegisterRedDot(dotId, reddot);
            if (!idToComponentListDic.ContainsKey(dotId))
            {
                idToComponentListDic.Add(dotId, new List<ARedDot>());
            }
            idToComponentListDic[dotId].Add(reddot);
            RefreshRedDot(dotId);
        }

        //移除注册
        public void UnRegisterRedDot(int dotId, ARedDot redDot)
        {
            if (dotId == 0) { return; }

            if (idToComponentListDic == null || !idToComponentListDic.ContainsKey(dotId) || idToComponentListDic[dotId].Count < 1) { return; }


            List<ARedDot> list = idToComponentListDic[dotId];

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == redDot)
                {
                    list.RemoveAt(i);
                }
            }
        }

        //设置红点状态 
        public void SetAcive(int dotId, bool show)
        {
            if (dotId == 0) { return; }

            if (!redDotStateDic.ContainsKey(dotId))
            {
                redDotStateDic.Add(dotId, show);
            }
            else
            {
                if (redDotStateDic[dotId] == show)
                {
                    return;
                }
                redDotStateDic[dotId] = show;
            }

            RefreshRedDot(dotId);
        }

        /// <summary>
        /// 检测红点状态
        /// </summary>
        /// <param name="dotId"></param>
        /// <returns></returns>
        public bool CheckState(int dotId)
        {
            if (dotId == 0) { return false; }

            if (redDotStateDic.ContainsKey(dotId))
            {
                return redDotStateDic[dotId];
            }

            return false;
        }

        /// <summary>
        /// 检测红点状态 如果未激活 递归检测子项如果有激活 为true  否则false
        /// </summary>
        /// <param name="dotId"></param>
        /// <returns></returns>
        public bool IsActive(int dotId)
        {
            if (dotId == 0) { return false; }

            //检测本身
            if (CheckState(dotId))
            {
                return true;
            }

            //递归检测子项状态
            if (parentToChildListDic.ContainsKey(dotId))
            {
                List<int> childList = parentToChildListDic[dotId];
                bool boo;
                for (int i = 0; i < childList.Count; i++)
                {
                    boo = IsActive(childList[i]);
                    if (boo)
                    {
                        return boo;
                    }
                }
            }
            return false;
        }

        /**刷新红点显示 */
        private void RefreshRedDot(int dotId)
        {
            if (dotId == 0) { return; }

            //刷新红点组件
            if (idToComponentListDic.ContainsKey(dotId))
            {
                for (int i = 0; i < idToComponentListDic[dotId].Count; i++)
                {
                    idToComponentListDic[dotId][i].RefreshActive();
                }
            }

            //递归刷新父/父父 红点组件
            if (childToParentDic.ContainsKey(dotId))
            {
                int parentId = childToParentDic[dotId];
                RefreshRedDot(parentId);
            }
        }
    }

}
