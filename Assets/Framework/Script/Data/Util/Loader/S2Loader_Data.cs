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

using System.Threading; 

// 로드 이벤트 콜백 데이터
public class S2LoadEvent
{
    public eDataType m_eType;                   // 현재 로드타입
    public string m_strFileName;                // 현재 로드파일이름
    public bool m_bIsSuccess;                   // 현재 파일 로드상태

    public S2Pair<int, int> m_pCount;           // 로드 카운트 정보<Total, Current>
    public S2Pair<float, float> m_pTime;        // 로드 시간 정보<Total, Current>

    public float m_fPercent;                    // 진행도(0 ~ 100%)
    public bool m_bIsFail;                      // 하나라도 로드실패한적이 있는가?
    public bool m_bIsAsyncPrograss;             // 어싱크 프로그래스 정보인가?(어싱크는 로드 순서가 없기에 현재파일 정보를 보내줄 수가 없다.)
}

// 로드 데이터 정보
public class S2LoadData
{
    public eDataType    m_eDataType;            // 데이터 타입
    public string       m_strName;              // 파일명
    public Func<bool>   m_pTriggerLoadCall;     // 로드콜 조건
    public Action                               // 로드 콜백
    <
        S2LoadData,
        Action<string, S2LoadStartInfo>,
        Action<string, S2LoadEndInfo>
    > m_pLoadFunc;
    
    public float        m_fLoadTime;            // 로드 시간
    public bool         m_bIsSuccess;           // 로드 성공여부
    public bool         m_bIsDone;              // 로드 완료여부

    public S2LoadData()
    {
        m_fLoadTime    = 0.0f;
        m_bIsSuccess   = false;
        m_bIsDone      = false;
        m_pTriggerLoadCall = () => { return true; };
    }
}

// 로드 시작에 필요한 정보
public class S2LoadStartInfo
{
    public WWW              m_pWWW      = null;
    public AsyncOperation   m_pAsync    = null;

    public S2LoadStartInfo() { }
    public S2LoadStartInfo(WWW pWWW)
    {   m_pWWW = pWWW;        }
    public S2LoadStartInfo(AsyncOperation pAsync)
    {   m_pAsync = pAsync;   }

    public float GetPrograss()
    {
        if (null != m_pWWW)
            return m_pWWW.progress;

        if (null != m_pAsync)
            return m_pAsync.progress;

        return 0.0f;
    }
}

// 로드 종료 정보
public class S2LoadEndInfo
{
    public bool m_bIsSuccess;

    public S2LoadEndInfo() { }
    public S2LoadEndInfo(bool bIsSuccess)
    {
        m_bIsSuccess = bIsSuccess;
    }
}

public partial class S2Loader
{
    // 로드 정보
    public S2LoadPrograss m_pPrograss = new S2LoadPrograss();

    // 이벤트
    public S2Event EventToComplate = new S2Event();
    public S2Event EventToProgress = new S2Event();

    public void Initialize()
    {
        m_pPrograss.Initialize();
        EventToComplate.Clear();
        EventToProgress.Clear();
    }
}