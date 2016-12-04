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

public partial class S2MonsterManager
{
    void DelToMonster()
    {
        foreach (S2Monster pUnit in m_pDelMonsters)
            m_pMonsters.Remove(pUnit);

        m_pDelMonsters.Clear();
    }

    void AddToMonster()
    {
        foreach (S2Monster pUnit in m_pAddMonsters)
            m_pMonsters.Add(pUnit);

        m_pAddMonsters.Clear();   
    }

    void GenToMonster()
    {
        // 이 조건은 스테이지 구성되면 스테이지 Gen조건에 따라 처리되도록
        if (0 != m_pAddMonsters.Count ||
            0 != m_pMonsters.Count)
            return;

        eMonster eType  = eMonster.Dragon;
        S2Monster pUnit = GetToAIClass(eType);
        if (null == pUnit)
            return;

        pUnit.CreateMonster(GetStrToPrefabName(eType), eType);
        pUnit.AddEventToDestroy(OnEventToMonsterDie);
        pUnit.SetParent("S2Monsters");

        m_pAddMonsters.Add(pUnit);
    }

    void OnEventToMonsterDie(S2Monster pUnit)
    {
        m_pDelMonsters.Add(pUnit);
    }

    string GetStrToPrefabName(eMonster eType)
    {
        switch (eType)
        {
            case eMonster.None:
            case eMonster.Max:
            case eMonster.Dragon:   return "Dragon";
        }
        return string.Empty;
    }

    S2Monster GetToAIClass(eMonster eType)
    {
        switch (eType)
        {
            case eMonster.None:
            case eMonster.Dragon:
            case eMonster.Max:      return new S2Dragon();
        }
        return null;
    }
}