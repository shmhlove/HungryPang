/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 10월 17일
★ E-Mail ☞ shmhlove@naver.com

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public class S2CameraSample : S2CameraBase
{
    // 시스템 : 업데이트
    public override void FrameMove() { }

    // 이벤트 : 다른 카메라로 교체되어 꺼질때
    public override void OnChangeOut(S2CameraBase pInCamera)
    { 
    }

    // 이벤트 : 이 카메라로 교체되어 켜질때
    public override void OnChangeIn(S2CameraBase pOutCamera)
    { 
    }
}
