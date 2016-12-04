/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 13일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐판 상태함수를 모아둔 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Puzzle
{
    // 초기화 : 상태변경 후 초기화
    public void InitState(ePuzzleState eState)
    {
        switch (eState)
        {
            case ePuzzleState.MoveProper:       InitStateToMoveProper();        break;
            case ePuzzleState.CheckFillBlock:   InitStateToCheckFillBlock();    break;
            case ePuzzleState.FallDown:         InitStateToFallDown();          break;
            case ePuzzleState.CheckMatch:       InitStateToCheckMatch();        break;
            case ePuzzleState.MatchCrush:       InitStateToMatchCrush();        break;
            case ePuzzleState.CheckNoMatch:     InitStateToCheckNoMatch();      break;
            case ePuzzleState.MixingBlock:      InitStateToMixingBlock();       break;
            case ePuzzleState.ControlIdle:      InitStateToControlIdle();       break;
            case ePuzzleState.Swaping:          InitStateToSwaping();           break;
            case ePuzzleState.RevertSwaping:    InitStateToRevertSwaping();     break;
        }
    }
    // 종료 : 상태변경 전 종료
    public void FinalState(ePuzzleState eState)
    {
        switch (eState)
        {
            case ePuzzleState.MoveProper:       FinalStateToMoveProper();        break;
            case ePuzzleState.CheckFillBlock:   FinalStateToCheckFillBlock();    break;
            case ePuzzleState.FallDown:         FinalStateToFallDown();          break;
            case ePuzzleState.CheckMatch:       FinalStateToCheckMatch();        break;
            case ePuzzleState.MatchCrush:       FinalStateToMatchCrush();        break;
            case ePuzzleState.CheckNoMatch:     FinalStateToCheckNoMatch();      break;
            case ePuzzleState.MixingBlock:      FinalStateToMixingBlock();       break;
            case ePuzzleState.ControlIdle:      FinalStateToControlIdle();       break;
            case ePuzzleState.Swaping:          FinalStateToSwaping();           break;
            case ePuzzleState.RevertSwaping:    FinalStateToRevertSwaping();     break;
        }
    }
    // 업데이트 : 상태 후 업데이트
    public void FrameMoveState(ePuzzleState eState)
    {
        if (true == IsStopState())
            return;

        switch (eState)
        {
            case ePuzzleState.MoveProper:       FrameMoveToMoveProper();        break;
            case ePuzzleState.CheckFillBlock:   FrameMoveToCheckFillBlock();    break;
            case ePuzzleState.FallDown:         FrameMoveToFallDown();          break;
            case ePuzzleState.CheckMatch:       FrameMoveToCheckMatch();        break;
            case ePuzzleState.MatchCrush:       FrameMoveToMatchCrush();        break;
            case ePuzzleState.CheckNoMatch:     FrameMoveToCheckNoMatch();      break;
            case ePuzzleState.MixingBlock:      FrameMoveToMixingBlock();       break;
            case ePuzzleState.ControlIdle:      FrameMoveToControlIdle();       break;
            case ePuzzleState.Swaping:          FrameMoveToSwaping();           break;
            case ePuzzleState.RevertSwaping:    FrameMoveToRevertSwaping();     break;
        }
    }

    // 상태 : 블럭들 각자 자리로 퍼트리기
    /////////////////////////////////////////////////////////
    void InitStateToMoveProper()
    {
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        S2Util.ForToDouble(m_iMaxRow, m_iMaxCol, (iRow, iCol) =>
        {
            S2BlockInfo pBlock = GetBlockInfo(iRow, iCol);
            pUI.CreateBlock(pBlock, iRow, iCol, m_pCallbackToUI);
            pUI.BlockToProperMove(pBlock, Vector2.zero, iRow, iCol);
            DelBlockInfo(iRow, iCol);
        });
    }
    void FinalStateToMoveProper() { }
    void FrameMoveToMoveProper()
    {
        if (true == IsEmptyBlock())
            return;

        ChangeState(ePuzzleState.ControlIdle);
    }

    // 상태 : 퍼즐판 채우기
    /////////////////////////////////////////////////////////
    void InitStateToCheckFillBlock() { }
    void FinalStateToCheckFillBlock() { }
    void FrameMoveToCheckFillBlock()
    {
        if (true == CheckStateToFillBlock())
            ChangeState(ePuzzleState.FallDown);
        else
            ChangeState(ePuzzleState.CheckMatch);
    }

    // 상태 : 퍼즐판 채우는 중
    /////////////////////////////////////////////////////////
    void InitStateToFallDown()
    { 
        foreach(KeyValuePair<S2BlockInfo, S2Int2> kvp in m_pFallDownBlocks)
        {
            MoveBlockToFallDown(kvp.Key, kvp.Value.Value1, kvp.Value.Value2);
        }
        m_pFallDownBlocks.Clear();   
    }
    void FinalStateToFallDown() { }
    void FrameMoveToFallDown()
    {
        if (true == IsEmptyBlock())
            return;

        ChangeState(ePuzzleState.CheckMatch);
    }

    // 상태 : 매칭체크
    /////////////////////////////////////////////////////////
    void InitStateToCheckMatch() { }
    void FinalStateToCheckMatch() { }
    void FrameMoveToCheckMatch()
    {
        if (true == CheckStateToMatch())
            ChangeState(ePuzzleState.MatchCrush);
        else if (true == IsBeforeState(ePuzzleState.Swaping))
            ChangeState(ePuzzleState.RevertSwaping);
        else
            ChangeState(ePuzzleState.CheckNoMatch);
    }

    // 상태 : 매칭블럭 깨기
    /////////////////////////////////////////////////////////
    void InitStateToMatchCrush()
    {
        string strDevInfo = string.Empty;

        m_pCrushBlocks.Clear();
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        foreach (KeyValuePair<ePuzzleBlockType, List<S2BlockInfo>> kvp in m_pMatchBlocks)
        {
            if (0 == kvp.Value.Count)
                continue;

            strDevInfo += string.Format("{0}({1})\n", kvp.Key.ToString(), kvp.Value.Count);

            foreach(S2BlockInfo pBlock in kvp.Value)
            {
                m_pCrushBlocks.Add(pBlock);
                pUI.BlockToCrush(pBlock);
                DelBlockInfo(pBlock.m_iRow, pBlock.m_iCol);
            }
        }

        // Dev : 매칭 출력
        if (false == string.IsNullOrEmpty(strDevInfo))
        {
            strDevInfo = strDevInfo.Substring(0, strDevInfo.Length - 1);
            S2UIDevInfo pDevUI = Single.UIInGameFront.GetPanel<S2UIDevInfo>();
            pDevUI.SetMatchBlockLabel(strDevInfo);
        }

        // 콤보처리
        ComboCount(m_pSelBlock1, m_pCrushBlocks[0], m_pMatchBlocks.Count);

        // Match이벤트 콜
        S2PuzzleMatchEvent pMatchInfo   = new S2PuzzleMatchEvent();
        pMatchInfo.m_dicMatchBlocks     = new Dictionary<ePuzzleBlockType, List<S2BlockInfo>>(m_pMatchBlocks);
        pMatchInfo.m_iComboCount        = m_iComboCount;
        pMatchInfo.m_eComboType         = m_eComboType;
        EventToMatch.CallBack<S2PuzzleMatchEvent>(this, pMatchInfo);

        // 매칭블럭 초기화
        m_pMatchBlocks.Clear();
    }
    void FinalStateToMatchCrush()
    {
        m_pSelBlock1 = null;
        m_pSelBlock2 = null;
    }
    void FrameMoveToMatchCrush()
    {
        if (0 != m_pCrushBlocks.Count)
            return;

        // 캐릭터 액션 종료시 까지 대기

        ChangeState(ePuzzleState.CheckFillBlock);
    }

    // 상태 : 절대 매칭되지 않는지 체크
    /////////////////////////////////////////////////////////
    void InitStateToCheckNoMatch() { }
    void FinalStateToCheckNoMatch() { }
    void FrameMoveToCheckNoMatch()
    {
        if (false == CheckStateToNoMatch())
            ChangeState(ePuzzleState.ControlIdle);
        else
            ChangeState(ePuzzleState.MixingBlock);
    }

    // 상태 : 절대 매칭되지 않아 섞는 중
    /////////////////////////////////////////////////////////
    void InitStateToMixingBlock()
    {
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        S2Util.ForToDouble(m_iMaxRow, m_iMaxCol, (iRow, iCol) =>
        {
            S2BlockInfo pBlock = GetBlockInfo(iRow, iCol);
            pUI.BlockToProperMove(pBlock, Vector2.up);
            DelBlockInfo(iRow, iCol);
        });
    }
    void FinalStateToMixingBlock() { }
    void FrameMoveToMixingBlock()
    {
        if (true == IsEmptyBlock())
            return;

        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        pUI.DestoryBlock();
        
        Initialize(m_iMaxRow, m_iMaxCol);
    }

    // 상태 : 조작대기
    /////////////////////////////////////////////////////////
    void InitStateToControlIdle()
    {
        m_pSelBlock1 = null;
        m_pSelBlock2 = null;
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        S2Util.ForToDouble(m_iMaxRow, m_iMaxCol, (iRow, iCol) =>
        {
            S2BlockInfo pBlock = GetBlockInfo(iRow, iCol);
            pUI.BlockToButton(pBlock, true);
            pUI.BlockToSelect(pBlock, false);
        });

        // 스왑 실패시에는 제어시간 초기화 하면 안된다.
        if (false == IsBeforeState(ePuzzleState.RevertSwaping))
            StartControlTimer();
    }
    void FinalStateToControlIdle()
    {
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        S2Util.ForToDouble(m_iMaxRow, m_iMaxCol, (iRow, iCol) =>
        {
            pUI.BlockToButton(GetBlockInfo(iRow, iCol), false);
        });
    }
    void FrameMoveToControlIdle()
    {
        // @@ 개발용 디버그
        S2UIDevInfo pDevPanel = Single.UIInGameFront.GetPanel<S2UIDevInfo>();
        pDevPanel.SetComboLabel(S2Util.Format("{0}({1}), Timer({2:F1}/{3:F1}sec)",
            m_eComboType, m_iComboCount, GetControlTimer(), Single.Hard.m_fComboTimer));

        if (null == m_pSelBlock1)
            return;

        if (null == m_pSelBlock2)
            return;

        ChangeState(ePuzzleState.Swaping);
    }

    // 상태 : 스왑 중
    /////////////////////////////////////////////////////////
    void InitStateToSwaping() 
    {
        // 선택된 블럭이 없으면 Null참조 오류 나도록 놔두자!!
        MoveBlockToSwap(m_pSelBlock1, m_pSelBlock2.m_iRow, m_pSelBlock2.m_iCol);
        MoveBlockToSwap(m_pSelBlock2, m_pSelBlock1.m_iRow, m_pSelBlock1.m_iCol);
    }
    void FinalStateToSwaping() { }
    void FrameMoveToSwaping()
    {
        if (true == IsEmptyBlock())
            return;

        ChangeState(ePuzzleState.CheckMatch);
    }

    // 상태 : 스왑 취소 중
    /////////////////////////////////////////////////////////
    void InitStateToRevertSwaping() 
    {
        // 선택된 블럭이 없으면 Null참조 오류 나도록 놔두자!!
        MoveBlockToSwap(m_pSelBlock1, m_pSelBlock2.m_iRow, m_pSelBlock2.m_iCol);
        MoveBlockToSwap(m_pSelBlock2, m_pSelBlock1.m_iRow, m_pSelBlock1.m_iCol);
    }
    void FinalStateToRevertSwaping() 
    {
        m_pSelBlock1 = null;
        m_pSelBlock2 = null;
    }
    void FrameMoveToRevertSwaping()
    {
        if (true == IsEmptyBlock())
            return;

        ChangeState(ePuzzleState.ControlIdle);
    }
}