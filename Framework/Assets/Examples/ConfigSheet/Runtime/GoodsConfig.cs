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
	public class GoodsConfig
	{
        [SerializeField]
        int id;
        /// <summary>
        /// 第一行描述 第二行变量名 第三行类型 （ String,Short,Int,Long,Float,Double,Enum,Bool） 如果类型加上(Int,Array)就是数组 (注意“,”英文) 
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
        int quality;
        /// <summary>
        /// 品质 默认0白 1银 2金 
        /// </summary>
        public int Quality { get => quality; set => quality = value; }
        
	}
}