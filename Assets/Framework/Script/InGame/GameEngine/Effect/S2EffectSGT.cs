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
    // 이팩트 리스트
    private List<string> m_pDelEffects = new List<string>();
    private Dictionary<string, S2Effect> m_pAddEffects = new Dictionary<string, S2Effect>();
    private Dictionary<string, S2Effect> m_pEffects    = new Dictionary<string, S2Effect>();

    // 기타
    private int m_iAddCount = 0;
    private bool m_bIsPause = false;

    public override void OnInitialize()
    {
        m_iAddCount = 0;
        m_bIsPause = false;
    }
    public override void OnFinalize()   { }

    // 시스템 : 업데이트
    public void FrameMove()
    {
        if (true == m_bIsPause)
            return;

        DelEffect();
        AddEffect();
        UpdateEffect();
    }

    // 시스템 : 정지
    public void Pause()
    {
        m_bIsPause = true;
    }

    // 시스템 : 재개
    public void Resume()
    {
        m_bIsPause = false;
    }

    // 인터페이스 : 이팩트 추가(Who는 없으면 null 줘도됨.)
    public bool AddEffect(string strEffectName, S2EffectInfo pInfo, GameObject pWho)
    {
        // 이팩트 생성
        GameObject pObject = Single.ResourceData.GetGameObject(strEffectName);
        if (null == pObject)
        {
            S2Util.LogError("이팩트 : 프리팹이 없습니다.(Name : {0})", strEffectName);
            return false;
        }

        // 이팩트 부모용 빈 게임오브젝트 생성 ( 이펙트 자체의 Transform 문제로 부모가 필요해짐 )
        string strID = S2Util.Format("{0}_{1}", strEffectName, ++m_iAddCount);
        GameObject pParent = S2GameObject.CreateEmptyObject(strID);
        S2GameObject.SetParent(pParent, gameObject);
        
        // 컴포넌트 생성
        S2Effect pEffect = S2GameObject.GetComponent<S2Effect>(pParent);
        if (null == pEffect)
        {
            S2Util.LogError("이팩트 : S2Effect컴포넌트가 없습니다.(Name : {0})", strEffectName);
            return false;
        }

        // 초기화
        S2EffectEvent pEvent = new S2EffectEvent();
        pEvent.m_pEventToDestroy = OnEventToEffectDestroy;
        pEffect.Initialize(strID, pObject, pInfo, pWho, pEvent);
        
        // 등록
        m_pAddEffects.Add(strID, pEffect);
        return true;
    }

    // 인터페이스 : 이팩트 제거
    public void DelEffect(S2Effect pEffect)
    {
        m_pDelEffects.Add(pEffect.m_strID);
    }
}