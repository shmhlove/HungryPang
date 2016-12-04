/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 18일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 파일명 관련 하드 코드를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.IO;
using System.Collections;

public partial class S2HardCord : S2Singleton<S2HardCord>
{
    public eResourceType GetResourceTypeToEnumName(string strEnumName)
    {
        if ("Prefab" == strEnumName)
            return eResourceType.Prefab;

        if ("Texture" == strEnumName)
            return eResourceType.Texture;

        if ("Sound" == strEnumName)
            return eResourceType.Sound;

        return eResourceType.None;
    }

    public string GetStrSceneToEnum(eSceneType eType)
    {
        switch(eType)
        {
            case eSceneType.Loading:        return "Loading";
            case eSceneType.StartGame:      return "StartGame";
            case eSceneType.InGame:         return "InGame";
        }
        return string.Empty;
    }
}
