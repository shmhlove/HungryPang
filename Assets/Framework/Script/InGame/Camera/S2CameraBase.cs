/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 10월 17일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 연출카메라의 베이스 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public enum eCameraType
{
    None,
    Main,
    Sample,
    Zoom,
}

public class S2CameraBase : MonoBehaviour
{
    // 카메라 기본속성
    public Camera       m_pCamera       = null;
    public eCameraType  m_eCameraType   = eCameraType.None;
    public Animation    m_pAnimation    = null;

    // 기타
    protected bool      m_bIsActive     = false;

    // 시스템 : 생성될때
    public virtual void Awak() { }

    // 시스템 : 켜질때
    public virtual void OnEnable()
    {
        StartAnimation();
    }

    // 시스템 : 꺼질때
    public virtual void OnDisable()
    {
        StopAnimation();
    }

    // 시스템 : 업데이트
    public virtual void FrameMove() { }

    // 이벤트 : 다른 카메라로 교체되어 꺼질때
    public virtual void OnChangeOut(S2CameraBase pInCamera) { }

    // 이벤트 : 이 카메라로 교체되어 켜질때
    public virtual void OnChangeIn(S2CameraBase pOutCamera) { }

    // 인터페이스 : 활성화
    public void SetActive(bool bIsActive)
    {
        gameObject.SetActive((m_bIsActive = bIsActive));
    }

    public bool IsActive()
    {
        return m_bIsActive;
    }

    // 유틸 : 애니메이션 시작
    void StartAnimation()
    {
        if (null == m_pAnimation)
            return;

        m_pAnimation.Stop();
        m_pAnimation.Play();
    }

    // 유틸 : 애니메이션 종료
    void StopAnimation()
    {
        if (null == m_pAnimation)
            return;

        m_pAnimation.Stop();
    }
}
