/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 15일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 테이블클래스들의 베이스 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Data;
using System.Collections;

public class S2BaseTable
{
    // 파일이름
    public string m_strFileName;

    // 데이터 Reader
    DataTable       m_pExcelTable   = null;
    SQLiteQuery     m_pSQLiteTable  = null;

    public virtual bool IsLoadTable()                                         { return false; }
    public virtual bool? LoadExcel(DataTable pTable, string strTableName)     { return null; }
    public virtual bool? LoadDB(SQLiteQuery pTable, string strTableName)      { return null; }
    public virtual bool? LoadJson(S2Json pJson, string strFileName)           { return null; }
    public virtual bool? LoadStatic()                                         { return null; }

    bool? Return(bool? bReturnValue)
    {
        m_pExcelTable   = null;
        m_pSQLiteTable  = null;

        return bReturnValue;
    }

    public bool? LoadExcel(string strFileName)
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadExcel(null, null))
            return Return(null);

        // 예외처리 : 로드체크
        S2Excel pExcel = new S2Excel(strFileName);
        if (false == pExcel.CheckExcelFile())
            return Return(false);

        // 예외처리 : 테이블체크
        DataTable pTableList = pExcel.GetTable("TableList");
        if (null == pTableList)
            return Return(false);

        for (int iRow = 1; iRow < pTableList.Rows.Count; ++iRow)
        {
            string strTableName = pTableList.Rows[iRow][0].ToString();
            m_pExcelTable       = pExcel.GetTable(strTableName);
            bool? bResult = LoadExcel(m_pExcelTable, strTableName);
            if (null == bResult)
                return Return(null);
            if (false == bResult.Value)
                return Return(false);
        }

        return Return(true);
    }

    public bool? LoadDB(string strFileName) 
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadDB(null, null))
            return Return(null);

        // 예외처리 : 로드체크
        S2SQLite pSQLite = new S2SQLite(strFileName);
        if (false == pSQLite.CheckDBFile())
            return Return(false);

        // 예외처리 : 테이블체크
        SQLiteQuery pTableList = pSQLite.GetTable("TableList");
        if (null == pTableList)
            return Return(false);

        while (true == pTableList.Step())
        {
            string strTableName = pTableList.GetString("s_Sheets");
            m_pSQLiteTable      = pSQLite.GetTable(strTableName);
            bool? bResult = LoadDB(m_pSQLiteTable, strTableName);
            if (null != m_pSQLiteTable)
            {
                m_pSQLiteTable.Release();
                m_pSQLiteTable = null;
            }
            
            if (null == bResult)
                return Return(null);
            if (false == bResult.Value)
                return Return(false);
        }

        pTableList.Release();
        return Return(true);
    }

    public bool? LoadJson(string strFileName) 
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadJson(null, null))
            return Return(null);

        // 예외처리 : 로드체크
        S2Json pJson = new S2Json(strFileName);
        if (false == pJson.CheckJson())
            return Return(false);

        return Return(LoadJson(pJson, strFileName));
    }

    public bool? LoadStatic(string strFileName)
    {
        return Return(LoadStatic());
    }

    // 유틸함수 : 엑셀에서 String데이터 얻기
    public string GetStrToExcel(int iRows, int iColumn)
    {
        if (null == m_pExcelTable)
            return string.Empty;

        return Convert.ToString(m_pExcelTable.Rows[iRows][m_pExcelTable.Columns[iColumn].ColumnName]);
    }
    // 유틸함수 : 엑셀에서 int데이터 얻기
    public int GetIntToExcel(int iRows, int iColumn)
    {
        if (null == m_pExcelTable)
            return 0;

        return Convert.ToInt32(m_pExcelTable.Rows[iRows][m_pExcelTable.Columns[iColumn].ColumnName]);
    }
    // 유틸함수 : 엑셀에서 float데이터 얻기
    public float GetFloatToExcel(int iRows, int iColumn)
    {
        if (null == m_pExcelTable)
            return 0.0f;

        return (float)Convert.ToDouble(m_pExcelTable.Rows[iRows][m_pExcelTable.Columns[iColumn].ColumnName]);
    }
    // 유틸함수 : 엑셀에서 vector3데이터 얻기
    public Vector3 GetVectorToExcel(int iRows, int iColumn)
    {
        return new Vector3(GetFloatToExcel(iRows, iColumn),
                           GetFloatToExcel(iRows, iColumn + 1),
                           GetFloatToExcel(iRows, iColumn + 2));
    }

    // 유틸함수 : DB에서 String데이터 얻기
    public string GetStrToSQL(string strKey)
    {
        if (null == m_pSQLiteTable)
            return string.Empty;

        return m_pSQLiteTable.GetString(strKey);
    }
    // 유틸함수 : DB에서 int데이터 얻기
    public int GetIntToSQL(string strKey)
    {
        if (null == m_pSQLiteTable)
            return 0;

        return m_pSQLiteTable.GetInteger(strKey);
    }
    // 유틸함수 : DB에서 float데이터 얻기
    public float GetFloatToSQL(string strKey)
    {
        if (null == m_pSQLiteTable)
            return 0.0f;

        return (float)m_pSQLiteTable.GetDouble(strKey);
    }
    // 유틸함수 : DB에서 vector3데이터 얻기
    public Vector3 GetVectorSQL(string strKey)
    {
        return new Vector3( GetFloatToSQL(strKey + "X"),
                            GetFloatToSQL(strKey + "Y"), 
                            GetFloatToSQL(strKey + "Z"));
    }
}
