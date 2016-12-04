/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 20일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 UI Panel의 Base 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class S2UIBasePanel : MonoBehaviour
{
    public UIPanel  m_pPanel;
    public bool     m_bIsActive;

    public abstract Type GetPanelType();

    // 인터페이스 : 초기화
    public void Initialize()
    {
        m_bIsActive = gameObject.activeInHierarchy;
        m_pPanel    = GetComponent<UIPanel>();
    }

    // 인터페이스 : Active On/Off
    public void SetActive(bool bIsToggle)
    {
        if (m_bIsActive == bIsToggle)
            return;

        m_bIsActive = bIsToggle;
        gameObject.SetActive(bIsToggle);
    }

    // 인터페이스 : 자식 오브젝트 설정
    public void SetChild(Transform pChild)
    {
        if (null == pChild)
            return;

        pChild.SetParent(transform);
    }
}
