/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 09월 19일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 유닛의 HUD를 제어합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public class S2UnitHUD : MonoBehaviour
{
    public Transform m_pTop         = null;
    public Transform m_pBottom      = null;
    
    private S2UIHUD  m_pUI          = null;

    // 시스템 : 생성
    void Start()
    {
        GameObject pUI = NGUITools.AddChild(HUDRoot.go, Single.ResourceData.GetGameObjectToNoCopy("UI HUD"));
        if (null == pUI)
            return;

        m_pUI = pUI.GetComponent<S2UIHUD>();
        if (null == m_pUI)
        {
            Destroy(pUI);
            return;
        }

        // 하이어라키 설정
        S2UIUnitHUD pPanel = Single.UIInGameFront.GetPanel<S2UIUnitHUD>();
        pPanel.SetChild(pUI.transform);

        // 이름설정
        pUI.name = S2Util.Format("{0}_{1}", pUI.name, gameObject.name);
        
        // 초기화
        m_pUI.Initialize(m_pTop, m_pBottom);
        m_pUI.SetActive(false);
    }

    // 시스템 : 개체 제거
    void OnDestroy()
    {
        if (null == m_pUI)
            return;
        
        m_pUI.Destroy();
    }

    // 인터페이스 : Health게이지
    public void SetHealth(float fMaxHP, float fHP)
    {
        if (null == m_pUI)
            return;

        m_pUI.SetHealthGauge(Mathf.Clamp(S2Math.Divide(fHP, fMaxHP), 0.0f, 1.0f));
        m_pUI.SetActive(eHUDType.HealthGauge, true);
    }
}
