using Slf;
using UnityEngine;
using UnityEngine.UI;

namespace TestNameSpace
{
    //==========================
    // - Author:      slf         
    // - Description: 
    //==========================
    public class TestUI2 : UIBase
    {
        private AButton btn => ComponentFindUtil.Find<AButton>("btn", transform);

        public override void InitEvent()
        {
            base.InitEvent();
            btn.SetClickCallback(OnTap);
        }

        private void OnTap()
        {
            Close();
        }
    }

}