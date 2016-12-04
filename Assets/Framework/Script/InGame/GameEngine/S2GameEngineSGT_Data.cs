/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 25일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 게임 엔진 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2GameEngineSGT : S2Singleton<S2GameEngineSGT>
{
    [SerializeField]
    private S2Character m_pCharacter = new S2Character();
    public S2Character Character { get { return m_pCharacter; } }

    [SerializeField]
    private S2MonsterManager m_pMonsters = new S2MonsterManager();
    public S2MonsterManager Monsters { get { return m_pMonsters; } }

    [SerializeField]
    private S2Puzzle m_pPuzzle = new S2Puzzle();
    public S2Puzzle Puzzle { get { return m_pPuzzle; } }

    public bool m_bIsStartEngine;
    public bool m_bIsPause;
    private Dictionary<eEngineComponent, Action> m_dicPause  = new Dictionary<eEngineComponent, Action>();
    private Dictionary<eEngineComponent, Action> m_dicResume = new Dictionary<eEngineComponent, Action>();
}
