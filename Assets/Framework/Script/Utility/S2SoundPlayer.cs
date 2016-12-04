using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S2SoundPlayer : S2Singleton<S2SoundPlayer>
{
    private Dictionary<string, AudioSource> m_dicAudioSource = new Dictionary<string, AudioSource>();
    
    public override void OnInitialize()
    {
        gameObject.AddComponent<AudioListener>();
    }
    public override void OnFinalize() { }
    public void Play(string strClip, float fVolume, bool bIsLoop)
    {
        PlayClip(Single.ResourceData.GetAudioClip(strClip), fVolume, bIsLoop);
    }
    public void PlayClip(AudioClip pClip, float fVolume, bool bIsLoop)
    {
        if (null == pClip)
            return;

        if (false == m_dicAudioSource.ContainsKey(pClip.name))
        {
            GameObject pAudio = new GameObject(pClip.name);
            SetParent(pAudio);

            m_dicAudioSource.Add(pClip.name, pAudio.AddComponent<AudioSource>());
            m_dicAudioSource[pClip.name].clip = pClip;
        }

        m_dicAudioSource[pClip.name].loop   = bIsLoop;
        m_dicAudioSource[pClip.name].volume = fVolume;
        m_dicAudioSource[pClip.name].Play();
    }

    void SetParent(GameObject pObject)
    {
        if (null == pObject)
            return;

        pObject.transform.SetParent(transform);
    }
}
