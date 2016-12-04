/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 18일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 하드 코드를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.IO;
using System.Collections;

public partial class S2HardCord : S2Singleton<S2HardCord>
{
    // 플랫폼 이름
    public string GetPlatformName()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        return "PC";
#elif UNITY_ANDROID
		return "AOS";
#elif UNITY_IPHONE
		return "IOS";
#endif
    }

    // 플랫폼 코드
    public int GetPlatformCord()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        return 1;
#elif UNITY_ANDROID
		return 2;
#elif UNITY_IPHONE
		return 3;
#endif
    }
}
