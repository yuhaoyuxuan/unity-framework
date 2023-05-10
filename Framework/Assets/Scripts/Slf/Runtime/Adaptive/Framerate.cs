using UnityEngine;
using UnityEngine.UI;

namespace Slf
{

    //==========================
    // - Author:      slf         
    // - Description: 显示帧率
    //==========================
    public class Framerate : MonoBehaviour
    {
        /// <summary>
        /// 上次更新帧率的时间
        /// </summary>
        float lastUpdateTime;
        /// <summary>
        /// 更新帧率时间间隔
        /// </summary>
        float updateTime = 0.1f;
        /// <summary>
        /// 帧数
        /// </summary>
        int frame;
        /// <summary>
        /// 帧率
        /// </summary>
        float fps;


        GUIStyle gs;
        Rect rect;
        private void Start()
        {
            lastUpdateTime = Time.realtimeSinceStartup;
            gs = new GUIStyle();
            gs.fontSize = 50;
            rect = new Rect(0, 600, 1000, 400);

        }

        private void Update()
        {
            frame++;
            if (Time.realtimeSinceStartup - lastUpdateTime > updateTime)
            {
                fps = frame / (Time.realtimeSinceStartup - lastUpdateTime);
                frame = 0;
                lastUpdateTime = Time.realtimeSinceStartup;

            }
        }

        private void OnGUI()
        {
            GUI.Label(rect, "FPS:" + fps + "\nws=" + Screen.width + " hs=" + Screen.height + "\nw=" + GameAdaptive.Instance.ScreenWidth + " h=" + GameAdaptive.Instance.ScreenHeight + "\nwL=" + GameAdaptive.Instance.ScreenWidthL + " hL=" + GameAdaptive.Instance.ScreenHeightL, gs);
        }
    }

}