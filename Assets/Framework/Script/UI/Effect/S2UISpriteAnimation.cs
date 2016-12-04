/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 17일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 UITexture를 이용한 스프라이트 애니메이션 기능을 하는 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;

public class S2UISpriteAnimation : MonoBehaviour 
{
    public enum eWrapMode
    {
        Once,
        ClampToLast,
        ClampToStart,
        Loop,
    };
    public eWrapMode m_eWrapMode = eWrapMode.ClampToStart;

    public float        m_fTimeGap = 1.0f;
    public int          m_iTileCntX = 1;
    public int          m_iTileCntY = 1;
    public UITexture    m_pUITexture = null;

    private bool        m_bIsStop;
    private float       m_fAccTime;
    private int         m_iMaxIndex;
    private int         m_iCurIndex;
    private Vector2     m_vTileSize;

    void Initialize()
    {
        m_iMaxIndex = m_iTileCntX * m_iTileCntY;
        
        m_bIsStop   = false;
        m_fAccTime  = 0.0f;
        m_iCurIndex = 0;
        
        m_vTileSize = new Vector2(S2Math.Divide(1.0f, (float)m_iTileCntX),
                                  S2Math.Divide(1.0f, (float)m_iTileCntY));
    }

    void Start()
    {
        if (null == m_pUITexture)
        {
            S2Util.LogError("UISpriteAnimation은 UITexture 컴포넌트로 동작합니다. UITexture를 컴포넌트로 추가하세요!!");
            return;
        }

        Initialize();
    }

    void Update()
    {
        if (true == CheckStop())
            return;

        UpdateToAnimation();
        UpdateToTime();
    }

    void UpdateToAnimation()
    {
        if (true == CheckStop())
            return;

        Rect pRect   = m_pUITexture.uvRect;
        pRect.x      = (m_iCurIndex % m_iTileCntX) * m_vTileSize.x;
        pRect.y      = ((m_iTileCntY - 1) - (m_iCurIndex / m_iTileCntX)) * m_vTileSize.y;
        pRect.width  = m_vTileSize.x;
        pRect.height = m_vTileSize.y;
        m_pUITexture.uvRect = pRect;
    }

    void UpdateToTime()
    {
        m_fAccTime += Time.deltaTime;
        if (m_fTimeGap > m_fAccTime)
            return;

        m_fAccTime = 0.0f;
        ++m_iCurIndex;

        switch (m_eWrapMode)
        {
            case eWrapMode.Once:
                Single.Coroutine.NextUpdate(() => S2GameEngineSGT.DestroyObject(gameObject));
                break;
            case eWrapMode.ClampToLast:
                m_bIsStop = (m_iMaxIndex <= m_iCurIndex);
                break;
            case eWrapMode.ClampToStart:
                m_bIsStop = (m_iMaxIndex < m_iCurIndex);
                break;
            case eWrapMode.Loop:
                m_iCurIndex = S2Math.LoopNum(m_iCurIndex, 0, m_iMaxIndex);
                break;
        }

        m_iCurIndex = Mathf.Clamp(m_iCurIndex, 0, m_iMaxIndex);
    }

    bool CheckStop()
    {
        return (true == m_bIsStop) || (null == m_pUITexture);
    }

    public void Play()
    {
        Initialize();
    }

    public void Pause()
    {
        m_bIsStop = true;
    }

    public void Resum()
    {
        m_bIsStop = false;
    }

    public bool IsPlay()
    {
        return (false == m_bIsStop);
    }

    public void SetTexture(Texture pTexture, int iTileX, int iTileY)
    {
        m_iTileCntX              = iTileX;
        m_iTileCntY              = iTileY;
        Initialize();

        m_pUITexture.mainTexture = pTexture;
    }

    public void SetSpeed(float fSpeed)
    {
        m_fTimeGap = fSpeed;
    }

    [S2AttributeToShowFunc]
    void Reset()
    {
        Initialize();
    }
}