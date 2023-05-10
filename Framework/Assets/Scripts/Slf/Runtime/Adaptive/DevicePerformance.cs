using UnityEngine;
using UnityEngine.UI;
namespace Slf
{
    /// <summary>
    /// 设备性能级别
    /// </summary>
    public enum DevicePerformanceLevel
    {
        Low,
        Mid,
        High
    }

    //==========================
    // - Author:      slf         
    // - Description: 设备性能检测
    //==========================
    public class DevicePerformance
    {
        /// <summary>
        /// 获取设备性能评级
        /// </summary>
        /// <returns>性能评级</returns>
        public static DevicePerformanceLevel GetDevicePerformanceLevel()
        {
            //处理器数量
            int processorCount = SystemInfo.processorCount;
            //显存
            int graphicsMemorySize = SystemInfo.graphicsMemorySize;
            //内存
            int systemMemorySize = SystemInfo.systemMemorySize;

            //集显
            if (SystemInfo.graphicsDeviceVendorID == 32902)
            {
                return DevicePerformanceLevel.Low;
            }
            else //NVIDIA系列显卡（N卡）和AMD系列显卡
            {
                //根据目前硬件配置三个平台设置了不一样的评判标准（仅个人意见）

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WSA_10_0
                if (processorCount <= 2)
                    return DevicePerformanceLevel.Low;
#elif UNITY_STANDALONE_OSX || UNITY_IPHONE
            if (processorCount <= 2)
                return DevicePerformanceLevel.Low;
#elif UNITY_ANDROID
            if (processorCount <= 4)
                return DevicePerformanceLevel.Low;
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WSA_10_0
                if (graphicsMemorySize >= 4000 && systemMemorySize >= 8000)
                    return DevicePerformanceLevel.High;
                else if (graphicsMemorySize >= 2000 && systemMemorySize >= 4000)
                    return DevicePerformanceLevel.Mid;
                else
                    return DevicePerformanceLevel.Low;
#elif UNITY_STANDALONE_OSX || UNITY_IPHONE
            if (graphicsMemorySize >= 4000 && systemMemorySize >= 8000)
                return DevicePerformanceLevel.High;
            else if (graphicsMemorySize >= 2000 && systemMemorySize >= 4000)
                return DevicePerformanceLevel.Mid;
            else
                return DevicePerformanceLevel.Low;
#elif UNITY_ANDROID
            if (graphicsMemorySize >= 6000 && systemMemorySize >= 8000)
                return DevicePerformanceLevel.High;
            else if (graphicsMemorySize >= 2000 && systemMemorySize >= 4000)
                return DevicePerformanceLevel.Mid;
            else
                return DevicePerformanceLevel.Low;
#endif
                return DevicePerformanceLevel.Mid;
            }
        }
    }

}