/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 04일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 일반 클래스를 싱글턴으로 만들어 줍니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;

public static class Single
{
    // 글로벌 설정
    public static S2GlobalSGT      Global           { get { return S2GlobalSGT.GetInstance(); } }
    public static S2GameInfo       GameInfo         { get { return Single.Global.GameInfo; } }

    // 데이터
    public static S2PatchSGT       Patch            { get { return S2PatchSGT.GetInstance(); } }
    public static S2BundleSGT      Bundle           { get { return S2BundleSGT.GetInstance(); } }
    public static S2DataSGT        Data             { get { return S2DataSGT.GetInstance(); } }
    public static S2TableData      TableData        { get { return Single.Data.TableData; } }
    public static S2ResourcesData  ResourceData     { get { return Single.Data.ResourcesData; } }
    public static S2Loader         Loader           { get { return Single.Data.Loader; } }

    // 씬
    public static S2SceneSGT       Scene            { get { return S2SceneSGT.GetInstance(); } }
    
    // 게임엔진
    public static S2GameEngineSGT  Engine           { get { return S2GameEngineSGT.GetInstance(); } }
    public static S2Puzzle         Puzzle           { get { return Single.Engine.Puzzle; } }
    public static S2Character      Character        { get { return Single.Engine.Character; } }
    public static S2MonsterManager Monsters         { get { return Single.Engine.Monsters; } }
    public static S2DungeonSGT     Dungeon          { get { return S2DungeonSGT.GetInstance(); } }
    public static S2CameraSGT      Camera           { get { return S2CameraSGT.GetInstance(); } }
    public static S2EffectSGT      Effect           { get { return S2EffectSGT.GetInstance(); } }
    public static S2DamageSGT      Damage           { get { return S2DamageSGT.GetInstance(); } }

    // UI
    public static SGUIRootToStartGame UIStartGame   { get { return SGUIRootToStartGame.GetInstance(); } }
    public static SGUIRootToLoading UILoading       { get { return SGUIRootToLoading.GetInstance(); } }
    public static SGUIRootToInGameBack UIInGameBack { get { return SGUIRootToInGameBack.GetInstance(); } }
    public static SGUIRootToInGameFront UIInGameFront { get { return SGUIRootToInGameFront.GetInstance(); } }

    // 유틸리티
    public static S2SoundPlayer    Sound            { get { return S2SoundPlayer.GetInstance(); } }
    public static S2TimerSGT       Timer            { get { return S2TimerSGT.GetInstance(); } }
    public static S2EventSGT       Event            { get { return S2EventSGT.GetInstance(); } }
    public static S2CoroutineSGT   Coroutine        { get { return S2CoroutineSGT.GetInstance(); } }
    public static S2HardCord       Hard             { get { return S2HardCord.GetInstance(); } }
    public static S2FileSGT        File             { get { return S2FileSGT.GetInstance(); } }
    public static S2InputSGT       Input            { get { return S2InputSGT.GetInstance(); } }
}

public abstract class S2Singleton<T> : MonoBehaviour where T : S2Singleton<T>
{
    private static T        m_pInstance  = null;
    private static object   m_pLocker    = new object();

    // 다양화 : 초기화( 게임오브젝트에 붙은경우 Awake시, 직접 생성인 경우 Instance에 접근하는 순간 호출 됨 )
    public abstract void OnInitialize();

    // 다양화 : 종료( DonDestory가 설정된경우 어플이 종료될때, 아닌 경우에는 씬이 변경될때, 혹은 DoDestory로 명시적으로 제거할때 호출 됨 )
    public abstract void OnFinalize();

    // 시스템 : 시작
    private void Awake()
    {
        Initialize(this as T);
    }

    // 시스템 : 객체 제거시
    private void Destroy()
    {
        Destroyed();
    }

    // 시스템 : 객체 제거시
    private void OnDestroy()
    {
        Destroyed();
    }

    // 시스템 : 어플리케이션이 종료될때
    private void OnApplicationQuit()
    {
        Destroyed();
    }

    // 시스템 : 싱글턴 제거
    void Destroyed()
    {
        if (null == m_pInstance)
            return;

        m_pInstance.OnFinalize();
        m_pInstance = null;
    }

    // 유틸 : 객체 초기화
    static void Initialize(T pInstance)
    {
        if (null == pInstance)
            return;

        if (null == m_pInstance)
        {
            pInstance.SetParent("S2Singletons(Destroy)");

            m_pInstance = pInstance;
            m_pInstance.OnInitialize();
        }
        else if (m_pInstance != pInstance)
        {
            DestroyImmediate(pInstance.gameObject);
            return;
        }
    }
    
    // 유틸 : 싱글턴 부모설정
    GameObject SetParent(string strRootName)
    {
        return S2GameObject.SetParent(gameObject, strRootName);
    }

    // 인터페이스 : 객체얻기
    public static T GetInstance()
    {
        lock (m_pLocker)
        {
            if (null == m_pInstance)
            {
                T pSingle = S2GameObject.FindObjectOfType<T>();
                if (pSingle == null)
                    pSingle = S2GameObject.CreateEmptyObject(typeof(T).ToString()).AddComponent<T>();

                Initialize(pSingle);
            }

            return m_pInstance;
        }
    }

    // 인터페이스 : 객체생성
    public void CreateSingleton() { }

    // 인터페이스 : 씬이 제거되어도 싱글턴을 제거하지 않습니다.
    public void SetDontDestroy()
    {
        DontDestroyOnLoad(m_pInstance.SetParent("S2Singletons(DontDestroy)"));
    }

    // 인터페이스 : 싱클턴 제거
    public void DoDestroy()
    {
        if (null == m_pInstance)
            return;

        S2GameEngineSGT.DestroyObject(gameObject);
    }
}