using UnityEngine;
using UnityEngine.UI;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Description: 游戏适配
    //==========================
    public class GameAdaptive : SingletonComponent<GameAdaptive>
    {
        /// <summary>
        /// 设备性能等级
        /// </summary>
        public DevicePerformanceLevel Lv;

        public int ScreenWidth;
        public int ScreenHeight;

        public int ScreenWidthL;
        public int ScreenHeightL;
        private void Start()
        {
            //#if UNITY_EDITOR
            //            return;
            //#endif
#if !UNITY_EDITOR
            Debug.unityLogger.logEnabled = false;
#endif

            //防止屏幕变暗  屏幕休眠
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //屏蔽多点触控
            Input.multiTouchEnabled = false;

            float maxH = 1920;
            if (Screen.height > maxH)
            {
                float scale = maxH / Screen.height;
                ScreenWidth = (int)(Screen.width * scale);
                ScreenHeight = (int)(Screen.height * scale);
            }
            else
            {
                ScreenWidth = Screen.width;
                ScreenHeight = Screen.height;
            }

            if (ScreenWidth <= 720)
            {
                ScreenWidthL = ScreenWidth;
                ScreenHeightL = ScreenHeight;
            }
            else
            {
                ScreenWidthL = (int)(ScreenWidth * 0.7f);
                ScreenHeightL = (int)(ScreenHeight * 0.7f);
            }

            SetQualitySettings(DevicePerformance.GetDevicePerformanceLevel());
            Debug.LogWarning("质量:" + Lv + " CPU核心数:" + SystemInfo.processorCount + " 显存:" + SystemInfo.graphicsMemorySize + " 内存:" + SystemInfo.systemMemorySize);
        }

        /// <summary>
        /// 根据自身需要调整各级别需要修改的设置
        /// </summary>
        public void SetQualitySettings(DevicePerformanceLevel level)
        {
            Lv = level;
            if (level == DevicePerformanceLevel.High)
            {
                //设置垂直同步方案，VSyncs数值需要在每帧之间传递，使用0为不等待垂直同步。值必须是0，1或2。
                QualitySettings.vSyncCount = 1;
                //设置固定帧率 
                Application.targetFrameRate = 30;
                //if (Screen.width == ScreenWidth || Screen.height == ScreenHeight)
                //{
                //    return;
                //}
                ////设置分辨率
                //Screen.SetResolution(ScreenWidth, ScreenHeight, true);
            }
            else
            {
                //设置垂直同步方案，VSyncs数值需要在每帧之间传递，使用0为不等待垂直同步。值必须是0，1或2。
                QualitySettings.vSyncCount = 0;
                //设置固定帧率 
                Application.targetFrameRate = 30;
                //if (Screen.width == ScreenWidthL || Screen.height == ScreenHeightL)
                //{
                //    return;
                //}
                ////设置分辨率
                //Screen.SetResolution(ScreenWidthL, ScreenHeightL, true);
                //只适用于贴图，不适用于UI图片。贴图渲染 1/2
                //QualitySettings.masterTextureLimit = 1;
                ////前向渲染使用的像素灯的最大数量，建议最少为1
                //QualitySettings.pixelLightCount = 3;
                ////设置抗锯齿级别。选项有​​ 0_不开启抗锯齿，2_2倍，4_4倍和8_8倍采样。
                //QualitySettings.antiAliasing = 4;
                ////关闭阴影
                //QualitySettings.shadows = ShadowQuality.Disable;
            }
        }

        /// <summary>
        /// 显示帧率
        /// </summary>
        /// <returns></returns>
        public bool ShowFPS()
        {
            Framerate fr = gameObject.GetComponent<Framerate>();
            if(fr == null) {
                gameObject.AddComponent<Framerate>();
                return true;
            }
            fr.enabled = !fr.enabled;
            return fr.enabled;
        }

    }

}

