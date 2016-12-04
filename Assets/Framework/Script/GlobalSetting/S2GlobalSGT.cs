/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 09일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 전역 설정을 담당하는 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public class S2GlobalSGT : S2Singleton<S2GlobalSGT>
{
    // 해상도 관련
    public int m_iResolutionToWidth     = 720;
    public int m_iResolutionToHeight    = 1280;

    // 렌더링 프레임
    public int m_iFrameOfRender         = 60;

    // 리소스 로드타입
    private eResourcesLoadType m_eResourcesLoadType = eResourcesLoadType.Local_Resource;
    public eResourcesLoadType ResourcesLoadType
    {
        get { return m_eResourcesLoadType; }
        set 
        { 
            if (value != eResourcesLoadType.Local_Resource)
            {
                S2Util.LogWarning("아직 지원하지 않는 모드입니다.({0})", value);
                return;
            }
            m_eResourcesLoadType = value;
        }
    }

    // 리소스 로드모드
    public eResourcesLoadMode m_eResourcesLoadMode;

    // 번들캐시 크기
    public int m_iBundleCatchSize      = 200;

    // 게임정보
    [SerializeField]
    private S2GameInfo m_pGameInfo = new S2GameInfo();
    public S2GameInfo GameInfo { get { return m_pGameInfo; } }

    // 크래시 레포트
    [SerializeField]
    private S2CrashReporter m_pCrashReporter = new S2CrashReporter();
    public S2CrashReporter CrashReporter { get { return m_pCrashReporter; } }

    // 싱글톤 : 시작
    public override void OnInitialize()
    {
        SetDontDestroy();

        SetResolution(m_iResolutionToWidth, m_iResolutionToHeight, true);
        SetFrameRate(m_iFrameOfRender);
        SetSleepTime();

        // @@ 어플종료 실행
        Single.Input.CreateSingleton();
    }

    // 싱글톤 : 종료
    public override void OnFinalize() 
    { 

    }

    // 시스템 : 해상도 설정
    public void SetResolution(int iWidth, int iHeight, bool bIsFull)
    {
        m_iResolutionToWidth     = iWidth;
        m_iResolutionToHeight    = iHeight;
        //Screen.SetResolution(m_iResolutionToWidth, m_iResolutionToHeight, true);
    }
    public Vector2 GetResolution()
    {
        return new Vector2(m_iResolutionToWidth, m_iResolutionToHeight);
    }

    // 시스템 : 렌더링 프레임 설정
    public void SetFrameRate(int iFrame)
    {
        m_iFrameOfRender = iFrame;
        Application.targetFrameRate = m_iFrameOfRender;
    }

    // 시스템 : 절전모드
    public void SetSleepTime()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}