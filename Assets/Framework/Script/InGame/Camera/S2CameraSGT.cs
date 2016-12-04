/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 17일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 메인 카메라 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;

public partial class S2CameraSGT : S2Singleton<S2CameraSGT>
{
    // 시스템 : 생성
    public override void OnInitialize()
    {
        SetChildrenCamera();
    }

    // 시스템 : 삭제
    public override void OnFinalize() { }

    // 시스템 : 업데이트
    public void FrameMove()
    {
        if (null == m_pOnCamera)
            return;

        m_pOnCamera.FrameMove();
    }

    // 인터페이스 : 메인카메라 얻기
    public Camera GetMainCamera()
    {
        return m_pMainCamera;
    }

    // 인터페이스 : 현재 On된 카메라
    public Camera GetCureentCamera()
    {
        if (null == m_pOnCamera)
            return null;

        return m_pOnCamera.m_pCamera;
    }

    // 인터페이스 : 카메라 얻기
    public S2CameraBase GetCamera(eCameraType eType)
    {
        if (false == m_dicCameras.ContainsKey(eType))
            return null;

        return m_dicCameras[eType];
    }

    // 인터페이스 : 카메라 변경
    [S2AttributeToShowFunc]
    public void SetCamera()
    {
        SetCamera(m_eOnCamera);
    }
    public void SetCamera(eCameraType eType)
    {
        S2CameraBase pInCamera = GetCamera(eType);
        if (null == pInCamera)
        {
            S2Util.LogError("등록되지 않은 카메라입니다.(Type : {0})", eType);
            return;
        }

        if (null != m_pOnCamera)
        {
            m_pOnCamera.OnChangeOut(pInCamera);
            m_pOnCamera.SetActive(false);
        }

        pInCamera.OnChangeIn(m_pOnCamera);
        pInCamera.SetActive(true);

        m_eOnCamera = eType;
        m_pOnCamera = pInCamera;
    }

    // 인터페이스 : 메인카메라를 기준으로한 월드상 카메라 크기
    public Vector3 GetWorldSize()
    {
        return GetWorldSize(GetWidth(), GetHeight(), 0.0f);
    }
    public Vector3 GetWorldSize(Vector3 vScreenPos)
    {
        return GetWorldSize(vScreenPos.x, vScreenPos.y, vScreenPos.z);
    }
    public Vector3 GetWorldSize(float fX, float fY, float fZ)
    {
        if (null == m_pMainCamera)
            return Vector3.zero;

        return m_pMainCamera.ScreenToWorldPoint(new Vector3(fX, fY, fZ));
    }

    // 인터페이스 : 메인카메라를 기준으로한 뷰포트 크기
    public Rect GetViewportRect()
    {
        if (null == m_pMainCamera)
            return new Rect();

        return m_pMainCamera.rect;
    }
}
