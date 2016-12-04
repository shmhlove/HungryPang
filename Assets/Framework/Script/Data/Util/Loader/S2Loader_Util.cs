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
    // 유틸 : Coroutine으로 로드
    void CoroutineToLoad()
    {
        // 로드 실행(LoadCall()의 반환값 false의 의미는 Load함수 호출이 완료되었다는 의미)
        if (false == LoadCall())
            return;

        Single.Coroutine.NextFixed(CoroutineToLoad);
    }

    // 유틸 : Thread로 로드
    //void ThreadToLoad()
    //{
    //    while(true == LoadCall());
    //}

    // 유틸 : 어싱크 진행률 갱신
    void CoroutineToAsyncPrograss()
    {
        SendEventToAsyncPrograss();

        if (false == IsReMainLoadFiles())
            return;

        if (true == IsLoadDone())
            return;

        Single.Coroutine.WaitTime(CoroutineToAsyncPrograss, 0.2f);
    }

    // 유틸 : 로드 콜
    bool LoadCall()
    {
        S2LoadData pData = m_pPrograss.GetLoadData();
        if (null == pData)
            return false;

        if (false == pData.m_pTriggerLoadCall())
        {
            m_pPrograss.SetLoadData(pData);
            return true;
        }

        pData.m_pLoadFunc(pData, OnEventToLoadStart, OnEventToLoadDone);
        return true;
    }

    // 유틸 : 로드 리스트 추가
    void AddLoadList(List<Dictionary<string, S2LoadData>> pLoadList)
    {
        S2Util.ForeachToList<Dictionary<string, S2LoadData>>
        (pLoadList, (dicLoadList) =>
        {
            m_pPrograss.AddLoadInfo(dicLoadList);
        });
    }

    // 유틸 : 이벤트 추가
    void AddLoadEvent(EventHandler pComplate, EventHandler pProgress)
    {
        if (null != pComplate)
            EventToComplate.Add(pComplate);

        if (null != pProgress)
            EventToProgress.Add(pProgress);
    }

    // 유틸 : 현재 진행률 얻기
    float GetLoadPrograss()
    {
        // 로드할 파일이 없으면 100프로지~
        if (false == IsReMainLoadFiles())
            return 100.0f;

        float iProgress = 0.0f;
        S2Util.ForeachToDic<string, S2LoadStartInfo>(m_pPrograss.LoadingFiles, (pKey, pValue) => 
        {
            if (true == m_pPrograss.IsDone(pKey))
                return;

            iProgress += pValue.GetPrograss();
        });

        S2Pair<int, int> pCountInfo = m_pPrograss.GetCountInfo();
        float fCountGap         = S2Math.Divide(100.0f, pCountInfo.Value1);
        float fComplatePercent  = (fCountGap * pCountInfo.Value2);
        float fProgressPercent  = (fCountGap * iProgress);

        return (fComplatePercent + fProgressPercent);
    }

    // 이벤트 : 로드 시작
    void OnEventToLoadStart(string strFileName, S2LoadStartInfo pData)
    {
        m_pPrograss.SetLoadStart(strFileName, pData);
    }

    // 이벤트 : 파일 하나 로드 완료
    void OnEventToLoadDone(string strFileName, S2LoadEndInfo pInfo)
    {
        SendEventToPrograss(m_pPrograss.SetLoadFinish(strFileName, pInfo.m_bIsSuccess));
    }

    // 이벤트Send : 어싱크 진행률 갱신
    void SendEventToAsyncPrograss()
    {
        S2LoadEvent pArgs = new S2LoadEvent();
        pArgs.m_pCount          = m_pPrograss.GetCountInfo();
        pArgs.m_fPercent        = GetLoadPrograss();
        pArgs.m_bIsFail         = m_pPrograss.m_bIsFail;
        pArgs.m_bIsAsyncPrograss = true;
        EventToProgress.CallBack<S2LoadEvent>(this, pArgs);
    }

    // 이벤트Send : 파일 하나 완료
    void SendEventToPrograss(S2LoadData pData)
    {
        if (null == pData)
            return;

        S2LoadEvent pArgs = new S2LoadEvent();
        pArgs.m_eType           = pData.m_eDataType;
        pArgs.m_strFileName     = pData.m_strName;
        pArgs.m_pCount          = m_pPrograss.GetCountInfo();
        pArgs.m_pTime           = m_pPrograss.GetLoadTime(pData.m_strName);
        pArgs.m_bIsSuccess      = pData.m_bIsSuccess;
        pArgs.m_fPercent        = GetLoadPrograss();
        pArgs.m_bIsFail         = m_pPrograss.m_bIsFail;
        pArgs.m_bIsAsyncPrograss = false;
        EventToProgress.CallBack<S2LoadEvent>(this, pArgs);
    }

    // 이벤트Send : 로드 종료
    void SendEventToComplate()
    {
        if (false == EventToComplate.IsAddEvent())
            return;

        S2LoadEvent pArgs = new S2LoadEvent();
        pArgs.m_pCount          = m_pPrograss.GetCountInfo();
        pArgs.m_pTime           = new S2Pair<float, float>(m_pPrograss.GetLoadTime(), 0.0f);
        pArgs.m_bIsFail         = m_pPrograss.m_bIsFail;
        EventToComplate.CallBack<S2LoadEvent>(this, pArgs);
        EventToComplate.Clear();
    }

    // 인터페이스 : 로드가 완료되었는가?(성공/실패유무가 아님)
    public bool IsLoadDone()
    {
        return m_pPrograss.m_bIsDone;
    }

    // 인터페이스 : 특정 파일이 로드완료되었는가?(성공/실패유무가 아님)
    public bool IsLoadDone(string strFileName)
    {
        return m_pPrograss.IsDone(strFileName);
    }

    // 인터페이스 : 특정 타입이 로드완료되었는가?(성공/실패유무가 아님)
    public bool IsLoadDone(eDataType eType)
    {
        return m_pPrograss.IsDone(eType);
    }

    // 인터페이스 : 로드할 파일이 있는가?
    public bool IsReMainLoadFiles()
    {
        S2Pair<int, int> pCountInfo = m_pPrograss.GetCountInfo();
        if (0 == pCountInfo.Value1)
            return false;

        if (pCountInfo.Value1 == pCountInfo.Value2)
            return false;

        return true;
    }
}