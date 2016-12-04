/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 데미지 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Damage : MonoBehaviour
{
    // 유틸 : 부모데미지 속성 복사(유의사항 : 초기값)
    void CopyToParent()
    {
        if (true == string.IsNullOrEmpty(m_strParent))
            return;

        GameObject pObject = Single.ResourceData.GetGameObject(m_strParent);
        if (null == pObject)
        {
            S2Util.LogError("데미지 : 부모 프리팹이 없습니다.(ParentName : {0})", m_strParent);
            return;
        }

        S2Damage pParent = pObject.GetComponent<S2Damage>();
        if (null == pParent)
        {
            S2Util.LogError("데미지 : 부모 프리팹에 SGDamage컴포넌트가 없습니다.(ParentName  : {0})", m_strParent);
            return;
        }

        CopyTo(pParent);

        pParent.DestroyDamage();
    }

    // 유틸 : 데미지 속성복사
    void CopyTo(S2Damage pCopy)
    {
        if (null == pCopy)
            return;

        if (0 == m_iLifeTime)
            m_iLifeTime         = pCopy.m_iLifeTime;

        if (eBOOL.None == m_eEndToCrush)
            m_eEndToCrush       = pCopy.m_eEndToCrush;

        if (eHitUnitType.None == m_eHitUnit)
            m_eHitUnit          = pCopy.m_eHitUnit;

        if (-1 == m_iHitTimeToGap)
            m_iHitTimeToGap     = pCopy.m_iHitTimeToGap;

        if (0.0f == m_fDamage)
            m_fDamage           = pCopy.m_fDamage;

        if (eBOOL.None == m_eNoDamage)
            m_eNoDamage         = pCopy.m_eNoDamage;

        if (Vector3.zero == m_vStartOffset)
            m_vStartOffset      = pCopy.m_vStartOffset;

        if (Vector3.zero == m_vSpeed)
            m_vSpeed            = pCopy.m_vSpeed;

        if (0 == m_pEffects.Count)
        {
            m_pEffects          = new List<S2DamageEffect>(pCopy.m_pEffects);
        }
    }

    // 유틸 : 데미지 종료 라이프 타임 체크
    bool CountToLifeTime()
    {
        if (-1 == m_iLifeTime)
            return true;

        return (0 < --m_iLifeTime);
    }

    // 유틸 : 타이머시작
    void StartTimer()
    {
        Single.Timer.StartDeltaTime(this.GetHashCode().ToString());
    }
    float GetLeftTimer()
    {
        return Single.Timer.GetDeltaTimeToSecond(this.GetHashCode().ToString());
    }

    // 유틸 : 데미지 충돌판단 시간 처리
    bool CountToHitTime()
    {
        if (0 == m_iHitCheckTime)
            return true;

        return (0 == --m_iHitCheckTime);
    }

    // 유틸 : 데미지 이동
    void MoveToSpeed()
    {
        if (null == m_pTraceTarget)
        {
                AddPositionX(m_vSpeed.x * m_fDirection);
                AddPositionY(m_vSpeed.y);
        }
        else
        {
            float fSpeed = m_fTraceSpeed;
            float fAngle = m_fHommingAngle;
            if (true == m_bIsUseTraceCuv)
            {
                fSpeed *= m_pTraceCuvSpeed.Evaluate(GetLeftTimer());
                fAngle *= m_pTraceCuvAngle.Evaluate(GetLeftTimer());
            }

            Vector3 vPosition = GetPosition();
            S2Physics.GuidedMissile(ref vPosition, ref m_vTraceDirection,
                m_pTraceTarget.transform.localPosition, fAngle, fSpeed);
            SetPosition(vPosition);
        }
    }

    void ScaleToSpeed()
    {
        if (0.0f == m_fScaleSpeed)
            return;

        Vector3 vScale = GetScale() * m_fScaleSpeed;
        if (1.0f < m_fScaleSpeed)
        {
            if (m_vLimitScale.x < vScale.x) vScale.x = m_vLimitScale.x;
            if (m_vLimitScale.y < vScale.y) vScale.y = m_vLimitScale.y;
            if (m_vLimitScale.z < vScale.z) vScale.z = m_vLimitScale.z;
        }
        else
        {
            if (m_vLimitScale.x > vScale.x) vScale.x = m_vLimitScale.x;
            if (m_vLimitScale.y > vScale.y) vScale.y = m_vLimitScale.y;
            if (m_vLimitScale.z > vScale.z) vScale.z = m_vLimitScale.z;
        }
        SetScale(vScale);
    }

    // 유틸 : 이펙트 생성
    void CreateEffect(eDamageEffectCreateType eCreateType)
    {
        // 이팩트 생성
        foreach (S2DamageEffect pInfo in m_pEffects)
        {
            switch(eCreateType)
            {
                case eDamageEffectCreateType.Start:
                    if (true == pInfo.m_bCreateToStart)
                        CreateEffect(pInfo);
                    break;
                case eDamageEffectCreateType.End:
                    if (true == pInfo.m_bCreateToEnd)
                        CreateEffect(pInfo);
                    break;
                case eDamageEffectCreateType.LifeTime:
                    if (m_iLifeTime == pInfo.m_iCreateToLifeTime)
                        CreateEffect(pInfo);
                    break;
                case eDamageEffectCreateType.Crush:
                    if (true == pInfo.m_bCreateToCrush)
                        CreateEffect(pInfo);
                    break;
            }
        }
    }
    void CreateEffect(S2DamageEffect pInfo)
    {
        if (null == pInfo)
            return;

        Single.Effect.AddEffect(pInfo.m_strName, pInfo.m_pInfo, gameObject);
    }

    // 유틸 : 이벤트 콜
    void CallEvent(eDamageEventType eType)
    {
        if (null == m_pEvent)
            return;

        m_pEvent.CallEvent(eType, this);
    }
}