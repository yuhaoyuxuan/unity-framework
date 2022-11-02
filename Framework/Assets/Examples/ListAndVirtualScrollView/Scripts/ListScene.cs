using UnityEngine;
using UnityEngine.UI;
using Slf;
/// <summary>
/// 测试虚拟列表和普通列表的drawCall
/// </summary>
public class ListScene : MonoBehaviour
{
    public AVirtualScrollView sv0;//虚拟列表 垂直
    public AVirtualScrollView sv1;//虚拟列表 垂直格子
    public AVirtualScrollView sv2;//虚拟列表 水平
    public AVirtualScrollView sv3;//虚拟列表 水平格子
    public AItemList list;        //普通列表
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        string[] ds = new string[100];
        for (int i = 0; i < 100; i++)
        {
            ds[i] = i + "";
        }

        sv0.RefreshData(ds);
        sv1.RefreshData(ds);
        sv2.RefreshData(ds);
        sv3.RefreshData(ds);
        list.RefreshData(ds);
    }
}
