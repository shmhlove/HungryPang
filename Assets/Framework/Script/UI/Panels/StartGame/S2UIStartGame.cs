/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 07월 26일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 게임 시작UI 패널클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2UIStartGame : S2UIBasePanel 
{
    public Action m_pEventToStart = null;

    public override Type GetPanelType() { return typeof(S2UIStartGame); }

    public void OnEventToStartGame()
    {
        if (null == m_pEventToStart)
            return;

        m_pEventToStart();
    }
}