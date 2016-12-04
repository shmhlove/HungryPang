/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 28일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 데이터 로드 진행정보를 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2LoadPrograss
{
    // 로드 카운트 : <Total, Current>
    private S2Pair<int, int> m_pLoadCount = new S2Pair<int, int>(0, 0);

    // 로드 데이터 리스트(큐)
    private Queue<S2LoadData> m_qLoadQueue = new Queue<S2LoadData>();

    // 로드 데이터 정보 : <데이터타입, <파일명, 파일정보>>
    private Dictionary<eDataType, Dictionary<string, S2LoadData>> m_dicLoadDataInfo = new Dictionary<eDataType, Dictionary<string, S2LoadData>>();

    // 로드 완료 체커
    public bool m_bIsDone = false;
    // 실패한 로드가 하나라도 있는지 체크
    public bool m_bIsFail = false;

    // 로드 중인 파일 리스트
    private Dictionary<string, S2LoadStartInfo> m_dicLoadingFiles = new Dictionary<string, S2LoadStartInfo>();
    public Dictionary<string, S2LoadStartInfo> LoadingFiles { get { return m_dicLoadingFiles; } }

    public void Initialize()
    {
        m_pLoadCount.Initialize();

        m_qLoadQueue.Clear();

        m_bIsDone = false;
        m_bIsFail = false;
        m_dicLoadDataInfo.Clear();
        m_dicLoadingFiles.Clear();
    }

    public void AddLoadInfo(Dictionary<string, S2LoadData> dicLoadList)
    {
        foreach (KeyValuePair<string, S2LoadData> kvp in dicLoadList)
        {
            // 무결성 체크
            if (null == kvp.Value)
                continue;

            // 중복파일 체크
            S2LoadData pLoadData = GetLoadDataInfo(kvp.Value.m_strName);
            if (null != pLoadData)
            {
                S2Util.LogError("중복파일 발견!!!(FileName : {0})", kvp.Value.m_strName);
                continue;
            }

            // 초기화 및 등록
            SetLoadData(kvp.Value);
        }

        m_pLoadCount.Value1 += dicLoadList.Count;
    }

    S2LoadData GetLoadDataInfo(string strName)
    {
        foreach (KeyValuePair<eDataType, Dictionary<string, S2LoadData>> kvp in m_dicLoadDataInfo)
        {
            if (true == kvp.Value.ContainsKey(strName))
                return kvp.Value[strName];
        }
        return null;
    }

    public S2LoadData GetLoadData()
    {
        if (0 == m_qLoadQueue.Count)
            return null;

        S2LoadData pData = m_qLoadQueue.Dequeue();
        if (null == pData)
            return null;

        Single.Timer.StartDeltaTime(pData.m_strName);
        return pData;
    }

    public void SetLoadData(S2LoadData pData)
    {
        if (null == pData)
            return;

        pData.m_bIsDone = false;
        m_qLoadQueue.Enqueue(pData);

        if (false == m_dicLoadDataInfo.ContainsKey(pData.m_eDataType))
            m_dicLoadDataInfo.Add(pData.m_eDataType, new Dictionary<string, S2LoadData>());

        m_dicLoadDataInfo[pData.m_eDataType][pData.m_strName] = pData;
    }

    public S2LoadData SetLoadFinish(string strFileName, bool bIsSuccess)
    {
        S2LoadData pData = GetLoadDataInfo(strFileName);
        if (null == pData)
        {
            S2Util.LogError("추가되지 않은 파일이 로드됫다고 합니다~~({0})", strFileName);
            return null;
        }

        if (false == m_bIsFail)
            m_bIsFail = (false == bIsSuccess);

        pData.m_bIsSuccess  = bIsSuccess;
        pData.m_bIsDone     = true;
        
        ++m_pLoadCount.Value2;

        if (true == m_dicLoadingFiles.ContainsKey(strFileName))
            m_dicLoadingFiles.Remove(strFileName);

        if (m_pLoadCount.Value1 == m_pLoadCount.Value2)
            m_bIsDone = true;

        return pData;
    }

    public void SetLoadStart(string strFileName, S2LoadStartInfo pData)
    {
        m_dicLoadingFiles[strFileName] = pData;
    }

    public S2Pair<int, int> GetCountInfo()
    {
        return m_pLoadCount;
    }

    public void StartLoadTime()
    {
        Single.Timer.StartDeltaTime("LoadingTime");
    }

    public float GetLoadTime()
    {
        return Single.Timer.GetDeltaTimeToSecond("LoadingTime");
    }

    public S2Pair<float, float> GetLoadTime(string strFileName)
    {
        return new S2Pair<float, float>(GetLoadTime(),
             Single.Timer.GetDeltaTimeToSecond(strFileName));
    }

    public bool IsDone(string strFileName)
    {
        S2LoadData pData = GetLoadDataInfo(strFileName);
        if (null == pData)
            return true;

        return pData.m_bIsDone;
    }

    public bool IsDone(eDataType eType)
    {
        if (false == m_dicLoadDataInfo.ContainsKey(eType))
            return true;

        Dictionary<string, S2LoadData> dicDataInfo = m_dicLoadDataInfo[eType];
        foreach (KeyValuePair<string, S2LoadData> kvp in dicDataInfo)
        {
            if (null == kvp.Value)
                continue;

            if (false == kvp.Value.m_bIsDone)
                return false;
        }

        return true;
    }
}
