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

public partial class S2BGLayer : MonoBehaviour
{
    public int GetLeftIndex(int iIndex)
    {
        return (0 < iIndex) ? (iIndex - 1) : GetMaxIndex();
    }

    public int GetRightIndex(int iIndex)
    {
        return (GetMaxIndex() > iIndex) ? (iIndex + 1) : 0;
    }

    public int GetMaxIndex()
    {
        return (m_pTextures.Count - 1);
    }

    S2BGTexture GetTexture(int iOrder)
    {
        if (0 > iOrder || iOrder >= m_pTextures.Count)
            return null;

        return m_pTextures[iOrder];
    }

    // <-
    void RotationToLeft(int iIndex)
    {
        S2BGTexture pTexture = GetTexture(iIndex);
        if (null == pTexture)
            return;

        Vector3 vCameraSize = Single.Camera.GetWorldSize();
        float fTexRightPos = pTexture.GetRightPos();
        if (fTexRightPos > -vCameraSize.x)
            return;

        float fPosX = 0.0f;
        S2BGTexture pLeftTexture = GetTexture(GetLeftIndex(iIndex));
        if (null == pLeftTexture)
            fPosX = vCameraSize.x + pTexture.m_fHalfSize;
        else
            fPosX = (pLeftTexture.GetRightPos()) + pTexture.m_fHalfSize;

        pTexture.SetPositionX(fPosX);

        // <- 카메라 왼쪽편에 벗어난 모든 텍스쳐들 오른쪽으로 정렬시키기
        for(int iLoop = iIndex - 1; iLoop >= 0; --iLoop)
        {
            RotationToLeft(iLoop);
        }
    }

    // ->
    void RotationToRight(int iIndex)
    {
        S2BGTexture pTexture = GetTexture(iIndex);
        if (null == pTexture)
            return;

        Vector3 vCameraSize = Single.Camera.GetWorldSize();
        float fTexLeftPos = pTexture.GetLeftPos();
        if (fTexLeftPos < vCameraSize.x)
            return;
        
        float fPosX = 0.0f;
        S2BGTexture pRightTexture = GetTexture(GetRightIndex(iIndex));
        if (null == pRightTexture)
            fPosX = -vCameraSize.x - pTexture.m_fHalfSize;
        else
            fPosX = (pRightTexture.GetLeftPos()) - pTexture.m_fHalfSize;
        
        pTexture.SetPositionX(fPosX);

        // -> 카메라 오른쪽편에 벗어난 모든 텍스쳐들 왼쪽으로 정렬시키기
        for (int iLoop = iIndex - 1; iLoop >= 0; --iLoop)
        {
            RotationToRight(iLoop);
        }
    }
}