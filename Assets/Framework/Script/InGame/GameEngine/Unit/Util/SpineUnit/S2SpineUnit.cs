/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 스파인 유닛 관련 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Spine;

public abstract partial class S2SpineUnit : S2Unit
{
    // 다양화 : 애니메이션 클립명
    public abstract string GetClipName(int iStateID);

    // 다양화 : 초기화
    public abstract void Initialize();

    // 다양화 : 상태정의
    public abstract void SetActionTable();

    // 다양화 : 업데이트
    public override void FrameMove()
    {
        if (true == m_bIsPause)
            return;

        // Unit FrameMove
        base.FrameMove();

        // 예외처리 : 애니메이션 컴포넌트 체크
        if (false == CheckToAnimation())
            return;

        // 예외처리 : 현재 상태정보 얻기
        S2ActionState pState = GetStateInfo();
        if (null == pState)
            return;

        // FixedTic카운트
        ++pState.m_iFixedTic;

        // 상태 업데이트
        UpdateToState(pState, GetAnimFramesFromTime(pState));
    }

    // 시스템 : 정지
    public virtual void Pause()
    {
        m_bIsPause = true;
        m_pAnimation.timeScale = 0.0f;
    }

    // 시스템 : 재개
    public virtual void Resume()
    {
        m_bIsPause = false;
        m_pAnimation.timeScale = m_fStartTimeScale;
    }

    // 인터페이스 : 스파인 유닛 생성
    public bool CreateSpineUnit(string strName, string strUnitID, eUnitType eType)
    {
        if (false == CreateGameObject(strName, strUnitID, eType))
            return false;

        m_pAnimation = GetComponent<SkeletonAnimation>();
        if (null == m_pAnimation)
        {
            S2Util.LogError("스파인 유닛은 SkeletonAnimation을 컴포넌트로 포함해야 합니다!!");
            DestroyGameObject();
            return false;
        }

        m_fStartTimeScale = m_pAnimation.timeScale;

        Initialize();
        SetActionTable();

        return true;
    }

    // 인터페이스 : 스파인 유닛 제거
    public void DestroySpineUnit()
    {
        DestroyGameObject();
        RemoveFlowAction();
    }
    
    // 인터페이스 : 상태생성
    public S2ActionState CreateState(int iStateID, bool bIsLoop)
    {
        S2ActionState pState    = new S2ActionState();
        pState.m_iStateID       = iStateID;
        pState.m_bIsLoop        = bIsLoop;
        m_dicState[iStateID]    = pState;
        return pState;
    }

    // 인터페이스 : 상태변경
    public void ChangeToState(int iStateID)
    {
        S2ActionState pChangeState = GetStateInfo(iStateID);
        if (null == pChangeState)
            return;

        S2ActionState pState = GetStateInfo(GetCurrentState());
        if (pState == pChangeState)
            return;

        SetAnimation(pChangeState);

        if (null != pState)
        {
            pState.OnEventToChange();
            m_pAnimation.state.Complete -= pState.OnEventToComplete;
        }

        if (null != pChangeState)
        {
            pChangeState.OnEventToEnter();
            m_pAnimation.state.Complete += pChangeState.OnEventToComplete;
        }
        
        m_iBeforeStateID    = m_iStateID;
        m_iStateID          = iStateID;
    }
    
    // 메인루프 : 업데이트
    void UpdateToState(S2ActionState pState, List<int> iFrames)
    {
        // FlowAction 함수 콜
        if (false == CallToFlowAction(pState))
            return;

        // FrameUpdate 함수 콜
        if (false == CallToFrameUpdate(pState, iFrames))
            return;

        // FixedUpdate 함수 콜
        if (false == CallToFixedUpdate(pState))
            return;

        // 이벤트 함수(마지막 프레임) 업데이트
        if (true == CheckToEndAnimation(pState))
            pState.OnEventToEndAnim();
    }

    // FlowAction 함수 콜
    public bool CallToFlowAction(S2ActionState pState)
    {
        if (false == IsFlowAction())
            return true;

        FlowActionDelegate pAction = m_pFlowActions[0];
        if (true == pAction(pState.m_iStateID))
            m_pFlowActions.Remove(pAction);

        return IsState(pState.m_iStateID);
    }

    // FrameUpdate 함수 콜
    bool CallToFrameUpdate(S2ActionState pState, List<int> iFrames)
    {
        foreach (int iFrame in iFrames)
        {
            if (false == IsState(pState.m_iStateID))
                return false;

            pState.OnEventToFrameUpdate(iFrame);
        }

        return true;
    }

    // FixedUpdate함수 Call
    bool CallToFixedUpdate(S2ActionState pState)
    {
        pState.OnEventToFixedUpdate();

        return IsState(pState.m_iStateID);
    }
}