/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐판 데이터를 모아둔 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// 데이터 : 블럭상태
public enum ePuzzleState
{
    None,
    MoveProper,         // 블럭들 각자 자리로 퍼트리기
    CheckFillBlock,     // 블럭 채우기 체크
    FallDown,           // 블럭 채워지는 중
    CheckMatch,         // 매칭 체크
    MatchCrush,         // 매칭된 블럭 깨기
    CheckNoMatch,       // 절대 매칭되지 않는지 체크
    MixingBlock,        // 절대 매칭되지 않아 블럭 섞는 중
    ControlIdle,        // 조작 대기
    Swaping,            // 스왑 중
    RevertSwaping,      // 스왑 취소 중
}

// 데이터 : 블럭정보
[Serializable]
public class S2BlockInfo
{
    public ePuzzleBlockType m_eBlockType;
    
    public int m_iRow;
    public int m_iCol;
    public int m_iTargetRow;
    public int m_iTargetCol;

    public S2BlockInfo(ePuzzleBlockType eType)
    {
        m_eBlockType    = eType;
        m_iRow          = -1;
        m_iCol          = -1;
        m_iTargetRow    = -1;
        m_iTargetCol    = -1;
    }
}

// 데이터 : UI이벤트
public class S2PuzzleUIEvent
{
    public EventHandler m_pArrive;
    public EventHandler m_pSelect;
    public EventHandler m_pDrag;
    public EventHandler m_pCrush;
}

// 데이터 : Match 이벤트
public class S2PuzzleMatchEvent
{
    public int              m_iComboCount;
    public ePuzzleBlockType m_eComboType;
    public Dictionary<ePuzzleBlockType, List<S2BlockInfo>> m_dicMatchBlocks;
}

// 데이터 : 퍼즐판
public partial class S2Puzzle
{
    // 퍼즐판 크기
    public int m_iMaxRow;
    public int m_iMaxCol;

    // 퍼즐판 상태
    public ePuzzleState m_eState;
    public ePuzzleState m_eBeforeState;
    public bool         m_bIsStopState;

    // 퍼즐판(블럭들) 정보
    private S2BlockInfo[][] m_pSpace;

    // 떨어지고 있는 블럭 리스트
    private Dictionary<S2BlockInfo, S2Int2> m_pFallDownBlocks = new Dictionary<S2BlockInfo, S2Int2>();

    // 매칭 블럭 리스트
    private Dictionary<ePuzzleBlockType, List<S2BlockInfo>> m_pMatchBlocks = new Dictionary<ePuzzleBlockType, List<S2BlockInfo>>();
    private List<S2BlockInfo> m_pCrushBlocks = new List<S2BlockInfo>();

    // 스왑 블럭 정보
    [SerializeField]
    private S2BlockInfo m_pSelBlock1;
    private S2BlockInfo m_pSelBlock2;

    // 콤보정보
    private int m_iComboCount = 0;
    private ePuzzleBlockType m_eComboType = ePuzzleBlockType.None;
    
    // UI이벤트 콜백
    private S2PuzzleUIEvent m_pCallbackToUI = new S2PuzzleUIEvent();

    // 매칭 이벤트(캐릭터용)
    public S2Event EventToMatch = new S2Event();
    
    // 기타
    private bool m_bIsPause = false;

    void InitiToData()
    {
        // 퍼즐판 크기
        m_iMaxRow       = 0;
        m_iMaxCol       = 0;

        // 퍼즐판 상태
        m_eState        = ePuzzleState.None;
        m_eBeforeState  = ePuzzleState.None;

        // 퍼즐판(블럭들) 정보
        m_pSpace        = null;

        // 이동 블럭 리스트
        m_pFallDownBlocks   = new Dictionary<S2BlockInfo, S2Int2>();

        // 매칭 블럭 리스트
        m_pMatchBlocks  = new Dictionary<ePuzzleBlockType, List<S2BlockInfo>>();
        m_pCrushBlocks  = new List<S2BlockInfo>();

        // 스왑 블럭 정보
        m_pSelBlock1    = null;
        m_pSelBlock2    = null;

        // 콤보정보( 판 섞일때 초기화 안되게 )
        //m_iComboCount   = 0;
        //m_eComboType    = ePuzzleBlockType.None;

        // UI이벤트 콜백
        m_pCallbackToUI = new S2PuzzleUIEvent();

        // 기타
        m_bIsPause = false;
    }
}
