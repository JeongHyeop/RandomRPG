using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacter : Character
{
    //추적        
    public PlayerCharacter playerCharacter;
    //NavMeshAgent agent;
    //const float traceDist = 25.0f;
    //const float skillDist = 5.0f;

    public int level;
    public int exp;
    public void EnemyCharacterLoad(int _index)
    {
        isDie = false;

        characterObject = CGame.Instance.GameObject_from_prefab("Character/" + _index);        
        
        //Nav Mesh Agent 얻기
        characterObject.AddComponent<NavMeshAgent>();
        agent = characterObject.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        characterObject.transform.position = new Vector3(0, 0, 52);
        agent.enabled = true;

        //캐릭터 애니메이션 얻기
        characterAnimator = characterObject.GetComponent<Animator>();

        DataTable_Character data = CGame.Instance.dataTable.GetDataCharacter(_index);

        level = Random.RandomRange(data.level, data.level + 10);        
        exp = data.exp * level / 2;        

        type = data.characterType;
        index = data.index;
        name = data.name;

        //나중에 hp, attack 등 장비 맞춰서 끼워지면 +로 추가하기
        hp = data.hp + (level * 5);
        maxHP = hp;
        mp = data.mp;
        attack = data.attack + (level * 2);
        defence = data.defence + (level * 2);
        skillAttack = data.skillAttack + (level * 2);
        skillCoolTime = data.skillCoolTime;
        equippingItem.helmetIndex = data.equippingItem.helmetIndex;
        equippingItem.accessoriIndex = data.equippingItem.accessoriIndex;
        equippingItem.weaponIndex = data.equippingItem.weaponIndex;
        equippingBeauty.genderType = data.equippingBeauty.genderType;
        equippingBeauty.BodyIndex = data.equippingBeauty.BodyIndex;
        equippingBeauty.HairIndex = data.equippingBeauty.HairIndex;       

        //임시 무기
        int nRandomWeapon = Random.Range(0,10);
        equippingItem.equippingWeapon = new DataTable_Item();
        equippingItem.equippingHelmet = new DataTable_Item();
        equippingItem.equippingAcc = new DataTable_Item();
        equippingItem.equippingWeapon.weaponType = (eWeaponType)nRandomWeapon;

        //실제 착용하고 있는 장비 시트 만들고 밑에 추가하기
        ////무기
        //if (equippingItem.weaponIndex != -1)
        //{
        //    GameObject newWeapon = null;

        //    //머리붙일 위치를 자식 객체를 돌면서 찾음
        //    GameObject oldWeapon = CGame.Instance.GameObject_get_child(_parent, "Dummy Prop Right");

        //    newWeapon = CGame.Instance.GameObject_from_prefab("Item/" + equippingItem.weaponIndex);
        //    newWeapon.transform.parent = oldWeapon.transform;
        //    newWeapon.transform.localPosition = Vector3.zero;
        //    newWeapon.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);

        //    equippingItem.equippingWeapon = CGame.Instance.dataTable.GetItem(equippingItem.weaponIndex);
        //}

        ////헬멧
        //if (equippingItem.helmetIndex != -1)
        //{
        //    GameObject newHelmet = null;

        //    //머리붙일 위치를 자식 객체를 돌면서 찾음
        //    GameObject oldHead = CGame.Instance.GameObject_get_child(_parent, "Dummy Prop Head");

        //    newHelmet = CGame.Instance.GameObject_from_prefab("Item/" + equippingItem.helmetIndex);
        //    newHelmet.transform.parent = oldHead.transform;
        //    newHelmet.transform.localPosition = Vector3.zero;
        //    newHelmet.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);

        //    equippingItem.equippingHelmet = CGame.Instance.dataTable.GetItem(equippingItem.helmetIndex);
        //}
        //else
        //    equippingItem.equippingHelmet = new DataTable_Item();

        ////악세서리
        //if (equippingItem.accessoriIndex != -1)
        //{
        //    GameObject newAcc = null;

        //    //머리붙일 위치를 자식 객체를 돌면서 찾음
        //    GameObject oldBack = CGame.Instance.GameObject_get_child(_parent, "Dummy Prop Back");

        //    newAcc = CGame.Instance.GameObject_from_prefab("Item/" + equippingItem.accessoriIndex);
        //    newAcc.transform.parent = oldBack.transform;
        //    newAcc.transform.localPosition = Vector3.zero;
        //    newAcc.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);

        //    equippingItem.equippingAcc = CGame.Instance.dataTable.GetItem(equippingItem.accessoriIndex);
        //}
        //else
        //    equippingItem.equippingAcc = new DataTable_Item();                

    }
    public void EnemyUpdate()
    {
        if (isDie == false)
        {
            ActProcess();
        }
    }
    public override void ActProcess()
    {
        float dist = Vector3.Distance(characterObject.transform.position, targetObject.transform.position);

        //공격거리 범위 안        
        if (dist <= skillDist && skillTime >= skillCoolTime)
            ActSet(eCharacterAct.skillAttack);
        else if (dist <= attackDist)
            ActSet(eCharacterAct.attack);
        else if (dist <= traceDist)
            ActSet(eCharacterAct.run);
        else
            ActSet(eCharacterAct.idle);        
    }
    public override void ActSet(eCharacterAct _eAct)
    {
        if (isDie == true)
            return;

        fActDelayTime += Time.deltaTime;
        skillTime += Time.deltaTime;

        //액션 우선순위
        switch (_eAct)
        {
            case eCharacterAct.hit:
                if (eAct == eCharacterAct.die) return;
                if (eAct == eCharacterAct.disappear) return;
                break;

            case eCharacterAct.die:
                if (eAct == eCharacterAct.die) return;
                if (eAct == eCharacterAct.disappear) return;
                break;
        }

        eAct = _eAct;

        switch (eAct)
        {
            case eCharacterAct.None:
                break;
            case eCharacterAct.appear:
                break;
            case eCharacterAct.disappear:
                break;
            case eCharacterAct.idle:
                Idle();
                break;     
            case eCharacterAct.Jump:
                break;
            case eCharacterAct.run:
                Move();
                break;
            case eCharacterAct.attack:
                if (fActDelayTime > 1.0f)
                    Attack();
                break;
            case eCharacterAct.skillAttack:
                SkillAttack();
                break;
            case eCharacterAct.hit:
                Hit();                
                break;
            case eCharacterAct.die:
                Die();
                break;
            case eCharacterAct.corpse:
                break;
            case eCharacterAct.Max:
                break;
        }
    }    
    void Idle()
    {
        moveTime = 0;
        moveSpeed = 1;
        agent.speed = 0;
        cycle = 1.0f;
        characterAnimator.SetBool("Run", false);
        characterAnimator.SetBool("Walk", false);
        characterAnimator.SetBool("Dash", false);
    }
    public override void Move()
    {
        moveTime += Time.deltaTime;

        if (moveTime > cycle)
        {
            moveSpeed = moveSpeed >= maxSpeed ? maxSpeed : moveSpeed += 0.5f;
            agent.speed = moveSpeed;
            cycle += 1.0f;
        }

        agent.destination = targetObject.transform.position;
        agent.transform.LookAt(targetObject.transform);

        if (moveSpeed < 2.0f)
            characterAnimator.SetBool("Walk", true);
        else if (moveSpeed >= 2.0f && moveSpeed <= 3.5f)
        {
            characterAnimator.SetBool("Walk", false);
            characterAnimator.SetBool("Run", true);
        }
        else
        {
            characterAnimator.SetBool("Run", false);
            characterAnimator.SetBool("Dash", true);
        }
    }
    public override void Attack()
    {
        characterAnimator.Play("Melee Right Attack 03");
        fActDelayTime = 0;
        playerCharacter.ActSet(eCharacterAct.hit);

        //둔기류
        if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Club || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Wand
            || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Hammer || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Mace)
            CGame.Instance.PlaySound((int)eSound.eSound_blunt, GameObject.Find("Main Camera"), false);

        //검류
        else if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Dagger || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Sword)
            CGame.Instance.PlaySound((int)eSound.eSound_Sword, GameObject.Find("Main Camera"), false);

        //창류
        else if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Axe || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Arrow
            || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Knuckle || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Scythe
            || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Spear)
            CGame.Instance.PlaySound((int)eSound.eSound_Sword2, GameObject.Find("Main Camera"), false);

        if (attack < playerCharacter.defence)
        {
            playerCharacter.hp = playerCharacter.hp - 1 <= 0 ? 0 : playerCharacter.hp - 1;            
            return;
        }
        
        playerCharacter.hp = playerCharacter.hp - (attack - playerCharacter.defence) <= 0 ? 0 : playerCharacter.hp - (attack - playerCharacter.defence);
        HuntSceneUI.Instance.SetDamageText(playerCharacter.characterObject, (attack - playerCharacter.defence) <= 0 ? 0 : (attack - playerCharacter.defence));
        //HuntSceneUI.Instance.SetDamageText(playerCharacter.gameObject , attack - playerCharacter.defence);
        ActSet(eCharacterAct.idle);
    }
    public void SkillAttack()
    {
        characterAnimator.Play("Melee Right Attack 02");

        //둔기류
        if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Club || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Hammer)
            CGame.Instance.PlaySound((int)eSound.eSound_blunt, GameObject.Find("Main Camera"), false);

        //검류
        else if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Dagger || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Sword)
            CGame.Instance.PlaySound((int)eSound.eSound_Sword, GameObject.Find("Main Camera"), false);

        //창류
        else if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Axe || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Arrow
            || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Knuckle || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Scythe
            || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Spear)
            CGame.Instance.PlaySound((int)eSound.eSound_Sword2, GameObject.Find("Main Camera"), false);

        //법사
        else if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Wand || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Mace)
            CGame.Instance.PlaySound((int)eSound.eSound_Magic, GameObject.Find("Main Camera"), false);

        skillManager.SkillStart(type, eSkillState.eSkillState_Start, equippingItem.equippingWeapon.weaponType, characterObject);
        skillTime = 0;
        ActSet(eCharacterAct.idle);
    }
    void Hit()
    {
        if (hp <= 0)
        {
            ActSet(eCharacterAct.die);
            return;
        }
        if (eGenderType.eGenderType_Female == (eGenderType)equippingBeauty.genderType)
            CGame.Instance.PlaySound((int)eSound.eSound_FemaleHit, GameObject.Find("Main Camera"), false);
        else
            CGame.Instance.PlaySound((int)eSound.eSound_MaleHit, GameObject.Find("Main Camera"), false);

        skillManager.SkillStart(type, eSkillState.eSkillState_Hit, (eWeaponType)(100), characterObject);
        characterAnimator.Play("Take Damage");
        ActSet(eCharacterAct.idle);        
    }
    void Die()
    {
        if (eGenderType.eGenderType_Female == (eGenderType)equippingBeauty.genderType)
            CGame.Instance.PlaySound((int)eSound.eSound_FemaleDie, GameObject.Find("Main Camera"), false);
        else
            CGame.Instance.PlaySound((int)eSound.eSound_MaleDie, GameObject.Find("Main Camera"), false);
        
        characterAnimator.Play("Die");
        isDie = true;
    }
    public void DropItem()
    {

    }

    void ObjectCollision()
    {
        RaycastHit rayHit;
        //if (Physics.Raycast(characterObject.transform.position, characterObject.transform.forward, out rayHit, 1.0f) && rayHit.collider.tag == "Skill")
        //{
        //    hp = hp - CGame.Instance.player.playerCharacter.skillAttack <= 0 ? 0 : hp - CGame.Instance.player.playerCharacter.skillAttack;
        //    Debug.Log(hp);
        //    ActSet(eCharacterAct.hit);         
        //}            
    }
}
