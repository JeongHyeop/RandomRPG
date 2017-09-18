using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomScene : MonoBehaviour {
    public Text nickNameText;
    public Image nickCheckImg;
    bool nickNameCheck = false;

    //플레이어 보여질 객체
    public GameObject playerObject;    
    
    //인덱스
    eGenderType curGender = eGenderType.eGenderType_Female;
    int curWeaponIndex = 0;
    int maxWeaponIndex = 0;
    int curBodyIndex = 0;
    int maxBodyIndex = 0;
    int curHairIndex = 0;
    int maxHairIndex = 0;
    const int baseCount = 2;

    //UI
    public Text weaponName;
    public Text hairName;
    public Text BodyName;

    //회전
    float mouse = 0;
    bool rotationCheck = false;
    public Image rotationButtonImg;

    void Start()
    {        
        maxBodyIndex = CGame.Instance.dataTable.dataTableBeatuy.Length;
        maxHairIndex = maxBodyIndex;

        CGame.Instance.PlaySound((int)eSound.eSound_MainMenuBGM, GameObject.Find("Main Camera"), true);

        BodyButton(0);

        //캐릭터 이름 생성시 초기화            
        WeaponButton(0);
        HairButton(0);

    }
    void Update()
    {        
        if (Input.GetMouseButton(0) && rotationCheck == true)
        {
            mouse = CGame.Instance.width / 2 - Input.mousePosition.x;            
            playerObject.transform.Rotate((Vector3.up * mouse * Time.deltaTime) / 2);
        }
    }
    public void BackButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        CGame.Instance.CallPopup();
        Popup popup = GameObject.FindGameObjectWithTag("Popup").GetComponent<Popup>();
        popup.popupState = ePopupState.ePopupState_PrevScene;
    }
    public void SetNickName()
    {
        string str = nickNameText.text;

        if (str.Length <= 10 && str.Length >= 2 && str.Contains(" ") == false)
        {
            nickCheckImg.sprite = CGame.Instance.GetImage("Success");
            nickCheckImg.color = new Color(255, 255, 255, 255);
            nickNameCheck = true;            
        }
        else
        {
            nickCheckImg.sprite = CGame.Instance.GetImage("Fail");
            nickCheckImg.color = new Color(255, 255, 255, 255);
            nickNameText.text = null;
            nickNameCheck = false;
        }
    }
    public void FinishButton()
    {
        if (nickNameCheck == true)
        {
            CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
            CGame.Instance.CallPopup();
            Popup popup = GameObject.FindGameObjectWithTag("Popup").GetComponent<Popup>();
            popup.popupState = ePopupState.ePopupState_nextScene;

            //플레이어 캐릭터 저장
            EquipItem newItem = new EquipItem();
            newItem.accessoriIndex = (int)eItemType.eitemType_None;
            newItem.helmetIndex = (int)eItemType.eitemType_None;
            newItem.weaponIndex = curWeaponIndex;

            EquipBeauty newBeauty = new EquipBeauty();
            newBeauty.genderType = (int)curGender;
            newBeauty.BodyIndex = curBodyIndex;
            newBeauty.HairIndex = curHairIndex;

            CGame.Instance.player.playerCharacter.InitSetting(nickNameText.text, newItem, newBeauty);           

            //플레이어 저장
            CGame.Instance.LocalDB_init(nickNameText.text);           
        }
        else
        {
            StartCoroutine(CGame.Instance.ICallNotice("NickName Error"));
        }
    }
    public void GenderButton(int _nGender)
    {
        if (_nGender < 0 || 1 < _nGender)
            return;

        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);

        GameObject newCharacter = null;

        curGender = (eGenderType)_nGender;
        CGame.Instance.GameObject_del_child(playerObject);

        switch (curGender)
        {
            case eGenderType.eGenderType_Female:
                newCharacter = CGame.Instance.GameObject_from_prefab("Beauty/3");                
                curBodyIndex = 3;
                break;
            case eGenderType.eGenderType_Male:
                newCharacter = CGame.Instance.GameObject_from_prefab("Beauty/9");
                curBodyIndex = 9;
                break;         
        }
        HairButton(0);
        WeaponButton(0);
        newCharacter.transform.localPosition = playerObject.transform.localPosition;
        newCharacter.transform.parent = playerObject.transform;
        newCharacter.transform.rotation = new Quaternion(0, 0.5f, 0, 0);
    }
    public void BodyButton(int _trans)
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);

        GameObject newCharacter = null;
        
        CGame.Instance.GameObject_del_child(playerObject);

        switch (curGender)
        {
            case eGenderType.eGenderType_Female:
                maxBodyIndex = 5;
                break;
            case eGenderType.eGenderType_Male:
                maxBodyIndex = 11;
                break;
        }

        curBodyIndex += _trans;

        if (curBodyIndex < maxBodyIndex - baseCount)
            curBodyIndex = maxBodyIndex - baseCount;
        else if (curBodyIndex >= maxBodyIndex)
            curBodyIndex = maxBodyIndex;    

        newCharacter = CGame.Instance.GameObject_from_prefab("Beauty/" + curBodyIndex);
        newCharacter.transform.localPosition = playerObject.transform.localPosition;
        newCharacter.transform.parent = playerObject.transform;
        newCharacter.transform.rotation = new Quaternion(0, 0.5f, 0, 0);

        BodyName.text = CGame.Instance.dataTable.GetBeautyName(curBodyIndex);
    }
    public void HairButton(int _trans)
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        GameObject newHair = null;
        
        //머리붙일 위치를 자식 객체를 돌면서 찾음
        GameObject oldHair = CGame.Instance.GameObject_get_child(playerObject, "Dummy Prop Head");

        //그 후 기존 머리를 제거
        CGame.Instance.GameObject_del_child(oldHair);

        switch (curGender)
        {
            case eGenderType.eGenderType_Female:
                maxHairIndex = 2;
                break;
            case eGenderType.eGenderType_Male:
                maxHairIndex = 8;
                break;
        }

        curHairIndex += _trans;

        if (curHairIndex <= maxHairIndex - baseCount)
            curHairIndex = maxHairIndex - baseCount;
        else if (curHairIndex >= maxHairIndex)
            curHairIndex = maxHairIndex;

        newHair = CGame.Instance.GameObject_from_prefab("Beauty/" + curHairIndex);
        newHair.transform.parent = oldHair.transform;
        newHair.transform.localPosition = Vector3.zero;
        newHair.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);

        hairName.text = CGame.Instance.dataTable.GetBeautyName(curHairIndex);
    }
    public void WeaponButton(int _trans)
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);

        GameObject newWeapon = null;

        //머리붙일 위치를 자식 객체를 돌면서 찾음
        GameObject oldWeapon = CGame.Instance.GameObject_get_child(playerObject, "Dummy Prop Right");

        //그 후 기존 머리를 제거
        CGame.Instance.GameObject_del_child(oldWeapon);

        curWeaponIndex += _trans;

        if (curWeaponIndex + _trans < 0)
            curWeaponIndex = 0;
        else if (curWeaponIndex >= baseCount)
            curWeaponIndex = baseCount;

        newWeapon = CGame.Instance.GameObject_from_prefab("Item/" + curWeaponIndex);
        newWeapon.transform.parent = oldWeapon.transform;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = new Quaternion(-0.5f, 0, 0, 0.5f);

        weaponName.text = CGame.Instance.dataTable.GetItemName(curWeaponIndex);
    }
    public void RotationButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);

        rotationCheck = (rotationCheck == false) ? true : false;
        
        if (rotationCheck == true)
            rotationButtonImg.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        else
            rotationButtonImg.color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
    }
}
