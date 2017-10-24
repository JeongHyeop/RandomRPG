using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
    //스킬 데미지
    int damage = 0;

    //지속형 속도
    const float continuousSpeed = 10.0f;

    //단기형 속도
    const float shortSpeed = 4.0f;

    //스킬 타입
    eWeaponType weapon = eWeaponType.eWeaponType_None;

    //시전자 타입
    public eCharacterType characterType;

    //데미지를 얻기 위한 변수
    Player player = null;
    EnemyCharacter enemy = null;

    //
    Transform pos;
    bool active = false;

	void Start () {
        player = CGame.Instance.player;
        enemy = GameObject.Find("HuntScene").GetComponent<HuntScene>().enemy;
        switch (characterType)
        {
            case eCharacterType.eCharacterType_Hero:
                weapon = player.playerCharacter.equippingItem.equippingWeapon.weaponType;
                damage = player.playerCharacter.skillAttack + 
                player.playerCharacter.equippingItem.equippingWeapon.skillAttack +
                player.playerCharacter.equippingItem.equippingHelmet.skillAttack +
                player.playerCharacter.equippingItem.equippingAcc.skillAttack;
                transform.position = CGame.Instance.GameObject_get_child(player.playerCharacter.characterObject, "Dummy Prop Head").transform.position;
                pos = player.playerCharacter.characterObject.transform;
                break;
            case eCharacterType.eCharacterType_Enemy:
                weapon = enemy.equippingItem.equippingWeapon.weaponType;
                damage = enemy.skillAttack +
                enemy.equippingItem.equippingWeapon.skillAttack +
                enemy.equippingItem.equippingHelmet.skillAttack +
                enemy.equippingItem.equippingAcc.skillAttack;
                transform.position = CGame.Instance.GameObject_get_child(enemy.characterObject, "Dummy Prop Head").transform.position;
                pos = enemy.characterObject.transform;
                break;         
        }
        
	}

	void Update () {
        if (eWeaponType.eWeaponType_Wand == weapon || eWeaponType.eWeaponType_Special == weapon || eWeaponType.eWeaponType_Arrow == weapon)
            transform.Translate(pos.forward * continuousSpeed * Time.deltaTime);
        else
            transform.Translate(pos.forward * shortSpeed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider col)
    {
        if (active == false)
        {
            if (col.tag == "Enemy" || col.tag == "Player")
                StartCoroutine(Fire());            
        }
    }

    IEnumerator Fire()
    {
        if (characterType == eCharacterType.eCharacterType_Hero)
        {
            enemy.hp = enemy.hp - damage <= 0 ? 0 : enemy.hp - damage;
            HuntSceneUI.Instance.SetDamageText(enemy.playerCharacter.characterObject, damage);
        }
        else if (characterType == eCharacterType.eCharacterType_Enemy)
        {
            player.playerCharacter.hp = player.playerCharacter.hp - damage <= 0 ? 0 : player.playerCharacter.hp - damage;
            HuntSceneUI.Instance.SetDamageText(player.playerCharacter.characterObject, damage);
        }
        active = true;
        yield return new WaitForSeconds(1.0f);
        active = false;        
    }
}