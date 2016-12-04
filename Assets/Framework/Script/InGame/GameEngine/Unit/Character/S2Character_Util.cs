/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 캐릭터 유틸함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class S2Character : S2SpineUnit
{
    // 유틸 : 오류처리
    bool ErrorReturn()
    {
        DestroyCharacter();
        return false;
    }
}