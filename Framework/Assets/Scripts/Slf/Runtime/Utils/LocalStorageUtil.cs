using UnityEngine;
using UnityEngine.UI;
using Slf;

//==========================
// - Author:      slf         
// - Date:        2022/10/19 20:15:20
// - Description: 本地存储
//==========================
public class LocalStorageUtil
{
    public static int GetCacheInt(string key)
    {
        int temp = 0;
        if (PlayerPrefs.HasKey(key))
        {
            temp = PlayerPrefs.GetInt(key);
        }
        return temp;
    }
    public static void SetCacheInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }


    public static float GetCacheFloat(string key)
    {
        float temp = 0;
        if (PlayerPrefs.HasKey(key))
        {
            temp = PlayerPrefs.GetFloat(key);
        }
        return temp;
    }
    public static void SetCacheFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }


    public static string GetCacheStr(string key)
    {
        string temp = "";
        if (PlayerPrefs.HasKey(key))
        {
            temp = PlayerPrefs.GetString(key);
        }
        return temp;
    }
    public static void SetCacheStr(string key, string value)
    {
        PlayerPrefs.SetString(key, value);

    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    public static void DeleteCache(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
