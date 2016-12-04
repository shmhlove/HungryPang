/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 29일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 수학관련 유틸함수를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public static partial class S2Math
{
    // 나누기 : Int
    public static int Divide(int iUp, int iDown)
    {
        if (0 == iDown)
            return 0;
        return iUp / iDown;
    }
    // 나누기 : float
    public static float Divide(float fUp, float fDown)
    {
        if (0.0f == fDown)
            return 0.0f;
        return fUp / fDown;
    }
    // 나머지 : int
    public static int Modulus(int iUp, int iDown)
    {
        if (0 == iUp)
            return 0;
        if (0 == iDown)
            return 0;
        return iUp % iDown;
    }
    // 소수점 끝자리 자르기
    public static float Round(float fValue, int iOmit)
    {
        float fOmit = 1.0f;
        for(int iLoop=0; iLoop<iOmit; ++iLoop)
            fOmit *= 10.0f;

        return Mathf.Round(fValue * fOmit) / fOmit;
    }
    // 백분률 : int
    public static int Percent(int iMax, int iCurrent)
    {
        return (int)Percent((float)iMax, (float)iCurrent);
    }
    // 백분률 : float
    public static float Percent(float fMax, float fCurrent)
    {
        return Round(Divide(fCurrent, fMax), 2) * 100.0f;
    }
    // Start에서 End까지 Cur루핑
    public static int LoopNum(int iCur, int iStart, int iEnd)
    {
        if (iCur >= iEnd)
            iCur = iStart;
        return iCur;
    }
    // 방향벡터
    public static Vector3 GetDirection(Vector3 vFrom, Vector3 vTo)
    {
        return (vTo - vFrom).normalized;
    }
    // 벡터 요소곱하기
    public static Vector3 MulToVector(Vector3 vValue1, Vector3 vValue2)
    {
        Vector3 vMul = Vector3.zero;
        vMul.x = vValue1.x * vValue2.x;
        vMul.y = vValue1.y * vValue2.y;
        vMul.z = vValue1.z * vValue2.z;
        return vMul;
    }
}