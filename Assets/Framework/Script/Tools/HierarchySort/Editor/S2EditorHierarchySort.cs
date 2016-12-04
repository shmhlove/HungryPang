/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 10일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 유니티 에디터의 Hierarchy내 게임오브젝트들을 알파벳순으로 정렬시켜줍니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using UnityEditor;
using System.Collections;

public class S2EditorHierarchySort : BaseHierarchySort
{
	public override int Compare(GameObject lhs, GameObject rhs)
	{
		if (lhs == rhs)     return  0;
		if (lhs == null)    return -1;
		if (rhs == null)    return  1;

		return EditorUtility.NaturalCompare(lhs.name, rhs.name);
	}
}