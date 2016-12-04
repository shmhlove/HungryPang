/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 26일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 씬을 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2SceneHistory
{
    public eSceneType m_eTo;
    public eSceneType m_eForm;

    public S2SceneHistory(eSceneType eTo, eSceneType eForm)
    {
        m_eTo = eTo;
        m_eForm = eForm;
    }
}

public class S2SceneSGT : S2Singleton<S2SceneSGT>
{
    public eSceneType m_eCurrentScene   = eSceneType.None;
    public eSceneType m_eBeforeScene    = eSceneType.None;

    public List<S2SceneHistory> m_pHistory = new List<S2SceneHistory>();

    public override void OnInitialize()
    {
        SetDontDestroy();
    }
    public override void OnFinalize() { }

    public void GoTo(eSceneType eType)
    {
        if (eType == m_eCurrentScene)
            return;

        m_pHistory.Add(new S2SceneHistory(m_eCurrentScene, eType));
        m_eBeforeScene  = m_eCurrentScene;
        m_eCurrentScene = eType;
        
        Single.Coroutine.Async((bIsSuccess) =>
            S2Util.Assert(bIsSuccess, "씬 로드 실패!!(SceneType : {0})", eType),
            Application.LoadLevelAsync(Single.Hard.GetStrSceneToEnum(eSceneType.Loading)));
    }

    public AsyncOperation AddScene(string strSceneName, Action<bool> pDone)
    {
        AsyncOperation pAsyncInfo = Application.LoadLevelAdditiveAsync(strSceneName);
        Single.Coroutine.Async((bIsSuccess) => pDone(bIsSuccess), pAsyncInfo);
        return pAsyncInfo;
    }

    public bool IsHistory()
    {
        return (0 != m_pHistory.Count);
    }

    //[S2InspectorToShowFunc] void GoToPatchScene()       { GoTo(eSceneType.Patch); }
    //[S2InspectorToShowFunc] void GoToCreateTeamScene()  { GoTo(eSceneType.CreateTeam); }
    //[S2InspectorToShowFunc] void GoToOutGameScene()     { GoTo(eSceneType.OutGame); }
    //[S2InspectorToShowFunc] void GoToInGameScene()      { GoTo(eSceneType.InGame); }
}
