/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 04일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 유니티Debug를 Wrapper하여 몇 가지 제어를 할 수 있도록 수정하였습니다.
            - 큰 단점... 로그 더블클릭시 그 곳으로 가지지 않아요...ㅠㅠ

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngineInternal;

#if !UNITY_EDITOR
public static class Debug
{
    // 배포판 빌드시 m_bRelease를 true하면 로그를 출력하지 않습니다.
    public static bool m_bRelease = false;

    // Debug.Log로 출력을 할것인가? 말것인가?
    public static bool m_isOutputLog = true;

    // Debug.LogWarning으로 출력을 할것인가? 말것인가?
    public static bool m_isOutputLogWarning = true;

    // Debug.LogError로 출력을 할것인가? 말것인가?
    public static bool m_isOutputLogError = true;

    // 화면UI에 로그출력을 위한 이벤트
    public static S2Event EventLog = new S2Event();

    public static bool isDebugBuild
    {
        get { return UnityEngine.Debug.isDebugBuild; }
    }

    private static bool IsRelease()
    {
        if (m_bRelease)
        {
            if ((Application.platform == RuntimePlatform.WindowsEditor) ||
                (Application.platform == RuntimePlatform.OSXEditor))
                return true;
        }
        return false;
    }

    public static void Log(object message)
    {
        EventLog.CallBack<string>(null, "[00FF00]" + message + "[-]");

        if (IsRelease() || false == m_isOutputLog)
            return;

        UnityEngine.Debug.Log(message);
    }

    public static void Log(object message, UnityEngine.Object context)
    {
        EventLog.CallBack<string>(null, "[00FF00]" + message + "[-]");

        if (IsRelease() || false == m_isOutputLog)
            return;

        UnityEngine.Debug.Log(message, context);
    }

    public static void LogError(object message)
    {
        EventLog.CallBack<string>(null, "[FF0000]" + message + "[-]");

        if (IsRelease() || false == m_isOutputLogError)
            return;

        UnityEngine.Debug.LogError(message);
    }

    public static void LogError(object message, UnityEngine.Object context)
    {
        EventLog.CallBack<string>(null, "[FF0000]" + message + "[-]");

        if (IsRelease() || false == m_isOutputLogError)
            return;

        UnityEngine.Debug.LogError(message, context);
    }

    public static void LogWarning(object message)
    {
        EventLog.CallBack<string>(null, "[0000FF]" + message + "[-]");

        if (IsRelease() || false == m_isOutputLogWarning)
            return;

        UnityEngine.Debug.LogWarning(message.ToString());
    }

    public static void LogWarning(object message, UnityEngine.Object context)
    {
        EventLog.CallBack<string>(null, "[0000FF]" + message + "[-]");

        if (IsRelease() || false == m_isOutputLogWarning)
            return;

        UnityEngine.Debug.LogWarning(message.ToString(), context);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
        if (!condition) throw new Exception();
    }
}
#endif