/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 21일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 인게임 배경 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public partial class S2BackGround
{
    public S2BGMain m_pBGMain = null;
    public bool m_bIsPause = false;

    // 시스템 : 업데이트
    public void FrameMove()
    {
        if (null == m_pBGMain)
            return;

        if (true == m_bIsPause)
            return;

        m_pBGMain.FrameMove();
    }

    // 시스템 : 정지
    public void Pause()
    {
        m_bIsPause = true;
    }

    // 시스템 : 재개
    public void Resume()
    {
        m_bIsPause = false;
    }

    // 인터페이스 : 생성
    public void Create(string strBackGround)
    {
        GameObject pBackGround = Single.ResourceData.GetGameObject(strBackGround);
        S2GameObject.SetParent(pBackGround, "S2BackGround");
        m_pBGMain = S2GameObject.GetComponent<S2BGMain>(pBackGround);
    }

    // 인터페이스 : 이동
    public void Move(float fSpeed)
    {
        if (null == m_pBGMain)
            return;

        m_pBGMain.Move(fSpeed);
    }
}