using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S2UITweenStarter : MonoBehaviour 
{
    public List<TweenAlpha>     m_pTweenAlpha    = new List<TweenAlpha>();
    public List<TweenPosition>  m_pTweenPosition = new List<TweenPosition>();
    public List<TweenScale>     m_pTweenScale    = new List<TweenScale>();

    public bool                 m_bIsEnbaleStart = true;

    void OnEnable()
    {
        if (false == m_bIsEnbaleStart)
            return;

        StartTween();
    }

    public void StartTween()
    {
        foreach (TweenAlpha pTweener in m_pTweenAlpha)
        {
            if (null == pTweener)
                continue;

            pTweener.ResetToBeginning();
            pTweener.PlayForward();
        }

        foreach (TweenPosition pTweener in m_pTweenPosition)
        {
            if (null == pTweener)
                continue;

            pTweener.ResetToBeginning();
            pTweener.PlayForward();
        }

        foreach (TweenScale pTweener in m_pTweenScale)
        {
            if (null == pTweener)
                continue;

            pTweener.ResetToBeginning();
            pTweener.PlayForward();
        }
    }
}
