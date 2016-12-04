/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤
★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 20일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 리소스 리스트를 담습니다.
▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/

using UnityEngine;
using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class S2ResourcesTableData
{
    public string           m_strHash;
    public string           m_strName;
    public string           m_strExtension;
    public string           m_strPath;
    public eResourceType    m_eResourceType;
    public string           m_strSceneType;
}

public class S2ResourcesTable : S2BaseTable
{
    // Columns
    public enum eColumns
    {
        s_Hash,             // 해시값(MD5)
        s_Name,             // 파일명
        s_Extension,        // 확장자
        s_Path,             // 경로(Resources하위 상대경로)
        s_ResourceType,     // 리소스 타입
        s_SceneType,        // 씬 타입
    }

    // <SceneType, <DataName, Data>>
    Dictionary<string, Dictionary<string, S2ResourcesTableData>> m_dicResourcesTable =
        new Dictionary<string, Dictionary<string, S2ResourcesTableData>>();

    public S2ResourcesTable()
    {
        m_strFileName = "ResourcesTable";
    }

    public override bool IsLoadTable()
    {
        return (0 != m_dicResourcesTable.Count);
    }

    public override bool? LoadJson(S2Json pJson, string strFileName) 
    {
        if (null == pJson)
            return false;

        ClearResourcesData();

        int iMaxTable = pJson.Node["TableList"].Count;
        for (int iTable = 0; iTable < iMaxTable; ++iTable)
        {
            string strTableName = pJson.Node["TableList"][iTable]["s_Sheets"].Value;
            int iMaxData = pJson.Node[strTableName].Count;
            for(int iData = 0; iData < iMaxData; ++iData)
            {
                JSONNode pDataNode = pJson.Node[strTableName][iData];
                S2ResourcesTableData pData = new S2ResourcesTableData();
                pData.m_strHash             = pDataNode[eColumns.s_Hash.ToString()].Value;
                pData.m_strName             = pDataNode[eColumns.s_Name.ToString()].Value;
                pData.m_strExtension        = pDataNode[eColumns.s_Extension.ToString()].Value;
                pData.m_strPath             = pDataNode[eColumns.s_Path.ToString()].Value;
                pData.m_eResourceType       = Single.Hard.GetResourceTypeToEnumName(pDataNode[eColumns.s_ResourceType.ToString()].Value);
                pData.m_strSceneType        = pDataNode[eColumns.s_SceneType.ToString()].Value;

                AddResourcesData(pData);
            }
        }
        return true;
    }

    void ClearResourcesData()
    {
        m_dicResourcesTable.Clear();
    }

    void AddResourcesData(S2ResourcesTableData pData)
    {
        if (null == pData)
            return;

        string strKey = pData.m_strSceneType;
        if (false == m_dicResourcesTable.ContainsKey(strKey))
            m_dicResourcesTable[strKey] = new Dictionary<string, S2ResourcesTableData>();

        m_dicResourcesTable[strKey][pData.m_strName] = pData;
    }

    public Dictionary<string, S2ResourcesTableData> GetDataToList(eSceneType eType)
    {
        return GetDataToList(Single.Hard.GetStrSceneToEnum(eType));
    }

    public Dictionary<string, S2ResourcesTableData> GetDataToList(string strSceneType)
    {
        if (0 == m_dicResourcesTable.Count)
            LoadJson(m_strFileName);

        if (false == m_dicResourcesTable.ContainsKey(strSceneType))
            return null;

        return new Dictionary<string, S2ResourcesTableData>(m_dicResourcesTable[strSceneType]);
    }

    public S2ResourcesTableData GetData(string strFileName)
    {
        if (0 == m_dicResourcesTable.Count)
            LoadJson(m_strFileName);

        foreach(KeyValuePair<string, Dictionary<string, S2ResourcesTableData>> kvp in m_dicResourcesTable)
        {
            if (true == kvp.Value.ContainsKey(strFileName))
                return kvp.Value[strFileName];
        }

        return null;
    }
}

