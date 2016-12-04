/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 퍼즐UI 패널클래스에 필요한 유틸함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2UIPuzzle : S2UIBasePanel 
{
    // 생성 : 블럭 오브젝트
    S2UIPuzzleBlock CreateBlockObject()
    {
        if (true == CheckExceptionToBlock())
            return null;

        if (0 != m_pBlockPool.Count)
            return m_pBlockPool.Dequeue();

        GameObject pBlockObj = Single.ResourceData.GetGameObject("UI Puzzle - Block");
        if (null == pBlockObj)
        {
            S2Util.Log("퍼즐블럭 프리팹이 없습니다!!!");
            return null;
        }

        if (null == m_pBlockRoot)
        {
            m_pBlockRoot = new GameObject("BlockRoot");
            m_pBlockRoot.transform.SetParent(m_pBlockAnchor);
            m_pBlockRoot.transform.localPosition = new Vector3(m_vOffset.x, m_vOffset.y, 0.0f);
            m_pBlockRoot.transform.localScale    = new Vector3(1.0f, 1.0f, 1.0f);
        }

        pBlockObj.transform.SetParent(m_pBlockRoot.transform);
        return pBlockObj.GetComponent<S2UIPuzzleBlock>();
    }

    // 정보 : 블럭UI
    S2UIPuzzleBlock GetBlock(S2BlockInfo pInfo)
    {
        if (true == CheckExceptionToBlock())
            return null;

        if (null == pInfo)
            return null;

        if (false == m_pBlocks.ContainsKey(pInfo))
            return null;

        return m_pBlocks[pInfo];
    }

    // 정보 : 퍼즐판 반크기
    Vector2 GetHalfSizeToSpace()
    {
        return new Vector2((m_iWidth * 0.5f), (m_iHeight * 0.5f));
    }

    // 정보 : 블럭 크기
    Vector2 GetSizeToBlock()
    {
        return new Vector2( S2Math.Divide(m_iWidth, m_iMaxCol),
                            S2Math.Divide(m_iHeight, m_iMaxRow));
    }

    // 정보 : 블럭 반크기
    Vector2 GetHalfSizeToBlock()
    {
        return (GetSizeToBlock() * 0.5f);
    }

    // 정보 : 블럭위치
    Vector2 GetPositionToBlock(int iRow, int iCol)
    {
        Vector2 vBlockSize          = GetSizeToBlock();
        Vector2 vHalfSizeToBlock    = GetHalfSizeToBlock();
        Vector2 vHalfSizeToSpace    = GetHalfSizeToSpace();

        return new Vector2((vHalfSizeToBlock.x - vHalfSizeToSpace.x) + (vBlockSize.x * iCol),
                          ((vHalfSizeToSpace.y - vHalfSizeToBlock.y) - (vBlockSize.y * iRow)));
    }

    // 예외체크 : 퍼즐판이 동작할 수 있는 상태인가?
    bool CheckExceptionToBlock()
    {
        return ((null == m_pBlockPool) || (null == m_pBlocks));
    }
}