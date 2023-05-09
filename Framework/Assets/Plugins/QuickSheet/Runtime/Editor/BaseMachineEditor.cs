using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Graphs;

namespace QuickSheet.Editors
{
    /// <summary>
    /// A base class for a spreadsheet import setting.
    /// </summary>
    [CustomEditor(typeof(BaseMachine))]
    public class BaseMachineEditor : Editor
    {
        //all c# keywords.
        protected static string[] Keywords = new string[] {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
            "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum",
            "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto",
            "if", "implicit", "in", "in", "int", "interface", "internal", "is", "lock", "long",
            "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected",
            "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static",
            "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
            "ushort", "using", "virtual", "void", "volatile", "while",
        };

        protected BaseMachine machine;

        protected readonly string NoTemplateString = "No Template file is found";

        protected GUIStyle headerStyle = null;

        protected readonly Dictionary<string, bool> foldoutDist = new();

        protected virtual void OnEnable()
        {
            // Nothing to do here.
        }

        public override void OnInspectorGUI()
        {
            if (headerStyle == null)
            {
                headerStyle = GUIHelper.MakeHeader();
            }
        }

        /// <summary>
        /// It should be overried and implemented in the derived class
        /// </summary>
        protected virtual void Import(bool reimport = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// It should be overried and implemented in the derived class
        /// </summary>
        protected virtual void ImportSheet(string sheet, bool reimport = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// It should be overried and implemented in the derived class
        /// </summary>
        protected virtual void DeleteSheet(string sheet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check the given header column has valid name which should not be any c# keywords.
        /// </summary>
        protected bool IsValidHeader(string s)
        {
            // no case sensitive!
            string comp = s.ToLower();

            string found = Array.Find(Keywords, x => x == comp);
            if (string.IsNullOrEmpty(found))
                return true;

            return false;
        }

        /// <summary>
        /// Generate script files with the given templates.
        /// Total four files are generated, two for runtime and others for editor.
        /// </summary>
        protected virtual ScriptPrescription Generate(BaseMachine m)
        {
            if (m == null)
                return null;

            ScriptPrescription sp = new();
            CreateEntityClassScript(m, sp);
            CreateScriptableObjectClassScript(m, sp);
            CreateScriptableObjectEditorClassScript(m, sp);
            CreateAssetCreationScript(m, sp);

            AssetDatabase.Refresh();

            return sp;
        }

        /// <summary>
        /// Create a ScriptableObject class and write it down on the specified folder.
        /// </summary>
        protected void CreateScriptableObjectClassScript(BaseMachine machine, ScriptPrescription sp)
        {
            sp.className = "EntityScriptData";
            sp.namespaceName = machine.ScriptNameSpace;
            sp.template = GetTemplate("ScriptableObjectClass");

            // check the directory path exists
            string fullPath = TargetPathForClassScript(sp.className);
            string folderPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folderPath))
            {
                EditorUtility.DisplayDialog(
                    "Warning",
                    "The folder for runtime script files does not exist. Check the path " + folderPath + " exists.",
                    "OK");
                return;
            }

            StreamWriter writer = null;
            try
            {
                sp.mStringReplacements.Clear();
                foreach (var s in machine.SheetColumnList)
                {
                    sp.mStringReplacements.Add(s.sheet, s.sheet);
                }

                // write a script to the given folder.		
                writer = new StreamWriter(fullPath);
                writer.Write(new ScriptableObjectGenerator(sp).ToString());
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }


        /// <summary>
        /// Create a ScriptableObject editor class and write it down on the specified folder.
        /// </summary>
        protected void CreateScriptableObjectEditorClassScript(BaseMachine machine, ScriptPrescription sp)
        {
            sp.className = "EntityScriptDataEditor";
            sp.namespaceName = machine.ScriptNameSpace;
            sp.worksheetClassName = "EntityScriptData";
            sp.template = GetTemplate("ScriptableObjectEditorClass");

            // check the directory path exists
            string fullPath = TargetPathForEditorScript(sp.className);
            string folderPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folderPath))
            {
                EditorUtility.DisplayDialog(
                    "Warning",
                    "The folder for editor script files does not exist. Check the path " + folderPath + " exists.",
                    "OK");
                return;
            }

            StreamWriter writer = null;
            try
            {
                sp.mStringReplacements.Clear();
                foreach (var s in machine.SheetColumnList)
                {
                    sp.mStringReplacements.Add(s.sheet, s.sheet);
                }

                // write a script to the given folder.		
                writer = new StreamWriter(fullPath);
                writer.Write(new ScriptableObjectEditorGenerator(sp).ToString());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        /// <summary>
        /// Create a entity class which describes the spreadsheet and write it down on the specified folder.
        /// </summary>
        protected void CreateEntityClassScript(BaseMachine machine, ScriptPrescription sp)
        {
            sp.namespaceName = machine.ScriptNameSpace;
            foreach (var sheet in machine.SheetColumnList)
            {
                // check the directory path exists
                string fullPath = TargetPathForEntity(sheet.sheet);// TargetPathForData(machine.WorkSheetName);
                string folderPath = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(folderPath))
                {
                    EditorUtility.DisplayDialog(
                        "Warning",
                        "The folder for runtime script files does not exist. Check the path " + folderPath + " exists.",
                        "OK");
                    return;
                }

                List<MemberFieldData> fieldList = new List<MemberFieldData>();

                var temp = (ExcelMachine)machine;
                string error = string.Empty;
                //字段描述
                var titleDesc = new ExcelQuery(temp.excelFilePath, sheet.sheet).GetTitle(0, ref error).ToList<string>();
                int idx = 0;
                //FIXME: replace ValueType to CellType and support Enum type.
                foreach (ColumnHeader header in sheet.columnHeaderList)
                {
                    MemberFieldData member = new()
                    {
                        Name = header.name,
                        type = header.type,
                        IsArrayType = header.isArray,
                        Describe = titleDesc[idx]
                    };
                    idx++;

                    fieldList.Add(member);
                }

                //  sp.className = machine.WorkSheetName + "Data";
                sp.className = sheet.sheet;
                sp.template = GetTemplate("EntityClass");

                sp.memberFields = fieldList.ToArray();

                // write a script to the given folder.		
                using (var writer = new StreamWriter(fullPath))
                {

                    var str = new ScriptGenerator(sp).ToString();
                    writer.Write(str);
                    writer.Close();
                }
            }
        }

        protected virtual void CreateAssetCreationScript(BaseMachine m, ScriptPrescription sp)
        {
            Debug.LogWarning("!!! It should be implemented in the derived class !!!");
        }

        /// <summary>
        /// e.g. "Assets/Script/Data/Runtime/Item.cs"
        /// </summary>
        protected string TargetPathForClassScript(string worksheetName)
        {
            return Path.Combine("Assets/" + machine.RuntimeClassPath, worksheetName + "." + "cs");
        }

        /// <summary>
        /// e.g. "Assets/Script/Data/Editor/ItemEditor.cs"
        /// </summary>
        protected string TargetPathForEditorScript(string worksheetName)
        {
            return Path.Combine("Assets/" + machine.EditorClassPath, worksheetName + "." + "cs");
        }

        /// <summary>
        /// entity class script file has 'WorkSheetNameData' for its filename.
        /// e.g. "Assets/Script/Data/Runtime/ItemData.cs"
        /// </summary>
        protected string TargetPathForEntity(string worksheetName)
        {
            return Path.Combine("Assets/" + machine.RuntimeClassPath, worksheetName + "." + "cs");
        }

        /// <summary>
        /// e.g. "Assets/Script/Data/Editor/ItemAssetCreator.cs"
        /// </summary>
        protected string TargetPathForAssetFileCreateFunc(string worksheetName)
        {
            return Path.Combine("Assets/" + machine.EditorClassPath, worksheetName + "AssetCreator" + "." + "cs");
        }

        /// <summary>
        /// AssetPostprocessor class should be under "Editor" folder.
        /// </summary>
        protected string TargetPathForAssetPostProcessorFile(string worksheetName)
        {
            return Path.Combine("Assets/" + machine.EditorClassPath, worksheetName + "." + "cs");
        }

        /// <summary>
        /// Retrieves all ascii text in the given template file.
        /// </summary>
        protected string GetTemplate(string nameWithoutExtension)
        {
            string path = Path.Combine(GetAbsoluteCustomTemplatePath(), nameWithoutExtension + ".txt");
            if (File.Exists(path))
                return File.ReadAllText(path);

            path = Path.Combine(GetAbsoluteBuiltinTemplatePath(), nameWithoutExtension + ".txt");
            if (File.Exists(path))
                return File.ReadAllText(path);

            return NoTemplateString;
        }

        /// <summary>
        /// e.g. "Assets/QuickSheet/Templates"
        /// </summary>
        protected string GetAbsoluteCustomTemplatePath()
        {
            return Path.Combine(Application.dataPath, machine.TemplatePath);
        }

        /// <summary>
        /// e.g. "C:/Program File(x86)/Unity/Editor/Data"
        /// </summary>
        protected string GetAbsoluteBuiltinTemplatePath()
        {
            return Path.Combine(EditorApplication.applicationContentsPath, machine.TemplatePath);
        }


        /// <summary>
        /// Draw column headers on the Inspector view.
        /// </summary>
        protected void DrawHeaderSetting(BaseMachine m)
        {
            Color defaultColor = GUI.color;
            foreach (var sheet in m.SheetColumnList)
            {
                if (string.IsNullOrEmpty(sheet.sheet))
                    continue;

                EditorGUILayout.Separator();
                if (!foldoutDist.ContainsKey(sheet.sheet))
                {
                    foldoutDist[sheet.sheet] = true;
                }

                using (new GUILayout.HorizontalScope())
                {
                    foldoutDist[sheet.sheet] = EditorGUILayout.Foldout(foldoutDist[sheet.sheet], "Type Setting: ");
                    GUILayout.Space(30);
                    GUI.color = Color.cyan;
                    GUILayout.Label(sheet.sheet, EditorStyles.boldLabel);
                    GUI.color = defaultColor;
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Update", GUILayout.MaxWidth(65)))
                    {
                        ImportSheet(sheet.sheet);
                    }
                    if (GUILayout.Button("Reload", GUILayout.MaxWidth(65)))
                    {
                        if (EditorUtility.DisplayDialog("Warning", string.Format("Do you want to reload [{0}] setting?", sheet.sheet), "Ok", "Cancel"))
                        {
                            ImportSheet(sheet.sheet, true);
                        }
                    }
                    if (GUILayout.Button("Delete", GUILayout.MaxWidth(65)))
                    {
                        if (EditorUtility.DisplayDialog("Warning", string.Format("Do you want to delete [{0}] setting?", sheet.sheet), "Ok", "Cancel"))
                        {
                            DeleteSheet(sheet.sheet);
                        }
                    }
                }

                GUILayout.Space(5);
                if (foldoutDist[sheet.sheet])
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    // Title
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Member", EditorStyles.boldLabel, GUILayout.MinWidth(100));
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.MinWidth(80));
                        GUILayout.Label("Array", EditorStyles.boldLabel, GUILayout.MinWidth(40));
                    }

                    // Each cells
                    foreach (ColumnHeader header in sheet.columnHeaderList)
                    {
                        GUILayout.BeginHorizontal("box");
                        GUILayout.Space(5);
                        // show member field with label, read-only
                        EditorGUILayout.LabelField(header.name, GUILayout.MinWidth(100));
                        GUILayout.FlexibleSpace();

                        // specify type with enum-popup
                        var type = (CellType)EditorGUILayout.EnumPopup(header.type, GUILayout.Width(100));
                        if (type != header.type)
                        {
                            header.type = type;
                            EditorUtility.SetDirty(machine);
                        }

                        GUILayout.Space(10);
                        // array toggle
                        var isArray = EditorGUILayout.Toggle(header.isArray, GUILayout.Width(20));
                        if (header.isArray != isArray)
                        {
                            header.isArray = isArray;
                            EditorUtility.SetDirty(m);
                        }

                        GUILayout.Space(5);
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();
                }
            }
        }

        /// <summary>
        /// Extract only column-header string from the given string if it contains '|' dilimeter.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected string GetColumnHeaderString(string s)
        {
            if (s.Contains('|'))
            {
                string substr = s.Substring(0, s.IndexOf('|'));
                return substr;
            }

            return s;
        }

        /// <summary>
        /// Try to parse column-header if it contains '|'. Note postfix '!' means it has array type.
        /// e.g) 'Skill|string': Skill is string type.
        ///      'MyArray | int!' : MyArray is int array type. 
        /// </summary>
        /// <param name="s">A column header string in the spreadsheet.</param>
        /// <param name="order">A order number to sort column header.</param>
        /// <returns>A newly created ColumnHeader class instance.</returns>
        protected ColumnHeader ParseColumnHeader(string columnheader, int order,string sType = "")
        {
            // remove all white space. e.g.) "SkillLevel | uint"
            string cHeader = new(columnheader.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());

            CellType ctype = CellType.Undefined;
            bool bArray = false;
            if (cHeader.Contains('|'))
            {
                // retrive columnheader name.
                string substr = cHeader;
                bArray = cHeader.Contains("!");
                substr = cHeader.Substring(0, cHeader.IndexOf('|'));

                // retrieve CellType from the columnheader.
                int startIndex = cHeader.IndexOf('|') + 1;
                int length = cHeader.Length - cHeader.IndexOf('|') - (bArray ? 2 : 1);
                string strType = cHeader.Substring(startIndex, length).ToLower();
                ctype = (CellType)Enum.Parse(typeof(CellType), strType, true);

                return new ColumnHeader { name = substr, type = ctype, isArray = bArray, OrderNO = order };
            }

            if (!string.IsNullOrEmpty(sType))
            {
                if(sType.IndexOf(",Array") != -1)
                {
                    return new ColumnHeader { name = cHeader, type = (CellType)Enum.Parse(typeof(CellType), sType.Split(",")[0], true), isArray = true, OrderNO = order };
                }

                return new ColumnHeader { name = cHeader, type = (CellType)Enum.Parse(typeof(CellType), sType, true), OrderNO = order };
            }

            return new ColumnHeader { name = cHeader, type = CellType.Undefined, OrderNO = order };
        }
    }
}