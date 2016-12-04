/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 21일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 캐릭터 상태관련 함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// 상태 리스트
public enum eCharacterState
{
    Idle,
    AttackBlue,
    AttackGreen,
    AttackOrange,
    AttackRed,
    AttackViolet,
    Hit,
    Die,
}

public partial class S2Character : S2SpineUnit
{
    // 다양화 : 애니메이션 클립명
    public override string GetClipName(int iStateID)
    {
        switch ((eCharacterState)iStateID)
        {
            case eCharacterState.Idle:              return "idle";
            case eCharacterState.AttackBlue:
            case eCharacterState.AttackGreen:
            case eCharacterState.AttackOrange:
            case eCharacterState.AttackRed:
            case eCharacterState.AttackViolet:      return "attack";
            case eCharacterState.Hit:               return "hit";
            case eCharacterState.Die:               return "hit";
                
        }
        return string.Empty;
    }

    // 다양화 : 초기화
    public override void Initialize()
    {
        SetMaxHP(100.0f);

        // 위치와 방향
        SetDirection(eDirection.Right);
        SetPosition(new Vector3(-0.95f, -0.6f, 0.0f));
    }

    // 다양화 : 상태정의
    public override void SetActionTable()
    {
        S2ActionState pState = null;

        // 대기상태
        pState = CreateState(eCharacterState.Idle, true);

        // 공격상태
        pState = CreateState(eCharacterState.AttackBlue, false);
        pState.m_OnEndAnim     = OnEndToAttack;
        pState = CreateState(eCharacterState.AttackGreen, false);
        pState.m_OnEndAnim     = OnEndToAttack;
        pState = CreateState(eCharacterState.AttackOrange, false);
        pState.m_OnEndAnim     = OnEndToAttack;
        pState = CreateState(eCharacterState.AttackRed, false);
        pState.m_OnEndAnim     = OnEndToAttack;
        pState = CreateState(eCharacterState.AttackViolet, false);
        pState.m_OnEndAnim     = OnEndToAttack;
        
        // 피격상태
        pState = CreateState(eCharacterState.Hit, false);
        pState.m_OnEnter       = OnEnterToHit;
        pState.m_OnEndAnim     = OnEndToHit;

        // 죽음상태
        pState = CreateState(eCharacterState.Die, false);
        pState.m_OnEnter       = OnEnterToDie;
        pState.m_OnEndAnim     = OnEndToDie;
    }

    // 다양화(이벤트) : 데미지 충돌
    public override void CrushDamage(S2Damage pDamage)
    {
        AddHP(-pDamage.GetDamageValue());
        ChangeToState(eCharacterState.Hit);

        if (true == IsHPToZero())
            ChangeToState(eCharacterState.Die);
    }

    // 상태유틸
    public S2ActionState CreateState(eCharacterState eState, bool bIsLoop)
    {
        return CreateState((int)eState, bIsLoop);
    }
    void ChangeToState(eCharacterState eState)
    {
        ChangeToState((int)eState);
    }
    public bool IsState(eCharacterState eStateID)
    {
        return IsState((int)eStateID);
    }

    // 상태구현 : 공격
    void OnEnterToAttack(int iStateID, int iFrame)
    {

    }
    void OnChangeToAttack(int iStateID, int iFrame)
    {
        ChangeToState(eCharacterState.Idle);
    }
    void OnEndToAttack(int iStateID, int iFrame)
    {
        ChangeToState(eCharacterState.Idle);
    }

    // 상태구현 : 피격
    void OnEnterToHit(int iStateID, int iFrame)
    {
        Single.Puzzle.SetStopState(true);
    }
    void OnEndToHit(int iStateID, int iFrame)
    {
        Single.Puzzle.SetStopState(false);

        if (false == IsHPToZero())
            ChangeToState(eCharacterState.Idle);
        else
            ChangeToState(eCharacterState.Die);
    }

    // 상태구현 : 죽음
    void OnEnterToDie(int iStateID, int iFrame)
    {
        Single.Puzzle.SetStopState(true);
    }
    void OnEndToDie(int iStateID, int iFrame)
    {
        SetMaxHP(100.0f);
        Single.Puzzle.SetStopState(false);
        ChangeToState(eCharacterState.Idle);
    }

    // 데브 : 상태변경
    public eCharacterState m_eDevStateID;
    public void DevPlayState()
    {
        ChangeToState(m_eDevStateID);
    }
}