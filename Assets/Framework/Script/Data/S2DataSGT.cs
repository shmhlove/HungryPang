/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 15일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 게임내 모든 데이터를 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2DataSGT : S2Singleton<S2DataSGT>
{
    void FixedUpdate()
    {
        TableData.FrameMove();
        ResourcesData.FrameMove();
        SceneData.FrameMove();
        Loader.FrameMove();
    }

    public void Load(eSceneType eType, EventHandler pComplate = null, EventHandler pProgress = null)
    {
        OnEventToLoadStart();
        Loader.LoadStart(GetLoadList(eType), pComplate + OnEventToLoadComplate, pProgress);
    }

    public List<Dictionary<string, S2LoadData>> GetLoadList(eSceneType eType)
    {
        List<Dictionary<string, S2LoadData>> pLoadList = new List<Dictionary<string, S2LoadData>>();
        pLoadList.Add(SceneData.GetLoadList(eType));
        pLoadList.Add(TableData.GetLoadList(eType));
        pLoadList.Add(ResourcesData.GetLoadList(eType));
        return pLoadList;
    }

    void OnEventToLoadStart()
    {
        if (eResourcesLoadMode.AllLoad == Single.Global.m_eResourcesLoadMode)
            return;
        
        ResourcesData.OnFinalize();

        Single.Bundle.UnLoadAll(true);
        Resources.UnloadUnusedAssets();
        for (int iLoop = 0; iLoop < System.GC.MaxGeneration; ++iLoop)
        {
            System.GC.Collect(iLoop, GCCollectionMode.Forced);
        }
    }

    public void OnEventToLoadComplate(object pSender, EventArgs vArgs)
    {
        Single.Bundle.UnLoadAll(false);
        Resources.UnloadUnusedAssets();
        for (int iLoop = 0; iLoop < System.GC.MaxGeneration; ++iLoop)
        {
            System.GC.Collect(iLoop, GCCollectionMode.Forced);
        }
    }
}