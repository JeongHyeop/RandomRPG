using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopPanel : MonoBehaviour
{
    public Button baseitem1;
    public Button baseitem2;
    public Button baseitem3;
    public Button legenditem;
    public Button randombox;
    Player player;
    MainMenuScene mainmenu;
    //가격
    const int nBaseitemprice = 1000;
    const int nRandomboxprice = 100000;
    const int nLegenditemprice = 1000000;
    // Use this for initialization
    void Awake()
    {
        player = CGame.Instance.player;

        baseitem1.onClick.AddListener(delegate { Buybaseitem(eWeaponType.eWeaponType_Club); });
        baseitem2.onClick.AddListener(delegate { Buybaseitem(eWeaponType.eWeaponType_Dagger); });
        baseitem3.onClick.AddListener(delegate { Buybaseitem(eWeaponType.eWeaponType_Wand); });
        legenditem.onClick.AddListener(BuyLegendItem);
        randombox.onClick.AddListener(RandomBox);
    }
    void Buybaseitem(eWeaponType _eWeapontype)
    {
        if (player.playerData.gold < nBaseitemprice)
        {
            CGame.Instance.CallNotice("Not Enough Money...");
            return;
        }
        player.playerData.gold -= nBaseitemprice;
        switch (_eWeapontype)
        {
            case eWeaponType.eWeaponType_Club:
                CGame.Instance.nBuyItemindex = 0;
                CGame.Instance.SceneChange(5);
                break;
            case eWeaponType.eWeaponType_Dagger:
                CGame.Instance.nBuyItemindex = 1;
                CGame.Instance.SceneChange(5);
                break;
            case eWeaponType.eWeaponType_Wand:
                CGame.Instance.nBuyItemindex = 2;
                CGame.Instance.SceneChange(5);
                break;
        }
        

    }
    void RandomBox()
    {
        if (player.playerData.gold < nRandomboxprice)
        {
            CGame.Instance.CallNotice("Not Enough Money...");
            return;
        }
        player.playerData.gold -= nRandomboxprice;
        CGame.Instance.nBuyItemindex = Random.Range(0, 10);
        CGame.Instance.bRandomCheck = true;
        CGame.Instance.SceneChange(5);
    }
    void BuyLegendItem()
    {
        if (player.playerData.gold < nLegenditemprice)
        {
            CGame.Instance.CallNotice("Not Enough Money...");
            return;
        }
        player.playerData.gold -= nLegenditemprice;
        CGame.Instance.nBuyItemindex = 12;
        CGame.Instance.SceneChange(5);
    }
}
