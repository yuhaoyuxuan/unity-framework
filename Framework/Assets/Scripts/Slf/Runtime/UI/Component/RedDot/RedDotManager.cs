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
        private Dictionary<int, List<int>> childDic;
        //红点的父项
        private Dictionary<int, int> parentDic;
        //红点id 状态
        private Dictionary<int, bool> boolDic;
        //红点的组件
        private Dictionary<int, List<ARedDot>> cpDic;


        public void Init()
        {
            childDic = new Dictionary<int, List<int>>();
            parentDic = new Dictionary<int, int>();
            cpDic = new Dictionary<int, List<ARedDot>>();
            boolDic = new Dictionary<int, bool>();

            InitRegister();
        }

        //注册红点依赖
        private void InitRegister()
        {

            List<RedDotData> tempList = new List<RedDotData>()
            {
                new RedDotData(1),
                new RedDotData(2, 1),
            };


            RedDotData temp;
            for (int i = 0; i < tempList.Count; i++)
            {
                temp = tempList[i];
                if (temp.ParentID == 0) { continue; }


                parentDic.Add(temp.ID, temp.ParentID);
                if (!childDic.ContainsKey(temp.ParentID))
                {
                    childDic.Add(temp.ParentID, new List<int>());
                }
                childDic[temp.ParentID].Add(temp.ID);
            }
        }

        /// <summary>
        /// 创建红点数据
        /// </summary>
        /// <param name="temp"></param>
        public void CreateRegister(RedDotData temp)
        {
            if (temp.ParentID == 0) { return; }

            parentDic.Add(temp.ID, temp.ParentID);
            if (!childDic.ContainsKey(temp.ParentID))
            {
                childDic.Add(temp.ParentID, new List<int>());
            }
            childDic[temp.ParentID].Add(temp.ID);
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
            if (!cpDic.ContainsKey(dotId))
            {
                cpDic.Add(dotId, new List<ARedDot>());
            }
            cpDic[dotId].Add(reddot);
            RefreshRedDot(dotId);
        }

        //移除注册
        public void UnRegisterRedDot(int dotId, ARedDot redDot)
        {
            if (dotId == 0) { return; }

            if (cpDic == null || !cpDic.ContainsKey(dotId) || cpDic[dotId].Count < 1) { return; }


            List<ARedDot> list = cpDic[dotId];

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

            if (!boolDic.ContainsKey(dotId))
            {
                boolDic.Add(dotId, show);
            }
            else
            {
                if (boolDic[dotId] == show)
                {
                    return;
                }
                boolDic[dotId] = show;
            }

            RefreshRedDot(dotId);
        }

        //检测红点状态
        public bool IsActive(int dotId)
        {
            if (dotId == 0) { return false; }

            //检测本身
            if (boolDic.ContainsKey(dotId) && boolDic[dotId] == true)
            {
                return boolDic[dotId];
            }

            if (childDic.ContainsKey(dotId))
            {
                //递归检测 孩子/孙子 状态
                List<int> childList = childDic[dotId];
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
            if (cpDic.ContainsKey(dotId))
            {
                for (int i = 0; i < cpDic[dotId].Count; i++)
                {
                    cpDic[dotId][i].RefreshActive();
                }
            }

            //递归刷新父/父父 红点组件
            if (parentDic.ContainsKey(dotId))
            {
                int parentId = parentDic[dotId];
                RefreshRedDot(parentId);
            }
        }
    }

}
