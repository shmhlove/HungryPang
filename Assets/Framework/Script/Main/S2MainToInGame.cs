/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 14일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 메인 클래스(인게임)입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2MainToInGame : MonoBehaviour 
{
	void Start () 
    {
        if (null == S2GameObject.Find("UI Root (2D) - Loading"))
        {
            Single.UIInGameFront.SetActive(false);
            Single.UIInGameBack.SetActive(false);
            Single.ResourceData.GetGameObject("UI Root (2D) - Loading");
            S2UILoading pPanel = Single.UILoading.GetPanel<S2UILoading>();
            Single.Data.Load(eSceneType.InGame, OnEventToLoadComplate, pPanel.OnEventToPrograss);
        }

        CheckStartScene();
	}

    void FixedUpdate()
    {
        if (false == Single.Engine.m_bIsStartEngine)
            return;

        Single.Engine.UpdateToFixed();
    }

    void OnEventToLoadComplate(object pSender, EventArgs vArgs)
    {
        S2LoadEvent pInfo = Single.Event.GetArgs<S2LoadEvent>(vArgs);
        S2Util.Log("데이터 로드완료(" +
                    "로드시간:<color=yellow>{0}</color>sec)", 
                    S2Math.Round(pInfo.m_pTime.Value1, 2));
    }

    void CheckStartScene()
    {
        // 데이터 로드 완료체크
        if (false == Single.Data.IsLoadDone())
        {
            Single.Coroutine.NextUpdate(CheckStartScene);
            return;
        }

        // 엔진 시작 실패시 게임시작으로
        if (false == Single.Engine.StartEngine())
            Single.Scene.GoTo(eSceneType.StartGame);

        // 인게임 UI켜기
        Single.UIInGameFront.SetActive(true);
        Single.UIInGameBack.SetActive(true);

        // 로딩 UI제거
        Single.UILoading.DoDestroy();

        // BGM켜기
        Single.Sound.Play("BGM", 0.7f, true);
    }
}
