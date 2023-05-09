using System.Collections.Generic;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/12/29 17:20:18
    // - Description: ui id绑定数据 
    //==========================
    public class UIController
    {
        /// <summary>
        /// ui数据字典
        /// </summary>
        Dictionary<int, UIData> uiMap = new Dictionary<int, UIData>();

        /// <summary>
        /// 初始化数据注册
        /// </summary>
        public void Init()
        {
            Register(new UIData(1, "TestUI1", "Prefabs/TestUI1", LayerType.Scene, PopupType.None,  false, false, false));
            //Register(new UIData(2, "TestNameSpace.TestUI2", "Prefabs/TestUI2", LayerType.Panel, PopupType.MinToMax,  true));
        }

        /// <summary>
        /// 注册ui数据
        /// </summary>
        /// <param name="data">数据</param>
        public void Register(UIData data)
        {
            if (uiMap.ContainsKey(data.ID))
            {
                UnityEngine.Debug.LogError("Error Same ID = " + data.ID);
            }
            else
            {
                uiMap.Add(data.ID, data);
            }
        }

        /// <summary>
        /// 获取ui数据
        /// </summary>
        /// <param name="uiId">id</param>
        /// <returns>ui数据</returns>
        public UIData GetUIData(int uiId)
        {
            if (uiMap.ContainsKey(uiId))
            {
                return uiMap[uiId];
            }

            UnityEngine.Debug.LogError("Error None ID = " + uiId);
            return null;
        }
    }

}
