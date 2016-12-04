using UnityEngine;
using System.Collections;

public class SGUITweenSpriteSize : MonoBehaviour
{
    public UISprite m_spriteTarget = null;
    public enum eSizeDirection
    {
        Width,
        Height,
    }
    public eSizeDirection m_eSizeDirection = eSizeDirection.Width;
    public float m_fSizeFrom = 0f;
    public float m_fSizeTo = 0f;
    public float m_fTime = 0f;
    private float m_fDamping = 1f;

    private bool m_bStart = false;
    public enum eDirection
    {
        Direct,
        Inverse,
    }
    private eDirection m_eDirection = eDirection.Direct;
    private float m_fStartTime = 0f;
    private float m_fCurrentValue = 0f;


    void Start()
    {
        m_fDamping = 1f / m_fTime;
    }

    // for Test
//     void OnEnable()
//     {
//         StartTweenSize();
// 
//         Invoke("StartInverseTweenSize", 2f);
//     }

    public void StartTweenSize()
    {
        m_eDirection = eDirection.Direct;
        m_bStart = true;
        m_fStartTime = Time.time;
        m_fCurrentValue = 0f;
    }

    public void StartInverseTweenSize()
    {
        m_eDirection = eDirection.Inverse;
        m_bStart = true;
        m_fStartTime = Time.time;
        m_fCurrentValue = 0f;
    }

    public void StartDelayedInverseTweenSize(float _fDelay)
    {
        Invoke("StartInverseTweenSize", _fDelay);
    }

    void Update()
    {
        if (false == m_bStart)
            return;

        if (null == m_spriteTarget)
            return;

        m_fCurrentValue = (Time.time - m_fStartTime) * m_fDamping;
        if (1f <= m_fCurrentValue)
        {
            m_bStart = false;
        }

        switch (m_eDirection)
        {
            case eDirection.Direct:
                UpdateTween_Direct();
                break;

            case eDirection.Inverse:
                UpdateTween_Inverse();
                break;
        }
    }

    void UpdateTween_Direct()
    {
        switch(m_eSizeDirection)
        {
            case eSizeDirection.Width:
                {
                    if (m_spriteTarget.width == m_fSizeTo)
                    {
                        m_bStart = false;
                        return;
                    }

                    m_spriteTarget.width = (int)Mathf.Lerp(m_fSizeFrom, m_fSizeTo, m_fCurrentValue);
                }
                break;

            case eSizeDirection.Height:
                {
                    if (m_spriteTarget.height == m_fSizeTo)
                    {
                        m_bStart = false;
                        return;
                    }

                    m_spriteTarget.height = (int)Mathf.Lerp(m_fSizeFrom, m_fSizeTo, m_fCurrentValue); 
                }
                break;
        }
    }

    void UpdateTween_Inverse()
    {
        switch (m_eSizeDirection)
        {
            case eSizeDirection.Width:
                {
                    if (m_spriteTarget.width == m_fSizeFrom)
                    {
                        m_bStart = false;
                        return;
                    }

                    m_spriteTarget.width = (int)Mathf.Lerp(m_fSizeTo, m_fSizeFrom, m_fCurrentValue);
                }
                break;

            case eSizeDirection.Height:
                {
                    if (m_spriteTarget.height == m_fSizeFrom)
                    {
                        m_bStart = false;
                        return;
                    }

                    m_spriteTarget.height = (int)Mathf.Lerp(m_fSizeTo, m_fSizeFrom, m_fCurrentValue);
                }
                break;
        }
    }
}
