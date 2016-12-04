/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 04일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 이벤트를 관리하는 클래스입니다. 멤버로 사용하세요.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/

using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class S2EventSGT : S2Singleton<S2EventSGT>
{
    private S2Event pEvent = new S2Event();

    public override void OnInitialize() { }
    public override void OnFinalize() { }

    public S2EventParam<T> SetArgs<T>(T pArgs)
    {
        return new S2EventParam<T>(pArgs);
    }

    public T GetArgs<T>(EventArgs pArgs)
    {
        if (null == pArgs)
            return default(T);

        return ((S2EventParam<T>)pArgs).GetData;
    }

    public void SendEvent<T>(EventHandler pObserver)
    {
        if (null == pObserver)
            return;

        pEvent.Clear();
        pEvent.Add(pObserver);
        pEvent.CallBack(this);
    }

    public void SendEvent<T>(EventHandler pObserver, T pArgs)
    {
        if (null == pObserver)
            return;

        pEvent.Clear();
        pEvent.Add(pObserver);
        pEvent.CallBack<T>(this, pArgs);
    }
}

public class S2EventParam<T> : EventArgs
{
    private T epData;
    public T GetData                { get { return epData; } }
    public S2EventParam(T data)    { epData = data; }
}

public sealed class S2Event
{
    private event EventHandler m_pHandler = null;
    private Dictionary<EventHandler, string> m_dicHandler = new Dictionary<EventHandler, string>();

    public S2Event() { }
    public S2Event(EventHandler pObserver)
    {
        Add(pObserver);
    }

    public void Add(EventHandler pObserver, bool bCheckInstance = false)
    {
        if (true == bCheckInstance)
        {
            if (true == m_dicHandler.ContainsKey(pObserver))
                return;
        }
        else
        {
            if (true == m_dicHandler.ContainsValue(pObserver.Method.ToString()))
                return;
        }

        m_pHandler += pObserver;
        m_dicHandler.Add(pObserver, pObserver.Method.ToString());
    }

    public void Del(EventHandler pObserver)
    {
        if (false == m_dicHandler.ContainsKey(pObserver))
            return;

        m_pHandler -= pObserver;
        m_dicHandler.Remove(pObserver);
    }

    public void Clear()
    {
        m_pHandler = null;
        m_dicHandler.Clear();
    }

    public bool IsAddEvent()
    {
        return (0 != m_dicHandler.Count);
    }
    public bool IsAddEvent(EventHandler pObserver)
    {
        return m_dicHandler.ContainsKey(pObserver);
    }

    public void CallBack(object pSender)
    {
        if (null != m_pHandler)
            m_pHandler(pSender, null);
    }

    public void CallBack<T>(object pSender, T pArgs)
    {
        if (null != m_pHandler)
            m_pHandler(pSender, new S2EventParam<T>(pArgs));
    }

    public void CallBack(object pSender, EventArgs pArgs)
    {
        if (null != m_pHandler)
            m_pHandler(pSender, pArgs);
    }
}

/* Ex) 사용 예
 * class Sender
 * {
 *      public S2Event event = new S2Event();
 *      
 *      void ABCD(...)
 *      {
 *          // 이벤트 발생!!!
 *          event.CallBack<Vector3>(this, new Vector3());
 *      }
 * }
 * 
 * class Observer
 * {
 *      Sender pSender = new Sender();
 *      Observer()
 *      {
 *          pSender.event.Add( OnEventCallback );
 *      }
 *      void OnEventCallback(object pSender, EventArgs vArgs)
 *      {   
 *          Debug.Log( "Param : " + Single.Event.GetEventArgs<Vector3>(vArgs) );
 *      }
 * }
 */