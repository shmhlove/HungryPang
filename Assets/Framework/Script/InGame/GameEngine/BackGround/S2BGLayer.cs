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

public enum eMoveType
{
    None,
    UV,
    PosRotation,
}

[Serializable]
public partial class S2BGLayer : MonoBehaviour
{
    public eMoveType m_eMoveType         = eMoveType.None;
    public float     m_fRelativeSpeed    = 0.0f;

    public List<S2BGTexture> m_pTextures = new List<S2BGTexture>();

    void Start()
    {
        foreach(S2BGTexture pTexture in m_pTextures)
        {
            pTexture.Initialize();
        }
    }

    public void FrameMove()
    {
        
    }

    public void Move(float fSpeed)
    {
        switch(m_eMoveType)
        {
            case eMoveType.UV:          MoveToUV(fSpeed);           break;
            case eMoveType.PosRotation: MoveToPosRotation(fSpeed);  break;
        }
    }

    void MoveToUV(float fSpeed)
    {
        foreach(S2BGTexture pTexture in m_pTextures)
        {
            Vector2 vOffset = pTexture.renderer.material.mainTextureOffset;
            vOffset.x += (fSpeed * m_fRelativeSpeed * 0.05f);
            pTexture.renderer.material.mainTextureOffset = vOffset;
        }
    }

    void MoveToPosRotation(float fSpeed)
    {
        fSpeed *= -m_fRelativeSpeed;

        // 이동
        foreach (S2BGTexture pTexture in m_pTextures)
        {
            pTexture.AddPositionX(fSpeed);
        }

        // 로테이션
        int iMaxTexture = m_pTextures.Count;
        for (int iLoop = 0; iLoop < iMaxTexture; ++iLoop)
        {
            if (0.0f > fSpeed)
            {
                RotationToLeft(iLoop);
                continue;
            }
        
            if (0.0f < fSpeed)
            {
                RotationToRight(iLoop);
                continue;
            }
        }
    }
}