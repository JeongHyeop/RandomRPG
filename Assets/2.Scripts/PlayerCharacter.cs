using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacter : Character
{
    public EnemyCharacter enemy;
    public Vector3 moveAxis = Vector3.zero;
    float rotationSpeed = 100.0f;

    public void PlayerUpdate()
    {
        if (isDie == false)
        {
            ActProcess();
            ObjectCollision();
        }
    }

    //캐릭터 정보가 없을때 최초 1회 실행
    public void InitSetting(string _name, EquipItem _equipItem, EquipBeauty _equipBeauty)
    {
        Player player = CGame.Instance.player;

        player.playerCharacter.type = eCharacterType.eCharacterType_Hero;
        player.playerCharacter.index = 0;
        player.playerCharacter.name = _name;
        player.playerCharacter.hp = 1000;
        player.playerCharacter.mp = 1;        
        player.playerCharacter.attack = 100;
        player.playerCharacter.defence = 80;
        player.playerCharacter.skillAttack = 100;
        player.playerCharacter.skillCoolTime = 15;
        player.playerCharacter.equippingItem.helmetIndex = _equipItem.helmetIndex;
        player.playerCharacter.equippingItem.accessoriIndex = _equipItem.accessoriIndex;
        player.playerCharacter.equippingItem.weaponIndex = _equipItem.weaponIndex;
        player.playerCharacter.equippingBeauty.genderType = _equipBeauty.genderType;
        player.playerCharacter.equippingBeauty.BodyIndex = _equipBeauty.BodyIndex;
        player.playerCharacter.equippingBeauty.HairIndex = _equipBeauty.HairIndex;

        PlayerCharacterSave();
    }

    public void PlayerCharacterSave()
    {
        Player player = CGame.Instance.player;

        if (player.playerCharacter == null)
            return;

        string db =
           (int)player.playerCharacter.type + "\t" +
           player.playerCharacter.index + "\t" +
           player.playerCharacter.name + "\t" +
           player.playerCharacter.hp + "\t" +
           player.playerCharacter.mp + "\t" +           
           player.playerCharacter.attack + "\t" +
           player.playerCharacter.defence + "\t" +
           player.playerCharacter.skillAttack + "\t" +
           player.playerCharacter.skillCoolTime + "\t" +
           player.playerCharacter.equippingItem.helmetIndex + "\t" +
           player.playerCharacter.equippingItem.accessoriIndex + "\t" +
           player.playerCharacter.equippingItem.weaponIndex + "\t" +
           player.playerCharacter.equippingBeauty.genderType + "\t" +
           player.playerCharacter.equippingBeauty.BodyIndex + "\t" +
           player.playerCharacter.equippingBeauty.HairIndex + "\t" +
           "";

        PlayerPrefs.SetString("PlayerCharacter_Data", db);
    }

    public void PlayerCharacterLoad(GameObject _parent)
    {
        isDie = false;

        Player player = CGame.Instance.player;

        string db = PlayerPrefs.GetString("PlayerCharacter_Data");  //load       

        if (db.Length < 3)
            return;

        //로드 코드
        string[] Cells = db.Split("\t"[0]);

        player.playerCharacter.type = (eCharacterType)int.Parse(Cells[0]);
        player.playerCharacter.index = int.Parse(Cells[1]);
        player.playerCharacter.name = Cells[2];
        player.playerCharacter.hp = int.Parse(Cells[3]);
        maxHP = hp;
        player.playerCharacter.mp = int.Parse(Cells[4]);        
        player.playerCharacter.attack = int.Parse(Cells[5]);
        player.playerCharacter.defence = int.Parse(Cells[6]);
        player.playerCharacter.skillAttack = int.Parse(Cells[7]);
        player.playerCharacter.skillCoolTime = float.Parse(Cells[8]);
        player.playerCharacter.equippingItem.helmetIndex = int.Parse(Cells[9]);
        player.playerCharacter.equippingItem.accessoriIndex = int.Parse(Cells[10]);
        player.playerCharacter.equippingItem.weaponIndex = int.Parse(Cells[11]);
        player.playerCharacter.equippingBeauty.genderType = int.Parse(Cells[12]);
        player.playerCharacter.equippingBeauty.BodyIndex = int.Parse(Cells[13]);
        player.playerCharacter.equippingBeauty.HairIndex = int.Parse(Cells[14]);

        characterObject = _parent;

        if (equippingBeauty.genderType != -1)
        {
            //몸
            if (equippingBeauty.BodyIndex != -1)
            {
                GameObject newBody = null;

                newBody = CGame.Instance.GameObject_from_prefab("Beauty/" + equippingBeauty.BodyIndex);
                newBody.transform.localPosition = _parent.transform.localPosition;
                newBody.transform.parent = _parent.transform;
                
                characterAnimator = newBody.GetComponent<Animator>();
            }
            else
                return;

            //머리
            if (equippingBeauty.HairIndex != -1)
            {
                GameObject newHair = null;

                //머리붙일 위치를 자식 객체를 돌면서 찾음
                GameObject oldHair = CGame.Instance.GameObject_get_child(_parent, "Dummy Prop Head");

                newHair = CGame.Instance.GameObject_from_prefab("Beauty/" + equippingBeauty.HairIndex);
                newHair.transform.parent = oldHair.transform;
                newHair.transform.localPosition = Vector3.zero;
                newHair.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);
            }

            //무기
            if (equippingItem.weaponIndex != -1)
            {
                GameObject newWeapon = null;

                //머리붙일 위치를 자식 객체를 돌면서 찾음
                GameObject oldWeapon = CGame.Instance.GameObject_get_child(_parent, "Dummy Prop Right");

                newWeapon = CGame.Instance.GameObject_from_prefab("Item/" + equippingItem.weaponIndex);
                newWeapon.transform.parent = oldWeapon.transform;
                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);

                equippingItem.equippingWeapon = CGame.Instance.dataTable.GetItem(equippingItem.weaponIndex);
            }

            //헬멧
            if (equippingItem.helmetIndex != -1)
            {
                GameObject newHelmet = null;

                //머리붙일 위치를 자식 객체를 돌면서 찾음
                GameObject oldHead = CGame.Instance.GameObject_get_child(_parent, "Dummy Prop Head");

                newHelmet = CGame.Instance.GameObject_from_prefab("Item/" + equippingItem.helmetIndex);
                newHelmet.transform.parent = oldHead.transform;
                newHelmet.transform.localPosition = Vector3.zero;
                newHelmet.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);

                equippingItem.equippingHelmet = CGame.Instance.dataTable.GetItem(equippingItem.helmetIndex);
            }
            else
                equippingItem.equippingHelmet = new DataTable_Item();

            //악세서리
            if (equippingItem.accessoriIndex != -1)
            {
                GameObject newAcc = null;

                //머리붙일 위치를 자식 객체를 돌면서 찾음
                GameObject oldBack = CGame.Instance.GameObject_get_child(_parent, "Dummy Prop Back");

                newAcc = CGame.Instance.GameObject_from_prefab("Item/" + equippingItem.accessoriIndex);
                newAcc.transform.parent = oldBack.transform;
                newAcc.transform.localPosition = Vector3.zero;
                newAcc.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);

                equippingItem.equippingAcc = CGame.Instance.dataTable.GetItem(equippingItem.accessoriIndex);
            }
            else
                equippingItem.equippingAcc = new DataTable_Item();           

            ActSet(eCharacterAct.idle);
        }
    }

    public void FirstPersonView(Camera _camera)
    {
        GameObject pos = CGame.Instance.GameObject_get_child(characterObject, "Dummy Prop Head");

        Vector3 newPos = new Vector3(pos.transform.position.x, pos.transform.position.y, pos.transform.position.z);
        _camera.transform.position = newPos;
        _camera.transform.eulerAngles = characterObject.transform.eulerAngles;
    }
    public void ThirdPersonView(Camera _camera)
    {
        GameObject pos = CGame.Instance.GameObject_get_child(characterObject, "Dummy Prop Head");

        Vector3 newPos = new Vector3(pos.transform.position.x + 4.2f, pos.transform.position.y + 6.5f, pos.transform.position.z - 3.2f);
        _camera.transform.position = newPos;        
    }
    public void Winner(Camera _camera)
    {
        _camera.transform.position = new Vector3(characterObject.transform.position.x, characterObject.transform.position.y + 2, characterObject.transform.position.z - 2);
        _camera.gameObject.AddComponent<ObjectRotation>();
        _camera.GetComponent<ObjectRotation>().axis.y = 1;
        _camera.GetComponent<ObjectRotation>().speed = 50;
    }
    public override void ActSet(eCharacterAct _eAct)
    {
        if (isDie == true)
            return;

        fActDelayTime += Time.deltaTime;

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
            case eCharacterAct.walk:
                break;
            case eCharacterAct.Jump:
                if (fActDelayTime > 1.0f)
                    Jump();
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
        cycle = 1.0f;
        characterAnimator.SetBool("Run", false);
        characterAnimator.SetBool("Walk", false);
        characterAnimator.SetBool("Dash", false);
    }

    public override void Move()
    {
        Vector3 pos = new Vector3(characterObject.transform.eulerAngles.x, Mathf.Atan2(moveAxis.x, moveAxis.y) * Mathf.Rad2Deg, characterObject.transform.eulerAngles.z);
        characterObject.transform.eulerAngles = pos;

        //3인칭
        characterObject.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);     

        moveTime += Time.deltaTime;

        if (moveTime > cycle)
        {
            moveSpeed = moveSpeed >= maxSpeed ? maxSpeed : moveSpeed += 0.5f;
            cycle += 1.0f;
        }
        if (moveSpeed < 2.0f)
        {
            characterAnimator.SetBool("Walk", true);
        }
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

        //1인칭
        //if (moveAxis == Vector3.left)
        //{
        //    characterAnimator.SetBool("Run", false);
        //    characterAnimator.SetBool("Walk", false);
        //    characterAnimator.SetBool("Dash", false);
        //    characterObject.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        //}
        //else if (moveAxis == Vector3.right)
        //{
        //    characterAnimator.SetBool("Run", false);
        //    characterAnimator.SetBool("Walk", false);
        //    characterAnimator.SetBool("Dash", false);
        //    characterObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        //}
        //else if (moveAxis == Vector3.up)
        //{
        //    characterObject.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        //    moveTime += Time.deltaTime;

        //    if (moveTime > cycle)
        //    {
        //        moveSpeed = moveSpeed >= maxSpeed ? maxSpeed : moveSpeed += 0.5f;
        //        cycle += 1.0f;
        //    }
        //    if (moveSpeed < 2.0f)
        //    {         
        //        characterAnimator.SetBool("Walk", true);
        //    }
        //    else if (moveSpeed >= 2.0f && moveSpeed <= 3.5f)
        //    {
        //        characterAnimator.SetBool("Walk", false);
        //        characterAnimator.SetBool("Run", true);
        //    }
        //    else
        //    {
        //        characterAnimator.SetBool("Run", false);
        //        characterAnimator.SetBool("Dash", true);
        //    }            
        //}
        //else if (moveAxis == Vector3.down)
        //{            
        //    characterObject.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        //    characterAnimator.SetBool("Walk", true);            
        //}
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
    public void Jump()
    {        
        CGame.Instance.PlaySound((int)eSound.eSound_Jump, GameObject.Find("Main Camera"), false);
        characterAnimator.Play("Jump");
        fActDelayTime = 0;
        ActSet(eCharacterAct.run);
    }
    public override void ActProcess()
    {
        ActSet(eAct);
    }
    public override void Attack()
    {
        fActDelayTime = 0;

        float dist = Vector3.Distance(characterObject.transform.position, targetObject.transform.position);

        //둔기류
        if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Club || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Wand 
            || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Hammer || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Mace)
            CGame.Instance.PlaySound((int)eSound.eSound_blunt, GameObject.Find("Main Camera"), false);

        //검류
        else if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Dagger || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Sword)
            CGame.Instance.PlaySound((int)eSound.eSound_Sword, GameObject.Find("Main Camera"), false);

        //창류
        else if (equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Axe || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Arrow
            ||  equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Knuckle || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Scythe
            || equippingItem.equippingWeapon.weaponType == eWeaponType.eWeaponType_Spear)
            CGame.Instance.PlaySound((int)eSound.eSound_Sword2, GameObject.Find("Main Camera"), false);

        //공격거리 범위 안
        if (dist <= attackDist)
        {
            characterAnimator.Play("Melee Right Attack 03");
            enemy.ActSet(eCharacterAct.hit);

            if (attack < enemy.defence)
            {
                enemy.hp = enemy.hp - 1 <= 0 ? 0 : enemy.hp - 1;
                return;
            }

            enemy.hp = enemy.hp - (attack - enemy.defence) <= 0 ? 0 : enemy.hp - (attack - enemy.defence);
            HuntSceneUI.Instance.SetDamageText(enemy.characterObject, attack - enemy.defence);     
        }
        else
            characterAnimator.Play("Melee Right Attack 03");
        
        ActSet(eCharacterAct.idle);
    }
    public void SkillAttack()
    {
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

        characterAnimator.Play("Melee Right Attack 02");        
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
    void ObjectCollision()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(characterObject.transform.position, characterObject.transform.forward, out rayHit, 1.0f) && rayHit.collider.tag == "Obstacle")
            characterObject.transform.Translate((Vector3.back*1.5f) * moveSpeed * Time.deltaTime); 
        else if(Physics.Raycast(characterObject.transform.position, -characterObject.transform.forward, out rayHit, 1.0f) && rayHit.collider.tag == "Obstacle")
            characterObject.transform.Translate((Vector3.forward * 1.5f) * moveSpeed * Time.deltaTime);        
    }
}   
        
