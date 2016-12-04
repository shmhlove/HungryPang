/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 Json 파일을 로드하는 래퍼 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Data;

using System.IO;
using System.Threading;

using SimpleJSON;

public class S2Json
{
    private JSONNode m_pJsonNode = null;
    public JSONNode Node { get { return m_pJsonNode; } }

    public S2Json() { }
    public S2Json(string strFileName)
    {
        // 예외처리 : .json로 확장자 고정
        strFileName = Path.GetFileNameWithoutExtension(strFileName);
        strFileName = S2Util.Format("{0}.json", strFileName);

        // Streaming에서 .Json를 string형태로 읽어서 JSONNode로 저장
        string strReadPath = string.Empty;
#if UNITY_EDITOR || UNITY_STANDALONE
        strReadPath = S2Util.Format("{0}{1}", "file://", S2Path.GetPathToStreamingAssets());
#elif UNITY_ANDROID
        strReadPath = S2Util.Format("{0}{1}{2}", "jar:file://", S2Path.GetPathToAssets(), "!/assets");
#elif UNITY_IOS
        strReadPath = S2Util.Format("{0}{1}{2}", "file://", S2Path.GetPathToAssets(), "/Raw");
#endif
        string strReadFilePath  = S2Util.Format("{0}/JSons/{1}", strReadPath, strFileName);

        // Node얻기
        m_pJsonNode = GetNodeToJsonFile(strReadFilePath);
    }

    ~S2Json()
    {
        m_pJsonNode = null;
    }

    JSONNode GetNodeToJsonFile(string strFilePath)
    {
        byte[] pByte = null;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_ANDROID
        {
            WWW pWWW = new WWW(strFilePath);
            Download(pWWW);
            while (false == pWWW.isDone) ;

            if (true != string.IsNullOrEmpty(pWWW.error))
            {
                S2Util.LogError("Json(*.json)파일을 읽는 중 오류발생!!(Path:{0}, Error:{1})", strFilePath, pWWW.error);
                return null;
            }
            
            pByte = pWWW.bytes;
        }
#elif UNITY_IPHONE
        try
        {
            if (false == File.Exists(strFilePath))
            {
                S2Util.LogError("Json(*.json)파일을 읽는 중 오류발생!!(Path:{0}, Error:{1})", strFilePath, "파일이 없습니다!!");
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
        //System.Text.ASCIIEncoding pEncoder = new System.Text.ASCIIEncoding();
        System.Text.UTF8Encoding pEncoder = new System.Text.UTF8Encoding();
        return JSON.Parse(pEncoder.GetString(pByte));
    }

    IEnumerator Download(WWW www)
    {
        yield return www;
    }

    public bool CheckJson()
    {
        return (null != m_pJsonNode);
    }

#if UNITY_EDITOR_WIN
    public void Write(string strFileName, Dictionary<string, List<S2DataSet>> dicData)
    {
        string strNewLine = "\r\n";
        string strBuff = "{" + strNewLine;
        foreach (KeyValuePair<string, List<S2DataSet>> kvp in dicData)
        {
            strBuff += S2Util.Format("\t\"{0}\": [{1}", kvp.Key, strNewLine);
            foreach (S2DataSet pData in kvp.Value)
            {
                strBuff += "\t\t{" + strNewLine;
                for (int iCol = 0; iCol < pData.m_iMaxCol; ++iCol)
                {
                    strBuff += S2Util.Format("\t\t\t\"{0}\": {1},{2}", 
                        pData.m_ColumnNames[iCol],
                        pData.m_pDatas[iCol],
                        strNewLine);
                }
                strBuff = S2Util.Format("{0}{1}", strBuff.Substring(0, strBuff.Length - (strNewLine.Length + 1)), strNewLine);
                strBuff += "\t\t}," + strNewLine;
            }
            strBuff = S2Util.Format("{0}{1}", strBuff.Substring(0, strBuff.Length - (strNewLine.Length + 1)), strNewLine);
            strBuff += S2Util.Format("\t],{0}", strNewLine);
        }
        strBuff = S2Util.Format("{0}{1}", strBuff.Substring(0, strBuff.Length - (strNewLine.Length + 1)), strNewLine);
        strBuff += "}";

        string strSavePath = S2Path.GetPathToStreamingAssets();
        string strSaveFilePath = S2Util.Format("{0}/JSons/{1}.json", strSavePath, Path.GetFileNameWithoutExtension(strFileName));

        FileStream pFile        = new FileStream(strSaveFilePath, FileMode.Create, FileAccess.Write);
        StreamWriter pStream    = new StreamWriter(pFile);

        pStream.WriteLine(strBuff);
        pStream.Close();
        pFile.Close();
    }
#endif
}