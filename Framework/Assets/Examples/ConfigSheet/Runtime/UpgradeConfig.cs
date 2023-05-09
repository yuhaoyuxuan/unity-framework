using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// </summary>
namespace ConfigSheet
{
	[Serializable]
	public class UpgradeConfig
	{
        [SerializeField]
        int id;
        /// <summary>
        /// 标识  对应角色类型 升级数值配置表   计算公式 百分比*等级*属性值>=1 
        /// </summary>
        public int Id { get => id; set => id = value; }
        
        [SerializeField]
        string describe;
        /// <summary>
        /// 描述 
        /// </summary>
        public string Describe { get => describe; set => describe = value; }
        
        [SerializeField]
        float health;
        /// <summary>
        /// 生命值 每次递增百分比 最小值1  
        /// </summary>
        public float Health { get => health; set => health = value; }
        
        [SerializeField]
        float defense;
        /// <summary>
        /// 防御 每次递增百分比 最小值1 
        /// </summary>
        public float Defense { get => defense; set => defense = value; }
        
        [SerializeField]
        float attackdamage;
        /// <summary>
        /// 攻击伤害 每次递增百分比 最小值1 
        /// </summary>
        public float Attackdamage { get => attackdamage; set => attackdamage = value; }
        
	}
}