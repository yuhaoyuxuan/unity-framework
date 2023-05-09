using UnityEngine;
using System;

/// <summary>
/// !!! Note: Machine generated code !!!
///
/// A class which deriveds ScritableObject class so all its data 
/// can be serialized onto an asset data file.
/// </summary>
namespace ConfigSheet
{
    [Serializable]
    public class EntityScriptData : ScriptableObject 
    {	
        [SerializeField] 
        public string SheetName = string.Empty;
    
        public GoodsConfig[] GoodsConfigDataArray;
        
        public RoleConfig[] RoleConfigDataArray;
        
        public SkillConfig[] SkillConfigDataArray;
        
        public BuffConfig[] BuffConfigDataArray;
        
        public LevelConfig[] LevelConfigDataArray;
        
        public UpgradeConfig[] UpgradeConfigDataArray;
        
        public UpgradeCostConfig[] UpgradeCostConfigDataArray;
        
        void OnEnable()
        {		
            //#if UNITY_EDITOR
                    //hideFlags = HideFlags.DontSave;
            //#endif
            // Important:
            //    It should be checked an initialization of any collection data before it is initialized.
            //    Without this check, the array collection which already has its data get to be null 
            //    because OnEnable is called whenever Unity builds.
            // 
        
            if (GoodsConfigDataArray == null) GoodsConfigDataArray = new GoodsConfig[0];
            
            if (RoleConfigDataArray == null) RoleConfigDataArray = new RoleConfig[0];
            
            if (SkillConfigDataArray == null) SkillConfigDataArray = new SkillConfig[0];
            
            if (BuffConfigDataArray == null) BuffConfigDataArray = new BuffConfig[0];
            
            if (LevelConfigDataArray == null) LevelConfigDataArray = new LevelConfig[0];
            
            if (UpgradeConfigDataArray == null) UpgradeConfigDataArray = new UpgradeConfig[0];
            
            if (UpgradeCostConfigDataArray == null) UpgradeCostConfigDataArray = new UpgradeCostConfig[0];
            
        }
    
        //
        // Highly recommand to use LINQ to query the data sources.
        //

    }
}
