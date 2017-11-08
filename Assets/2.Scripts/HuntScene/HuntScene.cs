﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuntScene : MonoBehaviour {
    //맵 & 스카이
    public Skybox sky;
    public Camera mainCamera;
    GameObject map;
    DataTable_Hunt data;

    //플레이어
    public Text playerLevelText;
    public Text playerNickNameText;
    public Slider playerHP;
    public Slider playerMP;
    public GameObject playerObject;
    Player player = null;

    //Enemy
    public Text enemyLevelText;
    public Text enemyNickNameText;
    public Slider enemyHP;
    GameObject enemyObject;
    public EnemyCharacter enemy = null;
    int nEnemyIndex = -1;

    //Command UI
    Button jumpButton;
    Button attackButton;
    Button skillAttackButton;
    Image skillBtnImg;
    bool skillActive = true;    
    Animator skillBtnAnim;

    //UI
    Button optionButton;
    public GameObject optionPanel;
    bool bOptionActive = false;
    public GameObject difficultyPanel;
    public Image[] difficultyImg;
    HuntSceneUI huntUIManager;

    //결과
    public GameObject resultPanel;
    public Text resultText;
    public Text goldText;
    int nGold = 0;
    string strBattleResult;
    bool bResult = false;
    float delayTime = 0;
        
    //게임
    bool bGameLoading = false;
    eBattleState ebattleState;
    eBattleResult ebattleResult;
    float playTime = 0;

	void Start () {
        //DB 로드
        CGame.Instance.LocalDB_load();

        //맵 로드
        MapLoad();
        
        //Enemy 로드
        EnemyLoad();

        //Player 로드
        PlayerLoad();
        
        //조작 버튼
        jumpButton = GameObject.Find("JumpButton").GetComponent<Button>();
        jumpButton.onClick.AddListener(Jump);
        attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        attackButton.onClick.AddListener(Attack);
        skillAttackButton = GameObject.Find("SkillButton").GetComponent<Button>();
        skillBtnImg = CGame.Instance.GameObject_get_child(skillAttackButton.gameObject, "SkillImg").GetComponent<Image>();
        skillAttackButton.onClick.AddListener(SkillButton);
        skillBtnAnim = skillBtnImg.GetComponent<Animator>();
        skillBtnAnim.SetBool("BigAndSmall", skillActive);  

        //버튼
        optionButton = GameObject.Find("OptionButton").GetComponent<Button>();
        optionButton.onClick.AddListener(OptionButton);

        //BGM
        CGame.Instance.SetBGM(CGame.Instance.PlaySound((int)eSound.eSound_HuntBGM, GameObject.Find("Main Camera"), true));

        //배틀 로직
        BattleStateSet(eBattleState.eBattleState_init);
	}

    void Update()
    {
        //배틀 상황 업데이트
        BattleStateUpdate();
    }
    void BattleStateUpdate()
    {      
        if (bGameLoading == true)
        {
            playTime += Time.deltaTime;

            CameraZoom();
            //카메라 1인칭
            //player.playerCharacter.FirstPersonView(mainCamera);

            //쿼터뷰
           // player.playerCharacter.ThirdPersonView(mainCamera);

            if (player.playerCharacter.isDie == false && enemy.isDie == false)
                BattleStateSet(eBattleState.eBattleState_play);
            else
                BattleStateSet(eBattleState.eBattleState_result);
        }
    }
    void CameraZoom()
    {
        Vector3 moving = Vector3.Lerp(enemy.characterObject.transform.position, player.playerCharacter.characterObject.transform.position, playTime);
        moving.x += 4.2f;
        moving.y += 6.5f;
        moving.z -= 3.2f;
        mainCamera.transform.position = moving;
    }
    void BattleStateSet(eBattleState _state)
    {
        ebattleState = _state;

        switch (ebattleState)
        {
            case eBattleState.eBattleState_None:
                break;
            case eBattleState.eBattleState_init:
                BattleStateInit();
                break;
            case eBattleState.eBattleState_play:
                BattleStatePlay();
                break;
            case eBattleState.eBattleState_result:
                if (bResult == false)
                    BattleStateResultSet();
                else
                    BattleStateResult();
                break;
        }
    }
    void BattleStateInit()
    {
        //타겟 설정
        player.playerCharacter.SetTargetObject(enemyObject);
        enemy.SetTargetObject(player.playerCharacter.characterObject);
        enemy.playerCharacter = player.playerCharacter;

        //카메라
        mainCamera.transform.position = new Vector3(enemy.characterObject.transform.position.x + 4.2f, enemy.characterObject.transform.position.y + 6.5f, enemy.characterObject.transform.position.z - 3.2f);

        //난이도 패널 띄우기
        StartCoroutine(Difficulty());
    }
    IEnumerator Difficulty()
    {
        difficultyPanel.SetActive(true);
        for (int i = 0; i < data.levelOfDifficulty; i++)
        {
            difficultyImg[i].gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < data.levelOfDifficulty; i++)
        {
            difficultyImg[i].gameObject.SetActive(false);
        }
        difficultyPanel.SetActive(false);

        bGameLoading = true;
    }
    void BattleStatePlay()
    {
        //플레이어 업데이트
        player.playerCharacter.PlayerUpdate();
        if (player.playerCharacter.hp > 0)
            playerHP.value = (float)player.playerCharacter.hp / (float)player.playerCharacter.maxHP;
        else
        {
            playerHP.value = 0;
            player.playerCharacter.ActSet(eCharacterAct.die);
        }

        //적 업데이트            
        enemy.EnemyUpdate();
        if (enemy.hp > 0)
            enemyHP.value = (float)enemy.hp / (float)enemy.maxHP;
        else
        {
            enemyHP.value = 0;
            enemy.ActSet(eCharacterAct.die);
        }

        //적 방향표시
        HuntSceneUI.Instance.SetArrowSign(player.playerCharacter.characterObject, enemy.characterObject);
    }

    int nTemp = 0;
    void BattleStateResultSet()
    {
        if (enemy.isDie == true)
            ebattleResult = eBattleResult.eBattleResult_PlayerWin;
        if (player.playerCharacter.isDie == true)
            ebattleResult = eBattleResult.eBattleResult_EnemyWin;
        if (enemy.isDie == true && player.playerCharacter.isDie == true)
            ebattleResult = eBattleResult.eBattleResult_Draw;

        switch (ebattleResult)
        {
            case eBattleResult.eBattleResult_Draw:
                strBattleResult = "Draw";
                break;
            case eBattleResult.eBattleResult_PlayerWin:
                strBattleResult = "Win";
                CGame.Instance.PlaySound((int)eSound.eSound_Win, GameObject.Find("Main Camera"), false);
                enemy.characterObject.SetActive(false);
                break;
            case eBattleResult.eBattleResult_EnemyWin:
                CGame.Instance.PlaySound((int)eSound.eSound_Lose, GameObject.Find("Main Camera"), false);
                strBattleResult = "Lose";
                break;
        }
        resultText.text = strBattleResult;
        bResult = true;
        resultPanel.SetActive(bResult);
        nGold = Random.Range(enemy.level * 50, enemy.level * 100);
        nTemp = nGold / 2;
        goldText.text = "0";
    }    
    void BattleStateResult()
    {
        delayTime += Time.deltaTime;
        switch (ebattleResult)
        {
            case eBattleResult.eBattleResult_Draw:
                if (delayTime > 1.5f)
                {
                    goldText.text = "0";
                    CGame.Instance.LocalDB_save();
                    CGame.Instance.SceneChange(3);
                }
                break;
            case eBattleResult.eBattleResult_PlayerWin:
                //player.playerCharacter.Winner(mainCamera);
                if (nTemp < nGold)
                {
                    nTemp += 1;
                    CGame.Instance.PlaySound((int)eSound.eSound_Coin, GameObject.Find("Main Camera"), false);
                    goldText.text = nTemp.ToString();
                }
                else
                {
                    player.playerData.gold += nGold;
                    player.updateExp += enemy.exp;                                  

                    CGame.Instance.LocalDB_save();
                    CGame.Instance.SceneChange(3);
                }
                break;
            case eBattleResult.eBattleResult_EnemyWin:
                if (delayTime > 1.5f)
                {                    
                    goldText.text = "0";
                    //아이템 뺏기기

                    CGame.Instance.LocalDB_save();
                    CGame.Instance.SceneChange(3);
                }
                break;         
        }
    }
    void OptionButton()
    { 
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        bOptionActive = bOptionActive == false ? true : false;
        optionPanel.SetActive(bOptionActive);
    }
    void Jump()
    {
        player.playerCharacter.ActSet(eCharacterAct.Jump);
    }
    void Attack()
    {
        player.playerCharacter.ActSet(eCharacterAct.attack);
    }
    void PlayerLoad()
    {
        //플레이어 초기화
        player = CGame.Instance.player;
        player.playerCharacter.PlayerCharacterLoad(playerObject);
        player.playerCharacter.ActSet(eCharacterAct.idle);

        //플레이어 UI
        playerLevelText.text = player.playerData.level.ToString();
        playerNickNameText.text = player.playerData.nickName;

        //적 캐릭터 얻기        
        player.playerCharacter.enemy = enemy;
    }
    void EnemyLoad()
    {
        //적 초기화        
        enemy = new EnemyCharacter();
        enemy.Init();
        enemy.EnemyCharacterLoad(nEnemyIndex);
        enemyObject = enemy.characterObject;

        //Ui
        enemyLevelText.text = enemy.level.ToString();
        enemyNickNameText.text = enemy.name;
    }
    void MapLoad()
    {
        //난이도에 맞춰서 맵로딩 배율 조정
        List<int> difficulty1 = new List<int>();    //40%
        List<int> difficulty2 = new List<int>();    //30%
        List<int> difficulty3 = new List<int>();    //15%
        List<int> difficulty4 = new List<int>();    //14%   //보너스 맵 1%

        int size = CGame.Instance.dataTable.dataTableHunt.Length;
        
        for (int i = 0; i < size - 1; i++)
        {
            DataTable_Hunt newData = CGame.Instance.dataTable.dataTableHunt[i];
            if (newData.levelOfDifficulty == 1)
                difficulty1.Add(newData.index);
            else if (newData.levelOfDifficulty == 2)
                difficulty2.Add(newData.index);
            else if (newData.levelOfDifficulty == 3)
                difficulty3.Add(newData.index);
            else if (newData.levelOfDifficulty == 4)
                difficulty4.Add(newData.index);
        }

        int[] nDifficultyIndex = { Random.RandomRange(0, difficulty1.Count), Random.RandomRange(0, difficulty2.Count), Random.RandomRange(0, difficulty3.Count), Random.RandomRange(0, difficulty4.Count) };
   
        List<int> randomList = new List<int>();

        for (int i = 0; i < 100; i++)
        {
            if (i <= 70)
                randomList.Add(difficulty1[nDifficultyIndex[0]]);
            else if (i > 70 && i <= 85)
                randomList.Add(difficulty2[nDifficultyIndex[1]]);
            else if (i > 85 && i <= 95)
                randomList.Add(difficulty3[nDifficultyIndex[2]]);
            else if (i > 95 && i <= 98)
                randomList.Add(difficulty4[nDifficultyIndex[3]]);
            else if (i == 99)                            //보너스맵
                randomList.Add(999);
        }

        int nRandom = Random.RandomRange(0, randomList.Count);
        if (nRandom == 99)
            nEnemyIndex = 999;
        else
            nEnemyIndex = randomList[nRandom];
                        
        DataTable_Hunt mapData = CGame.Instance.dataTable.GetDataHunt(nEnemyIndex);
        data = mapData;

        map = CGame.Instance.GameObject_from_prefab("Map/" + mapData.index);
        sky.material = CGame.Instance.GetMaterial("Sky/" + mapData.skyIndex);
    }    

    void SkillButton()
    {
        if (skillActive == true)
        {
            player.playerCharacter.ActSet(eCharacterAct.skillAttack);
            skillActive = false;
            skillBtnImg.fillAmount = 0;
            skillBtnAnim.SetBool("BigAndSmall", skillActive);
            StartCoroutine(CoolTime());
        }
    }

    IEnumerator CoolTime()
    {
        while (skillBtnImg.fillAmount < 1)
        {
            skillBtnImg.fillAmount += Time.smoothDeltaTime / player.playerCharacter.skillCoolTime;

            yield return null;
        }
        skillActive = true;
        skillBtnAnim.SetBool("BigAndSmall", skillActive);

        yield break;        
    }
}
