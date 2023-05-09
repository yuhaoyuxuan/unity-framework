using System.IO;
using System.Text.RegularExpressions;

namespace QuickSheet.Editors
{
    internal class ScriptableObjectGenerator : ScriptGenerator
    {
        public ScriptableObjectGenerator(ScriptPrescription scriptPrescription)
            : base(scriptPrescription)
        {
        }

        public override string ToString()
        {
            m_Text = base.ToString();

            WriteTempCode(@"(\t*)\$EntityClassArrayMembers", "public {0}[] {0}DataArray;");
            WriteTempCode(@"(\t*)\$InitDataClassDataArray", "if ({0}DataArray == null) {0}DataArray = new {0}[0];");

            return m_Text;
        }

        public void WriteTempCode(string matchKey, string format)
        {
            m_Writer = new StringWriter
            {
                NewLine = "\n"
            };
            // Do not change tabs to spcaes of the .txt template files.
            var match = Regex.Match(m_Text, matchKey);
            if (match.Success)
            {
                // Set indent level to number of tabs before $Functions keyword
                IndentLevel = match.Groups[1].Value.Length;
                if (m_Prescription.mStringReplacements != null)
                {
                    foreach (var sheet in m_Prescription.mStringReplacements.Values)
                    {
                        m_Writer.WriteLine(m_Indentation + string.Format(format, sheet));
                        WriteBlankLine();
                    }

                    m_Text = m_Text.Replace(match.Value + "\n", m_Writer.ToString());
                }
            }
        }
    }

    internal class ScriptableObjectEditorGenerator : ScriptGenerator
    {
        public ScriptableObjectEditorGenerator(ScriptPrescription scriptPrescription)
            : base(scriptPrescription)
        {
        }

        public override string ToString()
        {
            m_Text = base.ToString();

            m_Writer = new StringWriter
            {
                NewLine = "\n"
            };

            // Do not change tabs to spcaes of the .txt template files.
            var match = Regex.Match(m_Text, @"(\t*)\$LoadAndSetDataClassDataArray");
            if (match.Success)
            {
                // Set indent level to number of tabs before $Functions keyword
                IndentLevel = match.Groups[1].Value.Length;
                if (m_Prescription.mStringReplacements != null)
                {
                    foreach (var sheet in m_Prescription.mStringReplacements.Values)
                    {
                        m_Writer.WriteLine(m_Indentation + string.Format("ExcelQuery query{0} = new(path, \"{0}\");", sheet));
                        m_Writer.WriteLine(m_Indentation + string.Format("if (query{0} != null && query{0}.IsValid())", sheet));
                        m_Writer.WriteLine(m_Indentation + "{");
                        m_Writer.WriteLine(m_Indentation + string.Format("    targetData.{0}DataArray = query{0}.Deserialize<{0}>(3).ToArray();", sheet));
                        m_Writer.WriteLine(m_Indentation + "    EditorUtility.SetDirty(targetData);");
                        m_Writer.WriteLine(m_Indentation + "    AssetDatabase.SaveAssets();");
                        m_Writer.WriteLine(m_Indentation + "}");
                        m_Writer.WriteLine(m_Indentation + "else return false;");

                        WriteBlankLine();
                    }

                    m_Text = m_Text.Replace(match.Value + "\n", m_Writer.ToString());
                }
            }

            return m_Text;
        }

    }

    internal class AssetPostProcessorGenerator : ScriptGenerator
    {
        public AssetPostProcessorGenerator(ScriptPrescription scriptPrescription)
            : base(scriptPrescription)
        {
        }

        public override string ToString()
        {
            m_Text = base.ToString();

            m_Writer = new StringWriter
            {
                NewLine = "\n"
            };

            // Do not change tabs to spcaes of the .txt template files.
            var match = Regex.Match(m_Text, @"(\t*)\$LoadAndSetDataClassDataArray");
            if (match.Success)
            {
                // Set indent level to number of tabs before $Functions keyword
                IndentLevel = match.Groups[1].Value.Length;
                if (m_Prescription.mStringReplacements != null)
                {
                    foreach (var sheet in m_Prescription.mStringReplacements.Values)
                    {
                        m_Writer.WriteLine(m_Indentation + string.Format("ExcelQuery query{0} = new(filePath, \"{0}\");", sheet));
                        m_Writer.WriteLine(m_Indentation + string.Format("if (query{0} != null && query{0}.IsValid())", sheet));
                        m_Writer.WriteLine(m_Indentation + "{");
                        m_Writer.WriteLine(m_Indentation + string.Format("    data.{0}DataArray = query{0}.Deserialize<{0}>(3).ToArray();", sheet));
                        m_Writer.WriteLine(m_Indentation + "    EditorUtility.SetDirty(data);");
                        m_Writer.WriteLine(m_Indentation + "}");

                        WriteBlankLine();
                    }

                    m_Text = m_Text.Replace(match.Value + "\n", m_Writer.ToString());
                }
            }

            return m_Text;
        }
    }
}
