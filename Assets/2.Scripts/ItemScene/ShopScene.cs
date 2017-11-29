using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScene : MonoBehaviour {
    public GameObject itemspot;
    GameObject itemPrefab;
    //public Button equipbutton;
    public Button exitpbutton;
    public bool bRandombool = false;
    public eItemType eitemtypenum;
    public int nItemindex;
	// Use this for initialization
	void Start () {
        nItemindex = ItemShopPanel.nBuyItem;
        bRandombool = ItemShopPanel.bRandomBox;
        itemspot = GameObject.Find("Itemspot");
        eitemtypenum = CGame.Instance.dataTable.dataTableItem[nItemindex].itemType;
        exitpbutton = GameObject.Find("ExitButton").GetComponent<Button>();
        exitpbutton.gameObject.SetActive(false);
        Init(nItemindex, eScene.eScene_ItemScene, bRandombool);
        //equipbutton.onClick.AddListener(delegate { ItemEquip(nItemindex,eitemtypenum); });
        exitpbutton.onClick.AddListener(ExitScene);
    }


	public void Init(int _itemindex,eScene _Scenenum,bool _Randombox)
    {
        if (_Randombox == true)
        {
            
            StartCoroutine(CGame.Instance.ICallNotice("RandomBox Open!"));
            ItemShopPanel.bRandomBox = false;
        }
        exitpbutton.gameObject.SetActive(true);
        itemPrefab = CGame.Instance.GameObject_from_prefab("Item/" + _itemindex);
        itemPrefab.transform.parent = itemspot.transform;
        itemPrefab.transform.localPosition = new Vector3(0, 0, 0);
        switch (eitemtypenum)
        {
            case eItemType.eitemType_Helmet:
                itemPrefab.transform.localScale = new Vector3(1, 1, 1);
                itemPrefab.transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            case eItemType.eitemType_Weapon:
                itemPrefab.transform.localScale = new Vector3(2, 2, 2);
                itemPrefab.transform.rotation = Quaternion.Euler(180, 0, 0);
                break;
            case eItemType.eitemType_Accessori:
                itemPrefab.transform.localScale = new Vector3(1, 1, 1);
                itemPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }



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
        if(CGame.Instance.itemPanel.bItemActive == true)
        CGame.Instance.itemPanel.ExitItemMenu();
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }


}
