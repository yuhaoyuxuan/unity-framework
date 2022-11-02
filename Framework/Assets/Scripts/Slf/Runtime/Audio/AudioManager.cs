using System.Collections.Generic;
using UnityEngine;
namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2022/10/14 13:48:23
    // - Description: 声音管理
    //==========================
    public class AudioManager : Singleton<AudioManager>
    {
        private int _music = -1;
        /// <summary>
        /// 音乐开关
        /// </summary>
        public bool Music
        {
            get
            {
                if(_music == -1)
                {
                    _music = LocalStorageUtil.GetCacheInt("Music");
                }
                return _music == 0;
            }
            set
            {
                if (value)
                {
                    _music = 0;
                }
                else
                {
                    _music = 1;
                }
                LocalStorageUtil.SetCacheInt("Music", _music);
                PauseBgAudio(!value);
            }
        }

        private int _sound = -1;
        /// <summary>
        /// 音效开关
        /// </summary>
        public bool Sound
        {
            get
            {
                if (_sound == -1)
                {
                    _sound = LocalStorageUtil.GetCacheInt("Sound");
                }
                return _sound == 0;
            }
            set
            {
                if (value)
                {
                    _sound = 0;
                }
                else
                {
                    _sound = 1;
                }
                LocalStorageUtil.SetCacheInt("Sound", _sound);
            }
        }


        /// <summary>
        /// 默认音效数量
        /// </summary>
        private int defaultNum = 3;
        /// <summary>
        /// 音效对象池 默认创建defaultNum个
        /// </summary>
        private List<AudioMono> audioPool;
        private int poolIdx;
        /// <summary>
        /// 背景音乐列表 默认创建1个
        /// </summary>
        private List<AudioMono> bgAudioList;
        /// <summary>
        /// 缓存声音源
        /// </summary>
        private Dictionary<string, AudioClip> cacheAudio;
        public AudioManager()
        {
            audioPool = new List<AudioMono>();
            cacheAudio = new Dictionary<string, AudioClip>();
            for(int i = 0;i< defaultNum; i++)
            {
                audioPool.Add(new AudioMono());
            }

            bgAudioList = new List<AudioMono>()
            {
                new AudioMono()
            };
        }

        /// <summary>
        /// 暂停背景音乐 true暂停、false取消暂停
        /// </summary>
        /// <param name="boo">暂停、取消暂停</param>
        public void PauseBgAudio(bool boo)
        {
            if (bgAudioList[0].IsUse)
            {
                for(int i = 0; i < bgAudioList.Count; i++)
                {
                    if (boo)
                    {
                        bgAudioList[i].Pause();
                    }
                    else
                    {
                        bgAudioList[i].UnPause();
                    }
                }
            }
        }

        /// <summary>
        /// 播放背景音乐 是否单一的默认true
        /// true 会把其他背景音乐关闭
        /// false 会并存多个背景音乐
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isSingle">是否单一的</param>
        public void PlayBgAudio(string path, bool isSingle = true)
        {
            PlayBgAudio(GetAudioClip(path), isSingle);
        }

        public void PlayBgAudio(AudioClip clip, bool isSingle = true)
        {
            if (!Music)
            {
                return;
            }

            int len = bgAudioList.Count;
            if (isSingle)
            {
                for(int i =0;i< len; i++)
                {
                    bgAudioList[i].Stop();
                }

                bgAudioList[0].Play(clip, 0);
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    if (!bgAudioList[i].IsUse)
                    {
                        bgAudioList[i].Play(clip, 0);
                        return;
                    }
                }
                bgAudioList.Add(new AudioMono());
                bgAudioList[len].Play(clip, 0);
            }
        }


        /// <summary>
        /// 根据路径 播放音效 并 缓存声音源
        /// 如果缓存没有会默认从Resource文件夹内加载 并 缓存声音源（二次直接使用缓存 声音源）
        /// 推荐使用 PlaySound(AudioClip clip)
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loopNum">播放次数 0<循环播放  >0播放次数 </循环播放></param>
        /// <param name="delay">延迟播放 秒</param>
        public void PlayAudio(string path,int loopNum = 1,float delay = 0)
        {
            PlayAudio(GetAudioClip(path), loopNum, delay); 
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">声音源</param>
        /// <param name="loopNum">播放次数 0<循环播放  >0播放次数 </循环播放></param>
        /// <param name="delay">延迟播放 秒</param>
        public void PlayAudio(AudioClip clip, int loopNum = 1, float delay = 0)
        {
            if (!Sound)
            {
                return;
            }
            GetAudioMono().Play(clip, loopNum, delay);
        }

        /// <summary>
        /// 获取声音类
        /// </summary>
        /// <returns></returns>
        private AudioMono GetAudioMono()
        {
            int len = audioPool.Count;
            int idx;
            for (int i = 0; i < len; i++)
            {
                idx = (poolIdx + i) % len;
                if (!audioPool[idx].IsUse)
                {
                    poolIdx = (idx + 1) % len;
                    return audioPool[idx];
                }
            }
            audioPool.Add(new AudioMono());
            poolIdx = 0;
            return audioPool[len];
        }

        /// <summary>
        /// 获取声音源 如果缓存没有会 默认从resource文件夹下加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        private AudioClip GetAudioClip(string path)
        {
            if (cacheAudio.ContainsKey(path))
            {
                return cacheAudio[path];
            }

            AudioClip clip = Resources.Load<AudioClip>(path);
            cacheAudio.Add(path, clip);
            return clip;
        }
    }

}
