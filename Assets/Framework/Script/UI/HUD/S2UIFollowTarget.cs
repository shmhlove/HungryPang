/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 09월 19일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 HUD FollowTarget을 제어합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;

public class S2UIHUD_FollowTarget : MonoBehaviour
{
    // 기능
    public float        m_fPlusX      = 0.0f;
    public float        m_fPlusY      = 0.0f;

    // 타켓
    private Transform   m_pTarget     = null;

    // Camera
    private Camera      m_pGameCamera = null;
    private Camera      m_pUICamera   = null;

    // 시스템 : 오브젝트 프레임 시작할때
    void Start()
    {
        SetActive(false);
    }

    // 시스템 : 업데이트
    void Update()
    {
        if (false == CheckToComponent())
            return;

        // 월드위치를 뷰포트위치로 변환해서 카메라 Rect 적용
        Vector3 vTargetViewPos  = m_pGameCamera.WorldToViewportPoint(m_pTarget.position);
        {
            Rect pCameraRect = m_pGameCamera.rect;
            vTargetViewPos.x *= pCameraRect.width;
            vTargetViewPos.y *= pCameraRect.height;
            vTargetViewPos.x += pCameraRect.x;
            vTargetViewPos.y += pCameraRect.y;
            vTargetViewPos.z = 0.0f;
        }

        // 뷰포트위치를 월드위치로 변환
        Vector3 vTargetPos = m_pUICamera.ViewportToWorldPoint(vTargetViewPos);
        transform.position = vTargetPos;

        // Plus오프셋 적용
        Vector3 vLocalPos = transform.localPosition;
        vLocalPos.x += m_fPlusX;
        vLocalPos.y += m_fPlusY;
        transform.localPosition = vLocalPos;
    }

    // 유틸 : 컴포넌트 체크
    bool CheckToComponent()
    {
        if (null == m_pTarget)
            return false;

        m_pGameCamera = Single.Camera.GetCureentCamera();
        if (null == m_pGameCamera)
            return false;

        m_pUICamera = NGUITools.FindCameraForLayer(gameObject.layer);
        if (null == m_pUICamera)
            return false;

        return true;
    }

    // 유틸 : Visible 설정
    void SetActive(bool bIsActive)
    {
        if (bIsActive == gameObject.activeInHierarchy)
            return;

        gameObject.SetActive(bIsActive);
    }

    // 인터페이스 : 타켓설정
    public void SetTarget(Transform pTransform, float fPlusX, float fPlusY)
    {
        m_pTarget = pTransform;
        m_fPlusX  = fPlusX;
        m_fPlusY  = fPlusY;

        SetActive(true);
    }
}