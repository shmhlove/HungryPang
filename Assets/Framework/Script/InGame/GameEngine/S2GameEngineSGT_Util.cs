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
    void RegeditPauseFunc()
    {
        m_dicPause.Clear();
        m_dicPause.Add(eEngineComponent.Damage,         Single.Damage.Pause);
        m_dicPause.Add(eEngineComponent.Effect,         Single.Effect.Pause);
        m_dicPause.Add(eEngineComponent.Puzzle,         Puzzle.Pause);
        m_dicPause.Add(eEngineComponent.Character,      Character.Pause);
        m_dicPause.Add(eEngineComponent.Monster,        Monsters.Pause);
        m_dicPause.Add(eEngineComponent.Dungeon,        Single.Dungeon.Pause);
    }

    void RegeditResumeFunc()
    {
        m_dicResume.Clear();
        m_dicResume.Add(eEngineComponent.Damage,        Single.Damage.Resume);
        m_dicResume.Add(eEngineComponent.Effect,        Single.Effect.Resume);
        m_dicResume.Add(eEngineComponent.Puzzle,        Puzzle.Resume);
        m_dicResume.Add(eEngineComponent.Character,     Character.Resume);
        m_dicResume.Add(eEngineComponent.Monster,       Monsters.Resume);
        m_dicResume.Add(eEngineComponent.Dungeon,       Single.Dungeon.Resume);
    }
}