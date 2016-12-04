/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 29일
★ E-Mail ☞ shmhlove@neowiz.com
★ Desc   ☞ 이 클래스는 유니티 툴에 사용될 특성 클래스를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;

// 특성 : Mono를 상속받은 클래스내 함수를 인스펙터에 버튼형태로 노출합니다.
[AttributeUsage(AttributeTargets.Method)]
public class S2AttributeToShowFunc : Attribute
{
}

// 특성 : string Type의 몬스터의 모든 상태를 인스펙터에 Enum형태로 노출합니다.
public class S2AttributeToMonsterState : PropertyAttribute
{
}