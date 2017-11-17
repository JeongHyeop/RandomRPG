﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour {
    public Image itemimage;
    public Image itemgrade;
    public Text itemName;
    public Text itemGrade;
    public Text itemHP;
    public Text itemAttack;
    public Text itemDefence;
    public Text itemMana;
    public Text itemSkill;
    public Text itemCooltime;
    public Button exitbutton;
    public Button changebutton;
    public GameObject itemPanelGO;
    public bool bItemActive { private set; get; }

	void Start () {
        //초기화
        itemPanelGO = this.gameObject;
        bItemActive = true;
	}
	
    public void CallItemInfo(int _nItemIndex, eScene _eScenenum)
    {
        if (itemPanelGO == null)
            itemPanelGO = CGame.Instance.GameObject_from_prefab("Inventory");

        DataTable_Item newDTItem = CGame.Instance.dataTable.GetItem(_nItemIndex);

        bItemActive = true; itemPanelGO.SetActive(bItemActive);
        itemPanelGO.transform.parent = GameObject.Find("Canvas").transform;
        itemPanelGO.transform.position = itemPanelGO.transform.parent.position;

        string gradeitem = null;
        changebutton.gameObject.SetActive(false);
        exitbutton.onClick.AddListener(ExitItemMenu);

        switch (newDTItem.grade)
        {
            case eItemGrade.eItemGrade_Base:
                gradeitem = "Base";
                break;
            case eItemGrade.eItemGrade_Normal:
                gradeitem = "normal";
                break;
            case eItemGrade.eItemGrade_Rare:
                gradeitem = "Rare";
                break;
            case eItemGrade.eItemGrade_SuperRare:
                gradeitem = "SuperRare";
                break;
            case eItemGrade.eItemGrade_Legend:
                gradeitem = "Legend";
                break;
            default:
                break;
        }

        itemimage.sprite = CGame.Instance.GetImage("Item/" + newDTItem.itemIndex);
        itemgrade.sprite = CGame.Instance.GetImage("Item/GradeIconImg" + (int)newDTItem.grade);
        itemName.text = newDTItem.itemName;
        itemGrade.text = gradeitem;

        if (_eScenenum == eScene.eScene_HuntScene)
        {
            DataTable_Item equipedItem = null;
            changebutton.gameObject.SetActive(true);
            switch (newDTItem.itemType)
            {
                case eItemType.eitemType_None:
                    break;
                case eItemType.eitemType_Helmet:
                    equipedItem = CGame.Instance.player.playerCharacter.equippingItem.equippingHelmet;
                    break;
                case eItemType.eitemType_Weapon:
                    equipedItem = CGame.Instance.player.playerCharacter.equippingItem.equippingWeapon;
                    break;
                case eItemType.eitemType_Accessori:
                    equipedItem = CGame.Instance.player.playerCharacter.equippingItem.equippingAcc;
                    break;
                default:
                    break;
            }
            changebutton.onClick.AddListener(delegate { ChangeButton(newDTItem.itemType, _nItemIndex); });  

            CompStatue(itemHP, (float)newDTItem.hp, (float)equipedItem.hp);
            CompStatue(itemAttack, (float)newDTItem.attack, (float)equipedItem.attack);
            CompStatue(itemDefence, (float)newDTItem.defence, (float)equipedItem.defence);
            CompStatue(itemMana, (float)newDTItem.mp, (float)equipedItem.mp);
            CompStatue(itemSkill, (float)newDTItem.skillAttack, (float)equipedItem.skillAttack);
            CompStatue(itemCooltime, newDTItem.skillCoolTime, equipedItem.skillCoolTime);
        }
        else
        {
            itemHP.text = newDTItem.hp.ToString();
            itemAttack.text = newDTItem.attack.ToString();
            itemDefence.text = newDTItem.defence.ToString();
            itemMana.text = newDTItem.mp.ToString();
            itemSkill.text = newDTItem.skillAttack.ToString();
            itemCooltime.text = newDTItem.skillCoolTime.ToString();
        }
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            itemPanelGO.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 800);
    }

    private void CompStatue(Text _text, float _value, float _value1)
    {
        float nNewval = 0;
        string strArrow = null;
        Color cTextcolor;

        if (_value > _value1)
        {
            nNewval = _value - _value1;
            strArrow = "(↑)";
            cTextcolor = Color.blue;
        }
        else if (_value < _value1)
        {
            nNewval = _value1 - _value;
            strArrow = "(↓)";
            cTextcolor = Color.red;
        }
        else
            cTextcolor = Color.black;

        _text.text = _value.ToString() + " (" + nNewval + ")" + strArrow;
        _text.color = cTextcolor;
    }

    private void ExitItemMenu()
    {
        itemPanelGO.transform.parent = CGame.Instance.gameObject.transform;
        bItemActive = false; itemPanelGO.SetActive(bItemActive);
    }
    private void ChangeButton(eItemType _eItemType, int _nItemIndex)
    {
        switch (_eItemType)
        {
            case eItemType.eitemType_Helmet:
                //장착 사운드 넣기
                CGame.Instance.player.playerCharacter.equippingItem.helmetIndex = _nItemIndex;
                break;
            case eItemType.eitemType_Weapon:
                //장착 사운드 넣기
                CGame.Instance.player.playerCharacter.equippingItem.weaponIndex = _nItemIndex;
                break;
            case eItemType.eitemType_Accessori:
                //장착 사운드 넣기
                CGame.Instance.player.playerCharacter.equippingItem.accessoriIndex = _nItemIndex;
                break;
        }
        itemPanelGO.transform.parent = CGame.Instance.gameObject.transform;
        bItemActive = false; itemPanelGO.SetActive(bItemActive);
    }
}