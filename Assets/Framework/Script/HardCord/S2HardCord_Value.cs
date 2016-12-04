/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 18일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 하드 코드를 모아둡니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.IO;
using System.Collections;

public partial class S2HardCord : S2Singleton<S2HardCord>
{
    // 퍼즐판 관련
    public float m_fComboTimer             = 10.0f;

    // 블럭 텍스쳐 이름
    public string m_strBlockBlue           = "BlockBlue";
    public string m_strBlockGreen          = "BlockGreen";
    public string m_strBlockOrange         = "BlockOrange";
    public string m_strBlockRed            = "BlockRed";
    public string m_strBlockViolet         = "BlockViolet";

    // 데미지 프리팹 이름
    public string m_strDamageBlue           = "DamageBlue";
    public string m_strDamageGreen          = "DamageGreen";
    public string m_strDamageOrange         = "DamageOrange";
    public string m_strDamageRed            = "DamageRed";
    public string m_strDamageViolet         = "DamageViolet";
}
