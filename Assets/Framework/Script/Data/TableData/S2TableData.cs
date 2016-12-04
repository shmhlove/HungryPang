/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 15일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 모든 테이블 데이터를 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class S2TableData : S2BaseData
{
    // 테이블 리스트(<테이블타입, 테이블클래스>)
    private Dictionary<Type, S2BaseTable> m_dicTables = new Dictionary<Type, S2BaseTable>();

    // 파일 모니터.
    private FileSystemWatcher m_pFileMonitorToDB    = null;
    private FileSystemWatcher m_pFileMonitorToExcel = null;
    private FileSystemWatcher m_pFileMonitorToJson  = null;
    private Dictionary<eTableType, Type> m_dicChangedList = new Dictionary<eTableType, Type>();

    public override void OnInitialize()
    {
        m_dicTables.Clear();
		//m_dicTables.Add(typeof(S2SampleTable),      new S2SampleTable());
        m_dicTables.Add(typeof(S2ResourcesTable),   new S2ResourcesTable());

        RegisterFileMonitor();
        m_dicChangedList.Clear();
    }

    public override void OnFinalize()
    {
        if (null != m_pFileMonitorToDB)         m_pFileMonitorToDB.Dispose();
        if (null != m_pFileMonitorToExcel)      m_pFileMonitorToExcel.Dispose();
        if (null != m_pFileMonitorToJson)       m_pFileMonitorToJson.Dispose();
    }

    public override Dictionary<string, S2LoadData> GetLoadList(eSceneType eType)
    {
        Dictionary<string, S2LoadData> dicLoadList = new Dictionary<string, S2LoadData>();
        S2Util.ForeachToDic<Type, S2BaseTable>(m_dicTables, 
        (pKey, pValue) => {
            S2LoadData pLoadInfo = new S2LoadData();
            pLoadInfo.m_eDataType   = eDataType.Table;
            pLoadInfo.m_strName     = pValue.m_strFileName;
            pLoadInfo.m_pLoadFunc   = Load;

            dicLoadList.Add(pValue.m_strFileName, pLoadInfo);
        });
        return dicLoadList;
    }

    public override void Load(S2LoadData pInfo,
        Action<string, S2LoadStartInfo> pStart, Action<string, S2LoadEndInfo> pDone)
    {
        S2BaseTable pTable = GetTable(pInfo.m_strName);
        if (null == pTable)
        {
            S2Util.LogError("[TableData] 등록된 테이블이 아닙니다.!!({0})", pInfo.m_strName);
            pDone(pInfo.m_strName, new S2LoadEndInfo(false));
            return;
        }
        
        // 이미 로드된 데이터인지 체크
        if (true == pTable.IsLoadTable())
        {
            pDone(pInfo.m_strName, new S2LoadEndInfo(true));
            return;
        }

        // Static데이터 로드
        if (false == CheckLoadResult(pTable.LoadStatic(pTable.m_strFileName)))
        {
            pDone(pInfo.m_strName, new S2LoadEndInfo(false));
            return;
        }
        
        // Json데이터 로드
        if (false == CheckLoadResult(pTable.LoadJson(pTable.m_strFileName)))
        {
            pDone(pInfo.m_strName, new S2LoadEndInfo(false));
            return;
        }

        // SQLite데이터 로드
        if (false == CheckLoadResult(pTable.LoadDB(pTable.m_strFileName)))
        {
            pDone(pInfo.m_strName, new S2LoadEndInfo(false));
            return;
        }

        pDone(pInfo.m_strName, new S2LoadEndInfo(true));
    }

    public T GetTable<T>() where T : S2BaseTable
    {
        return GetTable(typeof(T)) as T;
    }

    public S2BaseTable GetTable(Type pType)
    {
        if (false == m_dicTables.ContainsKey(pType))
            return null;

        return m_dicTables[pType];
    }

    public S2BaseTable GetTable(string strFileName)
    {
        Type pType = GetTypeToFileName(strFileName);
        if (false == m_dicTables.ContainsKey(pType))
            return null;

        return m_dicTables[pType];
    }

    public Type GetTypeToFileName(string strFileName)
    {
        foreach(KeyValuePair<Type, S2BaseTable> kvp in m_dicTables)
        {
            if (strFileName == kvp.Value.m_strFileName)
                return kvp.Key;
        }

        return null;
    }

    bool CheckLoadResult(bool? bLoadResult)
    {
        if (null == bLoadResult)
            return true;

        return bLoadResult.Value;
    }

    public override void FrameMove()
    {
#if UNITY_EDITOR
        foreach (KeyValuePair<eTableType, Type> kvp in m_dicChangedList)
        {
            S2BaseTable pTable = GetTable(kvp.Value);
            if (null == pTable)
                return;

            switch (kvp.Key)
            {
                case eTableType.Static: pTable.LoadStatic(pTable.m_strFileName); break;
                case eTableType.Excel: pTable.LoadExcel(pTable.m_strFileName); break;
                case eTableType.DB: pTable.LoadDB(pTable.m_strFileName); break;
                case eTableType.Json: pTable.LoadJson(pTable.m_strFileName); break;
            }
        }

        m_dicChangedList.Clear();
#endif
    }

    void RegisterFileMonitor()
    {
#if UNITY_EDITOR
        if (null == m_pFileMonitorToDB)
        {
            m_pFileMonitorToDB                  = new FileSystemWatcher();
            m_pFileMonitorToDB.Path             = S2Util.Format("{0}/SQLite", S2Path.GetPathToStreamingAssets());
            m_pFileMonitorToDB.NotifyFilter     = (NotifyFilters.LastWrite | NotifyFilters.Size);
            m_pFileMonitorToDB.Filter           = "*.db";
            m_pFileMonitorToDB.Changed          += new FileSystemEventHandler(OnEventToChangedDB);
            m_pFileMonitorToDB.EnableRaisingEvents = true;
            m_pFileMonitorToDB.IncludeSubdirectories = true;
        }

        if (null == m_pFileMonitorToExcel)
        {
            m_pFileMonitorToExcel               = new FileSystemWatcher();
            m_pFileMonitorToExcel.Path          = S2Util.Format("{0}/Excels", S2Path.GetPathToTable());
            m_pFileMonitorToExcel.NotifyFilter  = (NotifyFilters.LastWrite | NotifyFilters.Size);
            m_pFileMonitorToExcel.Filter        = "*.xlsx";
            m_pFileMonitorToExcel.Changed       += new FileSystemEventHandler(OnEventToChangedExcel);
            m_pFileMonitorToExcel.EnableRaisingEvents = true;
            m_pFileMonitorToExcel.IncludeSubdirectories = true;
        }

        if (null == m_pFileMonitorToJson)
        {
            m_pFileMonitorToJson                = new FileSystemWatcher();
            m_pFileMonitorToJson.Path           = S2Util.Format("{0}/JSons", S2Path.GetPathToStreamingAssets());
            m_pFileMonitorToJson.NotifyFilter   = (NotifyFilters.LastWrite | NotifyFilters.Size);
            m_pFileMonitorToJson.Filter         = "*.json";
            m_pFileMonitorToJson.Changed        += new FileSystemEventHandler(OnEventToChangedJson);
            m_pFileMonitorToJson.EnableRaisingEvents = true;
            m_pFileMonitorToJson.IncludeSubdirectories = true;
        }
#endif
    }

    void OnEventToChangedDB(object pSender, FileSystemEventArgs pArgs)
    {
        string strFileName = Path.GetFileNameWithoutExtension(pArgs.FullPath);
        if (true == string.IsNullOrEmpty(strFileName))
            return;

        Type pType = GetTypeToFileName(strFileName);
        if (null == pType)
            return;

        m_dicChangedList.Add(eTableType.DB, pType);
    }

    void OnEventToChangedExcel(object pSender, FileSystemEventArgs pArgs)
    {
        string strFileName = Path.GetFileNameWithoutExtension(pArgs.FullPath);
        if (true == string.IsNullOrEmpty(strFileName))
            return;

        Type pType = GetTypeToFileName(strFileName);
        if (null == pType)
            return;

        m_dicChangedList.Add(eTableType.Excel, pType);
    }

    void OnEventToChangedJson(object pSender, FileSystemEventArgs pArgs)
    {
        string strFileName = Path.GetFileNameWithoutExtension(pArgs.FullPath);
        if (true == string.IsNullOrEmpty(strFileName))
            return;

        Type pType = GetTypeToFileName(strFileName);
        if (null == pType)
            return;

        m_dicChangedList.Add(eTableType.Json, pType);
    }
}
