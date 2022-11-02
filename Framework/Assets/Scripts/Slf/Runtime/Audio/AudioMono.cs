using UnityEngine;
using UnityEngine.UI;
using Slf;

//==========================
// - Author:      slf         
// - Date:        2022/10/14 13:53:16
// - Description: 声音源
//==========================
public class AudioMono
{
    /// <summary>
    /// 是否使用
    /// </summary>
    public bool IsUse;
    /// <summary>
    /// 循环次数
    /// </summary>
    private int loopNum;
    /// <summary>
    /// 延迟播放
    /// </summary>
    private float delay;

    private AudioSource aSource;
    public  AudioMono()
    {
        GameObject go = new GameObject("AudioItem");
        GameObject.DontDestroyOnLoad(go);
        aSource = go.AddComponent<AudioSource>();
    }
    /// <summary>
    /// 取消暂停
    /// </summary>
    public void UnPause()
    {
        if (IsUse)
        {
            aSource.UnPause();
        }
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        if (IsUse)
        {
            aSource.Pause();
        }
    }

    public void Stop()
    {
        Reset();
    }

    public void Play(AudioClip clip,int loopNum = 1 ,float delay = 0)
    {
        if (IsUse && aSource.clip == clip)
        {
            return;
        }
        IsUse = true;
        this.loopNum = loopNum;
        this.delay = delay;
        aSource.clip = clip;
        aSource.loop = loopNum <= 0;
        aSource.Pause();
        Play();
    }

    private void Play()
    {
        if (delay > 0)
        {
            aSource.PlayDelayed(delay);
        }
        else
        {
            aSource.Play();
        }

        if(loopNum > 0)
        {
            TimerManager.instance.Register(delay + aSource.clip.length, GetHashCode(), PlayComplete);
        }
    }

    private void PlayComplete()
    {
        if(loopNum == 1)
        {
            Reset();
        }
        else
        {
            loopNum--;
            Play();
        }
    }

    private void Reset()
    {
        aSource.Stop();
        aSource.clip = null;
        IsUse = false;
    }
}
