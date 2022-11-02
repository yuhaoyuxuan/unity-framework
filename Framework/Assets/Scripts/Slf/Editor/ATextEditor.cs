using UnityEditor.UI;
using UnityEditor;

//==========================
// - Author:      slf         
// - Date:        2021/10/13 16:06:22	
// - Description: 扩展文本
//==========================
//指定我们要自定义编辑器的脚本 
[CustomEditor(typeof(Slf.AText), true)]
//使用了 SerializedObject 和 SerializedProperty 系统，因此，可以自动处理“多对象编辑”，“撤销undo” 和 “预制覆盖prefab override”。
[CanEditMultipleObjects]
public class ATextEditor : TextEditor
{
    //对应我们在MyButton中创建的字段
    //PS:需要注意一点，使用SerializedProperty 必须在AButton类 sound字段前加[SerializeField]
    private SerializedProperty MaxWigth;
    private SerializedProperty Suffix;

    protected override void OnEnable()
    {
        base.OnEnable();
        MaxWigth = serializedObject.FindProperty("MaxWigth");
        Suffix = serializedObject.FindProperty("Suffix");
    }
    //并且特别注意，如果用这种序列化方式，需要在 OnInspectorGUI 开头和结尾各加一句 serializedObject.Update();  serializedObject.ApplyModifiedProperties();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();//空行
        serializedObject.Update();
        EditorGUILayout.PropertyField(MaxWigth);//显示我们创建的属性
        EditorGUILayout.PropertyField(Suffix);//显示我们创建的属性
        serializedObject.ApplyModifiedProperties();
    }
}
