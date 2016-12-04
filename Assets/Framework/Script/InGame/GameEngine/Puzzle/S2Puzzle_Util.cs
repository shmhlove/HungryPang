/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 13일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐판 유틸리티 함수를 모아둔 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2Puzzle
{
    // 생성 : 퍼즐판
    void CreatePuzzleInfo(int iMaxRow, int iMaxCol)
    {       
        // 데이터 초기화
        InitiToData();

        // UI 생성
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        pUI.CreatePuzzle(iMaxRow, iMaxCol);
        
        // 블럭 UI에 전달할 이벤트 정보 마련
        m_pCallbackToUI.m_pArrive   = OnEventUIToArrive;
        m_pCallbackToUI.m_pSelect   = OnEventUIToSelect;
        m_pCallbackToUI.m_pDrag     = OnEventUIToDrag;
        m_pCallbackToUI.m_pCrush    = OnEventUIToCrush;

        // 크기정보 저장
        m_iMaxRow = iMaxRow;
        m_iMaxCol = iMaxCol;

        // 퍼즐판 마련하기 : 생성 후 매칭 안될때 까지 매칭체크 및 부수기
        Single.Coroutine.NextUpdate(CreatePuzzleBlock);
    }

    // 생성 : 블럭
    void CreatePuzzleBlock()
    {
        if (null == m_pSpace)
            m_pSpace = new S2BlockInfo[m_iMaxRow][];

        // 빈 블럭 채우기
        S2Util.ForToDouble(m_iMaxRow, m_iMaxCol, (iRow, iCol) =>
        {
            if (null == m_pSpace[iRow])
                m_pSpace[iRow] = new S2BlockInfo[m_iMaxCol];
            else if (null != m_pSpace[iRow][iCol])
                return;

            S2BlockInfo pBlockInfo = new S2BlockInfo(GetNewBlockType());
            pBlockInfo.m_iRow = iRow;
            pBlockInfo.m_iCol = iCol;
            SetBlockInfo(pBlockInfo);
        });

        // 매칭체크
        if (true == CheckStateToMatch())
        {
            foreach (KeyValuePair<ePuzzleBlockType, List<S2BlockInfo>> kvp in m_pMatchBlocks)
            {
                foreach (S2BlockInfo pBlock in kvp.Value)
                    DelBlockInfo(pBlock.m_iRow, pBlock.m_iCol);
            }
            m_pMatchBlocks.Clear();
        }
        // 절대 매칭 안되는지 체크
        else if (false == CheckStateToNoMatch())
        {
            // 퍼즐판 상태 설정
            m_eBeforeState = ePuzzleState.None;
            m_eState = ePuzzleState.None;
            ChangeState(ePuzzleState.MoveProper);
            return;
        }

        Single.Coroutine.NextUpdate(CreatePuzzleBlock);
    }
    
    // 상태 : 상태변경
    void ChangeState(ePuzzleState eState)
    {
        FinalState(m_eState);
        m_eBeforeState  = m_eState;
        m_eState        = eState;
        InitState(m_eState);
    }

    // 상태 : 상태체크
    bool IsState(ePuzzleState eState)
    {
        return (eState == m_eState);
    }
    bool IsBeforeState(ePuzzleState eState)
    {
        return (eState == m_eBeforeState);
    }

    // 상태 : 채워야 할 블럭 체크
    bool CheckStateToFillBlock()
    {
        // 기존 블럭 자리 잡아주기
        m_pFallDownBlocks.Clear();
        Dictionary<int, int> pEmptyInfo = new Dictionary<int, int>();
        S2Util.ForInverseToDouble(m_iMaxRow - 1, m_iMaxCol - 1, (iRow, iCol) =>
        {
            if (false == pEmptyInfo.ContainsKey(iCol))
                pEmptyInfo[iCol] = 0;

            S2BlockInfo pInfo = GetBlockInfo(iRow, iCol);
            if (null == pInfo)
            {
                ++pEmptyInfo[iCol];
                return;
            }

            if (0 == pEmptyInfo[iCol])
                return;

            m_pFallDownBlocks.Add(pInfo, new S2Int2((iRow + pEmptyInfo[iCol]), iCol));
        });

        // 빈 블럭 생성 및 자리 잡아주기
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        foreach (KeyValuePair<int, int> kvp in pEmptyInfo)
        {
            for (int iLoop = 0; iLoop < kvp.Value; ++iLoop)
            {
                S2BlockInfo pBlockInfo = new S2BlockInfo(GetNewBlockType());
                pUI.CreateBlock(pBlockInfo, -(iLoop + 1), kvp.Key, m_pCallbackToUI);
                m_pFallDownBlocks.Add(pBlockInfo, new S2Int2((kvp.Value - 1) - (iLoop), kvp.Key));
            }
        }

        return (0 != m_pFallDownBlocks.Count);
    }

    // 상태 : 빈 블럭 체크
    bool IsEmptyBlock()
    {
        return S2Util.ForToDoubleOfBreak(m_iMaxRow, m_iMaxCol, true, (iRow, iCol) =>
        {
            return (null == GetBlockInfo(iRow, iCol));
        });
    }

    // 상태 : 매칭체크
    bool CheckStateToMatch()
    {
        int iMatchCount = 0;
        m_pMatchBlocks.Clear();
        S2Util.ForToDouble(m_iMaxRow, m_iMaxCol, (iRow, iCol) =>
        {
            if (false == IsSafePosition(iRow, iCol))
                return;

            // 매치블럭 리스트 체크
            List<S2BlockInfo> pMatchs = GetMatchBlocks(iRow, iCol);
            if (null == pMatchs)
                return;

            // 매치블럭 중복체크 및 추가
            S2BlockInfo pBasisBlock = GetBlockInfo(iRow, iCol);
            ePuzzleBlockType eType  = pBasisBlock.m_eBlockType;
            foreach(S2BlockInfo pBlock in pMatchs)
            {
                if (false == m_pMatchBlocks.ContainsKey(eType))
                    m_pMatchBlocks.Add(eType, new List<S2BlockInfo>());

                if (true == m_pMatchBlocks[eType].Contains(pBlock))
                    continue;

                m_pMatchBlocks[eType].Add(pBlock);
                ++iMatchCount;
            }
        });

        return (0 != iMatchCount);
    }
    
    // 상태 : 매칭체크(블럭 하나에 대해 주변블럭 검사)
    List<S2BlockInfo> GetMatchBlocks(int iRow, int iCol)
    {      
        // 상하체크
        List<S2BlockInfo> pRowBlocks = new List<S2BlockInfo>();
        GetMatchBlocks(iRow, iCol, eDirection.Up,     ref pRowBlocks);
        GetMatchBlocks(iRow, iCol, eDirection.Down,   ref pRowBlocks);

        // 좌우체크
        List<S2BlockInfo> pColBlocks = new List<S2BlockInfo>();
        GetMatchBlocks(iRow, iCol, eDirection.Left,   ref pColBlocks);
        GetMatchBlocks(iRow, iCol, eDirection.Right,  ref pColBlocks);

        // 3개이상 맞춰야 매칭
        if (3 > pRowBlocks.Count)   pRowBlocks.Clear();
        if (3 > pColBlocks.Count)   pColBlocks.Clear();

        // 상하/좌우 매칭블럭 하나로 뭉쳐주기
        List<S2BlockInfo> pMatchBlocks = new List<S2BlockInfo>();
        //pMatchBlocks.AddRange(pRowBlocks);
        //pMatchBlocks.AddRange(pColBlocks);
        foreach (S2BlockInfo pInfo in pRowBlocks)
        {
            if (true == pMatchBlocks.Contains(pInfo))
                continue;
            pMatchBlocks.Add(pInfo);
        }
        foreach (S2BlockInfo pInfo in pColBlocks)
        {
            if (true == pMatchBlocks.Contains(pInfo))
                continue;
            pMatchBlocks.Add(pInfo);
        }

        return pMatchBlocks;
    }

    // 상태 : 매칭체크(블럭 하나에 대해 지정된 방향으로 주변블럭 검사)
    void GetMatchBlocks(int iRow, int iCol, eDirection eDir, ref List<S2BlockInfo> pList)
    {
        S2BlockInfo pBasisBlock = GetBlockInfo(iRow, iCol);
        if (null == pBasisBlock)
            return;

        if (false == pList.Contains(pBasisBlock))
            pList.Add(pBasisBlock);

        while (true)
        {
            // 인덱스 갱신
            switch(eDir)
            {
                case eDirection.Left:   iCol -= 1; break; 
                case eDirection.Right:  iCol += 1; break; 
                case eDirection.Up:     iRow -= 1; break; 
                case eDirection.Down:   iRow += 1; break; 
            }

            // 인덱스 체크
            if (false == IsSafePosition(iRow, iCol))
                return;

            // 매칭블럭정보 얻기
            S2BlockInfo pBlock = GetBlockInfo(iRow, iCol);
            if (null == pBlock)
                return;

            // 매칭체크
            if (pBasisBlock.m_eBlockType != pBlock.m_eBlockType)
                return;

            // 중복체크
            if (true == pList.Contains(pBlock))
                return;

            // 리스팅
            pList.Add(pBlock);
        }
    }

    // 상태 : 매칭체크(절대 매칭되지 않는지 체크)
    bool CheckStateToNoMatch()
    {
        // 알리아싱
        List<S2BlockInfo> pMatchs1 = null;
        List<S2BlockInfo> pMatchs2 = null;

        // 매칭체크
        return S2Util.ForToDoubleOfBreak(m_iMaxRow, m_iMaxCol, false, (iRow, iCol) =>
        {
            // 우측 체크
            SwapBlockInfo(iRow, iCol, iRow, iCol + 1);
            pMatchs1 = GetMatchBlocks(iRow, iCol);
            pMatchs2 = GetMatchBlocks(iRow, iCol + 1);
            SwapBlockInfo(iRow, iCol, iRow, iCol + 1);
            if ((0 != pMatchs1.Count) || (0 != pMatchs2.Count))
                return false;

            // 하측 체크
            SwapBlockInfo(iRow, iCol, iRow + 1, iCol);
            pMatchs1 = GetMatchBlocks(iRow, iCol);
            pMatchs2 = GetMatchBlocks(iRow + 1, iCol);
            SwapBlockInfo(iRow, iCol, iRow + 1, iCol);
            if ((0 != pMatchs1.Count) || (0 != pMatchs2.Count))
                return false;

            return true;
        });
    }

    // 정보 : 블럭정보 스왑
    void SwapBlockInfo(int iRow1, int iCol1, int iRow2, int iCol2)
    {
        S2BlockInfo pBlock1 = GetBlockInfo(iRow1, iCol1);
        S2BlockInfo pBlock2 = GetBlockInfo(iRow2, iCol2);
        if ((null == pBlock1) || (null == pBlock2))
            return;

        pBlock1.m_iRow = iRow2;
        pBlock1.m_iCol = iCol2;
        pBlock2.m_iRow = iRow1;
        pBlock2.m_iCol = iCol1;

        SetBlockInfo(pBlock1);
        SetBlockInfo(pBlock2);
    }

    // 정보 : 블럭정보 얻기
    S2BlockInfo GetBlockInfo(int iRow, int iCol)
    {
        if (false == IsSafePosition(iRow, iCol))
            return null;

        return m_pSpace[iRow][iCol];
    }

    // 정보 : 블럭정보 설정
    void SetBlockInfo(S2BlockInfo pInfo)
    {
        if (null == pInfo)
            return;

        if (false == IsSafePosition(pInfo.m_iRow, pInfo.m_iCol))
            return;

        m_pSpace[pInfo.m_iRow][pInfo.m_iCol] = pInfo;
    }

    // 정보 : 블럭정보 제거
    void DelBlockInfo(int iRow, int iCol)
    {
        if (false == IsSafePosition(iRow, iCol))
            return;

        m_pSpace[iRow][iCol] = null;
    }

    // 정보 : 새로운 블럭의 타입 얻기
    ePuzzleBlockType GetNewBlockType()
    {
        List<ePuzzleBlockType> pTypes = new List<ePuzzleBlockType>();
        S2Util.ForeachToEnum<ePuzzleBlockType>((eType) => 
        {
            if (ePuzzleBlockType.None == eType)
                return;

            pTypes.Add(eType);
        });
        return S2Util.RandomN<ePuzzleBlockType>(pTypes);
    }

    // 체크 : 안전한 위치인가?
    bool IsSafePosition(int iRow, int iCol)
    {
        if (0 > iRow || iRow >= m_iMaxRow)
            return false;

        if (0 > iCol || iCol >= m_iMaxCol)
            return false;

        return true;
    }

    // 체크 : 스왑가능한 블럭관계인가?
    bool IsSwapPossible(S2BlockInfo pInfo1, S2BlockInfo pInfo2)
    {
        // 상
        if (((pInfo1.m_iRow - 1) == pInfo2.m_iRow) && (pInfo1.m_iCol == pInfo2.m_iCol))
            return true;

        // 하
        if (((pInfo1.m_iRow + 1) == pInfo2.m_iRow) && (pInfo1.m_iCol == pInfo2.m_iCol))
            return true;

        // 좌
        if ((pInfo1.m_iRow == pInfo2.m_iRow) && ((pInfo1.m_iCol - 1) == pInfo2.m_iCol))
            return true;

        // 우
        if ((pInfo1.m_iRow == pInfo2.m_iRow) && ((pInfo1.m_iCol + 1) == pInfo2.m_iCol))
            return true;

        return false;
    }

    // 유틸 : 블럭이동(스왑)
    void MoveBlockToSwap(S2BlockInfo pInfo, int iRow, int iCol)
    {
        if (null == pInfo)
            return;

        // 이동할 블럭정보 제거(빈칸처리)
        DelBlockInfo(pInfo.m_iRow, pInfo.m_iCol);

        // UI 명령
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        pUI.BlockToSwapMove(pInfo, iRow, iCol);
    }

    // 유틸 : 블럭이동(떨어지기)
    void MoveBlockToFallDown(S2BlockInfo pInfo, int iRow, int iCol)
    {
        if (null == pInfo)
            return;

        // 이동할 블럭정보 제거(빈칸처리)
        DelBlockInfo(pInfo.m_iRow, pInfo.m_iCol);

        // UI 명령
        S2UIPuzzle pUI = Single.UIInGameBack.GetPanel<S2UIPuzzle>();
        pUI.BlockToFallDownMove(pInfo, iRow, iCol);
    }

    // 유틸 : 콤보처리
    void ComboCount(S2BlockInfo pSelBlock, S2BlockInfo pFirstCrush, int iMatchTypeCount)
    {
        if (null == pSelBlock)
            return;

        if (null == pFirstCrush)
            return;

        if (0 == iMatchTypeCount)
            return;

        // 한 가지 속성이 깨진거면 깨진블럭이 콤보대상
        if (1 == iMatchTypeCount)
            ComboCount(pFirstCrush);
        // 두 가지 이상 속성이 깨지면 유저가 선택한 블럭이 콤보대상
        else
            ComboCount(pSelBlock);
    }

    void ComboCount(S2BlockInfo pBlock)
    {
        if (null == pBlock)
            return;

        bool bIsInitCombo = false;

        // 체크 : 제한시간을 넘겼나?
        if (Single.Hard.m_fComboTimer < GetControlTimer())
            bIsInitCombo = true;

        // 체크 : 콤보블럭인가?
        if (false == IsComboBlock(pBlock.m_eBlockType))
            bIsInitCombo = true;
        else
        {
            // 콤보체크 : 다른 블럭인가?
            if (m_eComboType != pBlock.m_eBlockType)
                bIsInitCombo = true;
        }
        
        if (true == bIsInitCombo)
            m_iComboCount = 1;
        else
            m_iComboCount++;

        m_iComboCount   = Mathf.Clamp(m_iComboCount, 1, 3);
        m_eComboType    = pBlock.m_eBlockType;
        
        // @@ 개발용 디버그
        S2UIDevInfo pDevPanel = Single.UIInGameFront.GetPanel<S2UIDevInfo>();
        pDevPanel.SetComboLabel(S2Util.Format("{0}({1}), Timer(Stop)",
            m_eComboType, m_iComboCount));
    }

    bool IsComboBlock(ePuzzleBlockType eType)
    {
        return true;
    }

    void StartControlTimer()
    {
        Single.Timer.StartDeltaTime("ControlTimer");
    }

    float GetControlTimer()
    {
        return Single.Timer.GetDeltaTimeToSecond("ControlTimer");
    }
}
