/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 Excel 파일을 Read/Write하는 래퍼 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;

public class S2Excel
{
    private string m_strFilePath        = string.Empty;
    private string m_strCopyFilePath    = string.Empty;

    public S2Excel(string strFileName)
    {
        // 예외처리 : .xlsx로 확장자 고정
        strFileName             = Path.GetFileNameWithoutExtension(strFileName);
        strFileName             = S2Util.Format("{0}.xlsx", strFileName);

        // Excel 원본 복사(엑셀파일 편집중 고려)
        string strPath          = S2Util.Format("{0}/Excels",   S2Path.GetPathToTable());
        m_strFilePath           = S2Util.Format("{0}/{1}",      strPath, strFileName);
        m_strCopyFilePath       = S2Util.Format("{0}/Temp_{1}", strPath, strFileName);

        // 예외처리 : 파일체크
        if (false == File.Exists(m_strFilePath))
        {
            S2Util.LogError("Excel(*.xlsx)파일이 없습니다.(Path:{0})", m_strFilePath);
            return;
        }
        
        File.Delete(m_strCopyFilePath);
        File.Copy(m_strFilePath, m_strCopyFilePath);
    }

    ~S2Excel()
    {
        if (string.Empty == m_strCopyFilePath)
            return;

        File.Delete(m_strCopyFilePath);
    }

    // 데이터 셋 생성( DB컨버팅에 사용 됨 )
    // 엑셀에 어떤 시트 어떤 데이터가 있는지 모르는 상태에서 모든 데이터를 string형태로 뽑아 버리는 기능
    public Dictionary<string, List<S2DataSet>> GetDataSet()
    {
        DataTable pTableList = GetTable("TableList");
        if (null == pTableList)
            return null;
        
        Dictionary<string, List<S2DataSet>> dicData = new Dictionary<string, List<S2DataSet>>();
        dicData.Add(pTableList.TableName, GetSheetDataSet(pTableList));

        for (int iRow = 1; iRow < pTableList.Rows.Count; ++iRow)
        {
            DataTable pTable = GetTable(pTableList.Rows[iRow][0].ToString());
            if (null == pTable)
                break;

            dicData.Add(pTable.TableName, GetSheetDataSet(pTable));
        }
        return dicData;
    }

    public List<S2DataSet> GetSheetDataSet(DataTable pTable)
    {
        if (null == pTable)
            return null;

        // 컬럼정보생성 : 컬럼별 데이터 타입과 컬럼명 얻기
        string[] pColName = new string[pTable.Columns.Count];
        string[] pColType = new string[pTable.Columns.Count];
        for (int iCol = 0; iCol < pTable.Columns.Count; ++iCol)
        {
            string strName = pTable.Columns[iCol].ColumnName;
            string strType = S2DataUtil.GetTypeToName(strName);
            if (null == strType)
            {
                S2Util.LogError("지정된 데이터 타입이 아닙니다!!(약속 : i_, f_, s_), ColName:{0}", strName);
                return null;
            }

            pColName[iCol] = strName;
            pColType[iCol] = strType;
        }

        // 데이터 읽기(첫번째 Row(설명Row)는 제외하기위해 1부터 시작)
        List<S2DataSet> pSheetData = new List<S2DataSet>();
        for (int iRow = 1; iRow < pTable.Rows.Count; ++iRow)
        {
            S2DataSet pDataSet = new S2DataSet();
            for (int iCol = 0; iCol < pTable.Columns.Count; ++iCol)
            {
                pDataSet.AddData(pTable.TableName, pColName[iCol], pColType[iCol],
                    pTable.Rows[iRow][pColName[iCol]].ToString());
            }

            pSheetData.Add(pDataSet);
        }

        return pSheetData;
    }

    // 테이블 얻기
    public DataTable GetTable(string strTableName)
    {
        if (string.Empty == m_strCopyFilePath)
            return null;

        return LoadToODBC(m_strCopyFilePath, strTableName);
    }

    // 엑셀파일을 ODBC로 로드합니다.
    DataTable LoadToODBC(string strFilePath, string strTableName)
    {
#if !UNITY_EDITOR_WIN
        return null;
#endif
        string strDriver = "Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}; Dbq=" + strFilePath + ";";
        OdbcConnection pConnector = new OdbcConnection(strDriver);
        pConnector.Open();

        OdbcCommand pCmd = new OdbcCommand(S2Util.Format("SELECT * FROM [{0}$]", strTableName), pConnector);
        OdbcDataReader pExecuteData = pCmd.ExecuteReader();

        DataTable pData = new DataTable();
        pData.Load(pExecuteData);
        pData.TableName = strTableName;

        pExecuteData.Close();
        pConnector.Close();

        return pData;
    }

    public bool CheckExcelFile()
    {
        return File.Exists(m_strCopyFilePath);
    }
}