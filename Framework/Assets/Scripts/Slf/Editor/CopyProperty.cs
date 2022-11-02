
using UnityEngine;
using UnityEditor;
using Slf;
using UnityEngine.UI;

//命名规则
//go = GameObject
//img = Image
//lbl = Text
//btn = AButton
//item = AItemRenderer
//list = AItemList
//mono = AmonoBehaviour 子类

//==========================
// - Author:      slf         
// - Date:        2021/09/13 10:47:21	
// - Description: 复制当前预制体属性 命名规则
//==========================
public class CopyProperty : MonoBehaviour
{

    static GameObject root;
    static string tempStr;

    [MenuItem("Tools/Copy Perfab property")]
    static void copyP()//复制
    {
        tempStr = "";
        root = Selection.activeGameObject;
        parse(root);
        copy(tempStr);
    }
    [MenuItem("GameObject/Copy Perfab property", priority = 0)]
    static void copyPs()//复制
    {
        tempStr = "";
        root = Selection.activeGameObject;
        parse(root);
        copy(tempStr);
    }

    static void parse(GameObject gameObject)
    {
        string name = gameObject.name;

        if (name.StartsWith("go"))
        {
            parseObj(gameObject);
        }
        else if (name.StartsWith("img"))
        {
            parseComponent<Image>(gameObject, "Image");
        }
        else if (name.StartsWith("lbl"))
        {
            AText type = parseComponent<AText>(gameObject, "AText");
            if(type == null)
            {
                parseComponent<Text>(gameObject,"Text");
            }
        }
        else if (name.StartsWith("tog"))
        {
            parseComponent<AToggle>(gameObject, "AToggle");
        }
        else if (name.StartsWith("btn"))
        {
            parseComponent<AButton>(gameObject, "AButton");
        }
        else if (name.StartsWith("item"))
        {
            parseItem(gameObject);
        }
        else if (name.StartsWith("list"))
        {
            parseComponent<AItemList>(gameObject, "AItemList");
        }
        else if (name.StartsWith("mono"))
        {
            parseMono(gameObject);
        }

        int len = gameObject.transform.childCount;
        while (--len >= 0)
        {
            parse(gameObject.transform.GetChild(len).gameObject);
        }
    }


    static T findType<T>(GameObject gameObject)
    {
        T type = gameObject.GetComponent<T>();
        if (type != null)
        {
            return type;
        }
        return default(T);
    }

    /// <summary>
    /// 解析组件
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="type"></param>
    static T parseComponent<T>(GameObject gameObject , string strType)
    {
        T type = findType<T>(gameObject);
        if (type != null)
        {

            string str = "private "+ strType + " " + gameObject.name + " => ComponentFindUtil.Find<" + strType + ">(\"" + getPath(gameObject) + "\",transform);";
            writeStr(str);
            return type;
        }
        return default(T);
	}

    static void parseObj(GameObject gameObject)
    {
        string str = "private GameObject " + gameObject.name + " => ComponentFindUtil.Find(\"" + getPath(gameObject) + "\",transform);";
        writeStr(str);
    }

    static void parseItem(GameObject gameObject)
    {
        AMonoBehaviour[] games = gameObject.GetComponents<AMonoBehaviour>();
        string name;
        string nameT;
        for (int i = 0; i < games.Length; i++)
        {
            name = games[i].GetType().BaseType.ToString();
            if (name.StartsWith("AItemRenderer"))
            {
                nameT = name.Split('[')[1].Split(']')[0];
                string str = "private AItemRenderer<" + nameT + "> " + gameObject.name + " => ComponentFindUtil.Find<AItemRenderer<" + nameT + ">>(\"" + getPath(gameObject) + "\",transform);";
                writeStr(str);
                return;
            }
        }
    }

    static void parseMono(GameObject gameObject)
    {
        AMonoBehaviour[] games = gameObject.GetComponents<AMonoBehaviour>();
        string name;
        string nameT;
        for (int i = 0; i < games.Length; i++)
        {
            name = games[i].ToString();
            nameT = name.Split('(')[1].Split(')')[0];
            string str = "private " + nameT + " " + gameObject.name + " => ComponentFindUtil.Find<" + nameT + ">(\"" + getPath(gameObject) + "\",transform);";
            writeStr(str);
            return;
        }
    }


    static string getPath(GameObject gameObject)
    {
        string path = gameObject.name;
        GameObject temp = gameObject.transform.parent.gameObject;
        while (temp != root)
        {
            path = temp.name + "/" + path;
            temp = temp.transform.parent.gameObject;
        }
        return path;
    }

    static void writeStr(string str)
    {
        tempStr += str + "\r\n";
    }

    // 将信息复制到剪切板当中
    static void copy(string str)
    {
        TextEditor editor = new TextEditor();
        editor.content = new GUIContent(str);
        editor.OnFocus();
        editor.Copy();
        Debug.Log(str);
    }
}
