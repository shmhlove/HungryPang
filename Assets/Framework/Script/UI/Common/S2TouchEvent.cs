/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 09월 12일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 터치 이벤트 클래스입니다.
 *          BoxCollider 컴포넌트가 포함되어 있어야합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2TouchEvent : MonoBehaviour 
{
    public BoxCollider  m_pCollider          = null;
    public Transform    m_pStickObject       = null;
    public float        m_fStickRadius       = 0.1f;
    public bool         m_bIsCenterOnPress   = true;

    private Vector3     m_vCenterPos         = Vector3.zero;
    private Vector3     m_vTouchPos          = Vector2.zero;
    private Vector3     m_vBeforePos         = Vector2.zero;
    private Vector3     m_vCurrentPos        = Vector2.zero;

    // SGEvent
    public S2Event eventTouch          = new S2Event(); // Press시
    public S2Event eventTouchPos       = new S2Event(); // Drag시 처음 터치한 위치로 부터의 상대위치
    public S2Event eventTouchDirection = new S2Event(); // Drag시 처음 터치한 위치로 부터의 방향
    public S2Event eventStickPos       = new S2Event(); // Drag시 StickObject 위치를 이용한 상대위치
    public S2Event eventStickDirection = new S2Event(); // Drag시 StickObject 위치를 이용한 방향
    public S2Event eventTouchEnd       = new S2Event(); // Release시

    // ------------------------------------------------------------
    // 시스템 콜백 함수들
    void Start()
    {
        m_vCenterPos    =
        m_vTouchPos     = 
        m_vBeforePos    = 
        m_vCurrentPos   = transform.position;
    }

    void OnPress(bool bIsOn)
    {
        if (null == m_pStickObject)
            return;

        if (true == bIsOn)
            SetPressToOn();
        else
            SetPressToOff();
    }

    void OnDrag(Vector2 vDelta)
    {
        if (null == m_pStickObject)
            return;

        m_vBeforePos    = m_vCurrentPos;
        m_vCurrentPos   = GetPosToTouch();

        SetStickPos(m_vCurrentPos);
        SendEventToDrag();
    }


    // ------------------------------------------------------------
    // 내부 유틸함수들
    void SetPressToOn()
    {
        m_vTouchPos = GetPosToTouch();
        
        if (true == m_bIsCenterOnPress)
            m_vCenterPos = m_vTouchPos;
        else
            m_vCenterPos = transform.position;

        m_vBeforePos    = m_vTouchPos;
        m_vCurrentPos   = m_vTouchPos;

        SetStickPos(m_vTouchPos);
        SendEventToTouch();
    }

    void SetPressToOff()
    {
        m_vBeforePos    = Vector3.zero;
        m_vCurrentPos   = Vector3.zero;

        SendEventToEndTouch();
    }

    Vector3 GetPosToTouch()
    {
        Ray pRay = UICamera.currentCamera.ScreenPointToRay(UICamera.lastTouchPosition);
        Vector3 vTouchPos = pRay.GetPoint(0.0f);
        vTouchPos.z = 0.0f;

        return vTouchPos;
    }

    void SetStickPos(Vector3 vPos)
    {
        if (null == m_pStickObject)
            return;

        Vector3 vLocation = (vPos - m_vCenterPos);
        if (m_fStickRadius < vLocation.magnitude)
            vPos = m_vCenterPos + (S2Math.GetDirection(m_vCenterPos, vPos) * m_fStickRadius);

        m_pStickObject.position = vPos;
    }

    Vector3 GetStickPos()
    {
        if (null == m_pStickObject)
            return Vector3.zero;

        return m_pStickObject.position;
    }

    // ------------------------------------------------------------
    // 인터페이스 관련
    public void SetSize(Vector2 vSize)
    {
        m_pCollider.size = vSize;
    }
    
    // ------------------------------------------------------------
    // 이벤트 관련
    void SendEventToTouch()
    {
        eventTouch.CallBack(this);
    }
    
    void SendEventToDrag()
    {
        eventTouchPos.CallBack<Vector2>(this,       (m_vCurrentPos - m_vTouchPos));
        eventTouchDirection.CallBack<Vector2>(this, (m_vCurrentPos - m_vTouchPos).normalized);
        eventStickPos.CallBack<Vector2>(this,       (GetStickPos() - m_vCenterPos));
        eventStickDirection.CallBack<Vector2>(this, (GetStickPos() - m_vCenterPos).normalized);
    }

    void SendEventToEndTouch()
    {
        eventTouchEnd.CallBack(this);
    }
}
