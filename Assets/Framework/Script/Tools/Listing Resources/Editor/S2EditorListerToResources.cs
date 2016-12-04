/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 18일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 Resources폴더에 존재하는 파일정보를 .Json파일로 리스팅 해 줍니다.(Load시 사용됨.)

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
public class S2EditorResourcesTable : Editor 
{
    [MenuItem("S2Tools/Listing Resources", false, 1)]
    [MenuItem("Assets/S2Tools/Listing Resources", false, 1)]
    static void SelectToMenu()
    {
        S2ListerToResources pRTable = new S2ListerToResources();
        pRTable.CreateResourcesTable();
    }
}
#endif