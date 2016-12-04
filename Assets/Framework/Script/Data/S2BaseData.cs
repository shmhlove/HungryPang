/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 15일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 데이터의 베이스 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public abstract class S2BaseData
{
    public abstract void OnInitialize();
    public abstract void OnFinalize();
    public abstract void FrameMove();

    public abstract Dictionary<string, S2LoadData> GetLoadList(eSceneType eType);

    // 로드가 성공하든 실패하든 pDone를 반드시 호출해줘야한다~!!
    // 어떻게 반드시 호출하게 강제 할수가 없네...
    public abstract void Load
    (
        S2LoadData pInfo,                           // 로드할 데이터 정보
        Action<string, S2LoadStartInfo> pStart,     // 로드시작시 호출해야할 콜백
        Action<string, S2LoadEndInfo> pDone         // 로드완료시 호출해야할 콜백
    );
}
