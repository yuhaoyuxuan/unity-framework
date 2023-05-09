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
	public class BuffConfig
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
        /// 描述（立即触发，然后间隔1秒一次） 
        /// </summary>
        public string Describe { get => describe; set => describe = value; }
        
        [SerializeField]
        string icon;
        /// <summary>
        /// 图标 
        /// </summary>
        public string Icon { get => icon; set => icon = value; }
        
        [SerializeField]
        string prefabstart;
        /// <summary>
        /// 预制体 buff特效 
        /// </summary>
        public string Prefabstart { get => prefabstart; set => prefabstart = value; }
        
        [SerializeField]
        int type;
        /// <summary>
        /// 类型 0防御 1攻击 2减血 3加血 
        /// </summary>
        public int Type { get => type; set => type = value; }
        
        [SerializeField]
        float percent;
        /// <summary>
        /// 类型的比例 (防、攻、血*Percent) 
        /// </summary>
        public float Percent { get => percent; set => percent = value; }
        
        [SerializeField]
        float duration;
        /// <summary>
        /// 持续时间（秒） 
        /// </summary>
        public float Duration { get => duration; set => duration = value; }
        
	}
}