/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 12일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐블럭 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum eBlockState
{
    Init,           // 초기화
    Idle,           // 대기중
    Proper,         // 자리잡는 중
    FallDown,       // 떨어지는 중
    Swap,           // 스왑되는 중
    Crush,          // 깨지는 중
}

public class S2UIPuzzleBlockState
{
    public eBlockState  m_eState;
    public Action       m_pEnter      = () => { };
    public Action       m_pUpdate     = () => { };
    public Action       m_pExit       = () => { };
}

public partial class S2UIPuzzleBlock : MonoBehaviour
{
    // 유틸 : 상태설정(초기화)
    void InitToState()
    {
        m_dicState.Clear();
        S2UIPuzzleBlockState pState = null;

        // 상태정의 : 초기화
        pState              = CreateState(eBlockState.Init);
        pState.m_pEnter     = OnEnterToInit;

        // 상태정의 : 대기중
        pState              = CreateState(eBlockState.Idle);
        
        // 상태정의 : 자리잡는 중
        pState              = CreateState(eBlockState.Proper);
        pState.m_pUpdate    = OnUpdateToProper;

        // 상태정의 : 떨어지는 중
        pState              = CreateState(eBlockState.FallDown);
        pState.m_pEnter     = OnEnterToFallDown;
        pState.m_pUpdate    = OnUpdateToFallDown;

        // 상태정의 : 스왑되는 중
        pState              = CreateState(eBlockState.Swap);
        pState.m_pUpdate    = OnUpdateToSwap;
        
        // 상태정의 : 부수기
        pState              = CreateState(eBlockState.Crush);
        pState.m_pEnter     = OnEnterToCrush;
        pState.m_pUpdate    = OnUpdateToCrush;
    }


    // 상태 : 초기화
    /////////////////////////////////////////////////////////
    void OnEnterToInit()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        SetActive(true);
        SetToggleButton(false);
        SetToggleSelect(false);
        SetActiveBlock(true);
        SetDefualtTexture();

        ChangeState(eBlockState.Idle);
    }


    // 상태 : 자리잡는 중
    /////////////////////////////////////////////////////////
    void OnUpdateToProper()
    {
        if (true == MoveBlock(Vector3.zero))
            return;

        SetArrive();
        ChangeState(eBlockState.Idle);
    }


    // 상태 : 떨어지는 중
    /////////////////////////////////////////////////////////
    bool bRebound = false;
    void OnEnterToFallDown()
    {
        bRebound = false;
    }
    void OnUpdateToFallDown()
    {
        if (true == MoveBlock(S2Physics.m_vGravity * 300.0f))
            return;

        if (false == bRebound)
        {
            m_vSpeed.y = 200.0f;
            SetPosition(m_vTargetPos + (m_vSpeed * Single.Timer.fixedDeltaTime));
            bRebound = true;
            return;
        }
        
        SetArrive();
        ChangeState(eBlockState.Idle);
    }


    // 상태 : 스왑되는 중
    /////////////////////////////////////////////////////////
    void OnUpdateToSwap()
    {
        if (true == MoveBlock(Vector3.zero))
            return;

        SetArrive();
        ChangeState(eBlockState.Idle);
    }


    // 상태 : 부수기
    /////////////////////////////////////////////////////////
    void OnEnterToCrush()
    {
        SetActiveBlock(false);
        m_pCrush.gameObject.SetActive(true);
        m_pCrush.Play();

        Single.Damage.AddDamageUIToTargetOffset(
            GetNameToDamage(m_pInfo.m_eBlockType),
            Single.UIInGameFront.GetUnit(m_pInfo.m_eBlockType), 
            GetPosition().x, GetPosition().y);
    }
    void OnUpdateToCrush()
    {
        if (true == m_pCrush.IsPlay())
            return;

        if (true == m_pAnimation.IsPlay())
            return;

        OnTriggerCrush();
        ChangeState(eBlockState.Idle);
    }
}