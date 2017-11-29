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
    public static int nBuyItem;
    public static bool bRandomBox = false;
    
    //가격
    const int nBaseitemprice = 1;
    const int nRandomboxprice = 1;
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
        CGame.Instance.LocalDB_save();
        switch (_eWeapontype)
        {
            case eWeaponType.eWeaponType_Club:
                nBuyItem = 0;
                
                break;
            case eWeaponType.eWeaponType_Dagger:
                nBuyItem = 1;
                
                break;
            case eWeaponType.eWeaponType_Wand:
                nBuyItem = 2;
                
                break;
        }
        CGame.Instance.SceneChange(5);
        
    }
    void RandomBox()
    {
        List<int> nList = new List<int>();
        int nIndex =0;
        if (player.playerData.gold < nRandomboxprice)
        {
            CGame.Instance.CallNotice("Not Enough Money...");
            return;
        }
        List<DataTable_Item> itemtable = new List<DataTable_Item>();
        for (int i = 0; i < CGame.Instance.dataTable.dataTableItem.Length-1; i++)
        {
            itemtable.Add(CGame.Instance.dataTable.dataTableItem[i]);
        }
        itemtable.Sort((x, y) => x.grade.CompareTo(y.grade));
        for (int i = 0; i < CGame.Instance.dataTable.dataTableItem.Length-2; i++)
        {
            if ( itemtable[i].grade != itemtable[i + 1].grade)
                nList.Add(i);
        }
        int nRandomNum = Random.Range(1, 100);

        if (nRandomNum <51)
            nIndex = Random.Range(0, nList[0]);
        else if (nRandomNum < 80)
            nIndex = Random.Range(nList[0]+1, nList[1]);
        else if (nRandomNum < 95)
            nIndex = Random.Range(nList[1] + 1, nList[2]);
        else if(nRandomNum < 99)
            nIndex = Random.Range(nList[2] + 1, nList[3]);
        else
            nIndex = Random.Range(nList[3] + 1, CGame.Instance.dataTable.dataTableItem.Length - 1);

        nBuyItem = itemtable[nIndex].itemIndex;
        //itemtable[nIndex].itemType == eItemType.eitemType_Weapon ? 
        player.playerData.gold -= nRandomboxprice;
        CGame.Instance.LocalDB_save();
        bRandomBox = true;
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
        CGame.Instance.LocalDB_save();
        nBuyItem = 12;
        CGame.Instance.SceneChange(5);
    }
}
