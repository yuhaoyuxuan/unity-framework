using UnityEditor;
using UnityEditor.UI;

//==========================
// - Author:      slf         
// - Date:        2022/10/31 15:30:22	
// - Description: 虚拟列表
//==========================
//指定我们要自定义编辑器的脚本 
[CustomEditor(typeof(Slf.AVirtualScrollView), true)]
//使用了 SerializedObject 和 SerializedProperty 系统，因此，可以自动处理“多对象编辑”，“撤销undo” 和 “预制覆盖prefab override”。
[CanEditMultipleObjects]
public class AVirtualScrollViewEditor : ScrollRectEditor
{
    //PS:需要注意一点，使用SerializedProperty 必须在AVirtualScrollView类 ItemRenderer字段前加[SerializeField]
    private SerializedProperty ItemRenderer;

    protected override void OnEnable()
    {
        base.OnEnable();
        ItemRenderer = serializedObject.FindProperty("ItemRenderer");
    }
    //并且特别注意，如果用这种序列化方式，需要在 OnInspectorGUI 开头和结尾各加一句 serializedObject.Update();  serializedObject.ApplyModifiedProperties();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();//空行
        serializedObject.Update();
        EditorGUILayout.PropertyField(ItemRenderer);//显示我们创建的属性
        serializedObject.ApplyModifiedProperties();
    }
}
