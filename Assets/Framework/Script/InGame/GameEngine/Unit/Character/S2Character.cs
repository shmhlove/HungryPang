/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 03일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 캐릭터 관련 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2CharacterInspector : MonoBehaviour
{
    public S2Character pInfo = null;

    // 데브 : 상태변경관련
    [S2AttributeToShowFunc]
    public void DevPlayState()
    {
        if (null == pInfo)
            return;

        pInfo.DevPlayState();
    }
}

[Serializable]
public partial class S2Character : S2SpineUnit
{
    // 다양화 : 업데이트
    public override void FrameMove()
    {
        base.FrameMove();

        if (true == m_bIsPause)
            return;

#if UNITY_EDITOR
        if (true == Input.GetKeyDown(KeyCode.Space))
            DevPlayState();
#endif
    }

    // 인터페이스 : 캐릭터 생성
    public bool CreateCharacter(string strPrefab)
    {
        if (false == CreateSpineUnit(strPrefab, strPrefab, eUnitType.Character))
            return ErrorReturn();

        // 하이어라키
        SetParent("S2Character");

        // 유닛 컴포넌트 추가
        S2CharacterInspector pInspector = GetComponent<S2CharacterInspector>();
        pInspector.pInfo = this;

        // 퍼즐 이벤트 등록
        Single.Puzzle.EventToMatch.Add(OnEventToMatchPuzzle);

        return true;
    }

    // 이벤트 : 퍼즐매치
    public void OnEventToMatchPuzzle(object pSender, EventArgs vArgs)
    {
        S2PuzzleMatchEvent pCrushInfo = Single.Event.GetArgs<S2PuzzleMatchEvent>(vArgs);
        S2Util.ForeachToDic<ePuzzleBlockType, List<S2BlockInfo>>(pCrushInfo.m_dicMatchBlocks, (eType, pMatchBlocks) =>
        {
            if (0 == pMatchBlocks.Count)
                return;

            //if (pCrushInfo.m_eComboType == eType)
            //{
                if (false == IsState(eCharacterState.Idle))
                    return;

                switch (eType)
                {
                    case ePuzzleBlockType.Blue:     ChangeToState(eCharacterState.AttackBlue);    break;
                    case ePuzzleBlockType.Green:    ChangeToState(eCharacterState.AttackGreen);   break;
                    case ePuzzleBlockType.Orange:   ChangeToState(eCharacterState.AttackOrange);  break;
                    case ePuzzleBlockType.Red:      ChangeToState(eCharacterState.AttackRed);     break;
                    case ePuzzleBlockType.Violet:   ChangeToState(eCharacterState.AttackViolet);  break;
                }
            //}
        });
    }

    // 인터페이스 : 캐릭터 제거
    public void DestroyCharacter()
    {
        //m_pBaseInfo = null;
        DestroySpineUnit();
    }
}