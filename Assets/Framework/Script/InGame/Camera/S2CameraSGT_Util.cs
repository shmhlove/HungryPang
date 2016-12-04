/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 17일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 메인 카메라 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public partial class S2CameraSGT : S2Singleton<S2CameraSGT>
{
    // 유틸 : 자식 카메라 찾아서 컨테이너에 담기
    void SetChildrenCamera()
    {
        m_dicCameras.Clear();
        S2Util.ForeachToArray<Transform>(GetComponentsInChildren<Transform>(),
        (pChild) =>
        {
            if (transform == pChild)
                return;

            S2CameraBase pCamera = pChild.GetComponent<S2CameraBase>();
            if (null == pCamera)
            {
                S2Util.LogError("S2CameraBase컴포넌트가 등록되지 않았습니다.(GameObject : {0})", pChild.name);
                return;
            }

            if (null == pCamera.m_pCamera)
            {
                pCamera.m_pCamera = pChild.GetComponent<Camera>();
                if (null == pCamera.m_pCamera)
                {
                    S2Util.LogError("Camera컴포넌트가 등록되지 않았습니다.(GameObject : {0})", pChild.name);
                    return;
                }
            }

            if (eCameraType.None == pCamera.m_eCameraType)
            {
                S2Util.LogError("CameraType None입니다. CameraType Enum추가 후 설정해 주세요.(GameObject : {0})", pChild.name);
                return;
            }

            S2CameraBase pDuplication = GetCamera(pCamera.m_eCameraType);
            if (null != pDuplication)
            {
                S2Util.LogError("중복된 CameraType입니다.(Type : {0}, \"{1}\", \"{2}\")", pCamera.m_eCameraType, pDuplication.name, pCamera.name);
                return;
            }

            m_dicCameras.Add(pCamera.m_eCameraType, pCamera);

            // 기본적으로 메인 카메라는 켜주고, 나머지는 다 커주자!!
            if (eCameraType.Main == pCamera.m_eCameraType)
                SetCamera(eCameraType.Main);
            else
                pCamera.SetActive(false);
        });

        if (0 == m_dicCameras.Count)
            S2Util.LogError("최소한 Camera 하나는 있어야 합니다.");
    }

    // 유틸 : 카메라 픽셀 크기
    float GetWidth()
    {
        if (null == m_pMainCamera)
            return 0.0f;

        return m_pMainCamera.pixelWidth;
    }
    float GetHeight()
    {
        if (null == m_pMainCamera)
            return 0.0f;

        return m_pMainCamera.pixelHeight;
    }
}
