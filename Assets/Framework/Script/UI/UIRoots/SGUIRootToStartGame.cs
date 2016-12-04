/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 UI Root(게임시작) 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SGUIRootToStartGame : S2Singleton<SGUIRootToStartGame>
{
    public List<S2UIBasePanel> m_pPanels = new List<S2UIBasePanel>();
    private Dictionary<Type, S2UIBasePanel> m_dicPanels = new Dictionary<Type, S2UIBasePanel>();

    public override void OnInitialize()
    {
        m_dicPanels.Clear();
        S2Util.ForeachToList<S2UIBasePanel>(m_pPanels, (pPanel) =>
        {
            m_dicPanels.Add(pPanel.GetPanelType(), pPanel);
        });
    }
    public override void OnFinalize() { }

    public T GetPanel<T>() where T : S2UIBasePanel
    {
        if (false == IsPanel<T>())
        {
            S2Util.LogError("없는 패널을 Get하려고 합니다!!!(Type:{0})", typeof(T));
            return null;
        }

        return m_dicPanels[typeof(T)] as T;
    }

    public bool IsPanel<T>() where T : S2UIBasePanel
    {
        return m_dicPanels.ContainsKey(typeof(T));
    }
}