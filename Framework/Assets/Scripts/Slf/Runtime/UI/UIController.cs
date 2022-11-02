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
            Register(new UIData(1, "Home", "Prefabs/Home", PopupType.None, LayerType.Scene, false, false, false));
            Register(new UIData(2, "LevelSelect", "Prefabs/LevelSelect", PopupType.MinToMax, LayerType.Sole, true, true, false));
            Register(new UIData(3, "Bargain", "Prefabs/Bargain", PopupType.None, LayerType.Sole, true, false, false));
            Register(new UIData(4, "Bag", "Prefabs/Bag/Bag", PopupType.MinToMax, LayerType.Panel, true, true, false));
            Register(new UIData(5, "Seting", "Prefabs/Seting", PopupType.MinToMax, LayerType.Panel, true, true, false));

            Register(new UIData(8, "Dialog", "Prefabs/Dialog", PopupType.None, LayerType.Panel, true, true, false));
            Register(new UIData(9, "Result", "Prefabs/Result/Result", PopupType.None, LayerType.Panel, true, false, false));
            Register(new UIData(10, "LoadingAnimation", "Prefabs/LoadingAnimation", PopupType.None, LayerType.Loading, true, false, false));
            Register(new UIData(11, "MessageTips", "Prefabs/Common/MessageTips", PopupType.None, LayerType.Loading, false, false, false));


            Register(new UIData(20, "GameGetProp", "Game/Prafeb/GameGetProp", PopupType.MinToMax, LayerType.Panel, true, false, false));
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
