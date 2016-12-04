/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 15일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 게임내 모든 데이터를 관리합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class S2DataSGT : S2Singleton<S2DataSGT>
{
    // 테이블 데이터
    private S2TableData m_pTableData = new S2TableData();
    public S2TableData TableData { get { return m_pTableData; } }

    // 리소스 데이터
    private S2ResourcesData m_pResourcesData = new S2ResourcesData();
    public S2ResourcesData ResourcesData { get { return m_pResourcesData; } }

    // 씬 데이터
    private S2SceneData m_pSceneData = new S2SceneData();
    public S2SceneData SceneData { get { return m_pSceneData; } }

    // 로더
    private S2Loader m_pLoader = new S2Loader();
    public S2Loader Loader { get { return m_pLoader; } }

    // 시스템 : 생성자
    public override void OnInitialize()
    {
        TableData.OnInitialize();
        ResourcesData.OnInitialize();
        SceneData.OnInitialize();

        SetDontDestroy();
    }

    // 시스템 : 소멸자
    public override void OnFinalize()
    {
        TableData.OnFinalize();
        ResourcesData.OnFinalize();
        SceneData.OnInitialize();
    }
}
