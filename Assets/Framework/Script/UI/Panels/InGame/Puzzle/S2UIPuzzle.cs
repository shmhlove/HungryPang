/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐UI 패널클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2UIPuzzle : S2UIBasePanel 
{
    // 데이터 : 퍼즐판 앵커와 Y축 오프셋
    public Vector2 m_vOffset        = Vector2.zero;
    public Transform m_pBlockAnchor = null;

    // 데이터 : 퍼즐판 크기 정보
    int m_iMaxRow   = 0;
    int m_iMaxCol   = 0;
    int m_iWidth    = 0;
    int m_iHeight   = 0;

    // 데이터 : 블럭 UI 관련(루트, 오브젝트 풀, Active 블럭 리스트)
    GameObject m_pBlockRoot = null;
    Queue<S2UIPuzzleBlock> m_pBlockPool = null;
    Dictionary<S2BlockInfo, S2UIPuzzleBlock> m_pBlocks = null;
    
    public override Type GetPanelType() { return typeof(S2UIPuzzle); }

    // 생성 : 퍼즐판
    public void CreatePuzzle(int iMaxRow, int iMaxCol)
    {
        UIPanel pPanel = gameObject.GetComponent<UIPanel>();
        if (null == pPanel)
        {
            S2Util.Log("[S2UIPuzzle] UIWidget을 얻을 수 없습니다.(퍼즐판 크기정보)");
            return;
        }

        DestoryPuzzle();

        m_iMaxRow   = iMaxRow;
        m_iMaxCol   = iMaxCol;
        m_iWidth    = (int)pPanel.GetViewSize().x;
        m_iHeight   = (int)pPanel.GetViewSize().y;

        m_pBlockPool = new Queue<S2UIPuzzleBlock>();
        m_pBlocks   = new Dictionary<S2BlockInfo, S2UIPuzzleBlock>();
    }

    // 제거 : 퍼즐판
    public void DestoryPuzzle()
    {
        if (true == CheckExceptionToBlock())
            return;

        S2GameObject.DestoryObject(m_pBlockRoot);

        m_pBlockRoot    = null;
        m_pBlockPool    = null;
        m_pBlocks       = null;
    }

    // 생성 : 블럭
    public void CreateBlock(S2BlockInfo pInfo, int iRow, int iCol, S2PuzzleUIEvent pEvent)
    {
        if (true == CheckExceptionToBlock())
            return;

        S2UIPuzzleBlock pBlock = CreateBlockObject();;
        pBlock.OnCreate(pInfo, pEvent);
        pBlock.SetSize(GetSizeToBlock());
        pBlock.SetPosition(GetPositionToBlock(iRow, iCol));
        
        DestoryBlock(pInfo);
        m_pBlocks.Add(pInfo, pBlock);
    }

    // 제거 : 모든 블럭 정보
    public void DestoryBlock()
    {
        foreach(KeyValuePair<S2BlockInfo, S2UIPuzzleBlock> kvp in m_pBlocks)
        {
            S2UIPuzzleBlock pBlock = kvp.Value;
            if (null == pBlock)
                return;

            pBlock.SetActive(false);
            m_pBlockPool.Enqueue(pBlock);
        }
        m_pBlocks = null;
    }

    // 제거 : 블럭 정보
    public void DestoryBlock(S2BlockInfo pInfo)
    {
        S2UIPuzzleBlock pBlock = GetBlock(pInfo);
        if (null == pBlock)
            return;

        pBlock.SetActive(false);
        m_pBlockPool.Enqueue(pBlock);
        m_pBlocks.Remove(pInfo);
    }

    // 블럭제어 : 블럭위치 변경(현 위치에서 TargetPos로)
    public void BlockToProperMove(S2BlockInfo pInfo, Vector3 vTargetPos)
    {
        S2UIPuzzleBlock pBlock = GetBlock(pInfo);
        if (null == pBlock)
            return;

        pBlock.SetMoveToProper(-1, -1, vTargetPos);
    }

    // 블럭제어 : 블럭위치 변경(StartPos에서 지정위치로)
    public void BlockToProperMove(S2BlockInfo pInfo, Vector3 vStartPos, int iTargetRow, int iTargetCol)
    {
        S2UIPuzzleBlock pBlock = GetBlock(pInfo);
        if (null == pBlock)
            return;

        pBlock.SetPosition(vStartPos);
        pBlock.SetMoveToProper(iTargetRow, iTargetCol, GetPositionToBlock(iTargetRow, iTargetCol));
    }

    // 블럭제어 : 블럭위치 변경(Swap)
    public void BlockToSwapMove(S2BlockInfo pInfo, int iTargetRow, int iTargetCol)
    {
        S2UIPuzzleBlock pBlock = GetBlock(pInfo);
        if (null == pBlock)
            return;

        pBlock.SetMoveToSwap(iTargetRow, iTargetCol, GetPositionToBlock(iTargetRow, iTargetCol));
    }

    // 블럭제어 : 블럭위치 변경(FallDown)
    public void BlockToFallDownMove(S2BlockInfo pInfo, int iTargetRow, int iTargetCol)
    {
        S2UIPuzzleBlock pBlock = GetBlock(pInfo);
        if (null == pBlock)
            return;

        pBlock.SetMoveToFallDown(iTargetRow, iTargetCol, GetPositionToBlock(iTargetRow, iTargetCol));
    }

    // 블럭제어 : 블럭 부수기
    public void BlockToCrush(S2BlockInfo pInfo)
    {
        S2UIPuzzleBlock pBlock = GetBlock(pInfo);
        if (null == pBlock)
            return;

        pBlock.SetCrush();
    }

    // 블럭제어 : 블럭 선택 On/Off
    public void BlockToSelect(S2BlockInfo pInfo, bool bIsSelect)
    {
        S2UIPuzzleBlock pBlock = GetBlock(pInfo);
        if (null == pBlock)
            return;

        pBlock.SetToggleSelect(bIsSelect);
    }

    // 블럭제어 : 버튼 On/Off
    public void BlockToButton(S2BlockInfo pInfo, bool bIsOn)
    {
        S2UIPuzzleBlock pBlock = GetBlock(pInfo);
        if (null == pBlock)
            return;

        pBlock.SetToggleButton(bIsOn);
    }

    // @=> 개발용 : 블럭 사이즈 변경
    public int iReSizeRow = 0;
    public int iReSizeCol = 0;
    [S2AttributeToShowFunc]
    public void ReSizing()
    {
        Single.Puzzle.Initialize(iReSizeRow, iReSizeCol);
    }
}