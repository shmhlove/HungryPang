/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 21일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 인게임 배경 레이어 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public partial class S2BGMain : MonoBehaviour
{
    public List<S2BGLayer> m_pLayers = new List<S2BGLayer>();

    public void FrameMove()
    {
        foreach(S2BGLayer pLayer in m_pLayers)
        {
            pLayer.FrameMove();
        }
    }

    public void Move(float fSpeed)
    {
        foreach (S2BGLayer pLayer in m_pLayers)
        {
            pLayer.Move(fSpeed);
        }
    }
}