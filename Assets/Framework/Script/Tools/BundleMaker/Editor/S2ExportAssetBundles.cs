using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;

public class S2ExportAssetBundles
{
    //private static string m_szDestFolderPath = "Assets/StreamingAssets/AssetBundles/";
    private static string m_szDestFolderPath = "Assets/Resources AssetBundle/";

    /// <summary>
    /// Excel로 데이터 파일 로드로 바꿔야 함.
    /// (에셋번들 제작 데이터를 파일로 관리)
    /// </summary>
    const string m_szPathPrefix = "Assets/Resources/";
    private static string[,] m_szBundleStr = new string[,]
    {
        {"Assets/Resources/InGame",                        "InGame"},
    };

    private static int m_iListNum = m_szBundleStr.Length / 2;

    static string GetAssetBundleName(string szAssetName)
    {
        string szTempAssetName = szAssetName.Replace("\\", "/");
        int iIndex = szTempAssetName.LastIndexOf("/");

        string szResourceName = "";
        if (iIndex < 0)
            szResourceName = szTempAssetName;
        else
            szResourceName = szTempAssetName.Remove(0, iIndex + 1);

        return szResourceName.Replace(".prefab", "");
    }

    static string GetSceneBundleName(string szAssetName)
    {
        string szTempAssetName = szAssetName.Replace("\\", "/");
        int iIndex = szTempAssetName.LastIndexOf("/");

        string szResourceName = "";
        if (iIndex < 0)
            szResourceName = szTempAssetName;
        else
            szResourceName = szTempAssetName.Remove(0, iIndex + 1);

        return szResourceName.Replace(".unity", "");
    }

    // 새 버전을 빌드 하기 전에 이전 버전 번들 파일들을 삭제한다
    private static void DeleteOldAssetBundles()
    {
        //string szPath = Application.streamingAssetsPath + "/AssetBundles/";
        string szPath = Application.dataPath + "/Resources AssetBundle/";

        DirectoryInfo dicDir = new DirectoryInfo(szPath);
        if (dicDir.Exists == false)
            return;

        FileInfo[] cFiles = dicDir.GetFiles("*.*", SearchOption.AllDirectories);

        foreach (FileInfo cFile in cFiles)
        {
            cFile.Attributes = FileAttributes.Normal;
        }
        Directory.Delete(szPath, true);
    }

    static void MakeSceneBundle(string szTarget, BuildTarget eBuildTarget)
    {
        DeleteOldAssetBundles();
        // Make Scene Asset Bundles
        string szScenePath = "Assets/Scene/";
        string[] szSceneFiles = Directory.GetFiles(szScenePath, "*.unity", SearchOption.AllDirectories);

        int iCount = 0;

        foreach (string szS in szSceneFiles)
        {
            string[] strLevel = new string[1];
            strLevel[0] = szS;
            string szName = GetSceneBundleName(szS);
            Debug.Log(szName);
            string szDestPath = m_szDestFolderPath + szTarget + "/Scene/" + szName + ".unity3d";
            Debug.Log(szDestPath);

            if (!Directory.Exists(m_szDestFolderPath + szTarget + "/Scene/"))
            {
                Directory.CreateDirectory(m_szDestFolderPath + szTarget + "/Scene/");
            }

            BuildPipeline.BuildStreamedSceneAssetBundle(strLevel, szDestPath, eBuildTarget);

            iCount += 1;
            if (iCount == 5)
            {
                if (!UnityEditor.EditorUtility.DisplayDialog("Build Asset Bundle Scene", "Continue Build Scene??", "Continue", "Stop"))
                    break;

                iCount = 0;
            }
        }
    }

    static void MakeAssetBundle(string szTarget, BuildTarget eBuildTarget)
    {
        for (int i = 0; i < m_iListNum; i++)
        {
            if (!UnityEditor.EditorUtility.DisplayDialog("Build Asset Bundle", m_szBundleStr[i, 0], "Start", "Stop"))
                continue;

            string szAssetPath = m_szBundleStr[i, 0];
            string[] szStrObj = Directory.GetFiles(szAssetPath, "*.prefab", SearchOption.AllDirectories);

            string[] strTempDirectory = Directory.GetDirectories(szAssetPath, "*", SearchOption.AllDirectories);
            foreach (string dir in strTempDirectory)
            {
                string strTemp = dir.Replace("\\", "/");

                Debug.Log("Sub Dir : " + strTemp);
            }


            foreach (string szO in szStrObj)
            {
                string szName = GetAssetBundleName(szO);
                Debug.Log(szName);
                string szPath = m_szDestFolderPath + szTarget + m_szBundleStr[i, 1] + szName + ".unity3d";
                string szResPath = m_szBundleStr[i, 0] + szName;
                szResPath = szResPath.Replace("Assets/Resources/", "");
                Debug.Log(szResPath);

                if (!Directory.Exists(m_szDestFolderPath + szTarget + m_szBundleStr[i, 1]))
                {
                    Directory.CreateDirectory(m_szDestFolderPath + szTarget + m_szBundleStr[i, 1]);
                }

                if (szPath.Length != 0)
                {
                    Object[] cObj = new Object[1];
                    cObj[0] = Resources.Load(szResPath, typeof(Object));

                    // Build the resource file from the active selection.
                    BuildPipeline.BuildAssetBundle(cObj[0], cObj, szPath,
                        BuildAssetBundleOptions.CollectDependencies
                        | BuildAssetBundleOptions.CompleteAssets
                        | BuildAssetBundleOptions.DeterministicAssetBundle,
                        eBuildTarget);
                }
            }
        }
    }

    static void MakeAssetBundleOne(BuildTarget eBuildTarget)
    {
        // Bring up save panel
        Object[] cSelection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        string szName = cSelection.GetValue(0).ToString();
        szName = szName.Replace(" (UnityEngine.GameObject)", "");
        string szPath = EditorUtility.SaveFilePanel("Save Resource", "Assets/Resources AssetBundle", szName, "unity3d");
        if (szPath.Length != 0)
        {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, cSelection, szPath,
                BuildAssetBundleOptions.CollectDependencies
                | BuildAssetBundleOptions.CompleteAssets
                | BuildAssetBundleOptions.DeterministicAssetBundle,
                eBuildTarget);
            Selection.objects = cSelection;
        }
    }

    public static void AutoMakeAssetBundle(BuildTarget _eBuildTarget, int _iPatchVersion)
    {
        string strDeviceName = string.Empty;
        switch (_eBuildTarget)
        {
            case BuildTarget.StandaloneWindows:
                strDeviceName = "PC";
                break;

            case BuildTarget.Android:
                strDeviceName = "AOS";
                break;

            case BuildTarget.iPhone:
                strDeviceName = "IOS";
                break;
        }

        // Resources AssetBundle 폴더 없으면 생성.
        string strTempTargetFolder1 = string.Format("Assets/{0}", "Resources AssetBundle");
        Object oTempTargetFolder1 = AssetDatabase.LoadAssetAtPath(strTempTargetFolder1, typeof(Object));
        if (null == oTempTargetFolder1)
        {
            AssetDatabase.CreateFolder("Assets", "Resources AssetBundle");
        }

        // Device 폴더(PC/AOS/IOS) 없으면 생성.
        string strTempTargetFolder2 = string.Format("{0}/{1}", strTempTargetFolder1, strDeviceName);
        Object oTempTargetFolder2 = AssetDatabase.LoadAssetAtPath(strTempTargetFolder2, typeof(Object));
        if (null == oTempTargetFolder2)
        {
            AssetDatabase.CreateFolder(strTempTargetFolder1, strDeviceName);
        }

        // 패치버전 폴더 없으면 생성.
        string strTempTargetFolder3 = string.Format("{0}/{1}", strTempTargetFolder2, _iPatchVersion);
        Object oTempTargetFolder3 = AssetDatabase.LoadAssetAtPath(strTempTargetFolder3, typeof(Object));
        if (null == oTempTargetFolder3)
        {
            AssetDatabase.CreateFolder(strTempTargetFolder2, _iPatchVersion.ToString());
        }

        // 패치버전 ServerVersion.txt에 저장.
        string strServerVersion = string.Format("{0}/{1}.txt", strTempTargetFolder1, "ServerVersion");
        File.WriteAllText(strServerVersion, _iPatchVersion.ToString());

        for (int i = 0; i < m_iListNum; i++)
        {
            Object oTargetFolder = AssetDatabase.LoadAssetAtPath(m_szBundleStr[i, 0], typeof(Object));
            //            Debug.Log("TargetFolder : " + oTargetFolder);

            Selection.activeObject = oTargetFolder;

            // Bring up save panel
            Object[] arrSelections = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            //             string szName = cSelection.GetValue(0).ToString();
            //             szName = szName.Replace(" (UnityEngine.GameObject)", "");
            string strName = m_szBundleStr[i, 1];

            //             string szPath = EditorUtility.SaveFilePanel("Save Resource", "Assets/AssetBundles", szName, "unity3d");
            //             Debug.Log("szPath : " + szPath);

            string strPath = string.Format("{0}/{1}/{2}/{3}/{4}.unity3d", Application.dataPath, "Resources AssetBundle", strDeviceName, _iPatchVersion, strName);

            Debug.Log("szPath : " + strPath);

            if (false == string.IsNullOrEmpty(strPath))
            {
                BuildAssetBundleEx(_eBuildTarget, Selection.activeObject, arrSelections, strPath);

                //Selection.objects = arrSelections;
            }

            Selection.activeObject = oTempTargetFolder3;
        }
    }

    //[MenuItem("S2Tools/AssetBundle/Auto Create Scene Bundle - For PC")]
    //static void TestCreateSceneBundle()
    //{
    //    // Make Scene Asset Bundles
    //    MakeSceneBundle("PC", BuildTarget.StandaloneWindows);
    //}

    //[MenuItem("S2Tools/AssetBundle/Auto Create Scene Bundle - For Android")]
    //static void TestCreateSceneBundleAndroid()
    //{
    //    // Make Scene Asset Bundles
    //    MakeSceneBundle("AOS", BuildTarget.Android);
    //}

    //[MenuItem("S2Tools/AssetBundle/Auto Create Scene Bundle - For iPhone")]
    //static void TestCreateSceneBundleiPhone()
    //{
    //    // Make Scene Asset Bundles
    //    MakeSceneBundle("IOS", BuildTarget.iPhone);
    //}

    //[MenuItem("S2Tools/AssetBundle/Auto Create All Asset Bundles - For PC")]
    //static void AutoCreateAssetBundles()
    //{
    //    // Make Asset Bundles
    //    MakeAssetBundle("PC", BuildTarget.StandaloneWindows);
    //}

    //[MenuItem("S2Tools/AssetBundle/Auto Create All Asset Bundles - For Android")]
    //static void AutoCreateAssetBundlesForAndroid()
    //{
    //    // Make Asset Bundles
    //    MakeAssetBundle("AOS", BuildTarget.Android);
    //}

    //[MenuItem("S2Tools/AssetBundle/Auto Create All Asset Bundles - For iPhone")]
    //static void AutoCreateAssetBundlesForiPhone()
    //{
    //    // Make Asset Bundles
    //    MakeAssetBundle("IOS", BuildTarget.iPhone);
    //}

    [MenuItem("S2Tools/AssetBundle/Create One AssetBundle From Just One Selection - For PC")]
    static void CreateAssetBundleOneResource()
    {
        // Make One Asset Bundles
        MakeAssetBundleOne(BuildTarget.StandaloneWindows);
    }

    [MenuItem("S2Tools/AssetBundle/Create One AssetBundle From Just One Selection - For Android")]
    static void CreateAssetBundleOneResourceAndroid()
    {
        // Make One Asset Bundles
        MakeAssetBundleOne(BuildTarget.Android);
    }

    [MenuItem("S2Tools/AssetBundle/Create One AssetBundle From Just One Selection - For iPhone")]
    static void CreateAssetBundleOneResourceiPhone()
    {
        // Make One Asset Bundles
        MakeAssetBundleOne(BuildTarget.iPhone);
    }

    public static uint BuildAssetBundleEx(BuildTarget _eBuildTarget, UnityEngine.Object parent, UnityEngine.Object[] sub, string outputPath)
    {
        uint crc = 0;
        // 활성화된 선택으로 부터 리소스 파일을 빌드합니다.
        if (parent != null && sub != null && sub.Length > 0)
        {
            BuildPipeline.BuildAssetBundle
                (parent, sub, outputPath, out crc,
                 BuildAssetBundleOptions.CollectDependencies
                 | BuildAssetBundleOptions.CompleteAssets
                 | BuildAssetBundleOptions.DeterministicAssetBundle,
                 _eBuildTarget);
        }
        else if (sub == null && parent != null)
        {
            BuildPipeline.BuildAssetBundle
                (parent, null, outputPath, out crc,
                 BuildAssetBundleOptions.CollectDependencies
                 | BuildAssetBundleOptions.CompleteAssets
                 | BuildAssetBundleOptions.DeterministicAssetBundle,
                 _eBuildTarget);
        }
        else if (sub != null && parent == null)
        {
            BuildPipeline.BuildAssetBundle
                (null, sub, outputPath, out crc,
                 BuildAssetBundleOptions.CollectDependencies
                 | BuildAssetBundleOptions.CompleteAssets
                 | BuildAssetBundleOptions.DeterministicAssetBundle,
                 _eBuildTarget);
        }

        return crc;
    }

    public static void AutoMakeAssetBundleAsName(BuildTarget _eBuildTarget, int _iPatchVersion)
    {
        string strDeviceName = string.Empty;
        switch (_eBuildTarget)
        {
            case BuildTarget.StandaloneWindows:
                strDeviceName = "PC";
                break;

            case BuildTarget.Android:
                strDeviceName = "AOS";
                break;

            case BuildTarget.iPhone:
                strDeviceName = "IOS";
                break;
        }

        // Resources AssetBundle 폴더 없으면 생성.
        string strTempTargetFolder1 = string.Format("Assets/{0}", "Resources AssetBundle");
        Object oTempTargetFolder1 = AssetDatabase.LoadAssetAtPath(strTempTargetFolder1, typeof(Object));
        if (null == oTempTargetFolder1)
        {
            AssetDatabase.CreateFolder("Assets", "Resources AssetBundle");
        }

        // Device 폴더(PC/AOS/IOS) 없으면 생성.
        string strTempTargetFolder2 = string.Format("{0}/{1}", strTempTargetFolder1, strDeviceName);
        Object oTempTargetFolder2 = AssetDatabase.LoadAssetAtPath(strTempTargetFolder2, typeof(Object));
        if (null == oTempTargetFolder2)
        {
            AssetDatabase.CreateFolder(strTempTargetFolder1, strDeviceName);
        }

        // 패치버전 폴더 없으면 생성.
        string strTempTargetFolder3 = string.Format("{0}/{1}", strTempTargetFolder2, _iPatchVersion);
        Object oTempTargetFolder3 = AssetDatabase.LoadAssetAtPath(strTempTargetFolder3, typeof(Object));
        if (null == oTempTargetFolder3)
        {
            AssetDatabase.CreateFolder(strTempTargetFolder2, _iPatchVersion.ToString());
        }
        // 패치버전 ServerVersion.txt에 저장.
        string strServerVersion = string.Format("{0}/{1}.txt", strTempTargetFolder2, "ServerVersion");
        File.WriteAllText(strServerVersion, _iPatchVersion.ToString());

        string strPatchList = string.Format("{0}/{1}.json", strTempTargetFolder2, "PatchList");
        if (File.Exists(strPatchList) == false)
        {
            using (var sw = new StreamWriter(File.Create(strPatchList)))
            {
            }
        }

        //load json
        string jsonText = System.IO.File.ReadAllText(strPatchList);
        JsonReader reader = new JsonReader(jsonText);
        JsonData jsonFileList = JsonMapper.ToObject(reader);

        for (int i = 0; i < m_iListNum; i++)
        {
            if (File.Exists(m_szBundleStr[i, 0]))
            {
                UpdateFileOneByOne(_eBuildTarget, _iPatchVersion, m_szBundleStr[i, 0], strTempTargetFolder3, ref jsonFileList);
            }
            else if (Directory.Exists(m_szBundleStr[i, 0]))
            {
                UpdateDirectory(_eBuildTarget, _iPatchVersion, m_szBundleStr[i, 0], strTempTargetFolder3, ref jsonFileList);
            }
        }

        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        JsonWriter writer = new JsonWriter(builder);
        writer.PrettyPrint = true;
        writer.IndentValue = 2;
        JsonMapper.ToJson(jsonFileList, writer);
        System.IO.File.WriteAllText(strPatchList, builder.ToString());

    }

    public static void UpdateDirectory(BuildTarget _eBuildTarget, int _iPatchVersion, string targetDirectory, string outputPath, ref JsonData jsonFileList)
    {
        string outputDirectory = CreateDirectoryRecursive(targetDirectory, outputPath);

        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
        {
            UpdateFileOneByOne(_eBuildTarget, _iPatchVersion, fileName, outputDirectory, ref jsonFileList);
        }

        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            UpdateDirectory(_eBuildTarget, _iPatchVersion, subdirectory, outputPath, ref jsonFileList);
        }
    }

    static void UpdateFileOneByOne(BuildTarget _eBuildTarget, int _iPatchVersion, string path, string outputDirectory, ref JsonData jsonFileList)
    {
        if (Path.GetExtension(path) == ".meta")
            return;
        if (Path.GetExtension(path) == ".DS_Store")
            return;
        if (Path.GetFileName(path) == "UI Root (2D) - CreateTeam.prefab")
            return;
        if (Path.GetFileName(path) == "UI Root (2D) - Loading.prefab")
            return;
        if (Path.GetFileName(path) == "UI Root (2D) - Login.prefab")
            return;
        if (Path.GetFileName(path) == "UI Root (2D) - OutGame.prefab")
            return;
        if (Path.GetFileName(path) == "UI Root (2D) - Patch.prefab")
            return;


        string key = Path.GetFileNameWithoutExtension(path);

        string subPath = path.Substring(path.LastIndexOf(m_szPathPrefix) + m_szPathPrefix.Length, path.Length - m_szPathPrefix.Length);

        JsonData data = new JsonData();
        data["Path"] = subPath;
        data["Version"] = _iPatchVersion;
        data["GUID"] = AssetDatabase.AssetPathToGUID(path);
        data["Clean"] = false;
        data["Removed"] = false;

        jsonFileList[key] = data;
        data["CRC"] = ProcessFileOneByOne(_eBuildTarget, path, outputDirectory);

        return;
        /*
        if(JsonDataContainsKey(jsonFileList,key))
        {
            if(jsonFileList[key]["GUID"].ToString() != AssetDatabase.AssetPathToGUID(path))
            {
                jsonFileList[key] = data;
                data["CRC"] = ProcessFileOneByOne(_eBuildTarget, path, outputDirectory);
            }
        }else
        {
            jsonFileList[key] = data;
            data["CRC"] = ProcessFileOneByOne(_eBuildTarget, path, outputDirectory);
        }
        */
    }

    public static string CreateDirectoryRecursive(string sourceDirectory, string outputDirectory)
    {
        string subPath = sourceDirectory.Substring(sourceDirectory.LastIndexOf(m_szPathPrefix) + m_szPathPrefix.Length, sourceDirectory.Length - m_szPathPrefix.Length);

        string output = outputDirectory + "/" + subPath;
        System.IO.Directory.CreateDirectory(output);
        return output;
    }

    // Insert logic for processing found files here. 
    //	public static void ProcessFileOneByOne(BuildTarget _eBuildTarget, string path, string outputPath) 
    static uint ProcessFileOneByOne(BuildTarget _eBuildTarget, string path, string outputPath)
    {
        Object t = AssetDatabase.LoadMainAssetAtPath(path);
        outputPath += "/" + Path.GetFileNameWithoutExtension(path) + ".unity3d";

        return BuildAssetBundleEx(_eBuildTarget, t, null, outputPath);
    }

    //check for has key (LitJson)
    static public bool JsonDataContainsKey(JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
        {
            return result;
        }
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key))
        {
            result = true;
        }
        return result;
    }
}
