using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace QuickSheet.Editors
{
    internal class ScriptGenerator
    {
        public static string DefaultIndentation = "    ";
        protected const int CommentWrapLength = 35;

        protected TextWriter m_Writer;
        protected string m_Text;
        protected ScriptPrescription m_Prescription;

        protected string m_Indentation;
        protected int m_IndentLevel = 0;

        protected int IndentLevel
        {
            set
            {
                m_IndentLevel = value;
                m_Indentation = string.Empty;
                for (int i = 0; i < m_IndentLevel; i++)
                    m_Indentation += DefaultIndentation;
            }
        }

        protected string NamespaceName => !string.IsNullOrEmpty(m_Prescription.namespaceName)
                  ? m_Prescription.namespaceName
                  : "Error_Empty_NameSpaceName";

        protected string ClassName => !string.IsNullOrEmpty(m_Prescription.className)
                    ? m_Prescription.className
                    : "Error_Empty_ClassName";

        protected string SpreadSheetName => !string.IsNullOrEmpty(m_Prescription.spreadsheetName)
                    ? m_Prescription.spreadsheetName
                    : "Error_Empty_SpreadSheetName";

        protected string WorkSheetClassName => !string.IsNullOrEmpty(m_Prescription.worksheetClassName)
                    ? m_Prescription.worksheetClassName
                    : "Error_Empty_WorkSheet_ClassName";

        protected string DataClassName => !string.IsNullOrEmpty(m_Prescription.dataClassName)
                    ? m_Prescription.dataClassName
                    : "Error_Empty_DataClassName";

        protected string AssetFileCreateFuncName => !string.IsNullOrEmpty(m_Prescription.assetFileCreateFuncName)
                    ? m_Prescription.assetFileCreateFuncName
                    : "Error_Empty_AssetFileCreateFunc_Name";

        protected string ImportedFilePath => !string.IsNullOrEmpty(m_Prescription.importedFilePath)
                    ? m_Prescription.importedFilePath
                    : "Error_Empty_FilePath";

        protected string AssetFilePath => !string.IsNullOrEmpty(m_Prescription.assetFilepath)
                    ? m_Prescription.assetFilepath
                    : "Error_Empty_AssetFilePath";

        protected string AssetPostprocessorClass => !string.IsNullOrEmpty(m_Prescription.assetPostprocessorClass)
                    ? m_Prescription.assetPostprocessorClass
                    : "Error_Empty_AssetPostprocessorClass";

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScriptGenerator(ScriptPrescription scriptPrescription)
        {
            m_Prescription = scriptPrescription;
        }


        /// <summary>
        /// Replace markdown keywords in the template text file which is currently read in.
        /// </summary>
        public override string ToString()
        {
            m_Text = m_Prescription.template;
            m_Writer = new StringWriter
            {
                NewLine = "\n"
            };

            // Make sure all line endings to be Unix (Mac OSX) format.
            m_Text = Regex.Replace(m_Text, @"\r\n?", delegate (Match m) { return "\n"; });
            m_Text = m_Text.Replace("$NamespaceName", NamespaceName);
            m_Text = m_Text.Replace("$ClassName", ClassName);
            m_Text = m_Text.Replace("$SpreadSheetName", SpreadSheetName);
            m_Text = m_Text.Replace("$WorkSheetClassName", WorkSheetClassName);
            m_Text = m_Text.Replace("$DataClassName", DataClassName);
            m_Text = m_Text.Replace("$AssetFileCreateFuncName", AssetFileCreateFuncName);

            m_Text = m_Text.Replace("$AssetPostprocessorClass", AssetPostprocessorClass);
            m_Text = m_Text.Replace("$IMPORT_PATH", ImportedFilePath);
            m_Text = m_Text.Replace("$ASSET_PATH", AssetFilePath);

            // Other replacements
            foreach (KeyValuePair<string, string> kvp in m_Prescription.mStringReplacements)
            {
                m_Text = m_Text.Replace(kvp.Key, kvp.Value);
            }

            // Do not change tabs to spcaes of the .txt template files.
            Match match = Regex.Match(m_Text, @"(\t*)\$MemberFields");
            if (match.Success)
            {
                // Set indent level to number of tabs before $Functions keyword
                IndentLevel = match.Groups[1].Value.Length;
                if (m_Prescription.memberFields != null)
                {
                    foreach (var field in m_Prescription.memberFields)
                    {
                        WriteMemberField(field);
                        WriteProperty(field);
                        WriteBlankLine();
                    }

                    m_Text = m_Text.Replace(match.Value + "\n", m_Writer.ToString());
                }
            }

            // Return the text of the script
            return m_Text;
        }

        private void PutCurveBracesOnNewLine()
        {
            m_Text = Regex.Replace(m_Text, @"(\t*)(.*) {\n((\t*)\n(\t*))?", delegate (Match match)
            {
                return match.Groups[1].Value + match.Groups[2].Value + "\n" + match.Groups[1].Value + "{\n" +
                   (match.Groups[4].Value == match.Groups[5].Value ? match.Groups[4].Value : match.Groups[3].Value);
            });
        }

        ///
        /// Write a member field of a data class.
        ///
        private void WriteMemberField(MemberFieldData field)
        {
            m_Writer.WriteLine(m_Indentation + "[SerializeField]");

            var fieldName = GetFieldNameForField(field);
            string tmp;
            if (field.type == CellType.Enum)
            {
                tmp = field.Name + " " + fieldName + ";";
            }
            else
            {
                if (field.IsArrayType)
                    tmp = field.Type + "[]" + " " + fieldName + " = new " + field.Type + "[0]" + ";";
                else
                    tmp = field.Type + " " + fieldName + ";";
            }

            m_Writer.WriteLine(m_Indentation + tmp);
        }

        ///
        /// Write a property of a data class.
        ///
        private void WriteProperty(MemberFieldData field)
        {
            string tmp = string.Empty;
            var propertyName = GetPropertyNameForField(field);
            var fieldName = GetFieldNameForField(field);

            if (field.type == CellType.Enum)
            {
                tmp += "public " + field.Name + " " + propertyName + " ";
            }
            else
            {
                tmp = $"/// <summary>\n{m_Indentation}/// {field.Describe} \n{m_Indentation}/// </summary>\n"+ m_Indentation;
                if (field.IsArrayType)
                {
                    tmp += "public " + field.Type + "[]" + " " + propertyName + " ";
                }
                else
                {
                    tmp += "public " + field.Type + " " + propertyName + " ";
                }
            }

            tmp += "{ get => " + fieldName + "; set => " + fieldName + " = value; }";

            m_Writer.WriteLine(m_Indentation + tmp);
        }

        /// <summary>
        /// Override to implement your own field name format.
        /// </summary>
        protected virtual string GetFieldNameForField(MemberFieldData field)
        {
            return field.Name.ToLower();
        }

        /// <summary>
        /// Override to implement your own property name format.
        /// </summary>
        protected virtual string GetPropertyNameForField(MemberFieldData field)
        {
            if (field.type == CellType.Enum)
                return field.Name.ToUpper();

            // To prevent an error can happen when the name of the column header has all lower case characters.
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;
            return ti.ToTitleCase(field.Name);
        }

        /// <summary>
        /// Write a blank line.
        /// </summary>
        protected void WriteBlankLine()
        {
            m_Writer.WriteLine(m_Indentation);
        }

        /// <summary>
        /// Write comment.
        /// </summary>
        /// <param name="comment"></param>
        private void WriteComment(string comment)
        {
            int index = 0;
            while (true)
            {
                if (comment.Length <= index + CommentWrapLength)
                {
                    m_Writer.WriteLine(m_Indentation + "// " + comment.Substring(index));
                    break;
                }
                else
                {
                    int wrapIndex = comment.IndexOf(' ', index + CommentWrapLength);
                    if (wrapIndex < 0)
                    {
                        m_Writer.WriteLine(m_Indentation + "// " + comment.Substring(index));
                        break;
                    }
                    else
                    {
                        m_Writer.WriteLine(m_Indentation + "// " + comment.Substring(index, wrapIndex - index));
                        index = wrapIndex + 1;
                    }
                }
            }
        }
    }
}