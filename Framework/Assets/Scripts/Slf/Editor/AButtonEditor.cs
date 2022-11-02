using UnityEditor.UI;
using UnityEditor;


//==========================
// - Author:      slf         
// - Date:        2021/09/12 16:06:22	
// - Description: 扩展按钮
//==========================
//指定我们要自定义编辑器的脚本 
[CustomEditor(typeof(Slf.AButton), true)]
//使用了 SerializedObject 和 SerializedProperty 系统，因此，可以自动处理“多对象编辑”，“撤销undo” 和 “预制覆盖prefab override”。
[CanEditMultipleObjects]
public class AButtonEditor : ButtonEditor
{
    //对应我们在MyButton中创建的字段
    //PS:需要注意一点，使用SerializedProperty 必须在AButton类 sound字段前加[SerializeField]
    private SerializedProperty Sound;

    protected override void OnEnable()
    {
        base.OnEnable();
        Sound = serializedObject.FindProperty("Sound");
    }
    //并且特别注意，如果用这种序列化方式，需要在 OnInspectorGUI 开头和结尾各加一句 serializedObject.Update();  serializedObject.ApplyModifiedProperties();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();//空行
        serializedObject.Update();
        EditorGUILayout.PropertyField(Sound);//显示我们创建的属性
        serializedObject.ApplyModifiedProperties();
    }
}
