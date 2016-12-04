/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 16일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 코루틴 관련 함수를 모아둔 클래스입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2CoroutineSGT : S2Singleton<S2CoroutineSGT>
{
    public override void OnInitialize() { }
    public override void OnFinalize() { }

    // yield return null : 다음 Update까지 대기
    public void NextUpdate(Action pAction)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToNextUpdate(pAction));
    }
    IEnumerator InvokeToNextUpdate(Action pAction)
    {
        yield return null;
        pAction();
    }
    //-----------------------------------------------


    // yield return new WaitForFixedUpdate() : 다음 FixedUpdate까지 대기
    public void NextFixed(Action pAction)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToNextFixed(pAction));
    }
    IEnumerator InvokeToNextFixed(Action pAction)
    {
        yield return new WaitForFixedUpdate();
        pAction();
    }
    //-----------------------------------------------


    // yield return new WaitForEndOfFrame() : 렌더링 작업이 끝날 때 까지 대기
    public void NextFrame(Action pAction)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToNextFrame(pAction));
    }
    IEnumerator InvokeToNextFrame(Action pAction)
    {
        yield return new WaitForEndOfFrame();
        pAction();
    }
    //-----------------------------------------------


    // yield return new WaitForSeconds : 지정한 시간까지 대기
    public void WaitTime(Action pAction, float fDelay)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToWaitTime(pAction, fDelay));
    }
    IEnumerator InvokeToWaitTime(Action pAction, float fDelay)
    {
        yield return new WaitForSeconds(fDelay);
        pAction();
    }
    //-----------------------------------------------


    //yield return new WWW(string) : 웹 통신 작업이 끝날 때까지 대기
    public void WWW(Action<WWW> pAction, WWW pWWW)
    {
        if (null == pAction)
            return;
        
        StartCoroutine(InvokeToWWW(pAction, pWWW));
    }
    IEnumerator InvokeToWWW(Action<WWW> pAction, WWW pWWW)
    {
        yield return pWWW;
        pAction(pWWW);
    }
    //-----------------------------------------------


    //yield return new AsyncOperation : 비동기 작업이 끝날 때 까지 대기 (씬로딩)
    public void Async(Action<bool> pAction, AsyncOperation pAsync)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToAsync(pAction, pAsync));
    }
    IEnumerator InvokeToAsync(Action<bool> pAction, AsyncOperation pAsync)
    {
        yield return pAsync;
        pAction(pAsync.isDone);
    }
    //-----------------------------------------------


    //yield return StartCoroutine(IEnumerator) : 다른 코루틴이 끝날 때 까지 대기
    public void Routine(Action pAction, IEnumerator pRoutine)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToRoutine(pAction, pRoutine));
    }
    IEnumerator InvokeToRoutine(Action pAction, IEnumerator pRoutine)
    {
        yield return StartCoroutine(pRoutine);
        pAction();
    }
}