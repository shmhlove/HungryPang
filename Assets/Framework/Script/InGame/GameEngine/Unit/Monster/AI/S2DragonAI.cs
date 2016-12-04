/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 09일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 드래곤 몬스터의 AI 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Dragon : S2Monster
{
    // 상태 리스트
    public enum eMonsterState
    {
        Idle,
        Attack,
        Hit,
        Die,
    }

    // 다양화 : 애니메이션 클립명
    public override string GetClipName(int iStateID)
    {
        switch ((eMonsterState)iStateID)
        {
            case eMonsterState.Idle:        return "flying";
            case eMonsterState.Attack:      return "flying";
            case eMonsterState.Hit:         return "hit";
            case eMonsterState.Die:         return "hit";
        }
        return string.Empty;
    }

    // 다양화 : 초기화
    public override void Initialize()
    {
        SetMaxHP(100.0f);

        // 초기 위치와 방향
        SetDirection(eDirection.Left);
        SetPosition(new Vector3(0.9f, -0.4f, 0.0f));
    }

    // 다양화 : 상태정의
    public override void SetActionTable()
    {
        S2ActionState pState = null;

        // 대기상태
        pState = CreateState(eMonsterState.Idle, true);
        pState.m_OnFrameUpdate = OnFrameUpdateToIdle;

        // 피격상태
        pState = CreateState(eMonsterState.Hit, false);
        pState.m_OnEndAnim      = OnEndToHit;

        // 죽음상태
        pState = CreateState(eMonsterState.Die, false);
        pState.m_OnEndAnim      = OnEndToDie;
    }

    // 다양화 : 데미지 충돌
    public override void CrushDamage(S2Damage pDamage)
    {
        AddHP(-pDamage.GetDamageValue());
        ChangeToState(eMonsterState.Hit);

        if (true == IsHPToZero())
            ChangeToState(eMonsterState.Die);
    }

    // 상태유틸
    public S2ActionState CreateState(eMonsterState eState, bool bIsLoop)
    {
        return CreateState((int)eState, bIsLoop);
    }
    void ChangeToState(eMonsterState eState)
    {
        ChangeToState((int)eState);
    }
    public bool IsState(eMonsterState eStateID)
    {
        return IsState((int)eStateID);
    }

    // 상태구현 : 대기
    void OnFrameUpdateToIdle(int iStateID, int iFrame)
    {
        string strTimerKey = "DragonAttack";
        if (5.0f < Single.Timer.GetDeltaTimeToSecond(strTimerKey))
        {
            AddDamageToAddOffset("DamageSample", 0.0f, -0.2f);
            Single.Timer.StartDeltaTime(strTimerKey);
        }
    }

    // 상태구현 : 피격
    void OnEndToHit(int iStateID, int iFrame)
    {
        if (false == IsHPToZero())
            ChangeToState(eMonsterState.Idle);
        else
            ChangeToState(eMonsterState.Die);
    }

    // 상태구현 : 죽음
    void OnEndToDie(int iStateID, int iFrame)
    {
        DestroyMonster();
    }

    // 다양화 : 데브(상태변경)
    public override void DevPlayState(string strStateID)
    {
        ChangeToState(S2Util.StringToEnum<eMonsterState>(strStateID));
    }
    public override List<string> GetStateList()
    {
        List<string> pState = new List<string>();
        S2Util.ForeachToEnum<eMonsterState>((eState) =>
        {
            pState.Add(eState.ToString());
        });
        return pState;
    }
}