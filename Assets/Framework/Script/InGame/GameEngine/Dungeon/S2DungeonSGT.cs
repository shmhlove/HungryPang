/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 11월 09일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 던전 싱글턴 클래스로 던전 오브젝트에 외부접근을 관리하고, 생성 / 제거를 관리 합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System.Collections;

public class S2DungeonSGT : S2Singleton<S2DungeonSGT>
{
    // 던전 오브젝트
    private S2Dungeon   m_pDungeon  = null;

    // 기타
    public bool         m_bIsPause  = false;

    // 다양화 : 초기화
    public override void OnInitialize()
    {
        m_pDungeon  = null;
        m_bIsPause  = false;
    }
    // 다양화 : 종료
    public override void OnFinalize()
    {
    }

    // 인터페이스 : 던전 생성
    public void CreateDungeon(string strDunName)
    {
        GameObject pDungeon = Single.ResourceData.GetGameObject(strDunName);
        if (null == pDungeon)
        {
            S2Util.LogError("던전 오브젝트가 없습니다.");
            return;
        }

        m_pDungeon = pDungeon.GetComponent<S2Dungeon>();
        if (null == m_pDungeon)
        {
            S2Util.LogError("던전 오브젝트에 S2Dungeon 컴포넌트가 없습니다.");
            S2GameObject.DestoryObject(pDungeon);
            return;
        }

        S2GameObject.SetParent(pDungeon, gameObject);
        m_pDungeon.OnInitialize();
    }

    // 인터페이스 : 업데이트
    public void FrameMove()
    {
        if (true == m_bIsPause)
            return;

        if (null == m_pDungeon)
            return;

        m_pDungeon.FrameMove();
    }

    // 시스템 : 정지
    public void Pause()
    {
        m_bIsPause = true;
    }

    // 시스템 : 재개
    public void Resume()
    {
        m_bIsPause = false;
    }
}
