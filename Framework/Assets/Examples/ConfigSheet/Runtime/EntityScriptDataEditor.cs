using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QuickSheet.Editors;

/// <summary>
/// !!! Note: Machine generated code !!!
/// </summary>
namespace ConfigSheet
{
    [CustomEditor(typeof(EntityScriptData))]
    public class EntityScriptDataEditor : BaseExcelEditor<EntityScriptData>
    {	    
        public override bool Load()
        {
            EntityScriptData targetData = target as EntityScriptData;

            string path = targetData.SheetName;
            if (!File.Exists(path))
                return false;

            ExcelQuery queryGoodsConfig = new(path, "GoodsConfig");
            if (queryGoodsConfig != null && queryGoodsConfig.IsValid())
            {
                targetData.GoodsConfigDataArray = queryGoodsConfig.Deserialize<GoodsConfig>(3).ToArray();
                EditorUtility.SetDirty(targetData);
                AssetDatabase.SaveAssets();
            }
            else return false;
            
            ExcelQuery queryRoleConfig = new(path, "RoleConfig");
            if (queryRoleConfig != null && queryRoleConfig.IsValid())
            {
                targetData.RoleConfigDataArray = queryRoleConfig.Deserialize<RoleConfig>(3).ToArray();
                EditorUtility.SetDirty(targetData);
                AssetDatabase.SaveAssets();
            }
            else return false;
            
            ExcelQuery querySkillConfig = new(path, "SkillConfig");
            if (querySkillConfig != null && querySkillConfig.IsValid())
            {
                targetData.SkillConfigDataArray = querySkillConfig.Deserialize<SkillConfig>(3).ToArray();
                EditorUtility.SetDirty(targetData);
                AssetDatabase.SaveAssets();
            }
            else return false;
            
            ExcelQuery queryBuffConfig = new(path, "BuffConfig");
            if (queryBuffConfig != null && queryBuffConfig.IsValid())
            {
                targetData.BuffConfigDataArray = queryBuffConfig.Deserialize<BuffConfig>(3).ToArray();
                EditorUtility.SetDirty(targetData);
                AssetDatabase.SaveAssets();
            }
            else return false;
            
            ExcelQuery queryLevelConfig = new(path, "LevelConfig");
            if (queryLevelConfig != null && queryLevelConfig.IsValid())
            {
                targetData.LevelConfigDataArray = queryLevelConfig.Deserialize<LevelConfig>(3).ToArray();
                EditorUtility.SetDirty(targetData);
                AssetDatabase.SaveAssets();
            }
            else return false;
            
            ExcelQuery queryUpgradeConfig = new(path, "UpgradeConfig");
            if (queryUpgradeConfig != null && queryUpgradeConfig.IsValid())
            {
                targetData.UpgradeConfigDataArray = queryUpgradeConfig.Deserialize<UpgradeConfig>(3).ToArray();
                EditorUtility.SetDirty(targetData);
                AssetDatabase.SaveAssets();
            }
            else return false;
            
            ExcelQuery queryUpgradeCostConfig = new(path, "UpgradeCostConfig");
            if (queryUpgradeCostConfig != null && queryUpgradeCostConfig.IsValid())
            {
                targetData.UpgradeCostConfigDataArray = queryUpgradeCostConfig.Deserialize<UpgradeCostConfig>(3).ToArray();
                EditorUtility.SetDirty(targetData);
                AssetDatabase.SaveAssets();
            }
            else return false;
            
            return true;
        }
    }
}
