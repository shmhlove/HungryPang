/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 11일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 Excel파일을 SQLite파일로 변환하는 기능을 합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
public class S2EditorExcelToDB : Editor 
{
    [MenuItem("S2Tools/Convert Excel To DB", false, 0)]
    static void SelectToMenu()
    {
        S2ToolsExcelToDB pTools = new S2ToolsExcelToDB();
        pTools.SelectExcel();
    }

    [MenuItem("Assets/S2Tools/Convert Excel To DB", false, 0)]
    static void SelectToAssetsMenu()
    {
        Object[] pSelects = Selection.objects;
        if (0 == pSelects.Length)
        {
            SelectToMenu();
            return;
        }
        else
        {
            S2ToolsExcelToDB pTools = new S2ToolsExcelToDB();
            foreach (Object pObject in pSelects)
            {
                pTools.ConvertExcelToDB(
                    S2Util.Format("{0}{1}", S2Path.GetPathToAssets(),
                        AssetDatabase.GetAssetOrScenePath(pObject).Substring(6)));
            }
        }
    }
}
#endif