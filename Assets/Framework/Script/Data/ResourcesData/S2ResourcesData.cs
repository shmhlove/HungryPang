/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 27일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 원본 리소스 데이터를 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2ResourcesData : S2BaseData 
{
    private Dictionary<string, UnityEngine.Object> m_dicResources = new Dictionary<string, UnityEngine.Object>();

    public override void OnInitialize() { }
    public override void OnFinalize()
    {
        m_dicResources.Clear();
    }
    public override void FrameMove() { }

    public override Dictionary<string, S2LoadData> GetLoadList(eSceneType eType)
    {
        S2ResourcesTable pTable = Single.TableData.GetTable<S2ResourcesTable>();
        Dictionary<string, S2LoadData> dicLoadList = new Dictionary<string, S2LoadData>();

        // Common 리소스 로드 리스트
        GetLoadList(pTable.GetDataToList("Common"), ref dicLoadList);

        // Scene별 리소스 로드 리스트
        switch (Single.Global.m_eResourcesLoadMode)
        {
            case eResourcesLoadMode.AllLoad:
                S2Util.ForeachToEnum<eSceneType>((eEnum) =>
                GetLoadList(pTable.GetDataToList(eEnum), ref dicLoadList));
                break;
            case eResourcesLoadMode.SceneLoad:
                GetLoadList(pTable.GetDataToList(eType), ref dicLoadList);
                break;
        }

        return dicLoadList;
    }

    void GetLoadList(Dictionary<string, S2ResourcesTableData> dicTable, ref Dictionary<string, S2LoadData> dicList)
    {
        if (null == dicTable)
            return;

        foreach (KeyValuePair<string, S2ResourcesTableData> kvp in dicTable)
        {
            if (true == m_dicResources.ContainsKey(kvp.Key))
                continue;

            if (true == CheckToFiltering(kvp.Value))
                continue;

            S2LoadData pLoadInfo    = new S2LoadData();
            pLoadInfo.m_eDataType   = eDataType.Resources;
            pLoadInfo.m_strName     = kvp.Key;
            pLoadInfo.m_pLoadFunc   = Load;

            dicList.Add(kvp.Key, pLoadInfo);
        }
    }

    bool CheckToFiltering(S2ResourcesTableData pTableData)
    {
        return false;
    }

    public override void Load(S2LoadData pInfo,
        Action<string, S2LoadStartInfo> pStart, Action<string, S2LoadEndInfo> pDone)
    {
        if (true == m_dicResources.ContainsKey(pInfo.m_strName))
        {
            pDone(pInfo.m_strName, new S2LoadEndInfo(true));
            return;
        }

        S2ResourcesTable pTable = Single.TableData.GetTable<S2ResourcesTable>();
        LoadAsync(pTable.GetData(pInfo.m_strName), pStart, pDone);
    }

    void LoadAsync(S2ResourcesTableData pTableData,
        Action<string, S2LoadStartInfo> pStart, Action<string, S2LoadEndInfo> pDone)
    {
        if (null == pTableData)
        {
            pDone(string.Empty, new S2LoadEndInfo(false));
            return;
        }

        switch (Single.Global.ResourcesLoadType)
        {
            case eResourcesLoadType.Local_Resource:
                LoadAsyncToNative(pTableData, pStart, pDone);
                break;

            case eResourcesLoadType.Local_AssetBundle:
            case eResourcesLoadType.Server_AssetBundle:
                LoadAsyncToAssetBundle(pTableData, pStart, pDone);
                break;
        }
    }

    void LoadAsyncToNative(S2ResourcesTableData pTableData,
        Action<string, S2LoadStartInfo> pStart, Action<string, S2LoadEndInfo> pDone)
    {
        if (true == m_dicResources.ContainsKey(pTableData.m_strName))
        {
            pDone(pTableData.m_strName, new S2LoadEndInfo(true));
            return;
        }

        ResourceRequest pRequest = Resources.LoadAsync(pTableData.m_strPath);
        pStart(pTableData.m_strName, new S2LoadStartInfo(pRequest));
        Single.Coroutine.Async((pIsDone) =>
        {
            //S2Util.Assert((null != pRequest.asset), "{0} 파일이 없습니다!!!", pTableData.m_strPath);

            m_dicResources.Add(pTableData.m_strName, pRequest.asset);
            pDone(pTableData.m_strName, new S2LoadEndInfo(true));
        }, pRequest);
    }

    void LoadAsyncToAssetBundle(S2ResourcesTableData pTableData,
        Action<string, S2LoadStartInfo> pStart, Action<string, S2LoadEndInfo> pDone)
    {
        if (true == m_dicResources.ContainsKey(pTableData.m_strName))
        {
            pDone(pTableData.m_strName, new S2LoadEndInfo(true));
            return;
        }

        AssetBundle pBundle = Single.Bundle.GetAssetBundle(pTableData.m_strSceneType);
        if (null == pBundle)
        {
            Single.Coroutine.Routine(() =>
            {
                pBundle = Single.Bundle.GetAssetBundle(pTableData.m_strSceneType);
                if (null != pBundle)
                    LoadAsyncToAssetBundle(pTableData, pStart, pDone);   
                else
                    pDone(pTableData.m_strName, new S2LoadEndInfo(false));
            },
            Single.Patch.StartPatchProcess(pTableData.m_strSceneType));
            return;
        }
        S2Util.Assert(pBundle.Contains(pTableData.m_strName), "{0}애셋번들에 {1}리소스가 없습니다!!!", pTableData.m_strSceneType, pTableData.m_strName);

        m_dicResources.Add(pTableData.m_strName, pBundle.Load(pTableData.m_strName));
        pDone(pTableData.m_strName, new S2LoadEndInfo(true));
    }

    UnityEngine.Object LoadSync(S2ResourcesTableData pTableData)
    {
        if (null == pTableData)
            return null;

        if (true == m_dicResources.ContainsKey(pTableData.m_strName))
            return m_dicResources[pTableData.m_strName];

        switch (Single.Global.ResourcesLoadType)
        {
            case eResourcesLoadType.Local_Resource:
                return LoadSyncToNative(pTableData);

            case eResourcesLoadType.Local_AssetBundle:
            case eResourcesLoadType.Server_AssetBundle:
                return LoadSyncToAssetBundle(pTableData);
        }
        return null;
    }

    UnityEngine.Object LoadSyncToNative(S2ResourcesTableData pTableData)
    {
        if (true == m_dicResources.ContainsKey(pTableData.m_strName))
            return m_dicResources[pTableData.m_strName];

        UnityEngine.Object pObject = Resources.Load(pTableData.m_strPath);
        if (null == pObject)
            return null;

        m_dicResources.Add(pTableData.m_strName, pObject);
        return pObject;
    }

    UnityEngine.Object LoadSyncToAssetBundle(S2ResourcesTableData pTableData)
    {
        if (true == m_dicResources.ContainsKey(pTableData.m_strName))
            return m_dicResources[pTableData.m_strName];

        // @@ 없으면 다운로드 받는거 까지해주고 싶다,,,
        AssetBundle pBundle = Single.Bundle.GetAssetBundle(pTableData.m_strSceneType);
        if (null == pBundle)
            return null;

        if (false == pBundle.Contains(pTableData.m_strName))
            return null;

        UnityEngine.Object pObject = pBundle.Load(pTableData.m_strName);
        if (null == pObject)
            return null;

        m_dicResources.Add(pTableData.m_strName, pObject);
        return pObject;
    }

    public UnityEngine.Object GetResources(string strName)
    {
        if (false == m_dicResources.ContainsKey(strName))
        {
            S2ResourcesTable pTable = Single.TableData.GetTable<S2ResourcesTable>();
            return LoadSync(pTable.GetData(strName));
        }

        return m_dicResources[strName];
    }

    public GameObject GetGameObject(string strPrefabName)
    {
        return S2GameObject.Instantiate(GetResources(strPrefabName));
    }

    public GameObject GetGameObjectToNoCopy(string strPrefabName)
    {
        return GetResources(strPrefabName) as GameObject;
    }

    public Texture GetTexture(string strTextrueName)
    {
        return GetResources(strTextrueName) as Texture;
    }

    public Material GetMaterial(string strMaterialName)
    {
        return GetResources(strMaterialName) as Material;
    }
}