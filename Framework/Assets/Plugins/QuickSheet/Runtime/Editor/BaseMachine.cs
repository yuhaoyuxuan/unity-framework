using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace QuickSheet.Editors
{
    /// <summary>
    /// A class which represents column header on the worksheet.
    /// </summary>
    [Serializable]
    public class ColumnHeader
    {
        public CellType type;
        public string name;
        public bool isEnable;
        public bool isArray;
        public ColumnHeader nextArrayItem;

        // used to order columns by ascending. (only need on excel-plugin)
        public int OrderNO { get; set; }
    }

    /// <summary>
    /// A class which represents sheet name and sheet's ColumnHeader list
    /// </summary>
    [Serializable]
    public class SheetColumnHeader
    {
        public string sheet;
        public List<ColumnHeader> columnHeaderList = new();
    }

    /// <summary>
    /// A class which stores various settings for a worksheet which is imported.
    /// </summary>
    public class BaseMachine : ScriptableObject
    {
        protected readonly static string ImportSettingFilename = "ImportEntityScriptData.asset";

        [SerializeField]
        private string templatePath = "QuickSheet/Templates";
        public string TemplatePath
        {
            get => templatePath;
            set => templatePath = value;
        }

        /// <summary>
        /// namespace of the created class file.
        /// </summary>
        [SerializeField]
        private string scriptNameSpace;
        public string ScriptNameSpace
        {
            get => scriptNameSpace;
            set => scriptNameSpace = value;
        }

        /// <summary>
        /// path the created ScriptableObject class file will be located.
        /// </summary>
        [SerializeField]
        private string scriptFilePath;
        public string RuntimeClassPath
        {
            get => scriptFilePath;
            set => scriptFilePath = value;
        }

        /// <summary>
        /// path the created editor script files will be located.
        /// </summary>
        [SerializeField]
        private string editorScriptFilePath;
        public string EditorClassPath
        {
            get => editorScriptFilePath;
            set => editorScriptFilePath = value;
        }

        [SerializeField]
        private string sheetName;
        public string SpreadSheetName
        {
            get => sheetName;
            set => sheetName = value;
        }

        public List<SheetColumnHeader> SheetColumnList
        {
            get => sheetColumnList;
            set => sheetColumnList = value;
        }

        [SerializeField]
        protected List<SheetColumnHeader> sheetColumnList;


        /// <summary>
        /// Return true, if the list is instantiated and has any its item more than one.
        /// </summary>
        /// <returns></returns>
        public bool HasColumnHeader(string sheet)
        {
            if (sheetColumnList != null && sheetColumnList.Exists(i => !string.IsNullOrEmpty(i.sheet) && i.sheet.Equals(sheet)))
                return true;

            return false;
        }

        readonly string DEFAULT_CLASS_PATH = "Scripts/Runtime";
        readonly string DEFAULT_EDITOR_PATH = "Scripts/Editor";

        protected void OnEnable()
        {
            if (sheetColumnList == null)
                sheetColumnList = new List<SheetColumnHeader>();
        }

        /// <summary>
        /// Initialize with default value whenever the asset file is enabled.
        /// </summary>
        public void ReInitialize()
        {
            if (string.IsNullOrEmpty(RuntimeClassPath))
                RuntimeClassPath = DEFAULT_CLASS_PATH;
            if (string.IsNullOrEmpty(EditorClassPath))
                EditorClassPath = DEFAULT_EDITOR_PATH;
        }
    }
}
