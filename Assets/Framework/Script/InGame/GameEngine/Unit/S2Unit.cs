/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 유닛 관련 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract partial class S2Unit
{
    // 유닛 정보
    public string       m_strPrefab;
    public string       m_strUnitID;
    public eUnitType    m_eUnitType;
    public Vector3      m_vPosition;
    public Quaternion   m_qRotate;
    public bool         m_bIsActive;

    // 유닛 상태
    public float        m_fHP;
    public float        m_fMaxHP;
    public float        m_fSpeed;
    public eDirection   m_eDirection;

    // 오브젝트
    public GameObject   m_pObject   = null;
    public BoxCollider  m_pCollider = null;
    public S2UnitHUD    m_pHUD      = null;
    public Action<S2Damage> m_pCrushCallback = null;


    // 다양화 : 데미지 충돌
    public abstract void CrushDamage(S2Damage pDamage);

    // 다양화 : 업데이트
    public virtual void FrameMove()
    {
        if (null != m_pHUD)
        {
            m_pHUD.SetHealth(m_fMaxHP, m_fHP);
        }
    }

    // 생성 : 유닛 오브젝트
    public bool CreateGameObject(string strPrefab, string strUnitID, eUnitType eType)
    {
        // 오브젝트 생성
        GameObject pObject = Single.ResourceData.GetGameObject(strPrefab);
        if (null == pObject)
            return false;

        // 기존 오브젝트 제거
        DestroyGameObject();

        // 오브젝트 등록
        m_pObject       = pObject;
        m_strPrefab     = strPrefab;
        m_strUnitID     = strUnitID;
        m_eUnitType     = eType;
        SetPosition(m_pObject.transform.position);
        SetActive(true);

        // 데미지 클래스에 유닛 등록
        Single.Damage.AddUnit(m_strUnitID, this);

        // 컴포넌트 얻기
        m_pCollider = GetComponent<BoxCollider>();
        m_pHUD      = GetComponent<S2UnitHUD>();

        return true;
    }

    // 생성 : 유닛 오브젝트
    public bool Create(GameObject pObject, eUnitType eType)
    {
        if (null == pObject)
            return false;

        // 기존 오브젝트 제거
        DestroyGameObject();

        // 오브젝트 등록
        m_pObject       = pObject;
        m_strPrefab     = pObject.name;
        m_strUnitID     = pObject.name;
        m_eUnitType     = eType;
        SetPosition(m_pObject.transform.position);
        SetActive(true);

        // 데미지 클래스에 유닛 등록
        Single.Damage.AddUnit(m_strUnitID, this);

        // 컴포넌트 얻기
        m_pCollider = pObject.GetComponent<BoxCollider>();
        m_pHUD      = pObject.GetComponent<S2UnitHUD>();

        return true;
    }

    // 제거 : 유닛 오브젝트
    public void DestroyGameObject()
    {
        if (null == m_pObject)
            return;

        Single.Damage.DelUnit(m_strUnitID);
        S2GameEngineSGT.DestroyObject(m_pObject);
        m_pObject = null;
    }

    // 유틸 : 하이어라키
    public void SetParent(string strParent)
    {
        if (null == m_pObject)
            return;

        S2GameObject.SetParent(m_pObject, strParent);
    }

    // 정보 : 컴포넌트 얻기
    public T GetComponent<T>() where T : Component
    {
        return S2GameObject.GetComponent<T>(m_pObject);
    }

    // 유틸 : 유닛활성화
    public void SetActive(bool bIsActive)
    {
        if (null == m_pObject)
            return;

        m_bIsActive = bIsActive;
        m_pObject.SetActive(bIsActive);
    }
    public bool IsActive()
    {
        return m_bIsActive;
    }

    // 정보 : 유닛 식별자
    public string GetUnitID()
    {
        return m_strUnitID;
    }

    // 위치 : 유닛 위치
    public void SetPosition(Vector3 vPos)
    {
        m_pObject.transform.position = m_vPosition = vPos;
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
    public void SetPositionZ(float fPosZ)
    {
        m_vPosition.z = fPosZ;
        SetPosition(m_vPosition);
    }
    public Vector3 GetPosition()
    {
        return m_vPosition;
    }

    // 유틸 : 유닛 상태
    public void SetMaxHP(float fMaxHP)
    {
        SetHP(m_fMaxHP = fMaxHP);
    }
    public float GetHP()
    {
        return m_fHP;
    }
    public void SetHP(float fHP)
    {
        m_fHP = fHP;
    }
    public void AddHP(float fHP)
    {
        SetHP(GetHP() + fHP);
    }
    public bool IsHPToZero()
    {
        return (0.0f >= GetHP());
    }

    // 유틸 : 데미지 생성
    public void AddDamage(string strDamageName)
    {
        Single.Damage.AddDamage(strDamageName, this);
    }
    public void AddDamageToOffset(string strDamageName, float fOffsetX, float fOffsetY)
    {
        Single.Damage.AddDamageToOffset(strDamageName, this, fOffsetX, fOffsetY);
    }
    public void AddDamageToAddOffset(string strDamageName, float fOffsetX, float fOffsetY)
    {
        Single.Damage.AddDamageToAddOffset(strDamageName, this, fOffsetX, fOffsetY);
    }
    
    // 유틸 : 콜리더
    public BoxCollider GetCollider()
    {
        return m_pCollider;
    }

    // 이벤트 : 데미지 충돌
    public void OnEventToCrushDamage(S2Damage pDamage)
    {
        CrushDamage(pDamage);
    }
}