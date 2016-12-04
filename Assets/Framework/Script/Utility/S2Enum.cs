/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 공통 Enum을 관리하는 클래스 입니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/

// 씬 종류
public enum eSceneType
{
    None,
    Loading,
    StartGame,
    InGame,
}

// 데이터 종류
public enum eDataType
{
    None,
    Table,
    Resources,
    Scene,
    GameData,
}

// 테이블(테이블) 데이터 종류
public enum eTableType
{
    Static,
    Excel,
    DB,
    Json,
}

// 리소스 데이터 종류
public enum eResourceType
{
    None,
    Prefab,
    Texture,
    Sound,
}

// 리소스 로드타입
public enum eResourcesLoadType
{
    Local_Resource,         // 에셋번들 제작 전 실제 리소스 사용(Local)
    Local_AssetBundle,      // 에셋번들 제작 후, 로컬 에셋번들 사용(Local)
    Server_AssetBundle,     // 에셋번들 제작 후, 서버에 올려서 사용(Server)
}

// 리소스 로드방식
public enum eResourcesLoadMode
{
    AllLoad,                // 전체 리소스 모두 로드
    SceneLoad,              // 씬별 필요한 리소스 로드
    // 미구현 : 번들일 경우 번들만 올려두고 필요할때 즉각로드
}

// UI Anchor
public enum eAnchorType
{
    Bottom,
    Bottom_Left,
    Bottom_Right,
    Center,
    Left,
    Right,
    Top,
    Top_Left,
    Top_Right,
}

// 방향
public enum eDirection
{
    Left,
    Right,
    Up,
    Down,
}

// Bool
public enum eBOOL
{
    None,
    False,
    True,
}

// 게임엔진 컴포넌트
public enum eEngineComponent
{
    None,
    Damage,
    Effect,
    Puzzle,
    Character,
    Monster,
    Dungeon,
}

// 유닛타입
public enum eUnitType
{
    None,
    Character,
    Monster,
    UIUnitBlue,
    UIUnitGreen,
    UIUnitOrange,
    UIUnitRed,
    UIUnitViolet,
}

// 몬스터 타입
public enum eMonster
{
    None,
    Dragon,
    Max,
}

// 데미지 충돌 유닛타입
public enum eHitUnitType
{
    None,
    Character       = eUnitType.Character,
    Monster         = eUnitType.Monster,
    UIUnitBlue      = eUnitType.UIUnitBlue,
    UIUnitGreen     = eUnitType.UIUnitGreen,
    UIUnitOrange    = eUnitType.UIUnitOrange,
    UIUnitRed       = eUnitType.UIUnitRed,
    UIUnitViolet    = eUnitType.UIUnitViolet,
    Switch,
    ALL,
}

// 퍼즐블럭 타입
public enum ePuzzleBlockType
{
    None,
    Blue,
    Green,
    Orange,
    Red,
    Violet,
}