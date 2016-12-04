/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 10월 17일
★ E-Mail ☞ shmhlove@naver.com

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public class S2CameraZoom : S2CameraBase
{
    // 시스템 : 업데이트
    public override void FrameMove()
    {
        if (false == IsActive())
            return;

        if (false == m_pAnimation.isPlaying)
        {
            Single.Camera.SetCamera(eCameraType.Main);
        }
    }
}
