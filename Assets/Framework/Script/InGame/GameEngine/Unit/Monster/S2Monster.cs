/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 08월 09일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 몬스터 관련 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class S2MonsterInspector : MonoBehaviour
{
    public S2Monster pInfo = null;

    // 데브 : 상태변경관련
    public List<string> GetState()
    {
        return pInfo.GetStateList();
    }
    [S2AttributeToShowFunc]
    public void DevPlayState()
    {
        if (null == pInfo)
            return;

        pInfo.DevPlayState(m_eDevStateID);
    }
    [S2AttributeToMonsterState]
    public string m_eDevStateID;
}

[Serializable]
public partial class S2Monster : S2SpineUnit
{
    // 다양화(이벤트) : 데미지 충돌
    public override void CrushDamage(S2Damage pDamage)
    {
        AddHP(-pDamage.GetDamageValue());
    }
    // 다양화 : 애니메이션 클립명
    public override string GetClipName(int iStateID)
    {
        S2Util.LogError("{0} 몬스터 AI 클래스에서 GetClipName함수를 정의해야합니다!!", m_strUnitID);
        return string.Empty;
    }
    // 다양화 : 초기화
    public override void Initialize()
    {
        S2Util.LogError("{0} 몬스터 AI 클래스에서 Initialize함수를 정의해야합니다!!", m_strUnitID);
    }
    // 다양화 : 상태정의
    public override void SetActionTable()
    {
        S2Util.LogError("{0} 몬스터 AI 클래스에서 SetActionTable함수를 정의해야합니다!!", m_strUnitID);
    }
    
    // 다양화 : 업데이트
    public override void FrameMove()
    {
        base.FrameMove();
    }
    
    // 다양화 : 데브(상태변경)
    public virtual void DevPlayState(string strStateID) { }
    public virtual List<string> GetStateList() { return new List<string>(); }

    // 인터페이스 : 몬스터 생성
    public bool CreateMonster(string strPrefab, eMonster eType)
    {
        if (false == CreateSpineUnit(strPrefab, ((int)eType).ToString(), eUnitType.Monster))
            return ErrorReturn();

        // 타입 설정
        m_eMonsterType = eType;

        // 유닛 컴포넌트 추가
        S2MonsterInspector pInspector = m_pObject.AddComponent<S2MonsterInspector>();
        pInspector.pInfo = this;

        return true;
    }

    // 인터페이스 : 몬스터 제거
    public void DestroyMonster()
    {
        CallEventToDestroy();
        DestroySpineUnit();
    }

    // 인터페이스 : 이벤트등록(몬스터 제거)
    public void AddEventToDestroy(Action<S2Monster> pEvent)
    {
        m_pEventToDestroy.Add(pEvent);
    }
}