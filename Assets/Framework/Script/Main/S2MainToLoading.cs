/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 07월 26일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 메인 클래스(로딩)입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2MainToLoading : MonoBehaviour 
{
    void Start()
    {
        // 씬별 데이터 로드
        S2UILoading pPanel = Single.UILoading.GetPanel<S2UILoading>();
        Single.Data.Load(Single.Scene.m_eCurrentScene, OnEventToLoadComplate, pPanel.OnEventToPrograss);
    }

    void OnEventToLoadComplate(object pSender, EventArgs vArgs)
    {
        S2LoadEvent pInfo = Single.Event.GetArgs<S2LoadEvent>(vArgs);
        S2Util.Log("데이터 로드완료(" +
                    "로드시간:<color=yellow>{0}</color>sec)", 
                    S2Math.Round(pInfo.m_pTime.Value1, 2));

        S2GameEngineSGT.DestroyObject(gameObject);
    }
}
