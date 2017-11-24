using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScene : MonoBehaviour {
    public GameObject itemspot;
    GameObject itemPrefab;
    //public Button equipbutton;
    public Button extipbutton;
    public eItemType eitemtypenum;
    public int nItemindex;
	// Use this for initialization
	void Start () {
        extipbutton.gameObject.SetActive(false);
        Init(CGame.Instance.nBuyItemindex, eScene.eScene_ItemScene,CGame.Instance.bRandomCheck);
        //equipbutton.onClick.AddListener(delegate { ItemEquip(nItemindex,eitemtypenum); });
        extipbutton.onClick.AddListener(ExitScene);

    }
	public void Init(int _itemindex,eScene _Scenenum,bool _Randombox)
    {
        if (_Randombox == true)
        {
            
            StartCoroutine(CGame.Instance.ICallNotice("RandomBox Open!"));
            CGame.Instance.bRandomCheck = false;
        }
        extipbutton.gameObject.SetActive(true);
        itemPrefab = CGame.Instance.GameObject_from_prefab("Item/" + _itemindex);
        itemPrefab.transform.parent = itemspot.transform;
        itemPrefab.transform.localScale = new Vector3(2, 2, 2);
        itemPrefab.transform.localPosition = new Vector3(0,0,0);
        itemPrefab.transform.rotation = Quaternion.Euler(180, 0, 0);
        CGame.Instance.PlaySound((int)eSound.eSound_LevelUp, GameObject.Find("Main Camera"), false);

        StartCoroutine(ItemPopup(_itemindex));
    }
    IEnumerator ItemPopup(int _itemindex)
    {
        yield return new WaitForSeconds(1.5f);
        CGame.Instance.itemPanel.CallItemInfo(_itemindex, eScene.eScene_ItemScene);
    }

   /* void ItemEquip(int _itemindex, eItemType newItemType)
    {
        switch (newItemType)
        {
            case eItemType.eitemType_Helmet:
                //장착 사운드 넣기
                CGame.Instance.player.playerCharacter.equippingItem.helmetIndex = _itemindex;
                break;
            case eItemType.eitemType_Weapon:
                //장착 사운드 넣기
                CGame.Instance.player.playerCharacter.equippingItem.weaponIndex = _itemindex;
                break;
            case eItemType.eitemType_Accessori:
                //장착 사운드 넣기
                CGame.Instance.player.playerCharacter.equippingItem.accessoriIndex = _itemindex;
                break;
        }
                ExitScene();
    }
    */
    void ExitScene()
    {
        //CGame.Instance.SceneChange((int)eScene.eScene_MainMenu);
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }


}
