/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 13일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐판 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public partial class S2Puzzle
{
    // 시스템 : 퍼즐판 초기화
    public void Initialize(int iMaxRow, int iMaxCol)
    {        
        CreatePuzzleInfo(iMaxRow, iMaxCol);
    }

    // 시스템 : 업데이트
    public void FrameMove()
    {
        if (true == m_bIsPause)
            return;

        FrameMoveState(m_eState);
    }

    // 시스템 : 정지
    public void Pause()
    {
        m_bIsPause = true;
    }

    // 시스템 : 재개
    public void Resume()
    {
        m_bIsPause = false;
    }

    // 인터페이스 : 상태정지
    public bool IsStopState()
    {
        return m_bIsStopState;
    }
    public void SetStopState(bool bIsStop)
    {
        m_bIsStopState = bIsStop;
    }
}
