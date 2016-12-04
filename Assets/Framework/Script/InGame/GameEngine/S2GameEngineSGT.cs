/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 25일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 게임 엔진 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;

public partial class S2GameEngineSGT : S2Singleton<S2GameEngineSGT>
{
    // 다양화 : 초기화
    public override void OnInitialize()
    {
        m_bIsStartEngine    = false;
        m_bIsPause          = false;
        m_dicPause.Clear();
        m_dicResume.Clear();
    }
    // 다양화 : 종료
    public override void OnFinalize()
    {
        OnInitialize();
    }

    // 인터페이스 : 엔진시작
    public bool StartEngine()
    {
        // 초기화
        OnInitialize();

        // 퍼즐 생성
        Puzzle.Initialize(Single.GameInfo.m_iPuzzleRow, Single.GameInfo.m_iPuzzleCol);

        // // @@ 임시 : 캐릭터 생성
        // Character.CreateCharacter("SpineBoy");
        
        // 던전 생성
        Single.Dungeon.CreateDungeon("Dun_Sample");

        // 정지 및 재개 함수 등록
        RegeditPauseFunc();
        RegeditResumeFunc();
        
        // 엔진시작 플래그On
        m_bIsStartEngine = true;

        return true;
    }

    // 인터페이스 : 업데이트
    public void UpdateToFixed()
    {
        if (true == m_bIsPause)
            return;

        if (false == m_bIsStartEngine)
            return;

        Single.Timer.FrameMove();
        Single.Damage.FrameMove();
        Single.Effect.FrameMove();
        Single.Camera.FrameMove();
        Single.Dungeon.FrameMove();

        Puzzle.FrameMove();
        //Character.FrameMove();
        //Monsters.FrameMove();
    }

    // 인터페이스 : 유닛얻기
    public S2Unit GetUnit(eUnitType eType)
    {
        switch(eType)
        {
            case eUnitType.Character:   return Character;
            case eUnitType.Monster:     return Monsters.GetMonster(0);
        }
        return null;
    }

    // 인터페이스 : Pause
    [S2AttributeToShowFunc]
    public void PauseEngine() { PauseEngine(eEngineComponent.None); }
    public void PauseEngine(eEngineComponent eType)
    {
        if (eEngineComponent.None == eType)
        {
            S2Util.ForeachToDic<eEngineComponent, Action>
                (m_dicPause, (pKey, pPause) => pPause());
            m_bIsPause = true;
            return;
        }

        if (false == m_dicPause.ContainsKey(eType))
            return;

        m_dicPause[eType]();
    }

    // 인터페이스 : Resume
    [S2AttributeToShowFunc]
    public void ResumeEngine() { ResumeEngine(eEngineComponent.None); }
    public void ResumeEngine(eEngineComponent eType)
    {
        if (eEngineComponent.None == eType)
        {
            S2Util.ForeachToDic<eEngineComponent, Action>
                (m_dicResume, (pKey, pResume) => pResume());
            m_bIsPause = false;
            return;
        }

        if (false == m_dicResume.ContainsKey(eType))
            return;

        m_dicResume[eType]();
    }

    // 인터페이스 : Pause OR Resume Toggle
    public void PauseToggle()
    {
        if (true == m_bIsPause) ResumeEngine();
        else                    PauseEngine();
    }
}