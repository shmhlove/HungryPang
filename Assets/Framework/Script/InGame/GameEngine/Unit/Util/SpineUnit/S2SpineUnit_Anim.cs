/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 스파인 유닛의 애니메이션 관리 함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Spine;

public abstract partial class S2SpineUnit : S2Unit
{
    // 재생 : 애니메이션 적용
    void SetAnimation(S2ActionState pState)
    {
        // 상태체크
        if (null == pState)
            return;

        // 애니메이션 변경
        ChangeToAnimation(pState);

        // 상태 변수들 초기화
        pState.Initialize();
    }

    // 재생 : 애니메이션 변경
    void ChangeToAnimation(S2ActionState pState)
    {
        // 예외처리 : 애니메이션 컴포넌트 체크
        if (false == CheckToAnimation())
            return;

        // 예외처리 : 상태체크
        if (null == pState)
            return;

        // 애니메이션 변경
        m_pAnimation.state.SetAnimation(0, GetClipName(pState.m_iStateID), pState.m_bIsLoop);
    }

    // 재생정보 : 이전 프레임 부터 현재 프레임까지 리스트(재생시간으로 만든 프레임)
    List<int> GetAnimFramesFromTime(S2ActionState pState)
    {
        if (true == IsEndAnim())
        {
            pState.m_iBeforeFrame   = pState.m_iFrame;
            pState.m_iFrame         = GetMaxFrame();
            return GetPastFrameList(pState.m_iBeforeFrame, pState.m_iFrame, pState.m_iFrame);
        }
        else
        {
            pState.m_iBeforeFrame   = pState.m_iFrame;
            pState.m_iFrame         = GetFrame();
            return GetPastFrameList(pState.m_iBeforeFrame, pState.m_iFrame, GetMaxFrame());
        }
    }

    // 재생정보 : 애니메이션 재생 진행률 얻기
    public float GetAnimPlayBack()
    {
        // 예외처리 : 애니메이션 컴포넌트 체크
        if (false == CheckToAnimation())
            return 0.0f;

        TrackEntry pEntry = m_pAnimation.state.GetCurrent(0);
        return (pEntry.Time / pEntry.EndTime) % 1.0f;
    }
    public float GetAnimPlayBackFromTic()
    {
        // 예외처리 : 애니메이션 컴포넌트 체크
        if (false == CheckToAnimation())
            return 0.0f;

        S2ActionState pState = GetStateInfo(GetCurrentState());
        if (null == pState)
            return 0.0f;
        
        int iMaxFrame       = GetMaxFrame();
        int iPlayFrame      = GetFrameToFixedTic((int)pState.m_iFixedTic);
        int iNormalFrame    = S2Math.Modulus(iPlayFrame, iMaxFrame);

        return S2Math.Divide(iNormalFrame, iMaxFrame);
    }

    // 재생정보 : 애니메이션 재생시간 얻기 ( Loop모션시 누적시간 아님 )
    float GetAnimPlayTime()
    {
        // 예외처리 : 애니메이션 컴포넌트 체크
        if (false == CheckToAnimation())
            return 0.0f;

        return (GetAnimMaxPlayTime() * GetAnimPlayBack());
    }
    float GetAnimPlayTimeFromTic()
    {
        // 예외처리 : 애니메이션 컴포넌트 체크
        if (false == CheckToAnimation())
            return 0.0f;

        return (GetAnimMaxPlayTime() * GetAnimPlayBackFromTic());
    }

    // 재생정보 : 애니메이션 전체 재생시간
    float GetAnimMaxPlayTime()
    {
        // 예외처리 : 애니메이션 컴포넌트 체크
        if (false == CheckToAnimation())
            return 0.0f;

        TrackEntry pEntry = m_pAnimation.state.GetCurrent(0);
        if (null == pEntry)
            return 0.0f;

        return pEntry.EndTime;
    }

    // 재생정보 : 애니메이션 재생시간으로 프레임 얻기
    public int GetFrame()
    {
        return Single.Timer.GetAnimFrameToSec(GetAnimPlayTime());
    }
    public int GetFrameFromTic()
    {
        return GetFrameFromTic(GetCurrentState());
    }
    public int GetFrameFromTic(int iStateID)
    {
        return Single.Timer.GetAnimFrameToSec(GetAnimPlayTimeFromTic());
    }

    // 재생정보 : 애니메이션 총 프레임 얻기
    public int GetMaxFrame()
    {
        return Single.Timer.GetAnimFrameToSec(GetAnimMaxPlayTime());
    }

    // 재생정보 : 애니메이션이 종료되었는가?
    public bool IsEndAnim()
    {
        // 예외처리 : 애니메이션 컴포넌트 체크
        if (false == CheckToAnimation())
            return false;

        S2ActionState pState = GetStateInfo(GetCurrentState());
        if (null == pState)
            return false;

        return pState.m_bIsComplate;
    }

    // 유틸 : FixedTic을 Frame 단위로 변환
    int GetFrameToFixedTic(int iFixedTic)
    {
        return Single.Timer.GetAnimFrameToFixedTic(iFixedTic);
    }
}