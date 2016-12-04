/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 01일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 로딩UI 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;

public class S2UILoading : S2UIBasePanel 
{
    public UISlider     m_pPrograss   = null;
    public UILabel      m_pPercent    = null;

    public override Type GetPanelType() { return typeof(S2UILoading); }

    void Start()
    {
        SetPrograss(0);
    }

    public void SetPrograss(float fPercent)
    {
        m_pPercent.text     = ((int)(fPercent * 100.0f)).ToString();
        m_pPrograss.value   = fPercent;
    }

    public void OnEventToPrograss(object pSender, EventArgs vArgs)
    {
        S2LoadEvent pInfo = Single.Event.GetArgs<S2LoadEvent>(vArgs);
        if (null == pInfo)
            return;

        // 어싱크 프로그래스
        if (true == pInfo.m_bIsAsyncPrograss)
        {
            S2Util.Log("로드 진행상황(" +
                       "Percent:<color=yellow>{0}</color>, " +
                       "Count:<color=yellow>{1}/{2}</color>)",
                       pInfo.m_fPercent,
                       pInfo.m_pCount.Value2,
                       pInfo.m_pCount.Value1);

            SetPrograss(S2Math.Divide(pInfo.m_fPercent, 100.0f));
            return;
        }
        
        // 싱크 프로그래스
        if (false == pInfo.m_bIsSuccess)
        {
            S2Util.LogError("<color=red>데이터 로드실패</color>(" +
                            "Type:<color=yellow>{0}</color>, " +
                            "Name:<color=yellow>{1}</color>, " +
                            "Percent:<color=yellow>{2}%</color>, " +
                            "현재Time:<color=yellow>{3}sec</color>, " +
                            "전체Time:<color=yellow>{4}sec</color>)",
                            pInfo.m_eType, pInfo.m_strFileName,
                            pInfo.m_fPercent,
                            S2Math.Round(pInfo.m_pTime.Value2, 2),
                            S2Math.Round(pInfo.m_pTime.Value1, 2));
        }
        else
        {
            S2Util.Log("데이터 로드성공(" +
                            "Type:<color=yellow>{0}</color>, " +
                            "Name:<color=yellow>{1}</color>, " +
                            "Percent:<color=yellow>{2}%</color>, " +
                            "현재Time:<color=yellow>{3}sec</color>, " +
                            "전체Time:<color=yellow>{4}sec</color>)",
                            pInfo.m_eType, pInfo.m_strFileName,
                            pInfo.m_fPercent,
                            S2Math.Round(pInfo.m_pTime.Value2, 2),
                            S2Math.Round(pInfo.m_pTime.Value1, 2));
        }
        
        SetPrograss(S2Math.Divide(pInfo.m_fPercent, 100.0f));
    }
}