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
}
