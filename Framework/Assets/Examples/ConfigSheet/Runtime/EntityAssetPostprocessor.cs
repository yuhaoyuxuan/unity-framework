using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using QuickSheet.Editors;

/// <summary>
/// !!! Machine generated code !!!
/// </summary>
namespace ConfigSheet
{
    public class EntityAssetPostprocessor : AssetPostprocessor 
    {
        static readonly string filePath = "Assets/Examples/ConfigSheet/ConfigSheet.xls";
        static readonly string assetFilePath = "Assets/Examples/ConfigSheet/EntityScriptData.asset";
    
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string asset in importedAssets) 
            {
                if (!filePath.Equals(asset))
                    continue;
                
                EntityScriptData data = (EntityScriptData)AssetDatabase.LoadAssetAtPath(assetFilePath, typeof(EntityScriptData));
                if (data == null) {
                    data = ScriptableObject.CreateInstance<EntityScriptData> ();
                    data.SheetName = filePath;
                    AssetDatabase.CreateAsset(data, assetFilePath);
                }
           
                ExcelQuery queryGoodsConfig = new(filePath, "GoodsConfig");
                if (queryGoodsConfig != null && queryGoodsConfig.IsValid())
                {
                    data.GoodsConfigDataArray = queryGoodsConfig.Deserialize<GoodsConfig>(3).ToArray();
                    EditorUtility.SetDirty(data);
                }
                
                ExcelQuery queryRoleConfig = new(filePath, "RoleConfig");
                if (queryRoleConfig != null && queryRoleConfig.IsValid())
                {
                    data.RoleConfigDataArray = queryRoleConfig.Deserialize<RoleConfig>(3).ToArray();
                    EditorUtility.SetDirty(data);
                }
                
                ExcelQuery querySkillConfig = new(filePath, "SkillConfig");
                if (querySkillConfig != null && querySkillConfig.IsValid())
                {
                    data.SkillConfigDataArray = querySkillConfig.Deserialize<SkillConfig>(3).ToArray();
                    EditorUtility.SetDirty(data);
                }
                
                ExcelQuery queryBuffConfig = new(filePath, "BuffConfig");
                if (queryBuffConfig != null && queryBuffConfig.IsValid())
                {
                    data.BuffConfigDataArray = queryBuffConfig.Deserialize<BuffConfig>(3).ToArray();
                    EditorUtility.SetDirty(data);
                }
                
                ExcelQuery queryLevelConfig = new(filePath, "LevelConfig");
                if (queryLevelConfig != null && queryLevelConfig.IsValid())
                {
                    data.LevelConfigDataArray = queryLevelConfig.Deserialize<LevelConfig>(3).ToArray();
                    EditorUtility.SetDirty(data);
                }
                
                ExcelQuery queryUpgradeConfig = new(filePath, "UpgradeConfig");
                if (queryUpgradeConfig != null && queryUpgradeConfig.IsValid())
                {
                    data.UpgradeConfigDataArray = queryUpgradeConfig.Deserialize<UpgradeConfig>(3).ToArray();
                    EditorUtility.SetDirty(data);
                }
                
                ExcelQuery queryUpgradeCostConfig = new(filePath, "UpgradeCostConfig");
                if (queryUpgradeCostConfig != null && queryUpgradeCostConfig.IsValid())
                {
                    data.UpgradeCostConfigDataArray = queryUpgradeCostConfig.Deserialize<UpgradeCostConfig>(3).ToArray();
                    EditorUtility.SetDirty(data);
                }
                
            }
        }
    }
}
