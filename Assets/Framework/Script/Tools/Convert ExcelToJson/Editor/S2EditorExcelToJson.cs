/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 23일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 Excel파일을 Json파일로 변환하는 기능을 합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
public class S2EditorExcelToJson : Editor 
{
    [MenuItem("S2Tools/Convert Excel To Json", false, 0)]
    static void SelectToMenu()
    {
        S2ToolsExcelToJson pTools = new S2ToolsExcelToJson();
        pTools.SelectExcel();
    }
    
    [MenuItem("Assets/S2Tools/Convert Excel To Json", false, 0)]
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
            S2ToolsExcelToJson pTools = new S2ToolsExcelToJson();
            foreach (Object pObject in pSelects)
            {
                pTools.ConvertExcelToJson(
                    S2Util.Format("{0}{1}", S2Path.GetPathToAssets(),
                        AssetDatabase.GetAssetOrScenePath(pObject).Substring(6)));
            }
        }
    }
}
#endif