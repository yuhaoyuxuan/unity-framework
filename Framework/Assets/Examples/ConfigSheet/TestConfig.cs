using ConfigSheet;
using UnityEngine;
using UnityEngine.UI;

//==========================
// - Author:      slf         
// - Description: 
//==========================
public class TestConfig : MonoBehaviour
{
    [SerializeField] Button btn;
    // Start is called before the first frame update
    [SerializeField]EntityScriptData configData;
    ConfigData cd;
    void Start()
    {
        btn.onClick.AddListener(LoadConfig);
    }

    void LoadConfig()
    {
        //配置表数据    因为数据没有在rsources文件夹下，无法动态加载
        //EntityScriptData configData = Resources.Load<EntityScriptData>("ConfigSheet/EntityScriptData");

        ParseConfig(configData);
    }

    void ParseConfig(EntityScriptData data)
    {
        cd = new ConfigData(data);
        Debug.Log("解析数据" + data);
        Debug.Log(cd.GoodsConfigDic[1]);
    }

}
