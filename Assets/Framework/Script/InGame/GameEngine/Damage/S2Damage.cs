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

// 데미지 이벤트 관련
public enum eDamageEventType
{
    Destroy,
}
public class S2DamageEvent
{
    public Action<S2Damage> m_pEventToDestroy      = null;

    public void CallEvent(eDamageEventType eType, S2Damage pDamage)
    {
        switch(eType)
        {
            case eDamageEventType.Destroy:
                if (null != m_pEventToDestroy)
                    m_pEventToDestroy(pDamage);
                break;
        }
    }
}

// 데미지 이펙트 관련
public enum eDamageEffectCreateType
{
    Start,
    End,
    LifeTime,
    Crush,
}
[Serializable]
public class S2DamageEffect
{
    public string   m_strName               = string.Empty;       // 이팩트 프리팹 이름

    public int      m_iCreateToLifeTime     = -1;                 // 생성:  LifeTime
    public bool     m_bCreateToCrush        = false;              // 생성 : 충돌시
    public bool     m_bCreateToStart        = false;              // 생성 : 데미지 생성될때 
    public bool     m_bCreateToEnd          = false;              // 생성 : 데미지 제거될때 

    [SerializeField]
    public S2EffectInfo m_pInfo                 = new S2EffectInfo();
}

// 데미지 오브젝트
[Serializable]
public partial class S2Damage : MonoBehaviour
{
    // 데미지 기본정보
    private S2Unit          m_pWho          = null;                                  // 데미지 발생한 유닛
    private BoxCollider     m_pCollider     = null;                                  // 데미지 콜리더
    private S2DamageEvent   m_pEvent        = null;                                  // 데미지 시간이 0이 되었을때

    // 데미지 속성
    [NonSerialized]
    public string           m_strID         = string.Empty;                          // 데미지 식별자
    public string           m_strParent     = string.Empty;                          // 부모 데미지 이름

    [SerializeField] private int              m_iLifeTime     = 0;                   // 데미지 라이프타임(틱 단위,, -1은 무한대)
    [SerializeField] private eBOOL            m_eEndToCrush   = eBOOL.None;          // 충돌 시 이 데미지를 즉시 제거할 것인가?

    [SerializeField] private float            m_fDamage       = 0.0f;                // 데미지 값
    [SerializeField] private eBOOL            m_eNoDamage     = eBOOL.None;          // 충돌되어도 데미지를 주지 않음
    [SerializeField] private eHitUnitType     m_eHitUnit      = eHitUnitType.None;   // 피격할 유닛타입
    [SerializeField] private int              m_iHitTimeToGap = -1;                  // 피격할 시간간격(틱 단위,,)

    [SerializeField] private Vector3          m_vStartOffset  = Vector3.zero;        // 발생자 피벗위치에서 어디에 생성될 것인가?
    [SerializeField] private Vector3          m_vSpeed        = Vector3.zero;        // 이동속도

    // 스케일
    public float            m_fScaleSpeed     = 0.0f;
    public Vector3          m_vLimitScale     = Vector3.zero;

    // 유도탄
    public bool             m_bIsUseTraceCuv  = false;
    public AnimationCurve   m_pTraceCuvSpeed  = AnimationCurve.Linear(0, 0, 1, 1); // 쫓아갈 커브속도
    public float            m_fTraceSpeed     = 0.0f;              // 쫓아갈 속도
    public AnimationCurve   m_pTraceCuvAngle  = AnimationCurve.Linear(0, 0, 1, 1); // 쫓아갈 커브속도
    public float            m_fHommingAngle   = 0.0f;              // 쫓아갈 회전속도
    public bool             m_bTraceRandomDir = false;             // 쫓아갈 초기 방향 랜덤
    public Vector3          m_pTraceStartDir  = Vector3.zero;      // 쫓아갈 초기 방향
    public GameObject       m_pTraceTarget  = null;                // 쫓아갈 타켓
    public Vector3          m_vTraceDirection = Vector3.zero;

    public AudioClip        m_pSoundCrush     = null;

    [SerializeField] private List<S2DamageEffect> m_pEffects = new List<S2DamageEffect>();// 생성할 이펙트 리스트

    // 변수
    private Vector3         m_vPosition     = Vector3.zero;        // 데미지 위치
    private int             m_iHitCheckTime = 0;                   // 피격할 시간간격 카운팅
    private float           m_fDirection    = 1.0f;                // 데미지 방향(1은 Right, -1은 Left)
    
    // 시스템 : 객체 제거시
    private void OnDestroy()
    {
        // 이팩트 생성
        CreateEffect(eDamageEffectCreateType.End);

        // 데미지 제거 이벤트 콜
        CallEvent(eDamageEventType.Destroy);

        // 오브젝트 제거
        DestroyDamage();
    }

    // 인터페이스 : 초기화
    public void Initialize(string strID, S2Unit pWho, S2DamageEvent pEvent)
    {
        // 데미지 기본정보 설정
        m_pWho                  = pWho;
        m_strID                 = strID;
        m_pEvent                = pEvent;
        m_pCollider             = gameObject.GetComponent<BoxCollider>();
        
        // 부모 데미지속성 복사
        CopyToParent();

        // 변수 초기화
        m_vPosition             = Vector3.zero;
        m_iHitCheckTime         = 0;

        // 데미지 방향설정
        SetDirection();

        // 데미지 오프셋 위치설정
        SetPosToOffset(m_vStartOffset);

        // 이팩트 생성
        CreateEffect(eDamageEffectCreateType.Start);

        // 데이미 타이머
        StartTimer();
    }

    // 데미지 업데이트
    public void FrameMove()
    {
        // 충돌 시간 갭 처리
        CountToHitTime();
        
        // 이동처리
        MoveToSpeed();

        // 스케일링
        ScaleToSpeed();

        // 이팩트 생성
        CreateEffect(eDamageEffectCreateType.LifeTime);

        // 라이프 시간 처리
        if (false == CountToLifeTime())
            DestroyDamage();
    }

    // 데미지 제거
    public void DestroyDamage()
    {
        if (null == gameObject)
            return;

        // 데미지 제거
        S2GameEngineSGT.DestroyObject(gameObject);
    }

    // 유닛과 충돌
    public void Crush(S2Unit pUnit)
    {
        // 충돌 체크타임 갱신
        if (-1 != m_iHitTimeToGap)
            m_iHitCheckTime = m_iHitTimeToGap;

        // 충돌시 데미지 제거체크
        if (eBOOL.True == m_eEndToCrush)
            m_iLifeTime = 0;

        Single.Sound.PlayClip(m_pSoundCrush, 0.3f, false);
        
        // 이팩트 생성
        CreateEffect(eDamageEffectCreateType.Crush);
    }

    // 데미지 위치
    public void SetPosition(Vector3 vPos)
    {
        transform.localPosition = m_vPosition = vPos;
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
            return (m_vPosition = transform.position);
        else
            return m_vPosition;
    }
    public void SetPosToAddOffset(Vector2 vOffset)
    {
        vOffset.x += m_vStartOffset.x;
        vOffset.y += m_vStartOffset.y;
        SetPosToOffset(vOffset);
    }
    public void SetPosToOffset(Vector2 vOffset)
    {
        Vector3 vOriPos = GetPosition();
        if (null != m_pWho)
            vOriPos   = m_pWho.GetPosition();

        SetPosToOffset(vOriPos, vOffset);
    }
    public void SetPosToOffset(Vector3 vPosition, Vector2 vOffset)
    {
        vPosition.x += vOffset.x * m_fDirection;
        vPosition.y += vOffset.y;
        SetPosition(vPosition);
    }

    // 데미지 스케일
    public void SetScale(Vector3 vScale)
    {
        transform.localScale = vScale;
    }
    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    // 데미지 정보
    public S2Unit GetWho()
    {
        return m_pWho;
    }
    public BoxCollider GetCollider()
    {
        return m_pCollider;
    }
    public float GetDamageValue()
    {
        if (eBOOL.True == m_eNoDamage)
            return 0.0f;

        return m_fDamage;
    }
    public void SetDamageValue(float fDmg)
    {
        m_fDamage = fDmg;
    }
    public int GetLife()
    {
        return m_iLifeTime;
    }
    public bool IsCheckCrushTime()
    {
        return (0 == m_iHitCheckTime);
    }
    public bool IsCheckToCrushUnitType(eUnitType eUnit)
    {
        // 모두 Hit대상인가?
        if (eHitUnitType.ALL == m_eHitUnit)
            return true;

        // 적이 Hit대상인가?
        if (eHitUnitType.Switch == m_eHitUnit)
        {
            if (null == m_pWho)
                return false;

            if (eUnitType.Character == m_pWho.m_eUnitType)
                return (eUnitType.Monster == eUnit);
            if (eUnitType.Monster == m_pWho.m_eUnitType)
                return (eUnitType.Character == eUnit);
        }
        
        // 지정된 유닛타입이 Hit대상인가?
        switch(eUnit)
        {
            case eUnitType.Character:   return (eHitUnitType.Character    == m_eHitUnit);
            case eUnitType.Monster:     return (eHitUnitType.Monster      == m_eHitUnit);
            case eUnitType.UIUnitBlue:  return (eHitUnitType.UIUnitBlue   == m_eHitUnit);
            case eUnitType.UIUnitGreen: return (eHitUnitType.UIUnitGreen  == m_eHitUnit);
            case eUnitType.UIUnitOrange:return (eHitUnitType.UIUnitOrange == m_eHitUnit);
            case eUnitType.UIUnitRed:   return (eHitUnitType.UIUnitRed    == m_eHitUnit);
            case eUnitType.UIUnitViolet:return (eHitUnitType.UIUnitViolet == m_eHitUnit);
        }

        return false;
    }

    // 유틸
    void SetDirection()
    {
        if (null == m_pWho)
            return;
        
        if (eDirection.Left == m_pWho.m_eDirection)
            m_fDirection = -1.0f;
        else
            m_fDirection = 1.0f;
    }
}