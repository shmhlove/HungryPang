/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 13일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐판 이벤트 콜백함수를 모아둔 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Puzzle
{
    // 이벤트 : 블럭 도착
    void OnEventUIToArrive(object pSender, EventArgs vArgs)
    {
        SetBlockInfo(Single.Event.GetArgs<S2BlockInfo>(vArgs));
    }

    // 이벤트 : 블럭 선택
    void OnEventUIToSelect(object pSender, EventArgs vArgs)
    {
        if (true == m_bIsPause)
            return;

        if (false == IsState(ePuzzleState.ControlIdle))
            return;

        if ((null != m_pSelBlock1) && (null != m_pSelBlock2))
            return;

        S2BlockInfo pInfo = Single.Event.GetArgs<S2BlockInfo>(vArgs);
        if (null == pInfo)
            return;
        
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        
        // 선택(첫블럭)
        if (null == m_pSelBlock1)
        {
            pUI.BlockToSelect(pInfo, true);
            m_pSelBlock1 = pInfo;
        }
        // 취소(첫블럭)
        else if (pInfo == m_pSelBlock1)
        {
        // 주석 : 선택취소 안하는게 느낌이 더 좋은듯
        //    pUI.BlockToSelect(pInfo, false);
        //    m_pSelBlock1 = null;
        }
        // 선택(두번째 블럭)
        else
        {
            pUI.BlockToSelect(m_pSelBlock1, false);
            if (true == IsSwapPossible(m_pSelBlock1, pInfo))
            {
                m_pSelBlock2 = pInfo;
            }
            else
            {
                pUI.BlockToSelect(pInfo, true);
                m_pSelBlock1 = pInfo;
            }
        }
    }

    // 이벤트 : 블럭 드래그
    void OnEventUIToDrag(object pSender, EventArgs vArgs)
    {
        if (true == m_bIsPause)
            return;

        if (false == IsState(ePuzzleState.ControlIdle))
            return;

        if ((null != m_pSelBlock1) && (null != m_pSelBlock2))
            return;

        if (null == m_pSelBlock1)
            return;

        S2Int2 pPos       = Single.Event.GetArgs<S2Int2>(vArgs);
        S2BlockInfo pInfo = GetBlockInfo(pPos.Value1, pPos.Value2);
        if (null == pInfo)
            return;

        if (pInfo == m_pSelBlock1)
            return;

        if (false == IsSwapPossible(m_pSelBlock1, pInfo))
            return;

        m_pSelBlock2 = pInfo;

        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        pUI.BlockToSelect(m_pSelBlock1, false);
    }

    // 이벤트 : 블럭 파괴
    void OnEventUIToCrush(object pSender, EventArgs vArgs)
    {
        S2BlockInfo pInfo = Single.Event.GetArgs<S2BlockInfo>(vArgs);
        if (null == pInfo)
            return;

        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        pUI.DestoryBlock(pInfo);
        
        m_pCrushBlocks.Remove(pInfo);
    }
}
