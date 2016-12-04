/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 09일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 몬스터들을 관리하는 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public partial class S2MonsterManager
{
    [SerializeField]
    private List<S2Monster> m_pMonsters     = new List<S2Monster>();

    [SerializeField]
    private List<S2Monster> m_pAddMonsters  = new List<S2Monster>();
    private List<S2Monster> m_pDelMonsters  = new List<S2Monster>();

    // 기타
    private bool m_bIsPause = false;

    // 시스템 : 업데이트
    public void FrameMove()
    {
        if (true == m_bIsPause)
            return;

        GenToMonster();
        DelToMonster();
        AddToMonster();

        foreach (S2Monster pUnit in m_pMonsters)
        {
            pUnit.FrameMove();
        }
    }

    // 시스템 : 정지
    public void Pause()
    {
        m_bIsPause = true;

        S2Util.ForeachToList<S2Monster>(m_pMonsters, (pMonster) => pMonster.Pause());
        S2Util.ForeachToList<S2Monster>(m_pAddMonsters, (pMonster) => pMonster.Pause());
    }

    // 시스템 : 재개
    public void Resume()
    {
        m_bIsPause = false;

        S2Util.ForeachToList<S2Monster>(m_pMonsters, (pMonster) => pMonster.Resume());
        S2Util.ForeachToList<S2Monster>(m_pAddMonsters, (pMonster) => pMonster.Resume());
    }

    public List<S2Monster> GetMonsters()
    {
        return m_pMonsters;
    }

    public S2Monster GetMonster(int iIndex)
    {
        if (iIndex > m_pMonsters.Count)
            return null;

        return m_pMonsters[iIndex];
    }
}