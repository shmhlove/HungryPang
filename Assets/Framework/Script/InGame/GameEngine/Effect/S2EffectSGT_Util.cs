/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 22일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 이펙트를 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class S2EffectSGT : S2Singleton<S2EffectSGT>
{
    // 유틸 : 이펙트정보 추가
    void AddEffect()
    {
        foreach (KeyValuePair<string, S2Effect> kvp in m_pAddEffects)
        {
            m_pEffects.Add(kvp.Key, kvp.Value);
        }
        m_pAddEffects.Clear();
    }

    // 유틸 : 이펙트정보 제거
    void DelEffect()
    {
        foreach (string strID in m_pDelEffects)
        {
            if (false == m_pEffects.ContainsKey(strID))
                continue;

            m_pEffects.Remove(strID);
        }
        m_pDelEffects.Clear();
    }

    // 유틸 : 이팩트 업데이트
    void UpdateEffect()
    {
        S2Util.ForeachToDic<string, S2Effect>(m_pEffects, 
            (pKey, pEffect) => pEffect.FrameMove());
    }

    // 이벤트 : 이팩트 제거
    void OnEventToEffectDestroy(S2Effect pEffect)
    {
        DelEffect(pEffect);
    }
}
