
using UnityEngine;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2022/08/09 15:37:51	
    // - Description: 数学相关工具
    //==========================
    public class MathUtil
    {
        public const double Math_PI = 3.141592653589793;
        /// <summary>
        /// 角度转换弧度常数
        /// </summary>
        public const float Math_Deg2Rad = 0.0174532924f;

        /// <summary>
        /// 获取二阶贝塞尔曲线路径数组
        /// </summary>
        /// <param name="startPos">开始点</param>
        /// <param name="controlPos">控制点</param>
        /// <param name="endPos">结束点</param>
        /// <param name="count">路径的数量</param>
        /// <returns></returns>
        public static Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float count = 5)
        {
            Vector3[] path = new Vector3[(int)count];
            for (int i = 1; i <= count; i++)
            {
                float t = i / count;
                path[i - 1] = Bezier2(startPos, controlPos, endPos, t);
            }
            return path;
        }


        /// <summary>
        /// 获取2阶贝塞尔曲线
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="controlPos"></param>
        /// <param name="endPos"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Vector3 Bezier2(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float t)
        {
            return (1 - t) * (1 - t) * startPos + 2 * t * (1 - t) * controlPos + t * t * endPos;
        }

        /// <summary>
        /// 获取3阶贝塞尔曲线 
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="controlPos1"></param>
        /// <param name="controlPos2"></param>
        /// <param name="endPos"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Vector3 Bezier3(Vector3 startPos, Vector3 controlPos1, Vector3 controlPos2, Vector3 endPos, float t)
        {
            float t2 = 1 - t;
            return t2 * t2 * t2 * startPos
                + 3 * t * t2 * t2 * controlPos1
                + 3 * t * t * t2 * controlPos2
                + t * t * t * endPos;
        }
    }
}
