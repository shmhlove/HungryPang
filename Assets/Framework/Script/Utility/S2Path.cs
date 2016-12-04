/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 18일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 경로관련 함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public static partial class S2Path
{
    // 경로 : (Root : Assets/Framework)
    public static string GetPathToWeData()
    {
        return S2Util.Format("{0}{1}", GetPathToAssets(), "/Framework");
    }

    // 경로 : (Root : Assets/Framework/DataToTable)
    public static string GetPathToTable()
    {
        return S2Util.Format("{0}{1}", GetPathToWeData(), "/DataToTable");
    }

    // 경로 : (Root : Assets/Resources)
    public static string GetPathToResources()
    {
        return S2Util.Format("{0}{1}", GetPathToAssets(), "/Resources");
    }

    // 경로 : (Root : Assets)
    public static string GetPathToAssets()
    {
        return Application.dataPath;
    }

    // 경로 : (Root : Assets/StreamingAssets)
    public static string GetPathToStreamingAssets()
    {
        return Application.streamingAssetsPath;
    }

    // 경로 : (Root : 사용자디렉토리/AppData/LocalLow/회사이름/프로덕트이름)
    public static string GetPathToPersistentDataPath()
    {
        return Application.persistentDataPath;
    }

    // 경로 : 서버주소
    public static string GetBaseURL()
    {
        return "https://drive.google.com/";
    }

    public static string GetURLToAssetBundle()
    {
        return GetBaseURL() + "";
    }
}