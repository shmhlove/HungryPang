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

// 이팩트 이벤트 관련
public enum eEffectEventType
{
    Destroy,
}
public class S2EffectEvent
{
    public Action<S2Effect> m_pEventToDestroy = null;

    public void CallEvent(eEffectEventType eType, S2Effect pEffect)
    {
        switch (eType)
        {
            case eEffectEventType.Destroy:
                if (null != m_pEventToDestroy)
                    m_pEventToDestroy(pEffect);
                break;
        }
    }
}

// 이팩트 정보
[Serializable]
public class S2EffectInfo
{
    public Vector3  m_vStartOffset          = Vector3.zero;       // 생성시 오프셋

    public bool     m_bIsTrace              = false;              // 데미지에 붙어 움직일 것인가?
    public Vector3  m_vTraceOffset          = Vector3.zero;       // 트레이스 오프셋

    public bool     m_bIsDieTogether        = true;               // 생성자와 함께 죽을것인가?
}

// 이팩트 오브젝트
public class S2EffectObject : MonoBehaviour
{
    public S2Effect m_pParent = null;

    // 시스템 : 객체 제거시
    private void OnDestroy()
    {
        if (null == m_pParent)
            return;

        m_pParent.OnDestroyToEffectObject();
    }
}

// 이팩트 클래스
[Serializable]
public partial class S2Effect : MonoBehaviour
{
    // 이팩트 기본정보
    public string           m_strID     = string.Empty;
    public S2EffectObject   m_pEffect   = null;
    public S2EffectInfo     m_pInfo     = null;
    public S2EffectEvent    m_pEvent    = null;
    public GameObject       m_pWho      = null;

    // 변수
    private Vector3         m_vPosition = Vector3.zero;

    // 인터페이스 : 초기화
    public void Initialize(string strID, GameObject pEffect, S2EffectInfo pInfo, GameObject pWho, S2EffectEvent pEvent)
    {
        m_strID     = strID;
        m_pInfo     = pInfo;
        m_pEvent    = pEvent;
        m_pWho      = pWho;

        // 이펙트 오브젝트
        m_pEffect = S2GameObject.GetComponent<S2EffectObject>(pEffect);
        m_pEffect.m_pParent = this;
        S2GameObject.SetParent(pEffect, gameObject);

        // 이팩트 시작위치 설정
        Vector3 vStartPos = GetPosition();
        if (null != m_pWho)
        {
            vStartPos   = m_pWho.transform.position;
            vStartPos.x += m_pInfo.m_vStartOffset.x;
            vStartPos.y += m_pInfo.m_vStartOffset.y;
        }
        SetPosition(vStartPos);
    }

    // 인터페이스 : 이펙트 오브젝트 자동 삭제시 호출됨
    public void OnDestroyToEffectObject()
    {
        DestroyEffect();
    }

    // 인터페이스 : 제거
    public void DestroyEffect()
    {
        if (null == gameObject)
            return;

        // 이팩트 제거 이벤트 콜
        CallEvent(eEffectEventType.Destroy);

        // 오브젝트 제거
        S2GameEngineSGT.DestroyObject(gameObject);
    }

    // 인터페이스 : 업데이트
    public void FrameMove()
    {
        if (null == m_pInfo)
            return;
        
        SetTrace();
        CheckToDieTogether();
    }

    // 인터페이스 : 이팩트 위치
    public void SetPosition(Vector3 vPos)
    {
        m_vPosition = vPos;
        transform.localPosition = m_vPosition;
        
    }
    public void SetPositionX(float fPosX)
    {
        m_vPosition.x = fPosX;
        SetPosition(m_vPosition);
    }
    public void SetPositionY(float fPosY)
    {
        m_vPosition.y = fPosY;
        SetPosition(m_vPosition);
    }
    public void AddPositionX(float fPosX)
    {
        m_vPosition.x += fPosX;
        SetPosition(m_vPosition);
    }
    public void AddPositionY(float fPosY)
    {
        m_vPosition.y += fPosY;
        SetPosition(m_vPosition);
    }
    public Vector3 GetPosition()
    {
        if (Vector3.zero == m_vPosition)
            return (m_vPosition = transform.localPosition);
        else
            return m_vPosition;
    }
}