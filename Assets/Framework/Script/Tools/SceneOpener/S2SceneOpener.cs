/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 14일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 씬을 쉽게 오픈시켜주는 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class S2SceneOpener : Editor
{
    static void SaveScene(string strPath)
    {
        EditorApplication.SaveScene(strPath);
    }

    static void OpenScene(string strPath)
    {
        EditorApplication.OpenScene(strPath);
    }

    static bool EqualsScene(string strPath)
    {
        if (true == EditorApplication.currentScene.Equals(strPath))
        {
            if (true == EditorUtility.DisplayDialog("Warning", "Same Scene..", "Ok"))
                return true;
        }

        return false;
    }

    static bool CheckErrorMessage()
    {
        if ((true == EditorApplication.isPlaying) ||
            (true == EditorApplication.isCompiling) ||
            (true == EditorApplication.isPaused) ||
            (true == EditorApplication.isUpdating))
        {
            if (true == EditorUtility.DisplayDialog("Error", "Unity is Busy..\n\n(Playing || Compiling || Paused || Updating..)", "Ok"))
                return true;
        }

        return false;
    }

    static void CheckSaveScene()
    {
        if (true == EditorUtility.DisplayDialog("Save Scene", "Are You Save Current Scene?", "Ok", "Cancel"))
        {
            SaveScene(EditorApplication.currentScene);
        }
    }

    static void LoadScene(string strPath)
    {
        if (true == CheckErrorMessage())
            return;

        if (true == EqualsScene(strPath))
            return;

        CheckSaveScene();
        OpenScene(strPath);
    }

    [MenuItem("S2Tools/SceneOpen/StartGame", false, 100)]
    static void LoadScene_StartGame()
    {
        LoadScene("Assets/Scene/StartGame.unity");
    }

    [MenuItem("S2Tools/SceneOpen/InGame", false, 100)]
    static void LoadScene_InGame()
    {
        LoadScene("Assets/Scene/InGame.unity");
    }
}

#endif