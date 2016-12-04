using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class S2PatchSGT : S2Singleton<S2PatchSGT>
{
    // 패치 리스트 관련
    private List<string> m_pPatchList = new List<string>();
    private S2Pair<int, int> m_pPatchCount = new S2Pair<int, int>(0, 0);

    // 버전관련(년/월/일/시/분)
    private int m_iDefaultPatchVersion  = 1501010101;
    private int m_iClientVersion        = 1501010101;
    private int m_iServerVersion        = 1501010101;

    // 이벤트
    private Action           m_pEventToComplate = null;
    private Action<int, int> m_pEventToPrograss = null;

    public override void OnInitialize() { }
    public override void OnFinalize()   { }

    public void StartPatch(Action pComplate, Action<int, int> pPrograss)
    {
        InitVersion();

        m_pEventToComplate = pComplate;
        m_pEventToPrograss = pPrograss;

        switch (Single.Global.ResourcesLoadType)
        {
            case eResourcesLoadType.Local_Resource:
                m_pEventToComplate();
                break;
            case eResourcesLoadType.Local_AssetBundle:
                StartCoroutine(StartPatchProcess());
                break;
            case eResourcesLoadType.Server_AssetBundle:
                StartCoroutine(StartPatchProcess());
                break;
        }
    }

    void InitVersion()
    {
        m_iClientVersion = EncryptedPlayerPrefs.GetInt("BundleVersion", m_iDefaultPatchVersion);
    }

    public IEnumerator StartPatchProcess(string strBundleName)
    {
        // 버전체크
        if (m_iDefaultPatchVersion == m_iServerVersion)
            yield return StartCoroutine(CheckServerVersion());

        yield return StartCoroutine(Single.Bundle.DownloadAssetBundle(GetPathToPatchFile(strBundleName), m_iServerVersion, OnEventToStartPatch, OnEventToDonePatch));
    }

    IEnumerator StartPatchProcess()
    {
        // 버전체크
        CheckClientVersion();
        yield return StartCoroutine(CheckServerVersion());

        // 서버와 버전이 같으면 패치 패스.
        if (m_iClientVersion >= m_iServerVersion)
        {
            CallEventToComplate();
            yield break;
        }

        // 패치 목록 다운
        yield return StartCoroutine(CheckPatchList());

        // 패치목록 없으면 끝.
        if (m_pPatchList.Count <= 0)
        {
            CallEventToComplate();
            yield break;
        }

        // 다운로드 시작
        m_pPatchCount.Value1 = m_pPatchList.Count;
        m_pPatchCount.Value2 = 0;

        foreach (string strBundleName in m_pPatchList)
        {
            StartCoroutine(Single.Bundle.DownloadAssetBundle(GetPathToPatchFile(strBundleName), m_iServerVersion, OnEventToStartPatch, OnEventToDonePatch));
        }

        // Update Client Version
        EncryptedPlayerPrefs.SetInt("BundleVersion", m_iServerVersion);
    }

    void CheckClientVersion()
    {
        // 클라 버전 1401010101로 고정[테스트용]
        // EncryptedPlayerPrefs.SetInt(m_strClientVersionKey, m_iDefaultPatchVersion);
        m_iClientVersion = EncryptedPlayerPrefs.GetInt("BundleVersion", m_iDefaultPatchVersion);
    }

    IEnumerator CheckServerVersion()
    {
        using (WWW www = new WWW(GetPathToServerVersion()))
        {
            yield return www;

            m_iServerVersion = (false == string.IsNullOrEmpty(www.text)) ? int.Parse(www.text) : m_iDefaultPatchVersion;
        }
    }

    IEnumerator CheckPatchList()
    {
        string strPatchListPath = GetPathToPatchList();
        string strPatchList = null;

        using (WWW www = new WWW(strPatchListPath))
        {
            yield return www;

            if (www.error == null)
                strPatchList = www.text;
            else
                yield return null;

            JSONNode node = JSON.Parse(strPatchList);
            if (null == node)
            {
                S2Util.LogError("[S2DesignData] JsonLoadFail!! File : {0}", "Assets/Resources AssetBundle/PatchList.json");
                yield return null;
            }

            m_pPatchList.Clear();
            foreach (JSONNode no in node.Childs)
            {
                m_pPatchList.Add(no["Key"]);
            }
        }
    }

    string GetPathToServerVersion()
    {
        string strServerPath = string.Empty;

        switch (Single.Global.ResourcesLoadType)
        {
            case eResourcesLoadType.Local_Resource:
                strServerPath = string.Format("file://{0}{1}", Application.dataPath, "/Resources AssetBundle/ServerVersion.txt");
                break;

            case eResourcesLoadType.Local_AssetBundle:
                strServerPath = string.Format("file://{0}{1}", Application.dataPath, "/Resources AssetBundle/ServerVersion.txt");
                break;

            case eResourcesLoadType.Server_AssetBundle:
                strServerPath = string.Format("{0}/{1}", S2Path.GetURLToAssetBundle(), "ServerVersion.txt");
                break;
        }

        Debug.Log("[strServerPath] " + strServerPath);

        return strServerPath;
    }

    string GetPathToPatchList()
    {
        string strPatchListPath = string.Empty;

        switch (Single.Global.ResourcesLoadType)
        {
            case eResourcesLoadType.Local_Resource:
                strPatchListPath = string.Format("file://{0}{1}", Application.dataPath, "/Resources AssetBundle/PatchList.json");
                break;

            case eResourcesLoadType.Local_AssetBundle:
                strPatchListPath = string.Format("file://{0}{1}", Application.dataPath, "/Resources AssetBundle/PatchList.json");
                break;

            case eResourcesLoadType.Server_AssetBundle:
                strPatchListPath = string.Format("{0}/{1}", S2Path.GetURLToAssetBundle(), "PatchList.json");
                break;
        }

        return strPatchListPath;
    }

    public string GetPathToPatchFile(string _strFileName)
    {
        string strPatchFilePath = string.Empty;

        switch (Single.Global.ResourcesLoadType)
        {
            case eResourcesLoadType.Local_Resource:
                strPatchFilePath = string.Format("file://{0}/{1}/{2}/{3}/{4}.unity3d", Application.dataPath, "Resources AssetBundle", Single.Hard.GetPlatformName(), m_iServerVersion.ToString(), _strFileName);
                break;

            case eResourcesLoadType.Local_AssetBundle:
                strPatchFilePath = string.Format("file://{0}/{1}/{2}/{3}/{4}.unity3d", Application.dataPath, "Resources AssetBundle", Single.Hard.GetPlatformName(), m_iServerVersion.ToString(), _strFileName);
                break;

            case eResourcesLoadType.Server_AssetBundle:
                strPatchFilePath = string.Format("{0}/{1}/{2}/{3}.unity3d", S2Path.GetURLToAssetBundle(), Single.Hard.GetPlatformName(), m_iServerVersion.ToString(), _strFileName);
                break;
        }

        return strPatchFilePath;
    }

    void OnEventToStartPatch(string strBundleName, WWW pWWW)
    {

    }

    void OnEventToDonePatch(string strBundleName, bool bIsSuccess)
    {
        CallEventToPrograss(m_pPatchCount.Value1, ++m_pPatchCount.Value2);

        // @@ 하나라도 실패났을때 Complate 예외처리 필요
        if (m_pPatchCount.Value1 <= m_pPatchCount.Value2)
            CallEventToComplate();
    }

    void CallEventToPrograss(int iTotalCount, int iCurrentCount)
    {
        if (null == m_pEventToPrograss)
            return;

        m_pEventToPrograss(iTotalCount, iCurrentCount);
    }

    void CallEventToComplate()
    {
        if (null == m_pEventToComplate)
            return;

        m_pEventToComplate();
    }
}