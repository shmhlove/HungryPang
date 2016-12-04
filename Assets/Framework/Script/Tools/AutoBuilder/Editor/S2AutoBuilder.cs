/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 13일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 유니티 메뉴에 빌드메뉴를 노출시켜 쉽게 빌드할수 있게 합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ProjectBuilder
{
    static string[] m_strScenes         = FindEnabledEditorScenes();
    static string m_strSaveDirectory    = "Build";

    //------------------------- [ Common ] ---------------------------
    private static string[] FindEnabledEditorScenes()
    {
        List<string> pEditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene pScene in EditorBuildSettings.scenes)
        {
            if (!pScene.enabled) continue;
            pEditorScenes.Add(pScene.path);
        }

        return pEditorScenes.ToArray();
    }

    static void GenericBuild(string[] strScenes, string strSavePath, BuildTarget pBuildTarget, BuildOptions pBuildOptions)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(pBuildTarget);
        string strResult = BuildPipeline.BuildPlayer(strScenes, strSavePath, pBuildTarget, pBuildOptions);
        if (strResult.Length > 0)
        {
            throw new Exception("BuildPlayer failure: " + strResult);
        }
    }

    //------------------------- [ iOS ] ---------------------------
    [MenuItem("S2Tools/CI/Build iOS", false, 100)]
    static void PerformiOSBuild()
    {
        /* 빌드 옵션을 따로 줄 경우
        BuildOptions opt = BuildOptions.SymlinkLibraries |
            BuildOptions.Development |
                BuildOptions.ConnectWithProfiler |
                BuildOptions.AllowDebugging |
                BuildOptions.Development;         
        */
        BuildOptions pOption = BuildOptions.None;

        PlayerSettings.iOS.sdkVersion       = iOSSdkVersion.DeviceSDK;
        PlayerSettings.iOS.targetOSVersion  = iOSTargetOSVersion.iOS_4_3;
        PlayerSettings.statusBarHidden      = true;

        char cSep = Path.DirectorySeparatorChar;
        string strDirectory = Path.GetFullPath(".") + cSep + m_strSaveDirectory + "/iOS";
        Directory.CreateDirectory(strDirectory);

        string strDate = string.Format("/{0}", DateTime.Now.ToString("yyyyMMdd"));
        string strPath = string.Format(strDirectory + strDate, PlayerSettings.bundleVersion);
        GenericBuild(m_strScenes, strPath, BuildTarget.iPhone, pOption);
    }

    //------------------------- [ Android ] ---------------------------
    [MenuItem("S2Tools/CI/Build Android", false, 100)]
    static void PerformAndroidBuildClient()
    {
        /* 빌드파일 Sign 으로 묶어서 빌드 할때
        PlayerSettings.Android.keystorePass = "키스토어 비번";
        PlayerSettings.Android.keyaliasPass = "키스토어 alias 이름";
        */
        BuildOptions pOption = BuildOptions.None;

        char cSep = Path.DirectorySeparatorChar;
        string strDirectory = Path.GetFullPath(".") + cSep + m_strSaveDirectory + "/Android";
        Directory.CreateDirectory(strDirectory);

        string strDate = string.Format("/{0}{1}", DateTime.Now.ToString("yyyyMMdd"), ".apk");
        string strPath = string.Format(strDirectory + strDate, PlayerSettings.bundleVersion);
        GenericBuild(m_strScenes, strPath, BuildTarget.Android, pOption);
    }
}