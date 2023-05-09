using System.Collections.Generic;
using System.Linq;
using ConfigSheet;
using UnityEngine;

//==========================
// - Author:      slf         
// - Description: 配置表解析
//==========================
public class ConfigData
{
    public Dictionary<int, GoodsConfig> GoodsConfigDic;
    public Dictionary<int, RoleConfig> RoleConfigDic;
    public Dictionary<int, SkillConfig> SkillConfigDic;
    public Dictionary<int, BuffConfig> BuffConfigDic;
    public Dictionary<int, LevelConfig> LevelConfigDic;
    public Dictionary<int, UpgradeConfig> UpgradeConfigDic;
    public Dictionary<int, UpgradeCostConfig> UpgradeCostConfig;


    /// <summary>
    /// 解析配置文件
    /// </summary>
    /// <param name="data"></param>
    public ConfigData(EntityScriptData data)
    {
        GoodsConfigDic = data.GoodsConfigDataArray.ToDictionary(i => i.Id, i => i);
        RoleConfigDic = data.RoleConfigDataArray.ToDictionary(i => i.Id, i => i);
        SkillConfigDic = data.SkillConfigDataArray.ToDictionary(i => i.Id, i => i);
        BuffConfigDic = data.BuffConfigDataArray.ToDictionary(i => i.Id, i => i);
        LevelConfigDic = data.LevelConfigDataArray.ToDictionary(i => i.Id, i => i);
        UpgradeConfigDic = data.UpgradeConfigDataArray.ToDictionary(i => i.Id, i => i);
        UpgradeCostConfig = data.UpgradeCostConfigDataArray.ToDictionary(i => i.Id, i => i);
    }

}
