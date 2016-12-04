/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 26일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 타이틀바 UI클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2UITitleBar : S2UIBasePanel 
{
    public override Type GetPanelType() { return typeof(S2UITitleBar); }

    public void OnBtnEventToOption()
    {
        Single.Engine.PauseToggle();
    }
}