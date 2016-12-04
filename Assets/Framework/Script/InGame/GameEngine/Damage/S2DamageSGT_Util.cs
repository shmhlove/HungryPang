/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 데미지관리 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2DamageSGT : S2Singleton<S2DamageSGT>
{
    // 유틸 : 데미지정보 추가
    void AddDamage()
    {
        foreach (KeyValuePair<string, S2Damage> kvp in m_pAddDamages)
        {
            m_pDamages.Add(kvp.Key, kvp.Value);
        }
        m_pAddDamages.Clear();
    }

    // 유틸 : 데미지정보 제거
    void DelDamage()
    {
        foreach (string strID in m_pDelDamages)
        {
            if (false == m_pDamages.ContainsKey(strID))
                continue;

            m_pDamages.Remove(strID);
        }
        m_pDelDamages.Clear();
    }

    // 유틸 : 유닛정보 제거
    void DelUnit()
    {
        foreach (string strUnitID in m_pDelUnits)
        {
            if (false == m_pUnits.ContainsKey(strUnitID))
                continue;

            m_pUnits.Remove(strUnitID);
        }
        m_pDelUnits.Clear();
    }
    
    // 유틸 : 유닛과 충돌체크
    void CheckToCrushUnit()
    {
        S2Util.ForeachToDic<string, S2Damage>(m_pDamages, (pDamageKey, pDamage) => 
        {
            if (false == pDamage.IsCheckCrushTime())
                return;

            S2Util.ForeachToDic<string, S2Unit>(m_pUnits, (pUnitKey, pUnit) =>
            {
                if (true == pUnit.IsHPToZero())
                    return;

                if (false == pDamage.IsCheckToCrushUnitType(pUnit.m_eUnitType))
                    return;

                BoxCollider pUnitCollider = pUnit.GetCollider();
                if (null == pUnitCollider)
                    return;

                BoxCollider pDamageCollider = pDamage.GetCollider();
                if (null == pDamageCollider)
                    return;

                if (false == pUnitCollider.bounds.Intersects(pDamageCollider.bounds))
                    return;

                // S2Util.Log("데미지 충돌!!(Unit : {0}, Damage : {1}", pUnit.m_strUnitID, pDamageKey);
                pUnit.OnEventToCrushDamage(pDamage);
                pDamage.Crush(pUnit);
            });
        });
    }

    // 유틸 : 데미지 업데이트
    void UpdateDamage()
    {
        S2Util.ForeachToDic<string, S2Damage>(m_pDamages, 
            (pKey, pDamage) => pDamage.FrameMove());
    }

    // 이벤트 : 데미지 제거될때
    void OnEventToDamageDestroy(S2Damage pDamage)
    {
        DelDamage(pDamage);
    }
}