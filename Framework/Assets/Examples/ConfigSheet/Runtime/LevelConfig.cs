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
	public class LevelConfig
	{
        [SerializeField]
        int id;
        /// <summary>
        /// 标识  关卡配置表 
        /// </summary>
        public int Id { get => id; set => id = value; }
        
        [SerializeField]
        string name;
        /// <summary>
        /// 名字 
        /// </summary>
        public string Name { get => name; set => name = value; }
        
        [SerializeField]
        string describe;
        /// <summary>
        /// 描述 
        /// </summary>
        public string Describe { get => describe; set => describe = value; }
        
        [SerializeField]
        string[] enemys = new string[0];
        /// <summary>
        /// 敌人id数组 （数组长度=波数）(角色id_角色等级 &=分割线) 
        /// </summary>
        public string[] Enemys { get => enemys; set => enemys = value; }
        
        [SerializeField]
        string[] reward = new string[0];
        /// <summary>
        /// 奖励 （数组长度=每波奖励）(奖励类型_奖励id_奖励数量  &=分割线) 
        /// </summary>
        public string[] Reward { get => reward; set => reward = value; }
        
	}
}