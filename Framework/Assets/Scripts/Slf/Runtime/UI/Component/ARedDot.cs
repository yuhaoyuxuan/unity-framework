using UnityEngine;
namespace Slf
{

    //==========================
    // - Author:      slf         
    // - Date:        2021/09/12 13:20:53	
    // - Description: 红点组件
    //==========================
    public class ARedDot : MonoBehaviour
    {
        /// <summary>
        /// 红点id
        /// </summary>
        public int DotId;
        public int RedDotId
        {
            get
            {
                return DotId;
            }
            set
            {
                if (value == 0 && DotId != 0)
                {
                    RedDotManager.instance.UnRegisterRedDot(DotId, this);
                }
                DotId = value;
                RedDotManager.instance.RegisterRedDot(DotId, this);
            }
        }


        void Start()
        {
            RedDotId = DotId;
        }

        void OnDestroy()
        {
            RedDotManager.instance.UnRegisterRedDot(DotId, this);
        }

        //刷新红点显示
        public void RefreshActive()
        {
            gameObject.SetActive(RedDotManager.instance.IsActive(DotId));
        }
    }

}
