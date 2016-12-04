/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 09월 19일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 HUD UI를 제어합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public enum eHUDType
{
    HealthGauge,
}

public class S2UIHUD : MonoBehaviour
{
    // HUD 컴포넌트
    public S2UIHUD_Health m_pHealth = null;

    // 붙을 위치 오브젝트
    private Transform m_pTop     = null;
    private Transform m_pBottom  = null;

    // 기타
    private bool     m_bIsActive = false;

    // 시스템 : 생성
    void Start()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale    = new Vector3(1.0f, 1.0f, 1.0f);
    }

    // 인터페이스 : 초기화
    public void Initialize(Transform pTop, Transform pBottom)
    {
        m_pTop      = pTop;
        m_pBottom   = pBottom;

        if (null != m_pHealth)
            m_pHealth.SetFollowTarget(m_pTop);

        SetActive(false);
    }

    // 인터페이스 : 제거
    public void Destroy()
    {
        S2GameEngineSGT.DestroyObject(gameObject);
    }

    // 인터페이스 : Active On/Off
    public void SetActive(bool bIsActive)
    {
        gameObject.SetActive(m_bIsActive = bIsActive);
    }
    public void SetActive(eHUDType eType, bool bIsActive)
    {
        if (true == bIsActive)
        {
            SetActive(bIsActive);
        }

        switch (eType)
        {
            case eHUDType.HealthGauge: SetActiveToHealth(bIsActive); break;
        }
    }
    void SetActiveToHealth(bool bIsActive)
    {
        if (null == m_pHealth)
            return;

        m_pHealth.SetActive(bIsActive);
    }

    // 인터페이스 : Health게이지 업데이트
    public void SetHealthGauge(float fPercent)
    {
        if (null == m_pHealth)
            return;

        m_pHealth.SetPercent(fPercent);
    }
}