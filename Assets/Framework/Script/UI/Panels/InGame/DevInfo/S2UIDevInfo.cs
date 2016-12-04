/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 02일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 개발용 정보를 보기위한 UI클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2UIDevInfo : S2UIBasePanel 
{
    public UILabel m_pCombo     = null;
    public UILabel m_pMatchInfo = null;

    public override Type GetPanelType() { return typeof(S2UIDevInfo); }

    public void SetMatchBlockLabel(string strText)
    {
        if (null == m_pMatchInfo)
            return;

        m_pMatchInfo.text = strText;
    }

    public void SetComboLabel(string strText)
    {
        if (null == m_pCombo)
            return;

        m_pCombo.text = strText;
    }
}