/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 15일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 데이터관련 유틸함수를 모아둔 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class S2DataUtil
{
    public static string GetTypeToName(string strName)
    {
        if (0 == strName.IndexOf("i_"))         return "int";
        else if (0 == strName.IndexOf("f_"))    return "float";
        else if (0 == strName.IndexOf("s_"))    return "text";

        return null;
    }

    public static string GetTypeToDB(string strType)
    {
        if ("int" == strType)   return "INTEGER";
        if ("float" == strType) return "FLOAT";
        if ("text" == strType)  return "TEXT";

        return null;
    }
}