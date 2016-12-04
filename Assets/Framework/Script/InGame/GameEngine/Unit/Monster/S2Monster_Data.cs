/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 09일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 몬스터의 데이터를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Monster : S2SpineUnit
{
    // 몬스터 기본정보
    private eMonster m_eMonsterType = eMonster.None;
    public eMonster MonsterType { get { return m_eMonsterType; } }

    // 이벤트 정보
    public List<Action<S2Monster>> m_pEventToDestroy = new List<Action<S2Monster>>();
}