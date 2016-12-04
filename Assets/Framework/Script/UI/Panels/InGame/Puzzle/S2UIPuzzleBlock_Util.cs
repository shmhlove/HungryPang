/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 01일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐블럭 클래스(유틸)입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2UIPuzzleBlock : MonoBehaviour
{
    // 유틸 : 상태얻기
    S2UIPuzzleBlockState GetState(eBlockState eState)
    {
        if (false == m_dicState.ContainsKey(eState))
            return null;

        return m_dicState[eState];
    }

    // 유틸 : 상태생성
    S2UIPuzzleBlockState CreateState(eBlockState eState)
    {
        S2UIPuzzleBlockState pState = new S2UIPuzzleBlockState();
        pState.m_eState    = eState;
        m_dicState[eState] = pState;
        return pState;
    }

    // 유틸 : 상태변경
    void ChangeState(eBlockState eChangeState)
    {
        S2UIPuzzleBlockState pCurState = GetState(m_eCurrentState);
        if (null != pCurState)
            pCurState.m_pExit();

        m_eCurrentState = eChangeState;

        S2UIPuzzleBlockState pChangeState = GetState(m_eCurrentState);
        if (null != pChangeState)
            pChangeState.m_pEnter();
    }

    // 유틸 : 기본텍스쳐로 설정
    void SetDefualtTexture()
    {
        if (null == m_pInfo)
            return;

        SetTexture(GetNameToBlock(m_pInfo.m_eBlockType), 1, 1);
    }

    // 유틸 : 블럭이동명령
    void SetMove(eBlockState eState, int iTargetRow, int iTargetCol, Vector3 vTarget, float fSpeed, float fWeightSpeed)
    {
        m_pInfo.m_iTargetRow    = iTargetRow;
        m_pInfo.m_iTargetCol    = iTargetCol;
        m_vTargetPos            = vTarget;
        m_vSpeed                = (vTarget - m_vPos).normalized * fSpeed;
        m_fWeightSpeed          = fWeightSpeed;
        
        ChangeState(eState);
    }

    // 유틸 : 블럭이동
    bool MoveBlock(Vector3 vForce)
    {
        m_vSpeed *= m_fWeightSpeed;
        SetPosition(S2Physics.CalculationEuler(vForce, GetPosition(), ref m_vSpeed));
        return ((m_vSpeed.magnitude * Single.Timer.fixedDeltaTime) < (m_vTargetPos - GetPosition()).magnitude);
    }

    // 유틸 : 블럭도착처리
    void SetArrive()
    {
        if (-1 != m_pInfo.m_iTargetRow)     m_pInfo.m_iRow = m_pInfo.m_iTargetRow;
        if (-1 != m_pInfo.m_iTargetCol)     m_pInfo.m_iCol = m_pInfo.m_iTargetCol;
        SetPosition(m_vTargetPos);
        
        m_vTargetPos    = Vector2.zero;
        m_vSpeed        = Vector2.zero;

        m_pInfo.m_iTargetRow = -1;
        m_pInfo.m_iTargetCol = -1;
        
        gameObject.name = S2Util.Format("Block : {0}, {1}", m_pInfo.m_iRow, m_pInfo.m_iCol);
        OnTriggerArrive();
    }

    // 유틸 : 텍스쳐 설정
    void SetTexture(string strTexName, int iRow, int iCol)
    {
        Texture pTex = Single.ResourceData.GetTexture(strTexName);
        if (null == pTex)
            return;

        m_pAnimation.SetTexture(pTex, iRow, iCol);
        m_pAnimation.Play();
    }

    // 유틸 : 블럭 리소스 이름
    string GetNameToBlock(ePuzzleBlockType eType)
    {
        switch(eType)
        {
            case ePuzzleBlockType.Blue:     return Single.Hard.m_strBlockBlue;
            case ePuzzleBlockType.Green:    return Single.Hard.m_strBlockGreen;
            case ePuzzleBlockType.Orange:   return Single.Hard.m_strBlockOrange;
            case ePuzzleBlockType.Red:      return Single.Hard.m_strBlockRed;
            case ePuzzleBlockType.Violet:   return Single.Hard.m_strBlockViolet;
        }
        return string.Empty;
    }

    // 유틸 : 데미지 리소스 이름
    string GetNameToDamage(ePuzzleBlockType eType)
    {
        switch(eType)
        {
            case ePuzzleBlockType.Blue:     return Single.Hard.m_strDamageBlue;
            case ePuzzleBlockType.Green:    return Single.Hard.m_strDamageGreen;
            case ePuzzleBlockType.Orange:   return Single.Hard.m_strDamageOrange;
            case ePuzzleBlockType.Red:      return Single.Hard.m_strDamageRed;
            case ePuzzleBlockType.Violet:   return Single.Hard.m_strDamageViolet;
        }
        return string.Empty;
    }
}