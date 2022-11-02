using UnityEngine;
using UnityEngine.UI;
using Slf;
using System;

namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2021/08/05 20:20:22	
    // - Description: 心跳
    //==========================
    public class SocketHearBeat
    {
        /// <summary>
        /// 心跳间隔
        /// </summary>
        private float interval = 5f;
        /// <summary>
        /// 最大丢包次数 
        /// </summary>
        private int maxPacketLoss = 2;

        //当前丢包次数
        private int curMissCount;
        //发送心跳
        public Action sendCallback;
        //心跳超时
        public Action timeOutCallback;


        public SocketHearBeat(Action send = null, Action timeOut = null , float interval = 5f, int packetLoss = 2)
        {
            sendCallback = send;
            timeOutCallback = timeOut;
            this.interval = interval;
            maxPacketLoss = packetLoss;
        }

        public void start()
        {
            curMissCount = 0;
            TimerManager.instance.Register(interval, GetHashCode(), send, true);
            send();
        }

        public void stop()
        {
            curMissCount = 0;
            TimerManager.instance.UnRegister(GetHashCode());
        }

        public void receive()
        {
            curMissCount = 0;
        }

        private void send()
        {
            curMissCount++;
            if (curMissCount > maxPacketLoss)
            {
                timeOut();
            }
            else
            {
                if (sendCallback != null)
                {
                    sendCallback();
                };
            }
        }

        //心跳超时
        private void timeOut()
        {
            stop();
            Debug.LogError("心跳超时");
            if (timeOutCallback != null)
            {
                timeOutCallback();
            };
        }

    }
}

