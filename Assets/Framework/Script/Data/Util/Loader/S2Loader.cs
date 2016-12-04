/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 04일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 데이터 로드를 담당합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Loader
{
    // 시스템 : 업데이트
    public void FrameMove()
    {
        if (true == IsLoadDone())
            SendEventToComplate();

        // 예외처리 : 로드완료가 한참동안 안될때 처리
        // @@ 로드완료가 안될때 TimeOut 같은 처리필요 : Data클래스에서 EventToLoadDone이 호출안되면 발생!!
    }

    // 인터페이스 : 로드 시작(로드 리스트, 완료콜백, 프로그래스콜백)
    public void LoadStart(List<Dictionary<string, S2LoadData>> pLoadList, 
        EventHandler pComplate = null, EventHandler pProgress = null)
    {
        // 로더 초기화
        Initialize();
        
        // 로드 리스트 추가
        AddLoadList(pLoadList);

        // 로드 이벤트 추가
        if (null == pComplate)  pComplate = OnEventToComplate;
        if (null == pProgress)  pProgress = OnEventToPrograss;
        AddLoadEvent(pComplate, pProgress);

        // 로드 타이머 시작
        m_pPrograss.StartLoadTime();

        // 어싱크 프로그래스 체크시작
        CoroutineToAsyncPrograss();

        // 예외처리 : 로드할 리스트가 있는가?
        if (false == IsReMainLoadFiles())
        {
            SendEventToComplate();
            return;
        }
        
        // 로드명령시작
        CoroutineToLoad();

        // 쓰레드로 로드 : 생성한 쓰레드에서 유니티 함수가 콜되면 Error발생
        //Thread pThread = new Thread(new ThreadStart(ThreadToLoad));
        //pThread.Start();
    }

    // 로그이벤트 : 프로그래스
    void OnEventToPrograss(object pSender, EventArgs vArgs)
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
                            S2Math.Round(pInfo.m_pTime.Value2, 3),
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
                            S2Math.Round(pInfo.m_pTime.Value2, 3),
                            S2Math.Round(pInfo.m_pTime.Value1, 2));
        }
    }

    // 로그이벤트 : 완료
    void OnEventToComplate(object pSender, EventArgs vArgs)
    {
        S2Util.Log("<color=yellow>데이터 로드 완료!!</color>");
    }
}