using System.IO;
using UnityEngine;
using UnityEngine.UI;

//==========================
// - Author:      slf         
// - Description: 
//==========================
public class LocalStorageDataTest : MonoBehaviour
{
    [SerializeField] Button btn;
    // Start is called before the first frame update

    /// <summary>
    /// asset文件路径
    /// </summary>
    string sourcePath = "Local Storage Data";

    [SerializeField] LocalStorageData source;

    [SerializeField] Text text;

    /// <summary>
    /// 本地化储存的数据路径
    /// </summary>
    string dataSavePath = "LocalStorageData.json";

    LocalStorageData localStorageData;
    bool isInit;
    object lockObj = new object();
    void Start()
    {
        dataSavePath = Application.persistentDataPath + "/" + dataSavePath;
        btn.onClick.AddListener(OnAdd);

        //LocalStorageData data = Resources.Load(sourcePath) as LocalStorageData;
        //LoadDataComplete(data);
        LoadDataComplete(source);
    }


    void LoadDataComplete(LocalStorageData data)
    {
        localStorageData = data;
        //发布包 改为读取本地持久化数据
#if !UNITY_EDITOR
        if (File.Exists(dataSavePath))
        {
            string json = File.ReadAllText(dataSavePath);
            JsonUtility.FromJsonOverwrite(json, localStorageData);
        }
        isInit = true;
        InvokeRepeating("SaveUserData", 0, 5);
#endif
        OnAdd();
    }

    private void OnAdd()
    {
        localStorageData.Money += 10;
        text.text = localStorageData.Money.ToString();
    }

    /// <summary>
    /// 持久化储存数据 保存本地
    /// </summary>
    private void SaveUserData()
    {
        Debug.Log("储存数据");
        if (!isInit)
        {
            return;
        }
        lock (lockObj)
        {
            string userDataJSON = JsonUtility.ToJson(localStorageData);
            File.WriteAllText(dataSavePath, userDataJSON);
        }
    }
}
