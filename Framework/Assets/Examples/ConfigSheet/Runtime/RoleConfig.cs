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
	public class RoleConfig
	{
        [SerializeField]
        int id;
        /// <summary>
        /// 标识  >100 怪物 
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
        string icon;
        /// <summary>
        /// 图标 
        /// </summary>
        public string Icon { get => icon; set => icon = value; }
        
        [SerializeField]
        string prefab;
        /// <summary>
        /// 预制体 
        /// </summary>
        public string Prefab { get => prefab; set => prefab = value; }
        
        [SerializeField]
        int quality;
        /// <summary>
        /// 品质 默认0白 1银 2金 
        /// </summary>
        public int Quality { get => quality; set => quality = value; }
        
        [SerializeField]
        int type;
        /// <summary>
        /// 类型 0防守 1攻击 2辅助 
        /// </summary>
        public int Type { get => type; set => type = value; }
        
        [SerializeField]
        int health;
        /// <summary>
        /// 生命值 
        /// </summary>
        public int Health { get => health; set => health = value; }
        
        [SerializeField]
        int defense;
        /// <summary>
        /// 防御 
        /// </summary>
        public int Defense { get => defense; set => defense = value; }
        
        [SerializeField]
        int attackdamage;
        /// <summary>
        /// 攻击伤害 
        /// </summary>
        public int Attackdamage { get => attackdamage; set => attackdamage = value; }
        
        [SerializeField]
        float attackinterval;
        /// <summary>
        /// 攻击间隔 秒 
        /// </summary>
        public float Attackinterval { get => attackinterval; set => attackinterval = value; }
        
        [SerializeField]
        int attackdistance;
        /// <summary>
        /// 攻击距离 0近战 1远攻 
        /// </summary>
        public int Attackdistance { get => attackdistance; set => attackdistance = value; }
        
        [SerializeField]
        int attackrange;
        /// <summary>
        /// 攻击范围 0全体 1单体 n多个 
        /// </summary>
        public int Attackrange { get => attackrange; set => attackrange = value; }
        
        [SerializeField]
        string attackeffect;
        /// <summary>
        /// 普攻远攻弹道 
        /// </summary>
        public string Attackeffect { get => attackeffect; set => attackeffect = value; }
        
        [SerializeField]
        string hiteffect;
        /// <summary>
        /// 受击效果 
        /// </summary>
        public string Hiteffect { get => hiteffect; set => hiteffect = value; }
        
        [SerializeField]
        int movespeed;
        /// <summary>
        /// 每秒的移动像素 
        /// </summary>
        public int Movespeed { get => movespeed; set => movespeed = value; }
        
        [SerializeField]
        int skill;
        /// <summary>
        /// 技能id 默认0 没有技能 
        /// </summary>
        public int Skill { get => skill; set => skill = value; }
        
	}
}