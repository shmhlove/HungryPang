/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 07월 26일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 메인 클래스(게임시작)입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2MainToStartGame : MonoBehaviour 
{
    void Start()
    {
        S2UIStartGame pPanel    = Single.UIStartGame.GetPanel<S2UIStartGame>();
        pPanel.m_pEventToStart  = OnEventToStartGame;
    }

    void OnEventToStartGame()
    {
        Single.Scene.GoTo(eSceneType.InGame);
    }
}
