/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 11일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 Excel파일을 SQLite파일로 변환하는 기능을 합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;

public class S2ToolsExcelToDB
{
    public void SelectExcel()
    {

        string strFilePath = EditorUtility.OpenFilePanel("Convert ExcelToDB", S2Util.Format("{0}/Excels", S2Path.GetPathToTable()), "xlsx");
        if (string.Empty == strFilePath)
            return;

        ConvertExcelToDB(strFilePath);
    }

    public void ConvertExcelToDB(string strFilePath)
    {
        string strFileName = Path.GetFileName(strFilePath);
        if (false == Path.GetExtension(strFileName).Equals(".xlsx"))
        {
            EditorUtility.DisplayDialog("[S2Tools] ExcelToDB",
                S2Util.Format("확장자가 xlsx파일만 가능합니다.!!(파일명:{0})", strFileName), "확인");
            return;
        }

        if (false == EditorUtility.DisplayDialog("[S2Tools] ExcelToDB", 
            S2Util.Format("{0} 파일을\nSQLite파일로\n컨버팅 하시겠습니까?", strFileName), "확인", "취소"))
            return;

        S2Excel pExcel = new S2Excel(strFileName);
        S2SQLite pSQLite = new S2SQLite();
        pSQLite.Write(strFileName, pExcel.GetDataSet());

        EditorUtility.DisplayDialog("[S2Tools] ExcelToDB", 
            S2Util.Format("{0} 파일\nSQLite파일로\n컨버팅 완료", strFileName), "확인");
    }
}
#endif
