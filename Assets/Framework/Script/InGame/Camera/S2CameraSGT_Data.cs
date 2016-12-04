/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 17일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 메인 카메라 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class S2CameraSGT : S2Singleton<S2CameraSGT>
{
    [SerializeField]
    private Camera          m_pMainCamera   = null;

    [SerializeField]
    private S2CameraBase    m_pOnCamera     = null;

    [SerializeField]
    private eCameraType     m_eOnCamera     = eCameraType.None;

    private Dictionary<eCameraType, S2CameraBase> m_dicCameras =
        new Dictionary<eCameraType, S2CameraBase>();
}
