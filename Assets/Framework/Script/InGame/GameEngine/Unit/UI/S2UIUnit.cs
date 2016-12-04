/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 UI유닛 관련 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2UIUnit : MonoBehaviour
{
    public enum eUnitState
    {
        None,
        Idle,
        JumpUp,
        JumpDown,
        CrushDmg,
    }

    public eUnitType        pUnitType;
    public S2UIUnitInfo     pInfo       = null;
    public GameObject       pEnerge     = null;
    public GameObject       pCharge     = null;
    public GameObject       pCrush      = null;
    public UISlider         pHPGauge    = null;
    public Animation        pAnimCrush  = null;

    private eUnitState      m_eState      = eUnitState.None;
    private int             m_iEnergeCount = 0;
    private float           m_fStartY   = 0.0f;

    private float m_fSpeed    = 0.0f;
    public float m_fDecSpeed  = 0.0f;
    public float m_fUpSpeed   = 0.0f;
    public float m_fDownSpeed = 0.0f;

    public void Awake()
    {
        pInfo = new S2UIUnitInfo();
        pInfo.Create(gameObject, pUnitType);
        pInfo.SetMaxHP(100.0f);
        pInfo.m_pCrushCallback  = OnEventToCrushDamage;
        
        m_iEnergeCount    = 0;
        m_fStartY         = transform.localPosition.y;
        ChangeState(eUnitState.Idle);
    }

    public void FixedUpdate()
    {
        SetGauge(pInfo.GetHP(), pInfo.m_fMaxHP);
        switch (m_eState)
        {
            case eUnitState.Idle:       UpdateToIdle();     break;
            case eUnitState.JumpUp:     UpdateToJumpUp();   break;
            case eUnitState.JumpDown:   UpdateToJumpDown(); break;
            case eUnitState.CrushDmg:   UpdateToCrushDmg(); break;
        }
    }
    void UpdateToIdle()
    {
        // 퍼즐이 대기중일때 에너지가 모여있으면 발사시켜주기
        if (ePuzzleState.ControlIdle != Single.Puzzle.m_eState)
            return;

        if (0 == m_iEnergeCount)
            return;

        m_fSpeed = m_fUpSpeed;
        ChangeState(eUnitState.JumpUp);
    }
    void UpdateToJumpUp()
    {
        Vector3 vPosition = transform.localPosition;
        vPosition.y += m_fSpeed;
        m_fSpeed -= m_fDecSpeed;
        if (0.0f > m_fSpeed)
        {
            S2Damage pDamage = Single.Damage.AddDamageUIToTargetOffset(
                "DamageAttack",
                Single.UIInGameFront.GetUnit(ePuzzleBlockType.None),
                vPosition.x, vPosition.y);
            pDamage.SetDamageValue((float)m_iEnergeCount);

            InitEnerge();
            m_fSpeed = m_fDownSpeed;
            ChangeState(eUnitState.JumpDown);
        }
        transform.localPosition = vPosition;
    }
    void UpdateToJumpDown()
    {
        Vector3 vPosition = transform.localPosition;
        vPosition.y -= m_fSpeed;
        if (m_fStartY >= vPosition.y)
        {
            vPosition.y = m_fStartY;
            ChangeState(eUnitState.Idle);
        }
        transform.localPosition = vPosition;
    }
    void UpdateToCrushDmg()
    {

    }

    void ChangeState(eUnitState eState)
    {
        m_eState = eState;
    }

    void InitEnerge()
    {
        pEnerge.SetActive(false);
        pCharge.SetActive(false);
        m_iEnergeCount = 0;
    }

    float m_fLerp = 0.0f;
    void SetGauge(float fHP, float fMaxHP)
    {
        if (null == pHPGauge)
            return;

        float fPercent = Mathf.Clamp(S2Math.Divide(fHP, fMaxHP), 0.0f, 1.0f);
        if (pHPGauge.value == fPercent)
        {
            m_fLerp = 0.0f;
            return;
        }

        m_fLerp += 0.05f * (0.5f + Mathf.Abs(pHPGauge.value - fPercent));
        pHPGauge.value = Mathf.Lerp(pHPGauge.value, fPercent, m_fLerp);
    }

    public void OnEventToCrushDamage(S2Damage pDamage)
    {
        if (null != pEnerge)
        {
            pEnerge.SetActive(true);
            ++m_iEnergeCount;
        }

        if (null != pCharge)
        {
            pCharge.SetActive(false);
            pCharge.SetActive(true);
        }

        if (null != pCrush)
        {
            pCrush.SetActive(false);
            pCrush.SetActive(true);

            pInfo.AddHP(-pDamage.GetDamageValue());
            if (true == pInfo.IsHPToZero())
                pInfo.SetMaxHP(100.0f);

            ChangeState(eUnitState.CrushDmg);
        }

        if (null != pAnimCrush)
        {
            pAnimCrush.Play();
        }
    }
}

public partial class S2UIUnitInfo : S2Unit
{
    // 다양화 : 업데이트
    public override void FrameMove()
    {
        
    }

    // 다양화(이벤트) : 데미지 충돌
    public override void CrushDamage(S2Damage pDamage)
    {
        if (null == m_pCrushCallback)
            return;

        m_pCrushCallback(pDamage);
    }
}