using System.Collections;
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
    Button autoButton;
    public Animator autoBtnAnim;
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
        autoButton = GameObject.Find("AutoButton").GetComponent<Button>();
        autoButton.onClick.AddListener(AutoButton);
        autoBtnAnim.SetBool("ZRotation", player.playerCharacter.bAutoMode);
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
        {
            playerHP.value = (float)player.playerCharacter.hp / (float)player.playerCharacter.maxHP;
            if (player.playerCharacter.bAutoMode == true && player.playerCharacter.bSkillActive == true)
                SkillButton();
        }
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
                    if (Input.GetMouseButtonUp(0))
                    {
                        nTemp = nGold;
                        delayTime = 0;
                    }
                }
                else
                {
                    if (delayTime > 2.0f)
                    {
                        player.playerData.gold += nGold;
                        player.updateExp += enemy.exp;
                        //아이템 먹기


                        CGame.Instance.LocalDB_save();
                        CGame.Instance.SceneChange(3);
                    }
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

        //UI
        enemyLevelText.text = enemy.level.ToString();
        enemyNickNameText.text = enemy.name;
    }

    void MapLoad()
    {
        //난이도에 맞춰서 맵로딩 배율 조정
        List<List<int>> difficulty = new List<List<int>>();
        for (int i = 0; i < 4; i++)             //네번 도는 이유는 난이도가 4까지 있기 때문
            difficulty.Add(new List<int>());

        int size = CGame.Instance.dataTable.dataTableHunt.Length;
        
        for (int i = 0; i < size - 1; i++)
        {
            DataTable_Hunt newData = CGame.Instance.dataTable.dataTableHunt[i];
            
            if (newData.levelOfDifficulty == 1)
                difficulty[0].Add(newData.index);
            else if (newData.levelOfDifficulty == 2)
                difficulty[1].Add(newData.index);
            else if (newData.levelOfDifficulty == 3)
                difficulty[2].Add(newData.index);
            else if (newData.levelOfDifficulty == 4)
                difficulty[3].Add(newData.index);
        }

        //각 난이도별 맵이있기 때문에 그 중 한가지 선별 작업
        int[] nDifficultyIndex = { Random.RandomRange(0, difficulty[0].Count), Random.RandomRange(0, difficulty[1].Count), Random.RandomRange(0, difficulty[2].Count), Random.RandomRange(0, difficulty[3].Count) };

        //여기서 레벨 디자인(난이도 조정)
        int[] nAdjust = new int[4];
        int playerLevel = CGame.Instance.player.playerData.level;

        if (playerLevel < 20)
        {
            nAdjust[0] = 0; nAdjust[1] = 1; nAdjust[2] = 2; nAdjust[3] = 3;
            nDifficultyIndex[0] = Random.RandomRange(0, difficulty[0].Count); nDifficultyIndex[1] = Random.RandomRange(0, difficulty[1].Count);
            nDifficultyIndex[2] = Random.RandomRange(0, difficulty[2].Count); nDifficultyIndex[3] = Random.RandomRange(0, difficulty[3].Count);
        }
        else if (playerLevel < 45)
        {
            nAdjust[0] = 1; nAdjust[1] = 0; nAdjust[2] = 2; nAdjust[3] = 3;
            nDifficultyIndex[0] = Random.RandomRange(0, difficulty[1].Count); nDifficultyIndex[1] = Random.RandomRange(0, difficulty[0].Count);
            nDifficultyIndex[2] = Random.RandomRange(0, difficulty[2].Count); nDifficultyIndex[3] = Random.RandomRange(0, difficulty[3].Count);
        }
        else if (playerLevel < 75)
        {
            nAdjust[0] = 2; nAdjust[1] = 3; nAdjust[2] = 1; nAdjust[3] = 0;
            nDifficultyIndex[0] = Random.RandomRange(0, difficulty[2].Count); nDifficultyIndex[1] = Random.RandomRange(0, difficulty[3].Count);
            nDifficultyIndex[2] = Random.RandomRange(0, difficulty[1].Count); nDifficultyIndex[3] = Random.RandomRange(0, difficulty[0].Count);
        }
        else if (playerLevel < 95 || playerLevel >= 95)
        {
            nAdjust[0] = 3; nAdjust[1] = 2; nAdjust[2] = 1; nAdjust[3] = 0;
            nDifficultyIndex[0] = Random.RandomRange(0, difficulty[3].Count); nDifficultyIndex[1] = Random.RandomRange(0, difficulty[2].Count);
            nDifficultyIndex[2] = Random.RandomRange(0, difficulty[1].Count); nDifficultyIndex[3] = Random.RandomRange(0, difficulty[0].Count);
        }

        List<int> randomList = new List<int>();

        for (int i = 0; i < 100; i++)
        {
            if (i <= 70)
                randomList.Add(difficulty[nAdjust[0]][nDifficultyIndex[0]]);
            else if (i > 70 && i <= 85)
                randomList.Add(difficulty[nAdjust[1]][nDifficultyIndex[1]]);
            else if (i > 85 && i <= 95)
                randomList.Add(difficulty[nAdjust[2]][nDifficultyIndex[2]]);
            else if (i > 95 && i <= 98)
                randomList.Add(difficulty[nAdjust[3]][nDifficultyIndex[3]]);
            else if (i == 99)                            //보너스맵
                randomList.Add(999);
        }

        int nRandom = Random.RandomRange(0, randomList.Count);        

        if (nRandom == 99)
            nEnemyIndex = 999;                          //보너스 캐릭
        else
            nEnemyIndex = randomList[nRandom];
                        
        DataTable_Hunt mapData = CGame.Instance.dataTable.GetDataHunt(nEnemyIndex);
        data = mapData;

        map = CGame.Instance.GameObject_from_prefab("Map/" + mapData.index);
        sky.material = CGame.Instance.GetMaterial("Sky/" + mapData.skyIndex);
    }
    void AutoButton()
    {
        player.playerCharacter.AutoMode();
        autoBtnAnim.SetBool("ZRotation", player.playerCharacter.bAutoMode);
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
