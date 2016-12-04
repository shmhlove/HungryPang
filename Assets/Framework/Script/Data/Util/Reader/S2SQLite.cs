/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 SQLite 파일을 로드하는 래퍼 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Data;

using System.IO;
using System.Threading;
using Community.CsharpSqlite;

public class S2SQLite
{
    private SQLiteDB m_pSQLiteDB = null;

    public S2SQLite() { }
    public S2SQLite(string strFileName)
    {
        // 예외처리 : .db로 확장자 고정
        strFileName = Path.GetFileNameWithoutExtension(strFileName);
        strFileName = S2Util.Format("{0}.db", strFileName);

        // Streaming에서 .db를 byte[]형태로 읽어서 로컬에 SQLiteDB로 저장
        string strReadPath = string.Empty;
#if UNITY_EDITOR || UNITY_STANDALONE
        strReadPath = S2Util.Format("{0}{1}", "file://", S2Path.GetPathToStreamingAssets());
#elif UNITY_ANDROID
        strReadPath = S2Util.Format("{0}{1}{2}", "jar:file://", S2Path.GetPathToAssets(), "!/assets");
#elif UNITY_IOS
        strReadPath = S2Util.Format("{0}{1}{2}", "file://", S2Path.GetPathToAssets(), "/Raw");
#endif
        string strReadFilePath  = S2Util.Format("{0}/SQLite/{1}", strReadPath, strFileName);
        string strSavePath      = S2Path.GetPathToPersistentDataPath();
        string strSaveFilePath  = S2Util.Format("{0}/{1}", strSavePath, strFileName);
        SaveBytesToDB(strSaveFilePath, GetBytesToDB(strReadFilePath));
        
        try
        {
            m_pSQLiteDB = new SQLiteDB();
            m_pSQLiteDB.Open(strSaveFilePath);
        }
        catch (System.Exception e)
        {
            S2Util.LogError("SQLite Read Fail : {0}", e.ToString());
        }
    }

    ~S2SQLite()
    {
        if (null == m_pSQLiteDB)
            return;

        m_pSQLiteDB.Close();
    }

    // .DB파일에 데이터 셋 쓰기
#if UNITY_EDITOR_WIN
    public void Write(string strFileName, Dictionary<string, List<S2DataSet>> dicData)
    {
        string strSavePath      = S2Path.GetPathToStreamingAssets();
        string strSaveFilePath  = S2Util.Format("{0}/SQLite/{1}.db", strSavePath, Path.GetFileNameWithoutExtension(strFileName));

        File.Delete(strSaveFilePath);

        try
        {
            m_pSQLiteDB = new SQLiteDB();
            m_pSQLiteDB.Open(strSaveFilePath);
            foreach (KeyValuePair<string, List<S2DataSet>> kvp in dicData)
            {
                // 테이블 생성
                if (false == CreateTable(kvp.Key, kvp.Value[0]))
                    break;

                // 생성한 테이블에 데이터 인설트
                if (false == InsertData(kvp.Key, kvp.Value))
                    break;
            }
        }
        catch (System.Exception e)
        {
            S2Util.LogError("SQLite Read Fail : {0}", e.ToString());
        }
    }
#endif

    // 쿼리 : 테이블 생성("CREATE TABLE `TableName` (`FieldName1` INTEGER, `FieldName2` INTEGER, `FieldName3` INTEGER);")
    bool CreateTable(string strTableName, S2DataSet pRowDataSet)
    {
        if (null == pRowDataSet)
            return false;

        string strFields = string.Empty;
        for (int iCol = 0; iCol < pRowDataSet.m_iMaxCol; ++iCol)
        {
            string strColName = pRowDataSet.m_ColumnNames[iCol];
            string strColType = pRowDataSet.m_ColumnTypes[iCol];
            strFields += S2Util.Format("\"{0}\" {1}", strColName, S2DataUtil.GetTypeToDB(strColType));

            if (iCol + 1 < pRowDataSet.m_iMaxCol)
                strFields += ", ";
        }

        string strQuery = string.Empty;
        if (string.Empty == strFields)
            strQuery = S2Util.Format("CREATE TABLE \"{0}\";", strTableName);
        else
            strQuery = S2Util.Format("CREATE TABLE \"{0}\" ({1});", strTableName, strFields);

        return WriteQuery(strQuery);
    }

    // 쿼리 : 데이터 추가("INSERT INTO 'TableName' VALUES(strValue1, 1);")
    bool InsertData(string strTableName, List<S2DataSet> pRowDataSets)
    {
        if (null == pRowDataSets)
            return false;

        int iMaxRow = pRowDataSets.Count;
        for (int iRow = 0; iRow < iMaxRow; ++iRow)
        {
            string strValues = string.Empty;
            for (int iCol = 0; iCol < pRowDataSets[iRow].m_iMaxCol; ++iCol)
            {
                strValues += pRowDataSets[iRow].m_pDatas[iCol];

                if (iCol + 1 < pRowDataSets[iRow].m_iMaxCol)
                    strValues += ", ";
            }

            string strQuery = S2Util.Format("INSERT INTO \"{0}\" VALUES({1});", strTableName, strValues);
            if (false == WriteQuery(strQuery))
                return false;
        }

        return true;
    }

    // 쿼리 : 테이블 얻기("SELECT * FROM PC1;";)
    public SQLiteQuery GetTable(string strTableName)
    {
        return ReadQuery(S2Util.Format("SELECT * FROM {0};", strTableName));
    }

    // 쓰는 쿼리
    bool WriteQuery(string strQuery)
    {
        if (null == m_pSQLiteDB)
            return false;

        if (string.Empty == strQuery)
            return false;

        string strErr = string.Empty;
        Sqlite3.exec(m_pSQLiteDB.Connection(), strQuery, null, null, ref strErr);

        if (string.Empty != strErr)
        {
            S2Util.LogError("QueryError : {0}\n{1}", strQuery, strErr);
            return false;
        }

        return true;
    }

    // 읽는 쿼리
    SQLiteQuery ReadQuery(string strQuery)
    {
        if (null == m_pSQLiteDB)
            return null;

        if (string.Empty == strQuery)
            return null;

        return new SQLiteQuery(m_pSQLiteDB, strQuery);
    }

    byte[] GetBytesToDB(string strFilePath)
    {
        byte[] pByte = null;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_ANDROID
        {
            WWW pWWW = new WWW(strFilePath);
            Download(pWWW);
            while (false == pWWW.isDone) ;

            if (true != string.IsNullOrEmpty(pWWW.error))
            {
                S2Util.LogError("SQLite(*.db)파일을 읽는 중 오류발생!!(Path:{0}, Error:{1})", strFilePath, pWWW.error);
                return null;
            }

            pByte = pWWW.bytes;
        }
#elif UNITY_IPHONE
        try
        {
            if (false == File.Exists(strFilePath))
            {
                S2Util.LogError("SQLite(*.db)파일을 읽는 중 오류발생!!(Path:{0}, Error:{1})", strFilePath, "파일이 없습니다!!");
                return null;
            }

        	FileStream pReadFile = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            pByte = new byte[pReadFile.Length];
            pReadFile.Read(pByte, 0, (int)pReadFile.Length);
            pReadFile.Close();
        }
        catch (System.Exception e)
        {
            S2Util.LogError("SQLite Read Fail : {0}", e.ToString())
        }
#endif
        return pByte;
    }

    void SaveBytesToDB(string strFilePath, byte[] pBytes)
    {
        if (null == pBytes)
            return;

        try
        {
            FileStream pWriteFile = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);
            pWriteFile.Write(pBytes, 0, pBytes.Length);
            pWriteFile.Close();
        }
        catch (System.Exception e)
        {
            S2Util.LogError("SQLite Write Fail : {0}", e.ToString());
        }
    }

    IEnumerator Download(WWW www)
    {
        yield return www;
    }

    public bool CheckDBFile()
    {
        return (null != m_pSQLiteDB);
    }
}

// 자주 사용하는 Query
// "DROP TABLE IF EXISTS PC1;";
// "CREATE TABLE IF NOT EXISTS PC1 (id INTEGER PRIMARY KEY, str_field TEXT, blob_field BLOB);";
// "INSERT INTO PC1 (str_field,blob_field) VALUES(?,?);";
// "SELECT * FROM PC1;";