/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 09일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 몬스터의 유틸함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Monster : S2SpineUnit
{
    // 유틸 : 오류처리
    bool ErrorReturn()
    {
        DestroyMonster();
        return false;
    }

    // 유틸 : 이벤트콜(몬스터제거)
    void CallEventToDestroy()
    {
        foreach (Action<S2Monster> pEvent in m_pEventToDestroy)
        {
            pEvent(this);
        }
    }
}