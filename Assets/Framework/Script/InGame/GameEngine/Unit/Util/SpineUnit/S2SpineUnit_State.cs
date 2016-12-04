/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 스파인 유닛의 FSM 관리 함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Spine;

public abstract partial class S2SpineUnit : S2Unit
{
    // 상태정보얻기
    public S2ActionState GetStateInfo()
    {
        return GetStateInfo(GetCurrentState());
    }
    public S2ActionState GetStateInfo(int iStateID)
    {
        if (false == CheckToStateMachine(iStateID))
            return null;

        return m_dicState[iStateID];
    }

    // 상태체크
    public bool IsState(int iStateID)
    {
        return (GetCurrentState() == iStateID);
    }

    // 현재상태
    public int GetCurrentState()
    {
        return m_iStateID;
    }

    // 기능 : FlowAction추가
    public void AddFlowAction(FlowActionDelegate pAction)
    {
        m_pFlowActions.Add(pAction);
    }

    // 기능 : FlowAction초기화
    public void RemoveFlowAction()
    {
        m_pFlowActions.Clear();

        if (null == Single.Timer)
            return;

        string strTimerKey = S2Util.Format("FlowAction_{0}", GetUnitID());
        Single.Timer.StopDeltaTime(strTimerKey);
    }

    // 기능 : FlowAction이 있는가?
    public bool IsFlowAction()
    {
        return (0 != m_pFlowActions.Count);
    }
}