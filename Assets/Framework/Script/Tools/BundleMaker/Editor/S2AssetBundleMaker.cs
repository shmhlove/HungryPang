using UnityEngine;
using UnityEditor;

using System;

public class S2AssetBundleMaker : EditorWindow
{
    private static EditorWindow m_editorWindow;

    int selGridInt = 0;
    string[] selStrings = { "ALL", "AOS", "IOS", "DESKTOP" };

    int selOptionGrid = 0;
    string[] selOptionStrings = { "Default", "by NAME"/*, "by SIZE"*/};

    private string m_strQuaterViewerName;

    private Camera m_camQuaterViewer;
    private RenderTexture m_RenderTexture;
    //private bool m_bIsPlayed = false;

    private float m_fZoomSpeed = 0.5f;

    private int m_iDefaultPatchVersion = 1401010101;    // 14/01/01/01/01(년/월/일/시/분)
    private int m_iPatchVersion = 1401010101;           // 1/01/001

    //     private int m_iPatchVersionMajor = 1;           // 10 * 10 * 100 단위
    //     private int m_iPatchVersionMinor = 0;           // 10 * 100 단위
    //     private int m_iPatchVersionBuild = 0;           // 100 단위

    //     private int m_iDefaultPatchVersion = 100000;    // 1/01/001
    //     private int m_iPatchVersion = 101001;           // 1/01/001
    //     private int m_iPatchVersionMajor = 1;           // 10 * 10 * 100 단위
    //     private int m_iPatchVersionMinor = 0;           // 10 * 100 단위
    //     private int m_iPatchVersionBuild = 0;           // 100 단위
    private bool m_bIsReadServerVersionFile = false;

    //     string myString = "Hello World";
    //     bool groupEnabled;
    //     bool myBool = true;
    //     float myFloat = 1.23f;

    [MenuItem("S2Tools/AssetBundle/S2AssetBundleMaker")]
    static void Init()
    {
        if (null != m_editorWindow)
        {
            m_editorWindow.ShowUtility();
            return;
        }

        m_editorWindow = EditorWindow.GetWindow(typeof(S2AssetBundleMaker));
        m_editorWindow.autoRepaintOnSceneChange = true;
        m_editorWindow.ShowUtility();
    }

    //     private void GetServerVersionFromFile()
    //     {
    //         string strServerVersion = string.Format("Assets/{0}/{1}.txt", "Resources AssetBundle", "ServerVersion");
    //         string strTemp = System.IO.File.ReadAllText(strServerVersion);
    //         
    //         m_iPatchVersion = int.Parse(strTemp);
    //         m_iPatchVersionMajor = m_iPatchVersion / 100000;
    //         m_iPatchVersionMinor = (m_iPatchVersion % 100000) / 1000;
    //         m_iPatchVersionBuild = (m_iPatchVersion % 100000) % 1000;
    //     }

    private bool ComputePatchVersion()
    {
        //         m_iPatchVersion = (m_iPatchVersionMajor * 100000)
        //                             + (m_iPatchVersionMinor * 1000)
        //                             + (m_iPatchVersionBuild);

        if (m_iPatchVersion < m_iDefaultPatchVersion)
            return false;

        return true;
    }

    private void CumputePatchVersionFromTime()
    {
        m_iPatchVersion = 0;

        Debug.Log("DateTime.Now : " + DateTime.Now);
        m_iPatchVersion = (DateTime.Now.Year % 100) * 100000000;
        Debug.Log("DateTime.Now.Year : " + DateTime.Now.Year % 100);
        m_iPatchVersion += DateTime.Now.Month * 1000000;
        Debug.Log("DateTime.Now.Month : " + DateTime.Now.Month);
        m_iPatchVersion += DateTime.Now.Day * 10000;
        Debug.Log("DateTime.Now.Day : " + DateTime.Now.Day);
        m_iPatchVersion += DateTime.Now.Hour * 100;
        Debug.Log("DateTime.Now.Hour : " + DateTime.Now.Hour);
        m_iPatchVersion += DateTime.Now.Minute;
        Debug.Log("DateTime.Now.Minute : " + DateTime.Now.Minute);

        Debug.Log("m_iPatchVersion : " + m_iPatchVersion);

    }

    /// <summary>
    /// 1. 리소스 파일 중복 체크(중복 리스트 Path 포함 출력)
    /// 2. 에셋번들 자동 묶음(PC/AOS/IOS 모두..? 아니면 버튼 분리? 한방에 하는게 나을지도..)
    /// 3. 
    /// </summary>
    void OnGUI()
    {
        //         if (GUILayout.Button("Check DateTime"))
        //         {
        //             CumputePatchVersionFromTime();
        //         }

        if (false == m_bIsReadServerVersionFile)
        {
            //GetServerVersionFromFile();
            CumputePatchVersionFromTime();
            m_bIsReadServerVersionFile = true;
        }

        GUILayout.Label("Auto Make AssetBundle", EditorStyles.boldLabel);
        //m_iPatchVersion = EditorGUILayout.IntField("Patch Version", m_iPatchVersion);
        EditorGUILayout.HelpBox("이번 패치에 적용될 버전", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Patch Version : ");
        EditorGUILayout.LabelField(m_iPatchVersion.ToString());
        EditorGUILayout.EndHorizontal();

        //         m_iPatchVersionMajor = EditorGUILayout.IntField("Major", m_iPatchVersionMajor);
        //         m_iPatchVersionMinor = EditorGUILayout.IntField("Minor", m_iPatchVersionMinor);
        //         m_iPatchVersionBuild = EditorGUILayout.IntField("Build", m_iPatchVersionBuild);
        //         
        //EditorGUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("Box");
        GUILayout.BeginVertical("Box");
        GUILayout.Label("Platform option", EditorStyles.boldLabel);
        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1);
        GUILayout.EndVertical();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Output option", EditorStyles.boldLabel);
        selOptionGrid = GUILayout.SelectionGrid(selOptionGrid, selOptionStrings, 1);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (GUILayout.Button("Auto Make AssetBundles"))
        {
            if (ComputePatchVersion() == true)
            {
                switch (selGridInt)
                {
                    case 0:
                        {
                            if (selOptionGrid == 0)
                            {
                                S2ExportAssetBundles.AutoMakeAssetBundle(BuildTarget.StandaloneWindows, m_iPatchVersion);
                                S2ExportAssetBundles.AutoMakeAssetBundle(BuildTarget.iPhone, m_iPatchVersion);
                                S2ExportAssetBundles.AutoMakeAssetBundle(BuildTarget.Android, m_iPatchVersion);
                            }
                            else if (selOptionGrid == 1)
                            {
                                S2ExportAssetBundles.AutoMakeAssetBundleAsName(BuildTarget.StandaloneWindows, m_iPatchVersion);
                                S2ExportAssetBundles.AutoMakeAssetBundleAsName(BuildTarget.iPhone, m_iPatchVersion);
                                S2ExportAssetBundles.AutoMakeAssetBundleAsName(BuildTarget.Android, m_iPatchVersion);
                            }
                        }
                        break;
                    case 1:
                        {
                            if (selOptionGrid == 0)
                            {
                                S2ExportAssetBundles.AutoMakeAssetBundle(BuildTarget.Android, m_iPatchVersion);
                            }
                            else if (selOptionGrid == 1)
                            {
                                S2ExportAssetBundles.AutoMakeAssetBundleAsName(BuildTarget.Android, m_iPatchVersion);
                            }
                        }
                        break;
                    case 2:
                        {
                            if (selOptionGrid == 0)
                            {
                                S2ExportAssetBundles.AutoMakeAssetBundle(BuildTarget.iPhone, m_iPatchVersion);
                            }
                            else if (selOptionGrid == 1)
                            {
                                S2ExportAssetBundles.AutoMakeAssetBundleAsName(BuildTarget.iPhone, m_iPatchVersion);
                            }
                        }
                        break;
                    case 3:
                        {
                            if (selOptionGrid == 0)
                            {
                                S2ExportAssetBundles.AutoMakeAssetBundle(BuildTarget.StandaloneWindows, m_iPatchVersion);
                            }
                            else if (selOptionGrid == 1)
                            {
                                S2ExportAssetBundles.AutoMakeAssetBundleAsName(BuildTarget.StandaloneWindows, m_iPatchVersion);
                            }
                        }
                        break;
                }
            }
        }

        //GUI.enabled = true;

        //         GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        //         myString = EditorGUILayout.TextField("Text Field", myString);

        // 
        //         groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        //         myBool = EditorGUILayout.Toggle("Toggle", myBool);
        //         myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        //         EditorGUILayout.EndToggleGroup();

    }

    void CheckDuplicatedResources()
    {

    }

    public void Awake()
    {
        //Initialize();
    }

    private void Initialize()
    {
        m_RenderTexture = new RenderTexture((int)position.width,
                    (int)position.height,
                    (int)RenderTextureFormat.Depth);

        m_strQuaterViewerName = "Camera - QuarterViewer";
        if (GameObject.Find(m_strQuaterViewerName))
        {
            m_camQuaterViewer = GameObject.Find(m_strQuaterViewerName).GetComponent<Camera>();
            if (null == m_camQuaterViewer)
            {
                Debug.Log("Can't Find 'Camera - QuarterView'");
            }
        }
    }

    public void Update()
    {
        if (null != m_camQuaterViewer && null != m_RenderTexture)
        {
            m_camQuaterViewer.targetTexture = m_RenderTexture;
            m_camQuaterViewer.Render();
            m_camQuaterViewer.targetTexture = null;

            if (m_RenderTexture.width != position.width ||
                m_RenderTexture.height != position.height)
            {
                m_RenderTexture = new RenderTexture((int)position.width,
                                (int)position.height,
                                (int)RenderTextureFormat.Depth);
            }
        }
    }

    //     void OnGUI()
    //     {
    //         if (!Application.isPlaying)
    //         {
    //             EditorGUILayout.HelpBox("Runs in Play mode", MessageType.Info);
    //             m_bIsPlyed = false;
    //             return;
    //         }
    //         else
    //         {
    //             if (false == m_bIsPlyed)
    //             {
    //                 Initialize();
    //                 m_bIsPlyed = true;
    //             }
    //         }
    // 
    //         if (null != m_camQuaterViewer && null != m_RenderTexture)
    //         {
    //             GUI.DrawTexture(new Rect(0.0f, 0.0f, position.width, position.height), m_RenderTexture);
    //         }
    // 
    //         UpdateZoom();
    //     }

    private void UpdateZoom()
    {
        Event e = Event.current;

        if (e == null)
            return;

        if (e.button == 0 && e.isMouse)
        {
            Debug.Log("e.delta : " + e.delta);

            // Zoom In
            if (e.delta.y < 0f)
            {
                if (m_camQuaterViewer.transform.position.z > -20f)
                {
                    m_camQuaterViewer.transform.position -= m_camQuaterViewer.transform.forward * m_fZoomSpeed;
                }
            }

            // Zoom Out
            if (e.delta.y > 0f)
            {
                if (m_camQuaterViewer.transform.position.z <= 0f)
                {
                    m_camQuaterViewer.transform.position += m_camQuaterViewer.transform.forward * m_fZoomSpeed;
                }
            }
        }
    }
}