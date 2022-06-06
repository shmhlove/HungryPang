/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 04일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 게임 오브젝트 관련 기능들을 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public static class S2GameObject
{
    // 프리팹 복사
    public static GameObject Instantiate(Object pObject)
    {
        if (null == pObject)
            return null;

        return GameObject.Instantiate(pObject) as GameObject;
    }

    // 빈 오브젝트 생성
    public static GameObject CreateEmptyObject(string strName)
    {
        return new GameObject(strName);
    }

    // 오브젝트 제거
    public static void DestoryObject(GameObject pObject)
    {
        if (null == pObject)
            return;

        GameObject.Destroy(pObject);
    }

    // 오브젝트 찾기 및 생성
    public static GameObject GetObject(string strRoot)
    {
        GameObject pRoot = Find(strRoot);
        if (null == pRoot)
            pRoot = CreateEmptyObject(strRoot);

        return pRoot;
    }

    // 전체 오브젝트에서 찾기(이름으로)
    public static GameObject Find(string strName)
    {
        return GameObject.Find(strName);
    }

    // 전체 오브젝트에서 찾기(타입으로)
    public static T FindObjectOfType<T>() where T : MonoBehaviour
    {
        return GameObject.FindObjectOfType(typeof(T)) as T;
    }

    // 자식 오브젝트에서 찾기
    public static GameObject FindChild(GameObject pRoot, string strName)
    {
        if (null == pRoot)
            return null;

        Transform pChildren = FindChild(pRoot.transform, strName);
        if (null == pChildren)
            return null;

        return pChildren.gameObject;
    }
    // 자식 오브젝트에서 찾기
    public static Transform FindChild(Transform pRoot, string strName)
    {
        if (null == pRoot)
            return null;

        return pRoot.Find(strName);
    }

    // 하이어라키 설정
    public static GameObject SetParent(GameObject pObject, string strParent)
    {
        return SetParent(pObject, GetObject(strParent));
    }
    public static GameObject SetParent(GameObject pChild, GameObject pParent)
    {
        if (null == pParent)
            return null;

        if (null == pChild)
            return null;

        pChild.transform.SetParent(pParent.transform);
        return pParent;
    }

    // 컴포넌트 얻기
    public static T GetComponent<T>(GameObject pObject) where T : Component
    {
        if (null == pObject)
            return default(T);

        T pComponent = pObject.GetComponent<T>();
        if (null == pComponent)
            pComponent = pObject.AddComponent<T>();

        return pComponent;
    }
}