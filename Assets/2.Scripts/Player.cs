using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData playerData { set; get; }
    public bool bPlayerData { set; get; }
    public PlayerCharacter playerCharacter {set; get;}
    public int maxExp;
    public int updateExp = 0;

    void Start()
    {
        playerData = new PlayerData();
        playerCharacter = new PlayerCharacter();
        playerCharacter.Init();        
    }
    void Update(){        
    }
}
//
public class PlayerData
{
    public string nickName;
    public int level;
    public int exp;
    public int gold;
    public int point;
}