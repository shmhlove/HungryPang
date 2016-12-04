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
    // 데미지 리스트
    private List<string> m_pDelDamages = new List<string>();
    private Dictionary<string, S2Damage> m_pAddDamages  = new Dictionary<string, S2Damage>();
    private Dictionary<string, S2Damage> m_pDamages     = new Dictionary<string, S2Damage>();

    // 유닛 리스트
    private List<string> m_pDelUnits = new List<string>();
    private Dictionary<string, S2Unit> m_pUnits = new Dictionary<string, S2Unit>();

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

        DelUnit();
        AddDamage();
        DelDamage();
        
        CheckToCrushUnit();
        UpdateDamage();
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

    // 인터페이스 : World 데미지 추가
    public string AddDamage(string strDamageName, S2Unit pWho)
    {
        // 게임오브젝트 생성
        GameObject pObject = Single.ResourceData.GetGameObject(strDamageName);
        if (null == pObject)
        {
            S2Util.LogError("데미지 : 프리팹이 없습니다.(Name : {0})", strDamageName);
            return string.Empty;
        }

        // 데미지 부모설정
        S2GameObject.SetParent(pObject, gameObject);

        // 컴포넌트 생성
        S2Damage pDamage = pObject.GetComponent<S2Damage>();
        if (null == pDamage)
        {
            S2Util.LogError("데미지 : SGDamage컴포넌트가 없습니다.(Name : {0})", strDamageName);
            return string.Empty;
        }

        // 데미지 초기화
        string strID = S2Util.Format("{0}_{1}", strDamageName, ++m_iAddCount);
        S2DamageEvent pEvent = new S2DamageEvent();
        pEvent.m_pEventToDestroy = OnEventToDamageDestroy;
        pDamage.Initialize(strID, pWho, pEvent);

        // 등록
        m_pAddDamages.Add(strID, pDamage);
        return strID;
    }
    public string AddDamageToOffset(string strDamageName, S2Unit pWho, float fOffsetX, float fOffsetY)
    {
        string strKey    = AddDamage(strDamageName, pWho);
        S2Damage pDamage = GetDamage(strKey);
        if (null == pDamage)
            return string.Empty;

        pDamage.SetPosToOffset(new Vector2(fOffsetX, fOffsetY));
        return strKey;
    }
    public string AddDamageToAddOffset(string strDamageName, S2Unit pWho, float fOffsetX, float fOffsetY)
    {
        string strKey    = AddDamage(strDamageName, pWho);
        S2Damage pDamage = GetDamage(strKey);
        if (null == pDamage)
            return string.Empty;

        pDamage.SetPosToAddOffset(new Vector2(fOffsetX, fOffsetY));
        return strKey;
    }

    // 인터페이스 : UI 데미지 추가
    public string AddDamageUI(string strDamageName, S2Unit pWho)
    {
        // 게임오브젝트 생성
        GameObject pObject = Single.ResourceData.GetGameObject(strDamageName);
        if (null == pObject)
        {
            S2Util.LogError("데미지 : 프리팹이 없습니다.(Name : {0})", strDamageName);
            return string.Empty;
        }
        Vector3 vScale    = pObject.transform.localScale;
        Vector3 vPosition = pObject.transform.localPosition;

        // 데미지 부모설정
        S2UIDamage pParent = Single.UIInGameFront.GetPanel<S2UIDamage>();
        S2GameObject.SetParent(pObject, pParent.GetRoot());
        pObject.transform.localScale    = vScale;
        pObject.transform.localPosition = vPosition;

        // 컴포넌트 생성
        S2Damage pDamage = pObject.GetComponent<S2Damage>();
        if (null == pDamage)
        {
            S2Util.LogError("데미지 : SGDamage컴포넌트가 없습니다.(Name : {0})", strDamageName);
            return string.Empty;
        }

        // 데미지 초기화
        string strID = S2Util.Format("{0}_{1}", strDamageName, ++m_iAddCount);
        S2DamageEvent pEvent = new S2DamageEvent();
        pEvent.m_pEventToDestroy = OnEventToDamageDestroy;
        pDamage.Initialize(strID, pWho, pEvent);

        // 등록
        m_pAddDamages.Add(strID, pDamage);
        return strID;
    }
    public string AddDamageUIToOffset(string strDamageName, S2Unit pWho, float fOffsetX, float fOffsetY)
    {
        string strKey    = AddDamageUI(strDamageName, pWho);
        S2Damage pDamage = GetDamage(strKey);
        if (null == pDamage)
            return string.Empty;

        pDamage.SetPosToOffset(new Vector2(fOffsetX, fOffsetY));
        return strKey;
    }
    public string AddDamageUIToAddOffset(string strDamageName, S2Unit pWho, float fOffsetX, float fOffsetY)
    {
        string strKey    = AddDamageUI(strDamageName, pWho);
        S2Damage pDamage = GetDamage(strKey);
        if (null == pDamage)
            return string.Empty;

        pDamage.SetPosToAddOffset(new Vector2(fOffsetX, fOffsetY));
        return strKey;
    }
    private bool m_bDirSwitch = false;
    public S2Damage AddDamageUIToTargetOffset(string strDamageName, GameObject pTarget, float fOffsetX, float fOffsetY)
    {
        string strKey    = AddDamageUI(strDamageName, null);
        S2Damage pDamage = GetDamage(strKey);
        if (null == pDamage)
            return null;

        pDamage.m_pTraceTarget = pTarget;

        if (true == pDamage.m_bTraceRandomDir)
        {
            pDamage.m_pTraceStartDir = new Vector3(m_bDirSwitch ? -1.0f : 1.0f, 0.0f, 0.0f);
            m_bDirSwitch = !m_bDirSwitch;
        }

        pDamage.m_vTraceDirection = pDamage.m_pTraceStartDir;
        pDamage.SetPosToOffset(new Vector2(fOffsetX, fOffsetY));
        return pDamage;
    }

    // 인터페이스 : 생성된 데미지 얻기
    S2Damage GetDamage(string strDamageName)
    {
        if (true == m_pDamages.ContainsKey(strDamageName))
            return m_pDamages[strDamageName];

        if (true == m_pAddDamages.ContainsKey(strDamageName))
            return m_pAddDamages[strDamageName];

        return null;
    }

    // 인터페이스 : 데미지 제거
    public void DelDamage(S2Damage pDamage)
    {
        m_pDelDamages.Add(pDamage.m_strID);
    }

    // 인터페이스 : 유닛 등록
    public void AddUnit(string strUnitID, S2Unit pUnit)
    {
        if (true == m_pUnits.ContainsKey(strUnitID))
            S2Util.LogError("데미지 : 유닛 등록 중 중복ID 발생(ID : {0})", strUnitID);

        m_pUnits[strUnitID] = pUnit;
    }

    // 인터페이스 : 유닛 제거
    public void DelUnit(string strUnitID)
    {
        if (false == m_pUnits.ContainsKey(strUnitID))
            return;

        m_pDelUnits.Add(strUnitID);
        m_pUnits[strUnitID] = null;
    }
}