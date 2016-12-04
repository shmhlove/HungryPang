/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 04일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 프로그램 전체적으로 사용되는 유틸함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class S2Pair<T1, T2>
{
    public T1 Value1;
    public T2 Value2;

    public S2Pair()
    {
        Initialize();
    }

    public S2Pair(T1 _Value1, T2 _Value2)
    {
        Value1 = _Value1;
        Value2 = _Value2;
    }

    public void Initialize()
    {
        Value1 = default(T1);
        Value2 = default(T2);
    }
}

public class S2Int2
{
    S2Pair<int, int> pData = null;
    public int Value1 { get { return pData.Value1; } set { pData.Value1 = value; } }
    public int Value2 { get { return pData.Value2; } set { pData.Value2 = value; } }

    public S2Int2()
    {
        Initialize();
    }

    public S2Int2(int _Value1, int _Value2)
    {
        pData = new S2Pair<int, int>(_Value1, _Value2);
    }

    public void Initialize()
    {
        pData = new S2Pair<int, int>();
    }
}

public static partial class S2Util
{
    // --------------------------------------------------------------------
    // 로그 관련 함수
    // String객체
    private static StringBuilder m_strBuilder = new StringBuilder(1024);
    public static StringBuilder S2String
    {
        get
        {
            m_strBuilder.Remove(0, m_strBuilder.Length);
            return m_strBuilder;
        }
    }

    // String조립
    public static string Format(string strMessage, params object[] pArgs)
    {
        StringBuilder strBuilder = S2String;
        return strBuilder.AppendFormat(strMessage, pArgs).ToString();
    }

    // 에러 로그
    public static void LogError(string strMessage, params object[] args)
    {
        UnityEngine.Debug.LogError(Format(strMessage, args));
    }

    // 워닝 로그
    public static void LogWarning(string strMessage, params object[] args)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogWarning(Format(strMessage, args));
#endif
    }

    // 일반 로그
    public static void Log(string strMessage, params object[] args)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.Log(Format(strMessage, args));
#endif
    }

    // try-Catch문에서 예외전달 메시지 만들어줌
    public static System.ArgumentException GetExceptionMsg(string strMessage, params object[] args)
    {
        return new System.ArgumentException(Format(strMessage, args));
    }

    // 에러로그와 리턴값을 하나로 묶음( EX : return ErrorReturn<bool>(false, "{0} 예외야!!", "나쁜"); )
    public static T ErrorReturn<T>(T tReturnValue, string strMessage, params object[] args)
    {
        LogError(strMessage, args);
        return tReturnValue;
    }

    // 콜스택 정보를 얻어줌
    public static StackFrame[] GetCallStack()
    {
        StackTrace stackTrace = new StackTrace();
        return stackTrace.GetFrames();
    }


    // --------------------------------------------------------------------
    // Assert 관련
    public static bool IsNull<T>(T value)
    {
        if (EqualityComparer<T>.Default.Equals(value, default(T)))
            return true;

        return false;
    }

    public static void Assert(bool condition)
    {
        if (UnityEngine.Debug.isDebugBuild && !condition) throw new Exception();
    }

    public static void Assert(bool condition, string log)
    {
        if (UnityEngine.Debug.isDebugBuild && !condition)
        {
            UnityEngine.Debug.Log(log);
            throw new Exception();
        }
    }

    public static void Assert(bool condition, string strMessage, params object[] args)
    {
        if (UnityEngine.Debug.isDebugBuild && !condition)
        {
            UnityEngine.Debug.Log(Format(strMessage, args));
            throw new Exception();
        }
    }

    public static void Verify(bool condition)
    {
#if UNITY_EDITOR
        if (UnityEngine.Debug.isDebugBuild && !condition)
        {
            EditorPauseOfToggle(true);
        }
#endif
    }

    public static void Verify(bool condition, string log)
    {
#if UNITY_EDITOR
        if (UnityEngine.Debug.isDebugBuild && !condition)
        {
            UnityEngine.Debug.Log(log);
            EditorPauseOfToggle(true);
        }
#endif
    }

    public static void Verify(bool condition, string strMessage, params object[] args)
    {
#if UNITY_EDITOR
        if (UnityEngine.Debug.isDebugBuild && !condition)
        {
            UnityEngine.Debug.Log(Format(strMessage, args));
            EditorPauseOfToggle(true);
        }
#endif
    }


    // --------------------------------------------------------------------
    // 형 변환 관련 ( Enum.Parse 엄청느립니다. 가급적 사용금지!!! )
    // String을 Enum으로
    public static T FormatStringToEnum<T>(string str, params object[] args)
    {
        return StringToEnum<T>(Format(str, args));
    }
    // String을 Enum으로
    public static T StringToEnum<T>(string str, string strErrorLog = "")
    {
        if ((true == string.IsNullOrEmpty(str)) || 
            (false == Enum.IsDefined(typeof(T), str)))
        {
            LogError("{0}(Enum:{1})", strErrorLog, str);
        }

        return (T)Enum.Parse(typeof(T), str);
    }


    // --------------------------------------------------------------------
    // 컨테이너 관련

    // Foreach Array
    public static void ForeachToArray<T>(T[] pArray, Action<T> pLambda)
    {
        foreach (T tArray in pArray)
            pLambda(tArray);
    }

    // Foreach Enum
    public static void ForeachToEnum<T>(Action<T> pLambda)
    {
        foreach (T eEnum in Enum.GetValues(typeof(T)))
            pLambda(eEnum);
    }

    // Foreach List
    public static void ForeachToList<T>(List<T> list, Action<T> pLambda)
    {        
        foreach (T tList in list)
            pLambda(tList);
    }

    // Foreach Condition List
    public static bool ForeachToListOfBreak<T>(List<T> list, bool bBreakCondition, Func<T, bool> pLambda)
    {
        foreach (T tList in list)
        {
            if (bBreakCondition == pLambda(tList))
                return bBreakCondition;
        }
        return !bBreakCondition;
    }

    // Foreach Dictionary
    public static void ForeachToDic<TKey, TValue>(Dictionary<TKey, TValue> dic, Action<TKey, TValue> pLambda)
    {
        foreach (KeyValuePair<TKey, TValue> kvp in dic)
            pLambda(kvp.Key, kvp.Value);
    }

    // Foreach Condition Dictionary
    public static bool ForeachToDicOfBreak<TKey, TValue>(Dictionary<TKey, TValue> dic, bool bBreakCondition, Func<TKey, TValue, bool> pLambda)
    {
        foreach (KeyValuePair<TKey, TValue> kvp in dic)
        {
            if (bBreakCondition == pLambda(kvp.Key, kvp.Value))
                return bBreakCondition;
        }
        return !bBreakCondition;
    }

    // for Double
    public static void ForToDouble(int iMaxToFirst, int iMaxToSecond, Action<int, int> pLambda)
    {
        for (int iLoop1 = 0; iLoop1 < iMaxToFirst; ++iLoop1)
        {
            for (int iLoop2 = 0; iLoop2 < iMaxToSecond; ++iLoop2)
                pLambda(iLoop1, iLoop2);
        }
    }
    public static bool ForToDoubleOfBreak(int iMaxToFirst, int iMaxToSecond, bool bBreakCondition, Func<int, int, bool> pLambda)
    {
        for (int iLoop1 = 0; iLoop1 < iMaxToFirst; ++iLoop1)
        {
            for (int iLoop2 = 0; iLoop2 < iMaxToSecond; ++iLoop2)
            {
                if (bBreakCondition == pLambda(iLoop1, iLoop2))
                    return bBreakCondition;
            }
        }
        return !bBreakCondition;
    }

    // Inverse for Double
    public static void ForInverseToDouble(int iMaxToFirst, int iMaxToSecond, Action<int, int> pLambda)
    {
        for (int iLoop1 = iMaxToFirst; iLoop1 >= 0; --iLoop1)
        {
            for (int iLoop2 = iMaxToSecond; iLoop2 >= 0; --iLoop2)
                pLambda(iLoop1, iLoop2);
        }
    }


    //-------------------------------------------------------------------------
    // Random관련
    // test_code
    public static void TestRandomTrue()
    {
        List<bool> list = new List<bool>();
        var p = 0.3f;
        for (int i = 0; i < 1000; i++)
            list.Add(S2Util.RandomTrue(p));

        var countTrue = list.Count(e => e == true);
        var countTotal = list.Count();
        var ratio = (float)countTrue / (float)countTotal;
        Log("  RandomTrue(" + p + "); " +
              "result ratio [" + ratio + "], " +
              "true count [" + countTrue + "], " +
              "total  count [" + countTotal + "]");
    }

    // 확률 P% 이하로 true 리턴. (단, 100% == 1.0f)
    public static bool RandomTrue(float p)
    {
        // 유니티 랜덤함수 직접 사용[blueasa / 2014-10-30]
        return (p > UnityEngine.Random.Range(0f, 1f));
    }

    // 가중치에 따라 랜덤하게 원소 선택 
    public static T RandomW<T>(List<T> values, List<float> weights)
    {
        // ex: weights = { 0.1, 0.3, 0.6};

        // assert(weight.Sum > 0);
        var subsums = new List<float>(weights.Count);

        var sum = weights.Aggregate(0.0f, (acc, f) =>
        { acc += f; subsums.Add(acc); return acc; });
        // ex : subsums = {0.1, 0.4, 1.0}, sum = 1.0 

        var r = UnityEngine.Random.Range(0.0f, sum);
        // ex : r = 0.35

        var index = subsums.FindIndex(f => (r < f));
        // ex : index = 1 

        return values[index];
    }

    // 가중치에 따라 랜덤하게 원소 선택 ( 무조건 하나는 선택되도록 )
    // 가중치를 정규화해서 합이 1인 값으로 만들고, 의미없는 원소는 제거하여 RandomW처리
    public static T RandomOneW<T>(List<T> values, List<float> weights)
    {       
        // 전체 가중치 합
        float fSum = 0.0f;
        foreach (float fValue in weights)
            fSum += fValue;

        // 예외처리 : 가중치 합이 0이면 선택할 원소가 없다...
        if(0.0f == fSum)
            return default(T);

        // 데이터 복사용 변수
        List<T> CopyValues = new List<T>();
        List<float> CopyWeights = new List<float>();

        // 가중치 정규화하면서 필요없는 원소는 제거
        for (int iLoop = 0; iLoop < weights.Count; ++iLoop)
        {
            if (0.0f == weights[iLoop])
                continue;
            
            CopyValues.Add(values[iLoop]);
            CopyWeights.Add(weights[iLoop] / fSum);
        }

        return RandomW(CopyValues, CopyWeights);
    }

    // RandomW<T> 와 스팩은 동일하나, 모든 원소의 가중치가 동일한 경우로 제한함. 
    public static T RandomN<T>(List<T> values)
    {
        var index = (int)UnityEngine.Random.Range(0.0f, values.Count);
        if (index == values.Count)
            index = values.Count - 1;
        return values[index];
    }

    // int일 경우, Max값도 나오도록 하기 위해 Max + 1을 함.
    public static int RandomRange(int _iMin, int _iMax)
    {
        if (_iMin == _iMax) return _iMin;
        return UnityEngine.Random.Range(_iMin, _iMax + 1);
    }
    public static float RandomRange(float _fMin, float _fMax)
    {
        if (_fMin == _fMax) return _fMin;
        return UnityEngine.Random.Range(_fMin, _fMax);
    }


    //-------------------------------------------------------------------------
    // 디바이스 정보관련
    // GUID
    public static string GetGUID()
    {
        System.Guid uid = System.Guid.NewGuid();
        return uid.ToString();
    }

    // UUID(이걸 사용)
    public static string GetUUID()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }


    //-------------------------------------------------------------------------
    // 유니티 관련
    // Missing컴포넌트 체크
    public static void CheckMissingComponent()
    {
#if UNITY_EDITOR
        UnityEngine.Object[] pObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (UnityEngine.Object pObject in pObjects)
        {
            GameObject pGameObject = pObject as GameObject;
            if (null == pGameObject)
                continue;

            Component[] pComponents = pGameObject.GetComponents<Component>();
            foreach (Component pComponent in pComponents)
            {
                if (null == pComponent)
                    S2Util.LogError("MissingComponent!!(GameObject{0})", pObject.name);
            }
        }
#endif
    }

    // 유니티 에디터의 Pause를 Toggle합니다.
    public static void EditorPauseOfToggle(bool bToggle)
    {
#if UNITY_EDITOR
        EditorApplication.isPaused = bToggle;
#endif
    }

    // 디렉토리 체크
    public static void Search(string strPath, Action<FileInfo> pCallBack)
    {
        DirectoryInfo pDirInfo = new DirectoryInfo(strPath);
        SearchFiles(pDirInfo, pCallBack);
        SearchDirs(pDirInfo, pCallBack);
    }

    static void SearchDirs(DirectoryInfo pDirInfo, Action<FileInfo> pCallBack)
    {
        DirectoryInfo[] pDirs = pDirInfo.GetDirectories();
        foreach (DirectoryInfo pDir in pDirs)
        {
            SearchFiles(pDir, pCallBack);
            SearchDirs(pDir, pCallBack);
        }
    }

    static void SearchFiles(DirectoryInfo pDirInfo, Action<FileInfo> pCallBack)
    {
        FileInfo[] pFiles = pDirInfo.GetFiles();
        foreach (FileInfo pFile in pFiles)
        {
            pCallBack(pFile);
        }
    }
}