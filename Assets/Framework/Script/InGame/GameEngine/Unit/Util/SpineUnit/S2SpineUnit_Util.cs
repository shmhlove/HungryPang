/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 스파인 유닛 관련 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Spine;

public abstract partial class S2SpineUnit : S2Unit
{
    public void SetDirection(eDirection eDir)
    {
        if (false == CheckToAnimation())
            return;

        m_eDirection = eDir;
        m_pAnimation.skeleton.FlipX = (eDirection.Left == eDir);
    }

    // 유틸 : Before에서 Now까지 지난간 프레임 리스트 얻기
    List<int> GetPastFrameList(int iBeforeFrame, int iNowFrame, int iMaxFrame)
    {
        List<int> iFrames = new List<int>();

        if (iBeforeFrame == iNowFrame)
        {
            return iFrames;
        }
        else if (iBeforeFrame < iNowFrame)
        {
            for (int iLoop = (iBeforeFrame + 1); iLoop <= iNowFrame; ++iLoop)
                iFrames.Add(iLoop);
        }
        else
        {
            for (int iLoop = (iBeforeFrame + 1); iLoop <= iMaxFrame; ++iLoop)
                iFrames.Add(iLoop);
            for (int iLoop = 0; iLoop <= iNowFrame; ++iLoop)
                iFrames.Add(iLoop);
        }

        return iFrames;
    }

    // 체크 : 애니메이션 종료체크(End 이벤트 콜에 필요)
    bool CheckToEndAnimation(S2ActionState pState)
    {
        // 이 조건은 모션이 완전 정지되었을때(OnePlay모션들 Play될때까지 계속 콜 된다)
        if (true == IsEndAnim())
            return true;

        // 이 조건은 모션이 다시 처음으로 돌아갔을때(루프모션)
        if (pState.m_iFrame < pState.m_iBeforeFrame)
            return true;
        
        return false;
    }

    // 예외처리 : 애니메이션 컴포넌트 체크
    bool CheckToAnimation()
    {
        return (null != m_pAnimation);
    }

    // 예외처리 : 상태정보체크
    bool CheckToStateMachine(int iStateID)
    {
        return m_dicState.ContainsKey(iStateID);
    }
}