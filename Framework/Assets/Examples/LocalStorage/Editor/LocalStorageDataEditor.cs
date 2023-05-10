using UnityEditor;
using UnityEngine;

//==========================
// - Author:      slf         
// - Description: 
//==========================
[CustomEditor(typeof(LocalStorageData))]
public class LocalStorageDataEditor : UnityEditor.Editor
{
    SerializedObject targetObject;

    public virtual void OnEnable()
    {
        LocalStorageData data = (LocalStorageData)target;
        targetObject = new SerializedObject(data);
    }

    public override void OnInspectorGUI()
    {
        if (target == null)
            return;

        GUILayout.Space(10);
        if (GUILayout.Button("Reset Initial"))
        {
            if (EditorUtility.DisplayDialog("Reset User Data",
                "User base data will be change to initial value，" +
                "Are you sure you want to reset it",
                "ok",
                "cancel"))
            {
                ResetInitial();
            }
        }

        GUILayout.Space(10);

        base.OnInspectorGUI();
    }

    public void ResetInitial()
    {
        var money = targetObject.FindProperty("money");
        money.intValue = 0;

        var moneyList = targetObject.FindProperty("moneyList");
        moneyList.arraySize = 0;


        var test = targetObject.FindProperty("test");
        test = null;

        var testList = targetObject.FindProperty("testList");
        testList.arraySize = 0;

        targetObject.ApplyModifiedProperties();
    }
}
