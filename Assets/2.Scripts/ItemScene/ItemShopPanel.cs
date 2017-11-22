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
        switch (_eWeapontype)
        {
            case eWeaponType.eWeaponType_Club:
                CGame.Instance.itemPanel.CallItemInfo(0, eScene.eScene_ItemScene);
                break;
            case eWeaponType.eWeaponType_Dagger:
                CGame.Instance.itemPanel.CallItemInfo(1, eScene.eScene_ItemScene);
                break;
            case eWeaponType.eWeaponType_Wand:
                CGame.Instance.itemPanel.CallItemInfo(2, eScene.eScene_ItemScene);
                break;
        }
        player.playerData.gold -= nBaseitemprice;

    }
    void RandomBox()
    {

    }
    void BuyLegendItem()
    {

    }
}
