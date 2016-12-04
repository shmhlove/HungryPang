/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 15일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 샘플데이터를 담습니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class S2SampleData
{
    public int      iDataID;
    public float    fAttribute1;
    public string   strAttribute2;
    public string   strAttribute3;
}

public class S2SampleTable : S2BaseTable
{
    // Columns
    public enum eColumns
    {
        i_DataID,
        f_Attribute1,
        s_Attribute2,
        s_Attribute3,
    }

    // <TableName, <DataID, Data>>
    Dictionary<string, Dictionary<int, S2SampleData>> m_dicSampleTable = new Dictionary<string, Dictionary<int, S2SampleData>>();

    public S2SampleTable()
    {
        m_strFileName = "_Sample";
    }

    public override bool IsLoadTable()
    {
        return (0 != m_dicSampleTable.Count);
    }

    public override bool? LoadExcel(DataTable pTable, string strTableName)
    {
        if (null == pTable)
            return false;

        for (int iRow = 1; iRow < pTable.Rows.Count; ++iRow)
        {
            S2SampleData pData = new S2SampleData();
            pData.iDataID           = GetIntToExcel(iRow,   (int)eColumns.i_DataID);
            pData.fAttribute1       = GetFloatToExcel(iRow, (int)eColumns.f_Attribute1);
            pData.strAttribute2     = GetStrToExcel(iRow,   (int)eColumns.s_Attribute2);
            pData.strAttribute3     = GetStrToExcel(iRow,   (int)eColumns.s_Attribute3);
            AddSampleData(strTableName, pData.iDataID, pData);
        }

        return true;
    }

    public override bool? LoadDB(SQLiteQuery pTable, string strTableName)
    {
        if (null == pTable)
            return false;

        while (true == pTable.Step())
        {
            S2SampleData pData = new S2SampleData();
            pData.iDataID           = GetIntToSQL(eColumns.i_DataID.ToString());
            pData.fAttribute1       = GetFloatToSQL(eColumns.f_Attribute1.ToString());
            pData.strAttribute2     = GetStrToSQL(eColumns.s_Attribute2.ToString());
            pData.strAttribute3     = GetStrToSQL(eColumns.s_Attribute3.ToString());
            AddSampleData(strTableName, pData.iDataID, pData);
        }

        return true;
    }

    public struct JsonSampleData
    {
        public int      iInt;
        public float    fFloat;
        public string   strString;
        public int[][]  arrInt;

        public void AddData(JSONNode pNode)
        {
            iInt                     = pNode["IntField"].AsInt;
            fFloat                   = pNode["FloatField"].AsFloat;
            strString                = pNode["StringField"].Value;
            JSONArray pArray         = pNode["ArrayField"].AsArray;
            arrInt                   = new int[pArray.Count][];
            for (int iLoop = 0; iLoop < pArray.Count; ++iLoop)
            {
                arrInt[iLoop]        = new int[2];
                arrInt[iLoop][0]     = pArray[iLoop][0].AsInt;
                arrInt[iLoop][1]     = pNode["ArrayField"][iLoop][1].AsInt;
            }
        }
    }
    public override bool? LoadJson(S2Json pJson, string strFileName)
    {
        if (null == pJson)
            return false;

        // 기본문법1 : { Field }
        JsonSampleData pBase1 = new JsonSampleData();
        pBase1.AddData(pJson.Node["RootNode1"]);

        // 기본문법2 : [ { Field }, { Field } ]
        JsonSampleData[] pBase2 = new JsonSampleData[2];
        int iMaxBunch = pJson.Node["RootNode2"].Count;
        for (int iBunch = 0; iBunch < iMaxBunch; ++iBunch)
        {
            pBase2[iBunch].AddData(pJson.Node["RootNode2"][iBunch]);
        }

        // 기본문법3 : [ [ { Field }, { Field } ], [ { Field }, { Field } ] ]
        int iMaxOutBunch = pJson.Node["RootNode3"].Count;
        JsonSampleData[][] pBase3 = new JsonSampleData[iMaxOutBunch][];
        for (int iOutBunch = 0; iOutBunch < iMaxOutBunch; ++iOutBunch)
        {
            int iMaxInBunch     = pJson.Node["RootNode3"][iOutBunch].Count;
            pBase3[iOutBunch]   = new JsonSampleData[iMaxInBunch];
            for(int iInBunch = 0; iInBunch < iMaxInBunch; ++iInBunch)
            {
                pBase3[iOutBunch][iInBunch].AddData(pJson.Node["RootNode3"][iOutBunch][iInBunch]);
            }
        }

        return true;
    }

    void AddSampleData(string strTable, int iDataID, S2SampleData pData)
    {
        if (false == m_dicSampleTable.ContainsKey(strTable))
            m_dicSampleTable.Add(strTable, new Dictionary<int,S2SampleData>());

        m_dicSampleTable[strTable][iDataID] = pData;
    }
}