using UnityEngine;
using System.Collections;


[AddComponentMenu("S2Technical/Effect/Sprite Animation")]
public class S2SpriteAnimation : MonoBehaviour
{
    public enum eAnimationType 
    { 
        Once,
        ClampForaver, 
        Loop 
    };

    public eAnimationType m_AniType = eAnimationType.Loop;

    public float m_FirstStartFrame = 0f;

    public float m_TimeGap = 1f;
    public int m_LoopAllFrame = 0;
    public int m_LoopStartFrame = 0;

    public int m_JumpStartFrame = 0;
    //public int m_JumpEndFrame = 0;

    public int m_uvTileX = 1;
    public int m_uvTileY = 1;

    private Vector2 m_v2TileSize;
    private int m_iTileIndex = 0;
    private int m_iTileIndex_old = 0;


    private bool m_bStartLoop = true;

    private bool m_bLoop = true;
    private float m_fFirstStartTime = 0f;
    private float m_fLoopAllTime = 0f;

    //private float m_deltaTime = 0f;
    private float tmp_deltaTime = 0f;

    private int m_iTotalIndex = 0;
    private float m_fAccumTime = 0f;

    public bool m_Reverse = false;

    void Start()
    {
        // ±âº»°ª(0) ÀÌ¸é Tile °¹¼ö·Î »Ì¾Æ³¿.
        if( 0 == m_LoopAllFrame)
        {
            m_LoopAllFrame = m_uvTileX * m_uvTileY;
        }

        m_v2TileSize = new Vector2(1.0f / m_uvTileX, 1.0f / m_uvTileY);

        m_iTotalIndex = m_uvTileX * m_uvTileY;
        m_fLoopAllTime = (float)(m_LoopAllFrame) / 30.0f;
        m_iTileIndex = 0;

        m_fFirstStartTime = (m_FirstStartFrame / 30.0f) * m_TimeGap;
        transform.renderer.enabled = false;

        if (renderer == null)
        {
            enabled = false;
        }

        Reset();
    }

    void Reset()
    {
        m_fAccumTime = 0f;
    }

    void SetSpriteSheet(int _iIndex)
    {
        int uIndex = _iIndex % m_uvTileX;
        int vIndex = _iIndex / m_uvTileX;
        Vector2 offset = new Vector2(uIndex * m_v2TileSize.x, 1.0f - m_v2TileSize.y - vIndex * m_v2TileSize.y);
        renderer.material.SetTextureOffset("_MainTex", offset);
        renderer.material.SetTextureScale("_MainTex", m_v2TileSize);
    }

    void SetSprite(bool bClampForaver)
    {
        m_iTileIndex = (int)(((((m_fAccumTime - tmp_deltaTime) % m_fLoopAllTime) / m_fLoopAllTime)) * m_LoopAllFrame);

        //Debug.Log("[m_iIndex] " + m_iTileIndex );


        if (m_iTileIndex < m_LoopStartFrame )//|| m_iTileIndex > m_LoopAllFrame - m_LoopLastFrame)
        {
            transform.renderer.enabled = false;
            return;
        }
        else
        {
            transform.renderer.enabled = true;
            m_iTileIndex -= m_LoopStartFrame;
        }


        if (m_iTileIndex_old > m_iTileIndex)
        {
            transform.renderer.enabled = false;
            if (bClampForaver)
                transform.renderer.enabled = true;
            m_bLoop = false;
            return;
        }

        if (m_iTileIndex + m_JumpStartFrame < m_iTotalIndex)
        {
            SetSpriteSheet(m_iTileIndex + m_JumpStartFrame);

        }
        else if (m_iTileIndex + m_JumpStartFrame >= m_iTotalIndex)
        {
            transform.renderer.enabled = false;
            
        }
        
    }

    void Update()
    {
        m_fAccumTime += (Time.deltaTime * m_TimeGap);
        //Debug.Log(Time.deltaTime);
        if (true == m_bStartLoop)
        {

            if (m_fAccumTime < m_fFirstStartTime)
            {
                tmp_deltaTime = m_fAccumTime;

                return;
            }
            else
            {
                m_bStartLoop = false;
            }

        }

        switch (m_AniType)
        {
            case eAnimationType.Once:
                {
                    if (true == m_bLoop)
                    {
                        SetSprite(false);
                        m_iTileIndex_old = m_iTileIndex;
                    }
                }
                break;
            case eAnimationType.ClampForaver:
                {
                    if (true == m_bLoop)
                    {
                        SetSprite(true);
                        m_iTileIndex_old = m_iTileIndex;
                    }
                }
                break;
            case eAnimationType.Loop:
                SetSprite(false);
                break;
        }
    }

}