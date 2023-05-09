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
        [SerializeField] private int dotId;
        public int RedDotId
        {
            get
            {
                return dotId;
            }
            set
            {
                if (value == 0 && dotId != 0)
                {
                    RedDotManager.Instance.UnRegisterRedDot(dotId, this);
                }
                dotId = value;
                RedDotManager.Instance.RegisterRedDot(dotId, this);
            }
        }


        void Start()
        {
            RedDotId = dotId;
        }

        void OnDestroy()
        {
            RedDotManager.Instance.UnRegisterRedDot(dotId, this);
        }

        //刷新红点显示
        public void RefreshActive()
        {
            gameObject.SetActive(RedDotManager.Instance.IsActive(dotId));
        }
    }

}
