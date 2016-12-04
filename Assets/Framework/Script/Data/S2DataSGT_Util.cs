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
    // 로드가 완료되었는가?(성공/실패유무가 아님)
    public bool IsLoadDone()
    {
        return Loader.IsLoadDone();
    }

    // 특정 파일이 로드완료되었는가?(성공/실패유무가 아님)
    public bool IsLoadDone(string strFileName)
    {
        return Loader.IsLoadDone(strFileName);
    }

    // 특정 타입이 로드완료되었는가?(성공/실패유무가 아님)
    public bool IsLoadDone(eDataType eType)
    {
        return Loader.IsLoadDone(eType);
    }

    // 로드중인지 체크(로드 중이거나 할 파일이 있는지  체크)
    public bool IsReMainLoadFiles()
    {
        return Loader.IsReMainLoadFiles();
    }
}