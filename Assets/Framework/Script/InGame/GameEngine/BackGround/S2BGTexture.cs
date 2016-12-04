/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 21일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 인게임 배경 텍스쳐 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public partial class S2BGTexture : MonoBehaviour
{
    [NonSerialized]
    public float m_fHalfSize     = 0.0f;
    [NonSerialized]
    public float m_fStartPosX    = 0.0f;
    [NonSerialized]
    public float m_fStartPosY    = 0.0f;

    public int m_iTexSizeX       = 0;
    public int m_iTexSizeY       = 0;

    private Vector3 m_vPosition  = Vector3.zero;

    public void Initialize()
    {
        SetSizing(Single.Camera.GetViewportRect(), Single.Global.GetResolution());
        SetPosition(transform.localPosition);

        m_fStartPosX        = GetPosition().x;
        m_fStartPosY        = GetPosition().y;

        Vector3 vSize       = transform.localScale * 0.5f;
        m_fHalfSize         = vSize.x;
    }

    [S2AttributeToShowFunc]
    public void SetSizing()
    {
        SetSizing(new Rect(0.0f, 0.653f, 1.0f, 0.33f), new Vector2(720, 1280));
    }
    public void SetSizing(Rect pViewport, Vector3 vResolution)
    {
        float fBaseWidth  = (vResolution.x * pViewport.width);
        float fBaseHeight = (vResolution.y * pViewport.height);
        float fQuadRatioWidth   = 3.43f;    // 카메라 사이즈에 맞춘 쿼드 사이즈(체험수치)
        float fQuadRatioHeight  = 2.0f;     // 카메라 사이즈에 맞춘 쿼드 사이즈(체험수치)

        Vector3 vQuadSize = Vector3.zero;

        vQuadSize.x = ((m_iTexSizeX / fBaseWidth) * fQuadRatioWidth);
        vQuadSize.y = ((m_iTexSizeY / fBaseHeight) * fQuadRatioHeight);

        transform.localScale = vQuadSize;
    }
    
    public void SetPosition(Vector3 vPos)
    {
        m_vPosition             = vPos;
        transform.localPosition = m_vPosition;
    }
    public void SetPositionX(float fPosX)
    {
        m_vPosition.x = fPosX;
        SetPosition(m_vPosition);
    }
    public void SetPositionY(float fPosY)
    {
        m_vPosition.y = fPosY;
        SetPosition(m_vPosition);
    }
    public void AddPositionX(float fPosX)
    {
        m_vPosition.x += fPosX;
        SetPosition(m_vPosition);
    }
    public void AddPositionY(float fPosY)
    {
        m_vPosition.y += fPosY;
        SetPosition(m_vPosition);
    }
    public Vector3 GetPosition()
    {
        return m_vPosition;
    }
    public float GetLeftPos()
    {
        return GetPosition().x - m_fHalfSize;
    }
    public float GetRightPos()
    {
        return GetPosition().x + m_fHalfSize;
    }
}