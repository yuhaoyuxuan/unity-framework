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
	public class SkillConfig
	{
        [SerializeField]
        int id;
        /// <summary>
        /// 标识 
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
        string prefabtrajectory;
        /// <summary>
        /// 预制体 弹道特效 
        /// </summary>
        public string Prefabtrajectory { get => prefabtrajectory; set => prefabtrajectory = value; }
        
        [SerializeField]
        string prefabhit;
        /// <summary>
        /// 预制体 受击特效 
        /// </summary>
        public string Prefabhit { get => prefabhit; set => prefabhit = value; }
        
        [SerializeField]
        int range;
        /// <summary>
        /// 范围 0全体 1单体 n多个 
        /// </summary>
        public int Range { get => range; set => range = value; }
        
        [SerializeField]
        int target;
        /// <summary>
        /// 目标 0我方  1敌人 
        /// </summary>
        public int Target { get => target; set => target = value; }
        
        [SerializeField]
        int type;
        /// <summary>
        /// 类型 0攻击 1治疗  2变身    8加Buff 
        /// </summary>
        public int Type { get => type; set => type = value; }
        
        [SerializeField]
        float percent;
        /// <summary>
        /// 攻击治疗的比例 (角色攻击力*Percent) 
        /// </summary>
        public float Percent { get => percent; set => percent = value; }
        
        [SerializeField]
        float extend;
        /// <summary>
        /// 扩展数据 根据Type 区分 类型2 变身持续时长  类型8 buffId 
        /// </summary>
        public float Extend { get => extend; set => extend = value; }
        
        [SerializeField]
        int uselimit;
        /// <summary>
        /// 使用限制 0时间(秒)  1攻击次数  2受击次数 
        /// </summary>
        public int Uselimit { get => uselimit; set => uselimit = value; }
        
        [SerializeField]
        float usevalue;
        /// <summary>
        /// 使用值 根据UseLimit组合 
        /// </summary>
        public float Usevalue { get => usevalue; set => usevalue = value; }
        
	}
}