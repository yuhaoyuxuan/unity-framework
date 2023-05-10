using UnityEngine;
using UnityEngine.UI;
using Slf;

//==========================
// - Author:      slf         
// - Date:        #CreateTime#
// - Description: 
//==========================
public class UIScene : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]Button btn;
    void Start()
    {
        UIManager.Instance.Init((RectTransform)transform);

        UIManager.Instance.UIController.Register(new UIData(2, "TestNameSpace.TestUI2", "Prefabs/TestUI2", LayerType.Panel, PopupType.MinToMax,  true,true));
        btn.onClick.AddListener(() => {
            UIManager.Instance.ShowView(1);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RedDotManager.Instance.SetAcive(1, !RedDotManager.Instance.CheckState(1));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            RedDotManager.Instance.SetAcive(2, !RedDotManager.Instance.CheckState(2));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RedDotManager.Instance.SetAcive(3, !RedDotManager.Instance.CheckState(3));
        }
    }
}
