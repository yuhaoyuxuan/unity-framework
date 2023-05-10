using System;
using System.Collections.Generic;
using UnityEngine;

//==========================
// - Author:      slf         
// - Description: 本地持久化储存数据
//==========================
[CreateAssetMenu(menuName = "LocalStorageData")]
public class LocalStorageData : ScriptableObject
{
    /// <summary>
    /// 标记序列化文件 int
    /// </summary>
    [SerializeField] private int money;
    public int Money { get => money; set => money = value; }

    /// <summary>
    /// 列表
    /// </summary>
    [SerializeField] private List<int> moneyList;
    public List<int> MoneyList => moneyList;

    /// <summary>
    /// 自定义数据
    /// </summary>
    [SerializeField] private TestLSD test;
    public TestLSD Test { get => test; set => test = value; }

    /// <summary>
    /// 自定义数据 列表
    /// </summary>
    [SerializeField] private List<TestLSD> testList;
    public List<TestLSD> TestList => testList;

}

/// <summary>
/// 自定义数据 要加上可序列化属性
/// </summary>
[Serializable]
public class TestLSD
{
    public int ID;
    public string Name;

    public TestLSD()
    {
        GUID = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// 唯一标识
    /// </summary>
    public string GUID;
    public override int GetHashCode()
    {
        return GUID.GetHashCode();
    }
}
