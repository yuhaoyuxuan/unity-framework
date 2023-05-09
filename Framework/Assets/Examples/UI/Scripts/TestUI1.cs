using Slf;
using UnityEngine;
using UnityEngine.UI;

//==========================
// - Author:      slf         
// - Description: 
//==========================
public class TestUI1 : UIBase
{
    private AButton btn => ComponentFindUtil.Find<AButton>("btn", transform);

    public override void InitEvent()
    {
        base.InitEvent();

        btn.SetClickCallback(OnTap);
    }

    private void OnTap()
    {
        UIManager.Instance.ShowView(2);
    }
}
