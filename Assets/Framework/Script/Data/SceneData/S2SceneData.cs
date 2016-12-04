/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 07월 26일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 씬 데이터를 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2SceneData : S2BaseData
{
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void FrameMove() { }

    public override Dictionary<string, S2LoadData> GetLoadList(eSceneType eType)
    {
        Dictionary<string, S2LoadData> dicLoadList = new Dictionary<string, S2LoadData>();
        if (true == Single.Scene.IsHistory())
        {
            string strSceneName = Single.Hard.GetStrSceneToEnum(eType);
            dicLoadList.Add(strSceneName, GetLoadData(strSceneName));
        }
        return dicLoadList;
    }

    S2LoadData GetLoadData(string strSceneName)
    {
        S2LoadData pLoadInfo = new S2LoadData();
        pLoadInfo.m_eDataType           = eDataType.Scene;
        pLoadInfo.m_strName             = strSceneName;
        pLoadInfo.m_pLoadFunc           = Load;
        pLoadInfo.m_pTriggerLoadCall    = () => 
        {
            return Single.Data.IsLoadDone(eDataType.Table);
        };
        return pLoadInfo;
    }

    public override void Load(S2LoadData pInfo,
        Action<string, S2LoadStartInfo> pStart, Action<string, S2LoadEndInfo> pDone)
    {
        AsyncOperation pAsyncInfo = Single.Scene.AddScene(pInfo.m_strName, (bIsSuccess) =>
        {
            pDone(pInfo.m_strName, new S2LoadEndInfo(bIsSuccess));
        });

        pStart(pInfo.m_strName, new S2LoadStartInfo(pAsyncInfo));
    }
}