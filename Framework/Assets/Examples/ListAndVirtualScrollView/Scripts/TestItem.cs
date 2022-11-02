using UnityEngine;
using UnityEngine.UI;
using Slf;

//==========================
// - Author:      slf         
// - Date:        2022/11/02 16:15:16	
// - Description: 测试单项渲染
//==========================
public class TestItem : AItemRenderer<string>
{
    private Text lbl => ComponentFindUtil.Find<Text>("lbl", transform);
    public override void DataChange()
    {
        lbl.text = Data;
    }
}
