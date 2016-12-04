/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 04일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 시간관련 코드가 담겨 있습니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class TimeEvent
{
    public int iFixedTic            = 0;                // TimeTic
	public S2Event pEvent          = new S2Event();   // 이벤트 객체
    public EventArgs pEventParam    = null;             // 이벤트 시 전달할 Args뭉치

    public TimeEvent(EventHandler pObserver, int _iFixedTic)
    {
        iFixedTic   = _iFixedTic;
        pEvent.Add(pObserver);
    }
    public TimeEvent(EventHandler pObserver, EventArgs pArgs, int _iFixedTic)
    {
        iFixedTic   = _iFixedTic;
        pEventParam = pArgs;
        pEvent.Add(pObserver);
    }
    public TimeEvent(EventHandler pObserver, float _fTime)
	{
        iFixedTic   = Single.Timer.GetFixedTicToSec(_fTime);
        pEvent.Add(pObserver);
	}
    public TimeEvent(EventHandler pObserver, EventArgs pArgs, float _fTime)
    {
        iFixedTic   = Single.Timer.GetFixedTicToSec(_fTime);
        pEventParam = pArgs;
        pEvent.Add(pObserver);
    }
}

public class AddValue
{
    public int m_iValue = 0;

    public void OnEventToUpdate(object pSender, EventArgs vArgs)
    {
        m_iValue = m_iValue + 1;
    }
}


public class S2TimerSGT : S2Singleton<S2TimerSGT>
{
    public override void OnInitialize()
    {
        SetDontDestroy();
    }

    public override void OnFinalize() { }

    private float m_fTime = 0f;
    public float time
    {
        get { return m_fTime; }
    }
    private float m_fDeltaTime = 0f;
    public float deltaTime
    {
        get { return m_fDeltaTime; }
    }
    private float m_fFixedTime = 0f;
    public float fixedTime
    {
        get { return m_fFixedTime; }
    }
    private float m_fFixedDeltaTime = 0f;
    public float fixedDeltaTime
    {
        get { return m_fFixedDeltaTime; }
    }
    private float m_fAnimDeltaTime = 0.0333333333333333f;
    public float AnimDeltaTime
    {
        get { return m_fAnimDeltaTime; }
    }
    private int m_iGameSpeed = 1;
    public int GameSpeed
    {
        set { m_iGameSpeed = value; }
        get { return m_iGameSpeed; }
    }

    //private bool m_bIsPause = false;

    // ------------------------------------------
    // TimmerEvent : <식별자, 이벤트>
    private List<string> m_listRemoveTimeEvent                  = new List<string>();
    private Dictionary<string, TimeEvent> m_dicAddTimeEvent     = new Dictionary<string, TimeEvent>();
    private Dictionary<string, TimeEvent> m_dicTimeEvent        = new Dictionary<string, TimeEvent>();

    void Start()
    {
        // 유니티에 설정된 고정시간 얻기
        m_fFixedTime        = Time.fixedTime;
        m_fFixedDeltaTime   = Time.fixedDeltaTime;
    }

    void Update()
    {
        // 흘러간 시간 갱신
        m_fTime             = Time.time;
        m_fDeltaTime        = Time.deltaTime;
    }

    // 배속을 제어하기위해서
    // FrameMove로 변경했고, 사용하려는 메인프레임워크에서 요녀석 호출해주어야함.
    public void FrameMove()
    {
        UpdateToTimeEvent();
        UpdateToDeltaTic();
    }

    public void PauseGame()
    {
        //m_bIsPause = true;
    }

    public void ResumeGame()
    {
        //m_bIsPause = false;
    }

    // ------------------------------------------
    // TimeEvent : X Time이 지난 후 콜백을 호출해준다.
    void UpdateToTimeEvent()
    {
        // 제거된 명령처리
        RemoveTimeEvent();

        // 추가된 명령처리
        AddTimeEvent();

        // TimeEvent
        foreach (KeyValuePair<string, TimeEvent> kvp in m_dicTimeEvent)
        {
            if (0 < --kvp.Value.iFixedTic)
                continue;

            kvp.Value.pEvent.CallBack(this, kvp.Value.pEventParam);

            if (false == m_listRemoveTimeEvent.Contains(kvp.Key))
                m_listRemoveTimeEvent.Add(kvp.Key);
        }
    }

    void RemoveTimeEvent()
    {
        foreach (string strKey in m_listRemoveTimeEvent)
        {
            if (true == m_dicTimeEvent.ContainsKey(strKey))
            {
                m_dicTimeEvent.Remove(strKey);
            }
        }
        m_listRemoveTimeEvent.Clear();
    }

    void AddTimeEvent()
    {
        foreach (KeyValuePair<string, TimeEvent> kvp in m_dicAddTimeEvent)
        {
            if (false == m_dicTimeEvent.ContainsKey(kvp.Key))
            {
                m_dicTimeEvent.Add(kvp.Key, kvp.Value);
            }
        }
        m_dicAddTimeEvent.Clear();
    }

    public void AddTimeEvent(string strID, EventHandler pObserver, float fLateTime)
    {
        AddTimeEvent(strID, new TimeEvent(pObserver, fLateTime));
    }

    public void AddTimeEvent(string strID, TimeEvent pTimeEvent)
    {
        m_dicAddTimeEvent[strID] = pTimeEvent;
    }

    public void DelTimeEvent(string strID)
    {
        if (true == m_listRemoveTimeEvent.Contains(strID))
            return;

        m_listRemoveTimeEvent.Add(strID);
    }

    public bool IsTimeEvent(string strID)
    {
        return m_dicTimeEvent.ContainsKey(strID);
    }


    // ------------------------------------------
    // DeltaTime : StartDeltaTime()호출 후 부터 GetDeltaTime()호출시까지 걸린시간을 구해준다.
    private Dictionary<string, DateTime> m_dicDeltaTime = new Dictionary<string, DateTime>();
    public void StartDeltaTime(string strID)
    {
        m_dicDeltaTime[strID] = DateTime.Now;
    }

    private TimeSpan GetDeltaTime(string strID)
    {
        if (false == m_dicDeltaTime.ContainsKey(strID))
            StartDeltaTime(strID);

        return (DateTime.Now - m_dicDeltaTime[strID]);
    }

    public float GetDeltaTimeToSecond(string strID)
    {
        return (float)(GetDeltaTime(strID).TotalMilliseconds / 1000.0);
    }

    public void StopDeltaTime(string strID)
    {
        m_dicDeltaTime.Remove(strID);
    }

    public bool IsDeltaTime(string strID)
    {
        return m_dicDeltaTime.ContainsKey(strID);
    }

    public bool IsLaterTime(string strID, float fLaterTime)
    {
        return (fLaterTime < GetDeltaTimeToSecond(strID));
    }


    // ------------------------------------------
    // DeltaFixedTic : StartDeltaFixed()호출 후 부터 GetDeltaFixed()호출시까지 걸린틱을 구해준다.
    private S2Event EventToTicUpdate = new S2Event();
    private Dictionary<string, AddValue> m_dicDeltaTic = new Dictionary<string, AddValue>();
    public void StartDeltaTic(string strID)
    {
        if (true == IsDeltaTic(strID))
        {
            m_dicDeltaTic[strID].m_iValue = 0;
            return;
        }
        
        AddValue pAddValue = new AddValue();

        m_dicDeltaTic[strID] = pAddValue;
        EventToTicUpdate.Add(pAddValue.OnEventToUpdate, true);
    }

    private int GetDeltaTic(string strID)
    {
        if (false == m_dicDeltaTic.ContainsKey(strID))
            StartDeltaTic(strID);

        return m_dicDeltaTic[strID].m_iValue;
    }

    public float GetDeltaTicToSecond(string strID)
    {
        return GetSecToFixedTic(GetDeltaTic(strID)) / Single.Timer.GameSpeed;
    }

    public void StopDeltaTic(string strID)
    {
        if (false == IsDeltaTic(strID))
            return;

        EventToTicUpdate.Del(m_dicDeltaTic[strID].OnEventToUpdate);
        m_dicDeltaTic.Remove(strID);
    }

    public bool IsDeltaTic(string strID)
    {
        return m_dicDeltaTic.ContainsKey(strID);
    }

    public bool IsLaterTic(string strID, int iLaterTic)
    {
        return (iLaterTic < GetDeltaTic(strID));
    }

    void UpdateToDeltaTic()
    {
        EventToTicUpdate.CallBack(this);
    }


    private InternetTime.SNTPClient m_cSNTPClient = null;
    public DateTime GetNowTime()
    {
        if (null == m_cSNTPClient)
        {
            try
            {
                m_cSNTPClient = new InternetTime.SNTPClient("time.nuri.net");   //참조할 NTP 서버 주소.
                m_cSNTPClient.Connect(false);
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR: " + e.Message);
                return new DateTime();
            }

            Debug.Log(m_cSNTPClient.ToString());
        }

        return m_cSNTPClient.DestinationTimestamp;
    }


    //----------------------------------------------------------------------------
    // 단위변환 : 초당 FixedTic
    int GetFixedTicPerSecond()
    {
        return Mathf.RoundToInt(1.0f / fixedDeltaTime);
    }

    //----------------------------------------------------------------------------
    // 단위변환 : 초당 AnimationFrame
    int GetAnimFramePerSecond()
    {
        return Mathf.RoundToInt(1.0f / AnimDeltaTime);
    }

    //----------------------------------------------------------------------------
    // 단위변환 : FixedTic을 시간(초)으로 변환
    public float GetSecToFixedTic(int iFixedTic)
    {
        return (iFixedTic * fixedDeltaTime);
    }

    //----------------------------------------------------------------------------
    // 단위변환 : 시간(초)을 FixedTic로 변환
    public int GetFixedTicToSec(float fSecond)
    {
        return Mathf.RoundToInt(fSecond * GetFixedTicPerSecond());
    }
    
    //----------------------------------------------------------------------------
    // 단위변환 : AnimationFrame을 FixedTic로 변환
    public int GetFixedTicToAnimFrame(int iAnimFrame)
    {
        return Mathf.RoundToInt(GetSecToAnimFrame(iAnimFrame) * GetFixedTicPerSecond());
    }

    //----------------------------------------------------------------------------
    // 단위변환 : FixedTic을 AnimationFrame으로 변환
    public int GetAnimFrameToFixedTic(int iFixedTic)
    {
        return Mathf.RoundToInt(GetSecToFixedTic(iFixedTic) * GetAnimFramePerSecond());
    }
    
    //----------------------------------------------------------------------------
    // 단위변환 : AnimationFrame을 시간(초)로 변환
    public float GetSecToAnimFrame(int iAnimFrame)
    {
        return (iAnimFrame * AnimDeltaTime);
    }

    //----------------------------------------------------------------------------
    // 단위변환 : 시간(초)을 AnimationFrame으로 변환
    public int GetAnimFrameToSec(float fSecTime)
    {
        return Mathf.RoundToInt(fSecTime * GetAnimFramePerSecond());
    }
}