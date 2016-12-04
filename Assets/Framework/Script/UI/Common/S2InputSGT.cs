/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 06월 10일
★ E-Mail ☞ shmhlove@neowiz.com
★ Desc   ☞ 이 클래스는 사용자 입력을 제어합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TouchEventParam
{
    public int iTouchPingerID;
    public Vector2 vScreenTouchPos;
    public Vector2 vWorldTouchPos;
}

public class S2InputSGT : S2Singleton<S2InputSGT>
{
    private Dictionary<int, Vector2> m_dicTouchEnter = new Dictionary<int, Vector2>();
    public Dictionary<int, Vector2> TouchEnter
    { get { return m_dicTouchEnter; } }

    private Dictionary<int, Vector2> m_dicTouchEnd = new Dictionary<int, Vector2>();
    public Dictionary<int, Vector2> TouchEnd
    { get { return m_dicTouchEnd; } }

    private Dictionary<int, Vector2> m_dicTouchMove = new Dictionary<int, Vector2>();
    public Dictionary<int, Vector2> TouchMove
    { get { return m_dicTouchMove; } }

    private List<int> m_pTouchOrders = new List<int>();
    public List<int> TouchOrders
    { get { return m_pTouchOrders; } }

    public S2Event EventToEnter = new S2Event();
    public S2Event EventToDrag  = new S2Event();
    public S2Event EventToEnd   = new S2Event();

    public override void OnInitialize() { }
    public override void OnFinalize() { }

    void Update()
    {
        // Mobile
        if (0 < Input.touchCount)
        {
            for (int iLoop = 0; iLoop < Input.touchCount; ++iLoop)
            {
                Touch pTouch = Input.GetTouch(iLoop);
                if (pTouch.phase.Equals(TouchPhase.Began))
                {
                    OnTouchEnter(pTouch.fingerId, pTouch.position);
                }
                else if (pTouch.phase.Equals(TouchPhase.Ended) ||
                        pTouch.phase.Equals(TouchPhase.Canceled))
                {
                    OnTouchEnd(pTouch.fingerId, pTouch.position);
                }
                else if (pTouch.phase.Equals(TouchPhase.Moved))
                {
                    OnTouchMove(pTouch.fingerId, pTouch.position);
                }
            }
        }
        // PC
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                OnTouchEnter(0, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                OnTouchEnd(0, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
            else if (Input.GetButton("Fire1"))
            {
                OnTouchMove(0, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
        }

        // Back키시 어플종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnTouchEnter(int iFingerID, Vector2 vTouchPos)
    {
        Vector3 vScreenPos = Vector3.zero;
        vScreenPos.x = (vTouchPos.x / Screen.width) * Single.Global.m_iResolutionToWidth;
        vScreenPos.y = (vTouchPos.y / Screen.height) * Single.Global.m_iResolutionToHeight;
        
        m_dicTouchEnter[iFingerID]  = vScreenPos;
        m_dicTouchMove[iFingerID]   = vScreenPos;
        m_dicTouchEnd.Remove(iFingerID);
        m_pTouchOrders.Add(iFingerID);

        TouchEventParam pEventParam;
        pEventParam.iTouchPingerID  = iFingerID;
        pEventParam.vScreenTouchPos = vScreenPos;
        pEventParam.vWorldTouchPos  = vTouchPos;
        EventToEnter.CallBack<TouchEventParam>(this, pEventParam);
    }

    void OnTouchEnd(int iFingerID, Vector2 vTouchPos)
    {
        Vector3 vScreenPos = Vector3.zero;
        vScreenPos.x = (vTouchPos.x / Screen.width) * Single.Global.m_iResolutionToWidth;
        vScreenPos.y = (vTouchPos.y / Screen.height) * Single.Global.m_iResolutionToHeight;

        m_dicTouchEnd[iFingerID] = vScreenPos;
        m_dicTouchEnter.Remove(iFingerID);
        m_dicTouchMove.Remove(iFingerID);
        m_pTouchOrders.Remove(iFingerID);

        TouchEventParam pEventParam;
        pEventParam.iTouchPingerID  = iFingerID;
        pEventParam.vScreenTouchPos = vScreenPos;
        pEventParam.vWorldTouchPos  = vTouchPos;
        EventToEnd.CallBack<TouchEventParam>(this, pEventParam);
    }

    void OnTouchMove(int iFingerID, Vector2 vTouchPos)
    {
        Vector3 vScreenPos = Vector3.zero;
        vScreenPos.x = (vTouchPos.x / Screen.width) * Single.Global.m_iResolutionToWidth;
        vScreenPos.y = (vTouchPos.y / Screen.height) * Single.Global.m_iResolutionToHeight;

        m_dicTouchMove[iFingerID] = vScreenPos;

        TouchEventParam pEventParam;
        pEventParam.iTouchPingerID  = iFingerID;
        pEventParam.vScreenTouchPos = vScreenPos;
        pEventParam.vWorldTouchPos  = vTouchPos;
        EventToDrag.CallBack<TouchEventParam>(this, pEventParam);
    }

    public int GetFirstFingerID()
    {
        if (0 == m_pTouchOrders.Count)
            return -1;

        return m_pTouchOrders[0];
    }

    public int GetLastFingerID()
    {
        if (0 == m_pTouchOrders.Count)
            return -1;

        return m_pTouchOrders[(m_pTouchOrders.Count - 1)];
    }

    public bool? IsRightTouchPingerID(int iPingerID)
    {
        if (false == TouchEnter.ContainsKey(iPingerID))
            return null;

        if ((Single.Global.m_iResolutionToWidth / 2.0f) < TouchEnter[iPingerID].x)
            return true;

        return false;
    }
}
