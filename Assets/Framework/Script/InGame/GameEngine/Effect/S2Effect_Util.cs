/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 22일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 이펙트를 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Effect : MonoBehaviour
{
    // 유틸 : 따라움직이기
    void SetTrace()
    {
        if (false == m_pInfo.m_bIsTrace)
            return;

        if (null == m_pWho)
            return;

        Vector3 vPosition = m_pWho.transform.position;

        vPosition.x += m_pInfo.m_vTraceOffset.x;
        vPosition.y += m_pInfo.m_vTraceOffset.y;
        vPosition.z = GetPosition().z;
        
        SetPosition(vPosition);
    }

    // 유틸 : Who와 함께 죽을 것인가?
    void CheckToDieTogether()
    {
        if (false == m_pInfo.m_bIsDieTogether)
            return;

        if (null == m_pWho)
            DestroyEffect();
    }

    // 유틸 : 이벤트 콜
    void CallEvent(eEffectEventType eType)
    {
        if (null == m_pEvent)
            return;

        m_pEvent.CallEvent(eType, this);
    }
}