/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 09월 19일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 UI 데미지 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2UIDamage : S2UIBasePanel 
{
    public GameObject m_pRoot = null;
    public override Type GetPanelType() { return typeof(S2UIDamage); }
    public GameObject GetRoot()         { return m_pRoot; }

    public void OnEnable()
    {
        S2UIPuzzle pPuzzle = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        m_pRoot.transform.localPosition = new Vector3(pPuzzle.m_vOffset.x, pPuzzle.m_vOffset.y, 0.0f);
        m_pRoot.transform.localScale    = new Vector3(1.0f, 1.0f, 1.0f);
    }
}