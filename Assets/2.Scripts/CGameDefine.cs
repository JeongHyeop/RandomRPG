using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharacterInformation
{
    eCharacterInfo_HP = 0,              //캐릭터 HP
    eCharacterInfo_MP = 1,              //캐릭터 MP    
    eCharacterInfo_Attack = 2,          //캐릭터 공격력
    eCharacterInfo_Defence = 3,         //캐릭터 방어력
    eCharacterInfo_SkillAttack = 4,     //캐릭터 스킬 공격력
    eCharacterInfo_SkillCollTime = 5,   //캐릭터 스킬 쿨타임
    eCharacterInfo_Exp = 6,             //캐릭터 경험치
}

public enum eCharacterType
{
    eCharacterType_Hero = 0,            //플레이어 영웅
    eCharacterType_Enemy = 1,           //적
}

public enum eLevelOfDifficulty
{
    eLevelOfDifficulty_Bonus = 0,        //게임 난이도 - 보너스
    eLevelOfDifficulty_Easy = 1,         //게임 난이도 - 하
    eLevelOfDifficulty_Normal = 2,       //게임 난이도 - 중
    eLevelOfDifficulty_Hard = 3,         //게임 난이도 - 상
    eLevelOfDifficulty_SuperHard = 4,    //게임 난이도 - 최상   
}

public enum eMap
{
    eMap_fire = 0,
}
public enum eItemGrade
{
    eItemGrade_Base      = 0,           //기본
    eItemGrade_Normal    = 1,           //일반
    eItemGrade_Rare      = 2,           //진귀
    eItemGrade_SuperRare = 3,           //초진귀
    eItemGrade_UltraRare = 4,           //초초진귀
    eItemGrade_Legend    = 5,           //전설
}
public enum eItemType
{
    eitemType_None = -1,                 //없다
    eitemType_Helmet = 1,                //헬멧
    eitemType_Weapon = 2,                //무기
    eitemType_Accessori = 3,             //보조
}
public enum eWeaponType
{
    eWeaponType_None    = -1,           //무기가 아님
    eWeaponType_Club    = 0,            //몽둥이
    eWeaponType_Dagger  = 1,            //단검
    eWeaponType_Wand    = 2,            //완드
    eWeaponType_Axe     = 3,            //도끼
    eWeaponType_Arrow   = 4,            //화살
    eWeaponType_Hammer  = 5,            //망치
    eWeaponType_Knuckle = 6,            //너클
    eWeaponType_Mace    = 7,            //둔기
    eWeaponType_Scythe  = 8,            //낫
    eWeaponType_Spear   = 9,            //창
    eWeaponType_Sword   = 10,           //대검
    eWeaponType_Special = 11,           //스페셜    
}

public enum eBeautyType
{
    eBeautyType_Hair = 0,               //머리
    eBeautyType_Body = 1,               //몸
}
public enum eGenderType
{
    eGenderType_Female = 0,              //여성
    eGenderType_Male =1,                 //남성
}
public enum eBattleState
{
    eBattleState_None = -1,
    eBattleState_init = 0,
    eBattleState_play = 1,
    eBattleState_result = 2,
};
public enum eBattleResult
{
    eBattleResult_Draw = 0,
    eBattleResult_PlayerWin = 1,
    eBattleResult_EnemyWin = 2,
}
public enum eSound
{
    eSound_Button = 0,
    eSound_LevelUp = 1,
    eSound_MainMenuBGM = 2,
    eSound_HuntBGM = 3,
    eSound_Coin = 4,
    eSound_GameIntro = 5,
    eSound_MaleHit = 6,
    eSound_FemaleHit = 7,
    eSound_MaleDie = 8,
    eSound_FemaleDie = 9,
    eSound_Win = 10,
    eSound_Lose = 11,
    eSound_Draw = 12,
    eSound_Jump = 13,

    //무기 Sound
    eSound_blunt = 14,          //몽둥이 등 둔기종류
    eSound_Sword = 15,          //단검, 한손검 등 얇은 검
    eSound_Sword2 = 16,         //도끼, 낫, 창 등
    eSound_Magic = 17,          //마법 무기 종류    
    eSound_Special = 25,         //스페셜    
}
// Act -------------------------------------
public enum eCharacterAct // animation + state
{
    None,
    appear,     // create, pos
    disappear,  // delete
    idle,
    walk,
    Jump,
    run,
    attack,     
    skillAttack,
    hit,        
    die,
    corpse,
    Max
};
