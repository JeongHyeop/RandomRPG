using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character// : MonoBehaviour
{
    public GameObject characterObject;
    public int index;
    public eCharacterType type;
    public string name;
    public int hp;
    public int mp;    
    public int attack;
    public int defence;
    public int skillAttack;
    public float skillCoolTime;
    public int maxHP;

    //
    protected float moveTime = 0;
    protected const float maxSpeed = 4.0f;
    protected float cycle = 1.0f;
    protected float moveSpeed = 1.0f;

    //
    public EquipBeauty equippingBeauty;
    public EquipItem equippingItem;

    //
    public GameObject targetObject;
    public const float attackDist = 3.0f;

    //
    public bool isDie = false;
    public eCharacterAct eAct = eCharacterAct.idle;
    public Animator characterAnimator;
    protected float fActDelayTime = 0;

    //
    protected SkillManager skillManager;

    //
    public abstract void Move();
    public abstract void Attack();
    public abstract void ActSet(eCharacterAct _eAct);
    public abstract void ActProcess();

    public void Init()
    {
        this.equippingBeauty = new EquipBeauty();
        this.equippingItem = new EquipItem();
        this.characterObject = new GameObject();
        skillManager = new SkillManager();
        eAct = eCharacterAct.idle;
    }
    public void SetTargetObject(GameObject _target)
    {
        targetObject = _target;
    }
    protected GameObject CreateCharacter(int _index){
        GameObject go = characterObject;
        DataTable_Character dtChar = null;

        int size = CGame.Instance.dataTable.dataTableCharacter.Length;

        for (int i = 1; i < size; i++)
        {
            dtChar = CGame.Instance.dataTable.dataTableCharacter[i];
            if (dtChar.name == "null")
                return null;
            else if (_index == dtChar.index)
                break;
        }
                       
        if (dtChar.equippingBeauty.genderType != -1)
        {
            //몸
            if (dtChar.equippingBeauty.BodyIndex != -1)
            {
                GameObject newBody = null;

                newBody = CGame.Instance.GameObject_from_prefab("Beauty/" + dtChar.equippingBeauty.BodyIndex);
                newBody.transform.localPosition = go.transform.localPosition;
                newBody.transform.parent = go.transform;
            }
            else
                return null;

            //머리
            if (dtChar.equippingBeauty.HairIndex != -1)
            {            
                GameObject newHair = null;

                //머리붙일 위치를 자식 객체를 돌면서 찾음
                GameObject oldHair = CGame.Instance.GameObject_get_child(go, "Dummy Prop Head");

                newHair = CGame.Instance.GameObject_from_prefab("Beauty/" + dtChar.equippingBeauty.HairIndex);
                newHair.transform.parent = oldHair.transform;
                newHair.transform.localPosition = Vector3.zero;
                newHair.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);
            }

            //무기
            if (dtChar.equippingItem.weaponIndex != -1)
            {
                GameObject newWeapon = null;

                //머리붙일 위치를 자식 객체를 돌면서 찾음
                GameObject oldWeapon = CGame.Instance.GameObject_get_child(go, "Dummy Prop Right");

                newWeapon = CGame.Instance.GameObject_from_prefab("Item/" + dtChar.equippingItem.weaponIndex);
                newWeapon.transform.parent = oldWeapon.transform;
                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);
            }

            //헬멧
            if (dtChar.equippingItem.helmetIndex != -1)
            {
                GameObject newHelmet = null;

                //머리붙일 위치를 자식 객체를 돌면서 찾음
                GameObject oldHead = CGame.Instance.GameObject_get_child(go, "Dummy Prop Head");

                newHelmet = CGame.Instance.GameObject_from_prefab("Item/" + dtChar.equippingItem.helmetIndex);
                newHelmet.transform.parent = oldHead.transform;
                newHelmet.transform.localPosition = Vector3.zero;
                newHelmet.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);
            }

            ////악세서리
            //if (dtChar.equippingItem.accessoriIndex != -1)
            //{

            //} 
            this.index = dtChar.index;
            this.type = dtChar.characterType;
            this.name = dtChar.name;
            this.hp = dtChar.hp;
            this.mp = dtChar.mp;            
            this.attack = dtChar.attack;
            this.defence = dtChar.defence;
            this.skillAttack = dtChar.skillAttack;
            this.skillCoolTime = dtChar.skillCoolTime;
            this.equippingItem.helmetIndex = dtChar.equippingItem.helmetIndex;
            this.equippingItem.accessoriIndex = dtChar.equippingItem.accessoriIndex;
            this.equippingItem.weaponIndex = dtChar.equippingItem.weaponIndex;
            this.equippingBeauty.genderType = dtChar.equippingBeauty.genderType;
            this.equippingBeauty.BodyIndex = dtChar.equippingBeauty.BodyIndex;
            this.equippingBeauty.HairIndex = dtChar.equippingBeauty.HairIndex;
        }
        else
            return null;

        return go;
    }  
}
