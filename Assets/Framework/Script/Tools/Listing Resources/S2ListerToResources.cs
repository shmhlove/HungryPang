/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 18일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 Resources폴더에 존재하는 파일정보를 .Json파일로 리스팅 해 줍니다.(Load시 사용됨.)

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;

// <파일명, 리소스정보>
using S2DicFile         = System.Collections.Generic.Dictionary<string, S2ResourcesTableData>;
using S2DicPairFile     = System.Collections.Generic.KeyValuePair<string, S2ResourcesTableData>;
// <씬명, <파일명, 리소스정보>>
using S2DicTable        = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, S2ResourcesTableData>>;
using S2DicPairTable    = System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, S2ResourcesTableData>>;

public class S2ListerToResources
{
    public void CreateResourcesTable()
    {
        int iFileCount      = 0;
        S2DicTable dicTable = new S2DicTable();
        S2Util.Search(S2Path.GetPathToResources(), (pFileInfo) => 
        {
            S2ResourcesTableData pInfo = MakeFileInfo(pFileInfo);
            if (null == pInfo)
                return;

            // 예외처리 : 중복체크
            string strDuplication = CheckToDuplication(dicTable, pInfo.m_strName);
            if (null != strDuplication)
            {
                EditorUtility.DisplayDialog("[S2Tools] Listing Resources",
                    S2Util.Format("중복 파일발견!! 파일명은 중복되면 안됩니다!! \r\n1번 : {0}\r\n2번 {1}",
                    strDuplication, pInfo.m_strName), "확인");
                    return;
            }
            
            if (false == dicTable.ContainsKey(pInfo.m_strSceneType))
                dicTable[pInfo.m_strSceneType] = new S2DicFile();

            dicTable[pInfo.m_strSceneType][pInfo.m_strName] = pInfo;
            iFileCount++;
        });

        WriteJson(dicTable);

        EditorUtility.DisplayDialog("[S2Tools] Listing Resources",
            S2Util.Format("리소스 리스팅 완료!!(파일수 : {0})", iFileCount), "확인");
    }
    
    S2ResourcesTableData MakeFileInfo(FileInfo pFile)
    {
        // 알리아싱
        string strRoot      = "Resources";
        string strFullName  = pFile.FullName.Substring(pFile.FullName.IndexOf(strRoot) + strRoot.Length + 1);
        string strExtension = Path.GetExtension(strFullName);
        
        // 예외처리 : 메타파일
        if (true == strExtension.Equals(".meta"))
            return null;
        
        // 경로에 확장자 제거
        string strPath = strFullName.Substring(0, strFullName.Length - strExtension.Length);
        
        // 기록
        S2ResourcesTableData pInfo = new S2ResourcesTableData();
        pInfo.m_strHash             = S2Hash.GetMD5ToFile(pFile.FullName);
        pInfo.m_strName             = Path.GetFileNameWithoutExtension(strFullName);
        pInfo.m_strExtension        = strExtension;
        pInfo.m_strPath             = strPath.Replace("\\", "/");
        pInfo.m_eResourceType       = GetResourceTypeToFileName(strFullName);
        pInfo.m_strSceneType        = strFullName.Split(new char[]{'\\'})[0];
        return pInfo;
    }
    
    string CheckToDuplication(S2DicTable dicFiles, string strCheckFile)
    {
        string strName = Path.GetFileNameWithoutExtension(strCheckFile);
        foreach (S2DicPairTable kvp1 in dicFiles)
        {
            foreach(S2DicPairFile kvp2 in kvp1.Value)
            {
                if (kvp2.Key == strName)
                    return kvp2.Value.m_strPath;
            }
        }
        return null;
    }

    eResourceType GetResourceTypeToFileName(string strFileName)
    {
        string strExt = Path.GetExtension(strFileName).ToLower();

        if (".prefab" == strExt)
            return eResourceType.Prefab;

        if ((".png" == strExt) ||
            (".tga" == strExt) ||
            (".pdf" == strExt))
            return eResourceType.Texture;

        if ((".wav" == strExt) ||
            (".ogg" == strExt) ||
            (".mp3" == strExt))
            return eResourceType.Sound;

        return eResourceType.None;
    }

    public void WriteJson(S2DicTable dicTable)
    {
        if (0 == dicTable.Count)
            return;

        string strNewLine = "\r\n";
        string strBuff = "{" + strNewLine;

        // 테이블 리스트 작성
        strBuff += S2Util.Format("\t\"{0}\": [{1}", "TableList", strNewLine);
        foreach (S2DicPairTable kvp in dicTable)
        {
            strBuff += "\t\t{" + strNewLine;
            strBuff += S2Util.Format("\t\t\t\"{0}\": \"{1}\"{2}", "s_Sheets", kvp.Key, strNewLine);
            strBuff += "\t\t}," + strNewLine;
        }
        strBuff = S2Util.Format("{0}{1}", strBuff.Substring(0, strBuff.Length - (strNewLine.Length + 1)), strNewLine);
        strBuff += S2Util.Format("\t],{0}", strNewLine);

        // 테이블별 내용작성
        foreach (S2DicPairTable kvp in dicTable)
        {
            strBuff += S2Util.Format("\t\"{0}\": [{1}", kvp.Key, strNewLine);
            foreach (S2DicPairFile pData in kvp.Value)
            {
                strBuff += "\t\t{" + strNewLine;

                strBuff += S2Util.Format("\t\t\t\"s_Name\": \"{0}\",{1}",
                    pData.Value.m_strName,
                    strNewLine);

                strBuff += S2Util.Format("\t\t\t\"s_Hash\": \"{0}\",{1}",
                    pData.Value.m_strHash,
                    strNewLine);

                strBuff += S2Util.Format("\t\t\t\"s_Extension\": \"{0}\",{1}",
                    pData.Value.m_strExtension,
                    strNewLine);

                strBuff += S2Util.Format("\t\t\t\"s_Path\": \"{0}\",{1}",
                    pData.Value.m_strPath,
                    strNewLine);

                strBuff += S2Util.Format("\t\t\t\"s_ResourceType\": \"{0}\",{1}",
                    pData.Value.m_eResourceType.ToString(),
                    strNewLine);

                strBuff += S2Util.Format("\t\t\t\"s_SceneType\": \"{0}\"{1}",
                    pData.Value.m_strSceneType,
                    strNewLine);

                strBuff += "\t\t}," + strNewLine;
            }
            strBuff = S2Util.Format("{0}{1}", strBuff.Substring(0, strBuff.Length - (strNewLine.Length + 1)), strNewLine);
            strBuff += S2Util.Format("\t],{0}", strNewLine);
        }
        strBuff = S2Util.Format("{0}{1}", strBuff.Substring(0, strBuff.Length - (strNewLine.Length + 1)), strNewLine);
        strBuff += "}";

        WriteBuff(strBuff, S2Util.Format("{0}/JSons/{1}", S2Path.GetPathToStreamingAssets(), "ResourcesTable.json"));
    }

    void WriteBuff(string strBuff, string strFilePath)
    {
        FileStream pFile        = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);
        StreamWriter pStream    = new StreamWriter(pFile);

        pStream.WriteLine(strBuff);
        pStream.Close();
        pFile.Close();
    }
}
#endif