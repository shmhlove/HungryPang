/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 07월 18일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 애셋번들을 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleInfo
{
    public string       m_strURL;
    public int          m_iVersion;
    public AssetBundle  m_pBundle;

    public AssetBundleInfo(string strURL, int iVersion, AssetBundle pBundle)
    {
        m_strURL    = strURL;
        m_iVersion  = iVersion;
        m_pBundle   = pBundle;
    }
}

public class S2BundleSGT : S2Singleton<S2BundleSGT>
{
    // 현재 메모리에 올라간 번들정보
    private Dictionary<string, AssetBundleInfo> m_dicAssetBundle = new Dictionary<string, AssetBundleInfo>();

    // 다운로드 중인 번들정보
    private Dictionary<string, WWW> m_dicLoadingBundle = new Dictionary<string, WWW>();

    public override void OnInitialize()
    {
        SetDontDestroy();

        Caching.maximumAvailableDiskSpace = Single.Global.m_iBundleCatchSize * 10 * 1024 * 1024;
        m_dicLoadingBundle.Clear();
    }

    public override void OnFinalize()
    {
        UnLoadAll(true);
    }

    public void InitCaching()
    {
        Caching.CleanCache();
    }

    public AssetBundle GetAssetBundle(string strURL)
    {
        string strBundleName = Path.GetFileNameWithoutExtension(strURL);
        if (false == m_dicAssetBundle.ContainsKey(strBundleName))
            return null;

        return m_dicAssetBundle[strBundleName].m_pBundle;
    }

    public IEnumerator DownloadAssetBundle(string strURL, int iVersion, Action<string, WWW> pStart, Action<string, bool> pDone)
    {
        // 예외처리 : 이미 캐시되었는가?
        string strBundleName = Path.GetFileNameWithoutExtension(strURL);
        if (true == m_dicAssetBundle.ContainsKey(strBundleName))
        {
            pDone(strBundleName, true);
            yield break;
        }

        // 예외처리 : 캐시가 준비되었는가?
        while (false == Caching.ready)
        {
            yield return null;
        }
        
        // 예외처리 : 이미 다운로드 중인가?
        while (true == m_dicLoadingBundle.ContainsKey(strURL))
        {
            yield return null;
        }

        // 다운로드 시작
        using (WWW www = WWW.LoadFromCacheOrDownload(strURL, iVersion))
        {
            m_dicLoadingBundle.Add(strURL, www);
            pStart(strURL, www);

            yield return www;

            m_dicLoadingBundle.Remove(strURL);

            if (www.error != null)
            {
                S2Util.LogError("Error : " + www.error);

                // 네트워크 연결이 안되었는지 체킹
                if (Application.internetReachability == NetworkReachability.NotReachable)
                { }
            }
            else
            {
                AssetBundleInfo pInfo = new AssetBundleInfo(strURL, iVersion, www.assetBundle);
                m_dicAssetBundle.Add(strBundleName, pInfo);
            }
        }

        pDone(strBundleName, m_dicAssetBundle.ContainsKey(strBundleName));
    }

    public void Unload(string strURL, bool bIsAllObjects)
    {
        string strBundleName = Path.GetFileNameWithoutExtension(strURL);
        if (false == m_dicAssetBundle.ContainsKey(strBundleName))
            return;

        m_dicAssetBundle[strBundleName].m_pBundle.Unload(bIsAllObjects);
        m_dicAssetBundle.Remove(strBundleName);
    }

    public void UnLoadAll(bool bIsAllObjects)
    {
        S2Util.ForeachToDic<string, AssetBundleInfo>(m_dicAssetBundle, 
        (pKey, pValue) => {
            pValue.m_pBundle.Unload(bIsAllObjects);
            pValue.m_pBundle = null;
        });

        m_dicAssetBundle.Clear();
    }
}
