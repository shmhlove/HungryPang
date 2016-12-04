/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 스파인 유닛들의 데이터를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Spine;

// 콜백함수 델리게이트
public delegate void StateFixedUpdateDelegate(int iStateID, int iFixedTic);
public delegate void StateFrameUpdateDelegate(int iStateID, int iFrame);
public delegate void StateEventDelegate(int iStateID, int iFrame);
public delegate bool FlowActionDelegate(int iStateID);

public class S2ActionState
{
    public int      m_iStateID;
    public bool     m_bIsLoop;
    public bool     m_bIsComplate;

    public int      m_iFixedTic;
    public int      m_iBeforeTic;
    public int      m_iFrame;
    public int      m_iBeforeFrame;

    public StateEventDelegate m_OnEnter;
    public StateEventDelegate m_OnChange;
    public StateEventDelegate m_OnEndAnim;
    public StateFrameUpdateDelegate m_OnFrameUpdate;
    public StateFixedUpdateDelegate m_OnFixedUpdate;
    public S2ActionState()
    {
        Initialize();
    }
    public void Initialize()
    {
        m_iFixedTic     = 0;
        m_iBeforeTic    = 0;
        m_iFrame        = 0;
        m_iBeforeFrame  = 0;
        m_bIsComplate   = false;
    }

    public void OnEventToComplete(Spine.AnimationState pState, int iTrackIndex, int iLoopCount)
    {
        m_bIsComplate = true;
    }

    public void OnEventToEnter()
    {
        if (null == m_OnEnter)
            return;
        
        m_OnEnter(m_iStateID, 0);
    }
    
    public void OnEventToChange()
    {
        if (null == m_OnChange)
            return;
        
        m_OnChange(m_iStateID, 0);
    }
    
    public void OnEventToEndAnim()
    {
        if (null == m_OnEndAnim)
            return;
        
        m_OnEndAnim(m_iStateID, 0);
    }

    public void OnEventToFrameUpdate(int iFrame)
    {
        if (null == m_OnFrameUpdate)
            return;

        m_OnFrameUpdate(m_iStateID, iFrame);
    }

    public void OnEventToFixedUpdate()
    {
        if (null == m_OnFixedUpdate)
            return;

        m_OnFixedUpdate(m_iStateID, m_iFixedTic);
    }
}

public abstract partial class S2SpineUnit : S2Unit
{
    // 유닛 기본 데이터
    public SkeletonAnimation m_pAnimation = null;

    // 상태 데이터
    public int m_iStateID           = 0;
    public int m_iBeforeStateID     = 0;
    public Dictionary<int, S2ActionState> m_dicState    = new Dictionary<int, S2ActionState>();
    private List<FlowActionDelegate> m_pFlowActions     = new List<FlowActionDelegate>();

    // 기타
    public bool m_bIsPause = false;
    public float m_fStartTimeScale = 1.0f;
}