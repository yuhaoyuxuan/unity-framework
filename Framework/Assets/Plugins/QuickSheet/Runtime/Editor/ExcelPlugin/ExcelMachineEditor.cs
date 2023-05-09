using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuickSheet.Editors
{
    /// <summary>
    /// Custom editor script class for excel file setting.
    /// </summary>
    [CustomEditor(typeof(ExcelMachine))]
    public class ExcelMachineEditor : BaseMachineEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            machine = target as ExcelMachine;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ExcelMachine machine = target as ExcelMachine;

            GUILayout.Label("Excel Spreadsheet Settings:", headerStyle);

            GUILayout.BeginHorizontal();
            GUILayout.Label("File:", GUILayout.Width(50));

            string path = string.Empty;
            if (string.IsNullOrEmpty(machine.excelFilePath))
                path = Application.dataPath;
            else
                path = machine.excelFilePath;

            machine.excelFilePath = GUILayout.TextField(path, GUILayout.Width(250));
            if (GUILayout.Button("...", GUILayout.Width(20)))
            {
                string folder = Path.GetDirectoryName(path);
#if UNITY_EDITOR_WIN
                path = EditorUtility.OpenFilePanel("Open Excel file", folder, "excel files;*.xls;*.xlsx");
#else // for UNITY_EDITOR_OSX
                path = EditorUtility.OpenFilePanel("Open Excel file", folder, "xls");
#endif
                if (path.Length != 0)
                {
                    machine.SpreadSheetName = Path.GetFileName(path);

                    // the path should be relative not absolute one to make it work on any platform.
                    int index = path.IndexOf("Assets");
                    if (index >= 0)
                    {
                        // set relative path
                        machine.excelFilePath = path.Substring(index);

                        // pass absolute path
                        machine.SheetNames = new ExcelQuery(path).GetSheetNames();
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error",
                            @"Wrong folder is selected.
                        Set a folder under the 'Assets' folder! \n
                        The excel file should be anywhere under  the 'Assets' folder", "OK");
                        return;
                    }
                }
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            GUILayout.Label("Class Settings:", headerStyle);
            machine.ScriptNameSpace = EditorGUILayout.TextField("NameSpace: ", machine.ScriptNameSpace);

            EditorGUILayout.Separator();

            GUILayout.Label("Path Settings:", headerStyle);
            machine.TemplatePath = EditorGUILayout.TextField("Template: ", machine.TemplatePath);
            machine.RuntimeClassPath = EditorGUILayout.TextField("Runtime: ", machine.RuntimeClassPath);
            machine.EditorClassPath = EditorGUILayout.TextField("Editor:", machine.EditorClassPath);
            //machine.DataFilePath = EditorGUILayout.TextField("Data:", machine.DataFilePath);

            EditorGUILayout.Separator();

            if (GUILayout.Button("Generate"))
            {
                if (string.IsNullOrEmpty(machine.SpreadSheetName))
                {
                    Debug.LogWarning("No spreadsheet or worksheet is specified.");
                    return;
                }

                Directory.CreateDirectory(Application.dataPath + Path.DirectorySeparatorChar + machine.RuntimeClassPath);
                Directory.CreateDirectory(Application.dataPath + Path.DirectorySeparatorChar + machine.EditorClassPath);

                ScriptPrescription sp = Generate(machine);
                if (sp != null)
                {
                    Debug.Log("Successfully generated!");
                }
                else
                    Debug.LogError("Failed to create a script from excel.");
            }

            EditorGUILayout.Separator();

            // Failed to get sheet name so we just return not to make editor on going.
            if (machine.SheetNames.Length == 0)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Error: Failed to retrieve the specified excel file.");
                EditorGUILayout.LabelField("If the excel file is opened, close it then reopen it again.");
                return;
            }

            // spreadsheet name should be read-only
            EditorGUILayout.TextField("Spreadsheet File: ", machine.SpreadSheetName);

            EditorGUILayout.Space();

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Worksheet: ", GUILayout.Width(100));
                machine.CurrentSheetIndex = EditorGUILayout.Popup(machine.CurrentSheetIndex, machine.SheetNames);

                if (GUILayout.Button("Refresh", GUILayout.Width(60)))
                {
                    // reopen the excel file e.g) new worksheet is added so need to reopen.
                    machine.SheetNames = new ExcelQuery(machine.excelFilePath).GetSheetNames();

                    // one of worksheet was removed, so reset the selected worksheet index
                    // to prevent the index out of range error.
                    if (machine.SheetNames.Length <= machine.CurrentSheetIndex)
                    {
                        machine.CurrentSheetIndex = 0;

                        string message = "Worksheet was changed. Check the 'Worksheet' and 'Update' it again if it is necessary.";
                        EditorUtility.DisplayDialog("Info", message, "OK");
                    }
                }
            }

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();

            if (machine.SheetColumnList != null && machine.SheetColumnList.Count > 0)
            {
                if (GUILayout.Button("Update All"))
                    Import();
                if (GUILayout.Button("Reload All"))
                    Import(true);
            }
            else
            {
                if (GUILayout.Button("Import All"))
                    Import();
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            DrawHeaderSetting(machine);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(machine);
            }
        }

        /// <summary>
        /// Import the specified excel file and prepare to set type of each cell.
        /// </summary>
        protected override void Import(bool reimport = false)
        {
            ExcelMachine machine = target as ExcelMachine;
            string path = machine.excelFilePath;

            if (string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog("Error",
                    "You should specify spreadsheet file first!",
                    "OK");
                return;
            }

            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog("Error",
                    string.Format("File at {0} does not exist.", path),
                    "OK");
                return;
            }

            foreach (var sheet in machine.SheetNames)
            {
                ImportSheet(sheet, reimport);
            }

            EditorUtility.SetDirty(machine);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Import one sheet of the specified excel file 
        /// and prepare to set type of each cell.
        /// </summary>
        protected override void ImportSheet(string sheet, bool reimport = false)
        {
            ExcelMachine machine = target as ExcelMachine;
            string path = machine.excelFilePath;

            ImportIntenal(path, sheet, reimport);

            EditorUtility.SetDirty(machine);
            AssetDatabase.SaveAssets();
        }

        protected override void DeleteSheet(string sheet)
        {
            ExcelMachine machine = target as ExcelMachine;

            var sheetColumn = machine.SheetColumnList.FirstOrDefault(i => i.sheet.Equals(sheet));
            if (sheetColumn != null)
            {
                machine.SheetColumnList.Remove(sheetColumn);

                EditorUtility.SetDirty(machine);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Generate AssetPostprocessor editor script file.
        /// </summary>
        protected override void CreateAssetCreationScript(BaseMachine m, ScriptPrescription sp)
        {
            ExcelMachine machine = target as ExcelMachine;

            sp.className = "EntityScriptData";

            // where the imported excel file is.
            sp.importedFilePath = machine.excelFilePath;

            // path where the .asset file will be created.
            string path = machine.excelFilePath.Remove(machine.excelFilePath.LastIndexOf("/"));
            path += "/" + sp.className + ".asset";
            sp.assetFilepath = path;
            sp.assetPostprocessorClass = "EntityAssetPostprocessor";
            sp.template = GetTemplate("PostProcessor");

            // write a script to the given folder.
            using var writer = new StreamWriter(TargetPathForAssetPostProcessorFile(sp.assetPostprocessorClass));
            writer.Write(new AssetPostProcessorGenerator(sp).ToString());
            writer.Close();
        }

        private void ImportIntenal(string path, string sheet, bool reimport = false)
        {
            int startRowIndex = 1;  //标识
            int typeRowIndex = 2;   //类型
            string error = string.Empty;
            var titles = new ExcelQuery(path, sheet).GetTitle(startRowIndex, ref error);
            var titleTypes = new ExcelQuery(path, sheet).GetTitle(typeRowIndex, ref error);
            if (titles == null || !string.IsNullOrEmpty(error))
            {
                EditorUtility.DisplayDialog("Error", error, "OK");
                return;
            }
            else
            {
                // check the column header is valid
                foreach (string column in titles)
                {
                    if (!IsValidHeader(column))
                    {
                        error = string.Format($"Invalid column header name {column}. Any c# keyword should not be used for column header. Note it is not case sensitive.");
                        EditorUtility.DisplayDialog("Error", error, "OK");
                        return;
                    }
                }
            }

            List<string> titleList = titles.ToList();
            List<string> titleTypeList = titleTypes.ToList();
            if (machine.HasColumnHeader(sheet) && reimport == false)
            {
                var sheetColumn = machine.SheetColumnList
                    .FirstOrDefault(i => i.sheet.Equals(sheet));
                var headerDic = sheetColumn
                    .columnHeaderList
                    .ToDictionary(header => header.name);

                // collect non-changed column headers
                var exist = titleList.Select(t => GetColumnHeaderString(t))
                    .Where(e => headerDic.ContainsKey(e))
                    .Select(t => new ColumnHeader { name = t, type = headerDic[t].type, isArray = headerDic[t].isArray, OrderNO = headerDic[t].OrderNO });

                // collect newly added or changed column headers
                var changed = titleList.Select(t => GetColumnHeaderString(t))
                    .Where(e => headerDic.ContainsKey(e) == false)
                    .Select(t => ParseColumnHeader(t, titleList.IndexOf(t), titleTypeList[titleList.IndexOf(t)]));

                // merge two list via LINQ
                var merged = exist.Union(changed).OrderBy(x => x.OrderNO);

                sheetColumn.columnHeaderList.Clear();
                sheetColumn.columnHeaderList = merged.ToList();
            }
            else
            {
                if (titleList.Count > 0)
                {
                    var sheetColumn = machine.SheetColumnList.FirstOrDefault(i => i.sheet.Equals(sheet));
                    if (sheetColumn == null)
                    {
                        sheetColumn = new SheetColumnHeader
                        {
                            sheet = sheet
                        };
                        machine.SheetColumnList.Add(sheetColumn);
                    }
                    int order = 0;
                    sheetColumn.columnHeaderList = titleList.Select(e => ParseColumnHeader(e, order++, titleTypeList[order-1])).ToList();
                }
                else
                {
                    string msg = string.Format($"An empty worksheet: [{sheet}]");
                    Debug.LogWarning(msg);
                }
            }
        }


    }
}
