/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 12일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐블럭 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2UIPuzzleBlock : MonoBehaviour
{
    // 자식오브젝트
    [SerializeField]
    private UISprite    m_pSelect    = null;
    [SerializeField]
    private S2TouchEvent m_pButton   = null;
    [SerializeField]
    private S2UISpriteAnimation m_pCrush = null;

    // 컴포넌트
    [SerializeField]
    private UITexture m_pSprite      = null;
    [SerializeField]
    private S2UISpriteAnimation m_pAnimation = null;
    [SerializeField]
    private S2BlockInfo m_pInfo     = null;

    // 기타
    public float m_fDragSensitive   = 0.035f;

    // 위치 및 이동정보
    private Vector3 m_vPos          = Vector3.zero;
    private Vector3 m_vTargetPos    = Vector3.zero;
    private Vector3 m_vSpeed        = Vector3.zero;
    private float   m_fWeightSpeed  = 1.0f;

    // 이벤트
    private S2Event EventToArrive   = new S2Event();
    private S2Event EventToSelect   = new S2Event();
    private S2Event EventToDrag     = new S2Event();
    private S2Event EventToCrush    = new S2Event();

    // 블럭상태관리
    private eBlockState m_eCurrentState  = eBlockState.Idle;
    Dictionary<eBlockState, S2UIPuzzleBlockState> m_dicState = new Dictionary<eBlockState, S2UIPuzzleBlockState>();

    // 시스템 : 클래스 루프
    void FixedUpdate()
    {
        if (false == m_dicState.ContainsKey(m_eCurrentState))
            return;

        m_dicState[m_eCurrentState].m_pUpdate();
    }

    // 인터페이스 : 블럭생성
    public void OnCreate(S2BlockInfo pInfo, S2PuzzleUIEvent pEvent)
    {
        if (null == pInfo)
            return;

        if (null != pEvent)
        {
            EventToArrive.Clear();
            EventToArrive.Add(pEvent.m_pArrive);
            EventToSelect.Clear();
            EventToSelect.Add(pEvent.m_pSelect);
            EventToDrag.Clear();
            EventToDrag.Add(pEvent.m_pDrag);
            EventToCrush.Clear();
            EventToCrush.Add(pEvent.m_pCrush);
        }

        m_pButton.eventTouch.Add(OnTriggerSelect);
        m_pButton.eventStickPos.Add(OnTriggerDrag);
        
        m_pInfo = pInfo;
        InitToState();
        ChangeState(eBlockState.Init);
    }

    // 인터페이스 : 블럭 사이즈 변경
    public void SetSize(Vector2 vSize)
    {
        m_pSprite.width     = (int)vSize.x;
        m_pSprite.height    = (int)vSize.y;
        m_pSelect.width     = (int)vSize.x;
        m_pSelect.height    = (int)vSize.y;
        m_pButton.SetSize(vSize);

        m_pCrush.m_pUITexture.width  = (int)(vSize.x * 2.7f);
        m_pCrush.m_pUITexture.height = (int)(vSize.y * 2.7f);
        m_pCrush.gameObject.SetActive(false);
    }

    // 인터페이스 : 블럭위치변경
    public void SetPosition(Vector3 vPos)
    {
        transform.localPosition = m_vPos = vPos;
    }

    public Vector3 GetPosition()
    {
        return m_vPos;
    }

    // 인터페이스 : 블럭이동명령(특정위치)
    public void SetMoveToProper(int iRow, int iCol, Vector2 vTarget)
    {
        SetMove(eBlockState.Proper, iRow, iCol, vTarget, 650.0f, 1.03f);
    }

    // 인터페이스 : 블럭이동명령(스왑)
    public void SetMoveToSwap(int iRow, int iCol, Vector2 vTarget)
    {
        SetMove(eBlockState.Swap, iRow, iCol, vTarget, 550.0f, 1.1f);
    }

    // 인터페이스 : 블럭이동명령(떨어지기)
    public void SetMoveToFallDown(int iRow, int iCol, Vector2 vTarget)
    {
        SetMove(eBlockState.FallDown, iRow, iCol, vTarget, 800.0f, 1.0f);
    }

    // 인터페이스 : 부수기명령
    public void SetCrush()
    {
        ChangeState(eBlockState.Crush);
    }

    // 인터페이스 : 활성화
    public void SetActive(bool bIsActive)
    {
        gameObject.SetActive(bIsActive);
    }

    // 인터페이스 : 버튼 On/Off
    public void SetToggleButton(bool bIsToggle)
    {
        m_pButton.enabled = bIsToggle;
    }

    // 인터페이스 : 선택이미지 On/Off
    public void SetToggleSelect(bool bIsToggle)
    {
        if (false == bIsToggle)
            m_pSprite.depth = 1;
        else
            m_pSprite.depth = 2;

        m_pSelect.enabled = bIsToggle;
    }

    // 인터페이스 : 블럭 이미지 On/Off
    public void SetActiveBlock(bool bIsActive)
    {
        m_pSprite.enabled = bIsActive;
    }
    
    // 유틸 : 방향값으로 다른블럭 위치얻기
    S2Int2 GetBlockInfoToDirection(Vector2 vDir)
    {
        if (vDir.x == vDir.y)
            return null;

        if (Mathf.Abs(vDir.x) < Mathf.Abs(vDir.y))
        {
            if (0.0f < vDir.y) return new S2Int2(m_pInfo.m_iRow - 1, m_pInfo.m_iCol);
            else               return new S2Int2(m_pInfo.m_iRow + 1, m_pInfo.m_iCol);
        }
        else
        {
            if (0.0f < vDir.x) return new S2Int2(m_pInfo.m_iRow, m_pInfo.m_iCol + 1);
            else               return new S2Int2(m_pInfo.m_iRow, m_pInfo.m_iCol - 1);
        }
    }

    // 이벤트알림 : 도착
    void OnTriggerArrive()
    {
        EventToArrive.CallBack<S2BlockInfo>(this, m_pInfo);
    }

    // 이벤트알림 : 선택
    public void OnTriggerSelect(object pSender, EventArgs vArgs)
    {
        EventToSelect.CallBack<S2BlockInfo>(this, m_pInfo);
    }

    // 이벤트알림 : 드레그
    public void OnTriggerDrag(object pSender, EventArgs vArgs)
    {
        Vector2 vLocalPos = Single.Event.GetArgs<Vector2>(vArgs);
        if (m_fDragSensitive > vLocalPos.magnitude)
            return;

        S2Int2 pPosInfo = GetBlockInfoToDirection(vLocalPos.normalized);
        if (null == pPosInfo)
            return;

        EventToDrag.CallBack<S2Int2>(this, pPosInfo);

    }
    
    // 이벤트알림 : 부서짐
    void OnTriggerCrush()
    {
        EventToCrush.CallBack<S2BlockInfo>(this, m_pInfo);
    }
}