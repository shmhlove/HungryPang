/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 15일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 엑셀 데이터 셋 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2DataSet
{
    public string           m_strTableName;
    public int              m_iMaxCol;
    public List<string>     m_ColumnNames = new List<string>();
    public List<string>     m_ColumnTypes = new List<string>();
    public List<string>     m_pDatas      = new List<string>();

    public void AddData(string strTableName, string strColName, string strColType, string strData)
    {
        m_strTableName = strTableName;

        m_ColumnNames.Add(strColName);
        m_ColumnTypes.Add(strColType);

        if ("text" == strColType)
            m_pDatas.Add(S2Util.Format("\"{0}\"", strData));
        else
            m_pDatas.Add(strData);

        m_iMaxCol++;
    }
}