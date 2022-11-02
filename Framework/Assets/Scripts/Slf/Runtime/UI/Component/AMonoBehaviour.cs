using System.Collections.Generic;
using UnityEngine;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/08/26 11:11:07	
    // - Description: 扩展unity脚本基类 做一些移除销毁相关
    //==========================
    public class AMonoBehaviour : MonoBehaviour
    {
        void Awake()
        {
            InitEvent();
        }

        void OnDestroy()
        {
            DestroyView();

            int hashCode = GetHashCode();
            ComponentFindUtil.Destroy(transform);
            TimerManager.instance.UnRegister(hashCode);
            PubSubManager.instance.unRegister(hashCode);
        }

        /**初始化事件 只会调用一次*/
        public virtual void InitEvent() {}
        /**销毁面板 只会调用一次*/
        public virtual void DestroyView() {}
    }

}