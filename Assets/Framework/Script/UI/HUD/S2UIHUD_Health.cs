/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 09월 19일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 HUD Health UI를 제어합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public class S2UIHUD_Health : MonoBehaviour
{
    // 컴포넌트
    public UISlider              m_pHealth       = null;
    public UISprite              m_pThumb        = null;
    
    // 기능
    public float                 m_fLerpSpeed    = 0.001f;

    // FollowTarget
    private S2UIHUD_FollowTarget m_pFollowTarget = null;

    // 기타
    private float   m_fPercent  = 0.0f;
    private bool    m_bIsActive = false;
    private float   m_fLerp     = 0.0f;

    // 시스템 : 오브젝트 프레임 시작할때
    void Start()
    {
        SetActive(false);
    }

    // 시스템 : 업데이트
    void Update()
    {
        if (null == m_pHealth)
            return;

        if (m_pHealth.value == m_fPercent)
        {
            m_fLerp = 0.0f;
            if (null != m_pThumb)
                m_pThumb.enabled = false;
            return;
        }

        m_fLerp += m_fLerpSpeed * (0.5f + Mathf.Abs(m_pHealth.value - m_fPercent));
        m_pHealth.value = Mathf.Lerp(m_pHealth.value, m_fPercent, m_fLerp);

        if (null != m_pThumb)
            m_pThumb.enabled = true;
    }

    // 인터페이스 : 활성화
    public void SetActive(bool bIsActive)
    {
        gameObject.SetActive(m_bIsActive = bIsActive);
    }

    // 인터페이스 : FollowTarget설정
    public void SetFollowTarget(Transform pTransform)
    {
        if (null == m_pFollowTarget)
            m_pFollowTarget = gameObject.AddComponent<S2UIHUD_FollowTarget>();

        Vector3 vLocalScale = transform.localScale;
        if (null != m_pHealth.backgroundWidget)
            vLocalScale = S2Math.MulToVector(m_pHealth.backgroundWidget.localSize, transform.localScale);

        m_pFollowTarget.SetTarget(pTransform, -vLocalScale.x * 0.5f, -vLocalScale.y);
    }

    // 인터페이스 : 게이지 업데이트
    public void SetPercent(float fPercent)
    {   
        m_fPercent = fPercent;
    }
}