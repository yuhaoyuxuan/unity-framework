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
	public class UpgradeCostConfig
	{
        [SerializeField]
        int id;
        /// <summary>
        /// 等级 升级消耗表 
        /// </summary>
        public int Id { get => id; set => id = value; }
        
        [SerializeField]
        int cost;
        /// <summary>
        /// 每次升级消耗货币数量  =0满级 
        /// </summary>
        public int Cost { get => cost; set => cost = value; }
        
	}
}