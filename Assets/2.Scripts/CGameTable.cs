using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Text;

public class CGameTable : MonoBehaviour {

    public DataTable_MenuScene[] dataTableMenuScene;
    public DataTable_Beauty[] dataTableBeatuy;
    public DataTable_Item[] dataTableItem;
    public DataTable_Character[] dataTableCharacter;
    public DataTable_Hunt[] dataTableHunt;

	void Start () {
        //데이터 로드        
        LoadTableDataBeauty();
        LoadTableDataItem();
        LoadTableDataCharacter();
        LoadTableDataMenuScene();
        LoadTableDataHunt();
        //위에 데이터 로드 하는거 하나의 함수로 만들어서 묶기 void LoadTableData()
    }
    TextAsset LoadTextAsset(string _txtFile)
    {
        TextAsset ta;
        ta = Resources.Load("Table/" + _txtFile) as TextAsset;
        return ta;
    }

    void LoadTableDataMenuScene()
    {
        if (dataTableMenuScene != null)
            return;

        string txtFilePath = "DataTable_MenuScene";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);
        
        DataTable_MenuScene[] info = new DataTable_MenuScene[line.Count];

        for (int i = 0; i < line.Count; i++)
        {
            if (line[i] == null) continue;
            if (i == 0) continue; 	                    // Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            info[i - 1] = new DataTable_MenuScene();

            info[i - 1].plane = Cells[0];            
            info[i - 1].sky = Cells[1];        

        }
        dataTableMenuScene = info;
    }
    void LoadTableDataCharacter()
    {
        if (dataTableCharacter != null)
            return;

        string txtFilePath = "DataTable_Character";

        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        DataTable_Character[] info = new DataTable_Character[line.Count];

        for (int i = 0; i < line.Count; i++)
        {
            if (line[i] == null) continue;
            if (i == 0) continue; 	                    // Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            info[i - 1] = new DataTable_Character();

            info[i - 1].characterType = (eCharacterType)int.Parse(Cells[0]);
            info[i - 1].index = int.Parse(Cells[1]);
            info[i - 1].name = Cells[2];
            info[i - 1].hp = int.Parse(Cells[3]);
            info[i - 1].mp = int.Parse(Cells[4]);            
            info[i - 1].attack = int.Parse(Cells[5]);
            info[i - 1].defence = int.Parse(Cells[6]);
            info[i - 1].skillAttack = int.Parse(Cells[7]);
            info[i - 1].skillCoolTime = float.Parse(Cells[8]);

            //
            EquipItem newItem = new EquipItem();

            newItem.helmetIndex = int.Parse(Cells[9]);
            newItem.accessoriIndex = int.Parse(Cells[10]);
            newItem.weaponIndex = int.Parse(Cells[11]);

            info[i - 1].equippingItem = newItem;

            //
            EquipBeauty newBeauty = new EquipBeauty();

            newBeauty.genderType = int.Parse(Cells[12]);
            newBeauty.BodyIndex = int.Parse(Cells[13]);
            newBeauty.HairIndex = int.Parse(Cells[14]);            

            info[i - 1].equippingBeauty = newBeauty;

            info[i - 1].exp = int.Parse(Cells[15]);
            info[i - 1].level = int.Parse(Cells[16]);
            info[i - 1].skillIndex = int.Parse(Cells[17]);
        }
        dataTableCharacter = info;
    }

    void LoadTableDataBeauty()
    {
        if (dataTableBeatuy != null)
            return;

        string txtFilePath = "DataTable_Beauty";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        DataTable_Beauty[] info = new DataTable_Beauty[line.Count];

        for (int i = 0; i < line.Count; i++)
        {
            if (line[i] == null) continue;
            if (i == 0) continue; 	                    // Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            info[i - 1] = new DataTable_Beauty();

            info[i - 1].beautyIndex = int.Parse(Cells[0]);
            info[i - 1].beautyType = int.Parse(Cells[1]);
            info[i - 1].genderType = int.Parse(Cells[2]);
            info[i - 1].beautyName = Cells[3];            
        }

        dataTableBeatuy = info;
    }

    void LoadTableDataItem()
    {
        if (dataTableItem != null)
            return;

        string txtFilePath = "DataTable_Item";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        DataTable_Item[] info = new DataTable_Item[line.Count];

        for (int i = 0; i < line.Count; i++)
        {
            if (line[i] == null) continue;
            if (i == 0) continue; 	                    // Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            info[i - 1] = new DataTable_Item();

            info[i - 1].itemIndex = int.Parse(Cells[0]);
            info[i - 1].itemType = (eItemType)int.Parse(Cells[1]);                     
            info[i - 1].itemName = Cells[2];            
            info[i - 1].weaponType = (eWeaponType)int.Parse(Cells[3]);
            info[i - 1].grade = (eItemGrade)int.Parse(Cells[4]);
            info[i - 1].hp = int.Parse(Cells[5]);
            info[i - 1].mp = int.Parse(Cells[6]);            
            info[i - 1].attack = int.Parse(Cells[7]);
            info[i - 1].defence = int.Parse(Cells[8]);
            info[i - 1].skillAttack = int.Parse(Cells[9]);
            info[i - 1].skillCoolTime = float.Parse(Cells[10]);
        }

        dataTableItem = info;
    }
    void LoadTableDataHunt()
    {
        if (dataTableHunt != null)
            return;

        string txtFilePath = "DataTable_Hunt";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        DataTable_Hunt[] info = new DataTable_Hunt[line.Count];

        for (int i = 0; i < line.Count; i++)
        {
            if (line[i] == null) continue;
            if (i == 0) continue; 	                    // Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            info[i - 1] = new DataTable_Hunt();

            info[i - 1].index = int.Parse(Cells[0]);
            info[i - 1].skyIndex = int.Parse(Cells[1]);
            info[i - 1].levelOfDifficulty = int.Parse(Cells[2]);
            info[i - 1].bossIndex = int.Parse(Cells[3]);
            info[i - 1].dropWeaponIndex = int.Parse(Cells[4]);
            info[i - 1].dropHelmetIndex = int.Parse(Cells[5]);
            info[i - 1].dropAccIndex = int.Parse(Cells[6]);
            info[i - 1].itemProbability = int.Parse(Cells[7]);
        }

        dataTableHunt = info;
    }

    public List<string> LineSplit(string text)
    {
        char[] text_buff = text.ToCharArray();

        List<string> lines = new List<string>();

        int linenum = 0;
        bool makecell = false;

        StringBuilder sb = new StringBuilder("");

        for (int i = 0; i < text.Length; i++)
        {
            char c = text_buff[i];

            if (c == '"')
            {
                char nc = text_buff[i + 1];
                if (nc == '"') { i++; } //next char
                else
                {
                    if (makecell == false) { makecell = true; c = nc; i++; } //next char
                    else { makecell = false; c = nc; i++; } //next char
                }
            }

            //0x0a : LF ( Line Feed : 다음줄로 캐럿을 이동 '\n')
            //0x0d : CR ( Carrage Return : 캐럿을 제일 처음으로 복귀 )			    
            if (c == '\n' && makecell == false)
            {
                char pc = text_buff[i - 1];
                if (pc != '\n')	//file end
                {
                    lines.Add(sb.ToString()); sb.Remove(0, sb.Length);
                    linenum++;
                }
            }
            else if (c == '\r' && makecell == false)
            {
            }
            else
            {
                sb.Append(c.ToString());
            }
        }

        return lines;
    }
    public DataTable_Item GetItem(int _index)
    {
        if (_index < 0)
            return null;

        for (int i = 0; i < dataTableItem.Length; i++)
        {
            if (dataTableItem[i].itemIndex == _index)
                return dataTableItem[i];
        }
        return null;
    }
    public string GetBeautyName(int _index)
    {
        if (_index < 0)
            return null;

        for (int i = 0; i < dataTableBeatuy.Length; i++)
        {
            if (dataTableBeatuy[i].beautyIndex == _index)
                return dataTableBeatuy[i].beautyName;
        }

        return null;
    }
    public DataTable_Character GetDataCharacter(int _nIndex)
    {
        if (_nIndex < 0)
            return null;

        for (int i = 0; i < dataTableCharacter.Length - 1; i++)
        {
            if (dataTableCharacter[i].index == _nIndex)
            {
                return dataTableCharacter[i];
            }
        }
        return null;
    }
    public DataTable_Hunt GetDataHunt(int _nIndex)
    {
        if (_nIndex < 0)
            return null;

        for (int i = 0; i < dataTableHunt.Length - 1; i++)
        {
            if (dataTableHunt[i].index == _nIndex)
            {
                return dataTableHunt[i];
            }
        }
        return null;
    }
    public string GetItemName(int _index)
    {
        if (_index < 0)
            return null;

        for (int i = 0; i < dataTableItem.Length; i++)
        {            
            if (dataTableItem[i].itemIndex == _index)
                return dataTableItem[i].itemName;
        }

        return null;
    }
}
//
public class DataTable_MenuScene
{
    public string plane;
    public string sky;
}
public class DataTable_Hunt
{
    public int index;
    public int skyIndex;
    public int levelOfDifficulty;
    public int bossIndex;
    public int dropWeaponIndex;
    public int dropHelmetIndex;
    public int dropAccIndex;
    public int itemProbability;
}
//
public class DataTable_Item
{
    public int itemIndex;
    public eItemType itemType;
    public string itemName;
    public eWeaponType weaponType;
    public eItemGrade grade;
    public int hp;
    public int mp;    
    public int attack;
    public int defence;
    public int skillAttack;
    public float skillCoolTime;
}
//
public class DataTable_Beauty
{
    public int beautyIndex;
    public int genderType;
    public int beautyType;    
    public string beautyName;
}
//
public class DataTable_Character
{
    public eCharacterType characterType;
    public int index;
    public string name;
    public int hp;
    public int mp;    
    public int attack;
    public int defence;
    public int skillAttack;
    public float skillCoolTime;
    public EquipItem equippingItem;
    public EquipBeauty equippingBeauty;
    public int exp;
    public int level;
    public int skillIndex;
}
//
public class PlayerLevelTable
{
    public int maxLevel;
    public int exp;
}
//
public class EquipItem
{
    //아이템 인덱스
    public int helmetIndex;              
    public int accessoriIndex;           
    public int weaponIndex;

    //실제 아이템
    public DataTable_Item equippingHelmet { set; get; }
    public DataTable_Item equippingWeapon { set; get; }
    public DataTable_Item equippingAcc { set; get; }
}
//
public class EquipBeauty
{
    public int genderType;    
    public int BodyIndex;
    public int HairIndex;
}
