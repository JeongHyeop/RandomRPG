using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenuScene : MonoBehaviour {   
    //옵션
    Button optionButton;
    public GameObject optionPanel;
    bool bOptionActive = false;

    //맵 변환시 중앙 UI 버튼
    public Image centerButtonImg;
    public Text centerButtonText;
    public GameObject[] uiCenterObject;
    bool bCenterButtonCheck;

    //맵 변환
    public MeshRenderer mapPlane;
    public Skybox sky;
    MenuSceneMapData menuMapData;
    int nCurIndex = 0;
    int nMaxIndex = 0;
    eUIState uiState;

    //라이트 & 효과
    public GameObject playerLight;
    public GameObject rankObj;
    public GameObject shopObj;
    public GameObject huntObj;

    //플레이어
    Player player;
    public GameObject playerObj;

    //플레이어 UI
    public Text levelText;
    public Text nickNameText;
    public Text gold;
    public Slider exp;
    public Text expText;
    bool bLevelActive = false;

    //플레이어 정보 UI
    public GameObject infoPanel;
    public Text infoNickname;
    public Text point;
    public Text hp;
    public Text mp;    
    public Text attack;
    public Text defence;
    public Text skillAttack;
    public Text skillCoolTime;

    //플레이어 정보 버튼 UI
    Button hpUpButton;
    Button attackUpButton;
    Button defenceUpButton;
    Button skillAttackUpButton;
    Button skillCoolTimeUpButton;

    //아이템 버튼
    Button weaponButton;
    Button helmetButton;
    Button accButton;

	void Start () {
        Init();        
	}
    void Init()
    {        
        bCenterButtonCheck = false;
        uiCenterObject[nCurIndex].SetActive(bCenterButtonCheck);

        playerLight.SetActive(true);
        
        //데이터 로드
        CGame.Instance.LocalDB_load();

        //맵 로드
        MapLoad();

        //플레이어 로드        
        PlayerLoad();

        //bgm        
        CGame.Instance.SetBGM(CGame.Instance.PlaySound((int)eSound.eSound_MainMenuBGM, GameObject.Find("Main Camera"), true));
        
        //UI        
        optionButton = GameObject.Find("OptionButton").GetComponent<Button>();
        optionButton.onClick.AddListener(OptionButton);

        expText.text = player.playerData.exp + " / " + player.maxExp;

        hpUpButton = CGame.Instance.GameObject_get_child(infoPanel, "HpUpButton").GetComponent<Button>();
        hpUpButton.onClick.AddListener(delegate { AbilityUp(eCharacterInformation.eCharacterInfo_HP); });

        attackUpButton = CGame.Instance.GameObject_get_child(infoPanel, "AttackUpButton").GetComponent<Button>();
        attackUpButton.onClick.AddListener(delegate { AbilityUp(eCharacterInformation.eCharacterInfo_Attack); });

        defenceUpButton = CGame.Instance.GameObject_get_child(infoPanel, "DefenceUpButton").GetComponent<Button>();
        defenceUpButton.onClick.AddListener(delegate { AbilityUp(eCharacterInformation.eCharacterInfo_Defence); });

        skillAttackUpButton = CGame.Instance.GameObject_get_child(infoPanel, "SkillAttackUpButton").GetComponent<Button>();
        skillAttackUpButton.onClick.AddListener(delegate { AbilityUp(eCharacterInformation.eCharacterInfo_SkillAttack); });

        skillCoolTimeUpButton = CGame.Instance.GameObject_get_child(infoPanel, "SkillCoolTimeUpButton").GetComponent<Button>();
        skillCoolTimeUpButton.onClick.AddListener(delegate { AbilityUp(eCharacterInformation.eCharacterInfo_SkillCollTime); });

        weaponButton = GameObject.Find("WeaponButton").GetComponent<Button>();
        weaponButton.onClick.AddListener(WeaponButton);

        helmetButton = GameObject.Find("HelemetButton").GetComponent<Button>();
        helmetButton.onClick.AddListener(HelmetButton);

        accButton = GameObject.Find("AccButton").GetComponent<Button>();
        accButton.onClick.AddListener(AccButton);
        UIItemLoad();

        //레벨        
        if (player.updateExp != 0)
            StartCoroutine(LevelUp());

        AbilityUp((eCharacterInformation)(-1));
    }
    void UIItemLoad()
    {
        //무기
        Image gradeImg = weaponButton.GetComponent<Image>();
        Image itemImg = CGame.Instance.GameObject_get_child(gradeImg.gameObject, "ItemImage").GetComponent<Image>();

        if (player.playerCharacter.equippingItem.equippingWeapon.itemIndex != -1)
        {
            gradeImg.sprite = CGame.Instance.GetImage("Item/GradeIconImg" + (int)player.playerCharacter.equippingItem.equippingWeapon.grade);
            itemImg.sprite = CGame.Instance.GetImage("Item/" + (int)player.playerCharacter.equippingItem.weaponIndex);
        }
        else
        {
            gradeImg.sprite = CGame.Instance.GetImage("Item/GradeIconImg0");
            itemImg.sprite = null;
        }

        //헬멧
        gradeImg = helmetButton.GetComponent<Image>();
        itemImg = CGame.Instance.GameObject_get_child(gradeImg.gameObject, "ItemImage").GetComponent<Image>();

        if (player.playerCharacter.equippingItem.equippingHelmet.itemIndex != -1)
        {
            gradeImg.sprite = CGame.Instance.GetImage("Item/GradeIconImg" + (int)player.playerCharacter.equippingItem.equippingHelmet.grade);
            itemImg.sprite = CGame.Instance.GetImage("Item/" + (int)player.playerCharacter.equippingItem.helmetIndex);
        }
        else
        {
            gradeImg.sprite = CGame.Instance.GetImage("Item/GradeIconImg0");
            itemImg.sprite = null;
        }

        //악세서리
        gradeImg = accButton.GetComponent<Image>();
        itemImg = CGame.Instance.GameObject_get_child(gradeImg.gameObject, "ItemImage").GetComponent<Image>();

        if (player.playerCharacter.equippingItem.equippingAcc.itemIndex != -1)
        {
            gradeImg.sprite = CGame.Instance.GetImage("Item/GradeIconImg" + (int)player.playerCharacter.equippingItem.equippingAcc.grade);
            itemImg.sprite = CGame.Instance.GetImage("Item/" + (int)player.playerCharacter.equippingItem.accessoriIndex);
        }
        else
        {
            gradeImg.sprite = CGame.Instance.GetImage("Item/GradeIconImg0");
            itemImg.sprite = null;
        }
    }

    private void WeaponButton()
    {
        if (player.playerCharacter.equippingItem.weaponIndex != -1)
        {
            CGame.Instance.itemPanel.CallItemInfo(player.playerCharacter.equippingItem.weaponIndex, eScene.eScene_MainMenu);
        }
    }
    private void HelmetButton()
    {
        if (player.playerCharacter.equippingItem.helmetIndex != -1)
        {
            CGame.Instance.itemPanel.CallItemInfo(player.playerCharacter.equippingItem.helmetIndex, eScene.eScene_MainMenu);
        }
    }
    private void AccButton()
    {
        if (player.playerCharacter.equippingItem.accessoriIndex != -1)
        {
            CGame.Instance.itemPanel.CallItemInfo(player.playerCharacter.equippingItem.accessoriIndex, eScene.eScene_MainMenu);
        }
    }

    IEnumerator LevelUp()
    {
        if (player.updateExp <= 0)
            StopCoroutine(LevelUp());

        player.maxExp = player.playerData.level * 300;

        bLevelActive = true;

        while (player.updateExp >= 0)
        {
            if (player.playerData.exp < player.maxExp)
            {                
                if (player.updateExp >= 100)
                {
                    player.playerData.exp += 100;
                    player.updateExp -= 100;
                }
                else if (player.updateExp < 100)
                {
                    player.playerData.exp += 10;
                    player.updateExp -= 10;
                }
                else
                {
                    player.playerData.exp += 1;
                    player.updateExp -= 1;
                }

                exp.value = (float)player.playerData.exp / (float)player.maxExp;
                expText.text = player.playerData.exp + " / " + player.maxExp;
                yield return null;
            }
            if (player.playerData.exp >= player.maxExp)
            {
                CGame.Instance.PlaySound((int)eSound.eSound_LevelUp, GameObject.Find("Main Camera"), false);
                CGame.Instance.PlayFx("LevelUp", player.playerCharacter.characterObject.transform.position);
                player.playerData.level += 1;
                levelText.text = player.playerData.level.ToString();
                player.playerData.exp = 0;
                player.playerData.point += 3;
                player.maxExp = player.playerData.level * 300;
                exp.value = 0;
                point.text = player.playerData.point.ToString();
                AbilityUp((eCharacterInformation)(-1));                
            }
        }

        bLevelActive = false;

        point.text = player.playerData.point.ToString();
        expText.text = player.playerData.exp + " / " + player.maxExp;
                
        CGame.Instance.LocalDB_save();
        yield return null;
    }
    public void AbilityUp(eCharacterInformation _eAbility)
    {
        if (player.playerData.point > 0)
        {
            hpUpButton.gameObject.SetActive(true);
            attackUpButton.gameObject.SetActive(true);
            defenceUpButton.gameObject.SetActive(true);
            skillAttackUpButton.gameObject.SetActive(true);
            skillCoolTimeUpButton.gameObject.SetActive(true);

            if ((int)_eAbility < 0)
                return;

            switch (_eAbility)
            {
                case eCharacterInformation.eCharacterInfo_HP:
                    player.playerCharacter.hp += 30;
                    hp.text = player.playerCharacter.hp.ToString() + " (+" +
                    (player.playerCharacter.equippingItem.equippingHelmet.hp +
                    player.playerCharacter.equippingItem.equippingAcc.hp +
                    player.playerCharacter.equippingItem.equippingWeapon.hp).ToString() + ")";
                    player.playerCharacter.maxHP = player.playerCharacter.hp;
                    break;
                case eCharacterInformation.eCharacterInfo_MP:
                    break;
                case eCharacterInformation.eCharacterInfo_Attack:
                    player.playerCharacter.attack += 10;
                    attack.text = player.playerCharacter.attack.ToString() + " (+" +
                    (player.playerCharacter.equippingItem.equippingHelmet.attack +
                    player.playerCharacter.equippingItem.equippingAcc.attack +
                    player.playerCharacter.equippingItem.equippingWeapon.attack).ToString() + ")";
                    break;
                case eCharacterInformation.eCharacterInfo_Defence:
                    player.playerCharacter.defence += 5;
                    defence.text = player.playerCharacter.defence.ToString() + " (+" +
                    (player.playerCharacter.equippingItem.equippingHelmet.defence +
                    player.playerCharacter.equippingItem.equippingAcc.defence +
                    player.playerCharacter.equippingItem.equippingWeapon.defence).ToString() + ")";
                    break;
                case eCharacterInformation.eCharacterInfo_SkillAttack:
                    player.playerCharacter.skillAttack += 50;
                    skillAttack.text = player.playerCharacter.skillAttack.ToString() + " (+" +
                    (player.playerCharacter.equippingItem.equippingHelmet.skillAttack +
                    player.playerCharacter.equippingItem.equippingAcc.skillAttack +
                    player.playerCharacter.equippingItem.equippingWeapon.skillAttack).ToString() + ")";
                    break;
                case eCharacterInformation.eCharacterInfo_SkillCollTime:
                    if (player.playerCharacter.skillCoolTime > 3.0f)
                    {
                        player.playerCharacter.skillCoolTime -= 0.2f;
                        skillCoolTime.text = player.playerCharacter.skillCoolTime.ToString() + " (-" +
                        (player.playerCharacter.equippingItem.equippingHelmet.skillCoolTime +
                        player.playerCharacter.equippingItem.equippingAcc.skillCoolTime +
                        player.playerCharacter.equippingItem.equippingWeapon.skillCoolTime).ToString() + ")";
                    }
                    else
                    {
                        skillCoolTimeUpButton.gameObject.SetActive(false);
                        return;
                    }
                    break;                
            }
            CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);

            player.playerData.point -= 1;
            point.text = player.playerData.point.ToString();

            player.playerCharacter.PlayerCharacterSave();
            CGame.Instance.LocalDB_save();

            if (player.playerData.point <= 0)
            {
                hpUpButton.gameObject.SetActive(false);
                attackUpButton.gameObject.SetActive(false);
                defenceUpButton.gameObject.SetActive(false);
                skillAttackUpButton.gameObject.SetActive(false);
                skillCoolTimeUpButton.gameObject.SetActive(false);
            }
        }
        else
        {            
            if (hpUpButton.gameObject.activeSelf == true)
            {
                hpUpButton.gameObject.SetActive(false);
                attackUpButton.gameObject.SetActive(false);
                defenceUpButton.gameObject.SetActive(false);
                skillAttackUpButton.gameObject.SetActive(false);
                skillCoolTimeUpButton.gameObject.SetActive(false);
            }
        }
    }
    void PlayerLoad()
    {
        //플레이어 얻기
        player = CGame.Instance.player;

        //플레이어 캐릭터 로드
        player.playerCharacter.PlayerCharacterLoad(playerObj);
        player.playerCharacter.characterObject.transform.rotation = new Quaternion(0, 0.5f, 0, 0.0f);

        //기본 플레이어 UI        
        nickNameText.text = player.playerData.nickName;
        gold.text = player.playerData.gold.ToString();      

        player.maxExp = player.playerData.level * 300;

        exp.value = (float)player.playerData.exp / (float)player.maxExp;
        levelText.text = player.playerData.level.ToString();
        
        //정보 UI
        infoNickname.text = nickNameText.text;
        point.text = player.playerData.point.ToString();

        hp.text = player.playerCharacter.hp.ToString() + " (+" + 
            (player.playerCharacter.equippingItem.equippingHelmet.hp +
            player.playerCharacter.equippingItem.equippingAcc.hp +
            player.playerCharacter.equippingItem.equippingWeapon.hp).ToString() + ")";

        mp.text = player.playerCharacter.mp.ToString() + " (+" +
            (player.playerCharacter.equippingItem.equippingHelmet.mp + 
            player.playerCharacter.equippingItem.equippingAcc.mp + 
            player.playerCharacter.equippingItem.equippingWeapon.mp).ToString() + ")";      

        attack.text = player.playerCharacter.attack.ToString() + " (+" + 
            (player.playerCharacter.equippingItem.equippingHelmet.attack + 
            player.playerCharacter.equippingItem.equippingAcc.attack +
            player.playerCharacter.equippingItem.equippingWeapon.attack).ToString() + ")";

        defence.text = player.playerCharacter.defence.ToString() + " (+" + 
            (player.playerCharacter.equippingItem.equippingHelmet.defence + 
            player.playerCharacter.equippingItem.equippingAcc.defence + 
            player.playerCharacter.equippingItem.equippingWeapon.defence).ToString() + ")";

        skillAttack.text = player.playerCharacter.skillAttack.ToString() + " (+" + 
            (player.playerCharacter.equippingItem.equippingHelmet.skillAttack + 
            player.playerCharacter.equippingItem.equippingAcc.skillAttack +
            player.playerCharacter.equippingItem.equippingWeapon.skillAttack).ToString() + ")";

        skillCoolTime.text = player.playerCharacter.skillCoolTime.ToString() + " (-" +
            (player.playerCharacter.equippingItem.equippingHelmet.skillCoolTime +
            player.playerCharacter.equippingItem.equippingAcc.skillCoolTime +
            player.playerCharacter.equippingItem.equippingWeapon.skillCoolTime).ToString() + ")";

    }
    void MapLoad()
    {
        nMaxIndex = CGame.Instance.dataTable.dataTableMenuScene.Length - 2;
        uiState = (eUIState)nCurIndex;

        menuMapData = new MenuSceneMapData();
        menuMapData.InitData();

        mapPlane.material = menuMapData.menuSceneData[nCurIndex].plane;
        sky.material = menuMapData.menuSceneData[nCurIndex].sky;
    }
    public void LeftButton()
    {
        if (bLevelActive == false)
        {
            CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
            nCurIndex = nCurIndex >= 1 ? nCurIndex - 1 : nMaxIndex;
            mapPlane.material = menuMapData.menuSceneData[nCurIndex].plane;
            sky.material = menuMapData.menuSceneData[nCurIndex].sky;
            SetUIState((eUIState)nCurIndex);
            UIItemLoad();
        }
        else
            StartCoroutine(CGame.Instance.ICallNotice("Level Up..."));
    }
    public void RightButton()
    {
        if (bLevelActive == false)
        {
            CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
            nCurIndex = nCurIndex < nMaxIndex ? nCurIndex + 1 : 0;
            mapPlane.material = menuMapData.menuSceneData[nCurIndex].plane;
            sky.material = menuMapData.menuSceneData[nCurIndex].sky;
            SetUIState((eUIState)nCurIndex);
            UIItemLoad();
        }
        else
            StartCoroutine(CGame.Instance.ICallNotice("Level Up..."));
    }
    void OptionButton()
    {
        if (bLevelActive == false)
        {
            CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
            bOptionActive = bOptionActive == false ? true : false;
            optionPanel.SetActive(bOptionActive);
        }
        else
            StartCoroutine(CGame.Instance.ICallNotice("Level Up..."));
    }
    void SetUIState(eUIState _uiState)
    {
        for (int i = 0; i < uiCenterObject.Length; i++)
            uiCenterObject[i].SetActive(false);
        bCenterButtonCheck = false;

        switch (_uiState)
        {
            case eUIState.eUIState_Info:
                //centerButtonImg.sprite = CGame.Instance.GetImage("InfoButton");
                centerButtonText.text = "Info";
                playerLight.SetActive(true);
                rankObj.SetActive(false);
                shopObj.SetActive(false);
                huntObj.SetActive(false);
                break;
            case eUIState.eUIState_Rank:
                //centerButtonImg.sprite = CGame.Instance.GetImage("ShopButton");
                centerButtonText.text = "Rank";                
                playerLight.SetActive(false);
                rankObj.SetActive(true);
                shopObj.SetActive(false);
                huntObj.SetActive(false);
                break;
            case eUIState.eUIState_Shop:
                //centerButtonImg.sprite = CGame.Instance.GetImage("RankButton");
                centerButtonText.text = "Shop";                
                playerLight.SetActive(false);
                rankObj.SetActive(false);
                shopObj.SetActive(true);
                huntObj.SetActive(false);
                break;
            case eUIState.eUIState_Hunt:
                //centerButtonImg.sprite = CGame.Instance.GetImage("HuntButton");
                centerButtonText.text = "Hunt";
                playerLight.SetActive(true);
                rankObj.SetActive(false);
                shopObj.SetActive(false);
                huntObj.SetActive(true);
                break;
        }
    }
    public void CenterButton()
    {
        if (nCurIndex == null)
            return;

        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);

        uiState = (eUIState)nCurIndex;

        if (uiState == eUIState.eUIState_Info || uiState == eUIState.eUIState_Rank || uiState == eUIState.eUIState_Shop)
        {
            bCenterButtonCheck = bCenterButtonCheck == false ? true : false;
            uiCenterObject[nCurIndex].SetActive(bCenterButtonCheck);

            //추후 다시 작업
            if(bCenterButtonCheck == true && uiState == eUIState.eUIState_Rank )
                StartCoroutine(CGame.Instance.ICallNotice("Coming Soon!"));

            CGame.Instance.LocalDB_save();
        }
        else if (uiState == eUIState.eUIState_Hunt)
        {
            CGame.Instance.SceneChange(4);      
        }
    }
}
public class MenuSceneMapData
{
    public List<MenuSceneMap> menuSceneData = new List<MenuSceneMap>();
    public void InitData()
    {
        for (int i = 0; i < CGame.Instance.dataTable.dataTableMenuScene.Length - 1; i++)
        {
            MenuSceneMap newData = new MenuSceneMap();

            newData.plane = CGame.Instance.GetMaterial(CGame.Instance.dataTable.dataTableMenuScene[i].plane);
            newData.sky = CGame.Instance.GetMaterial(CGame.Instance.dataTable.dataTableMenuScene[i].sky);

            menuSceneData.Add(newData);
        }
    }
}
public class MenuSceneMap
{
    public Material plane;
    public Material sky;
}
public enum eUIState
{
    eUIState_Info = 0,
    eUIState_Rank = 1,
    eUIState_Shop = 2,    
    eUIState_Hunt = 3,
}