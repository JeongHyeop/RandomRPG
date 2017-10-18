using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuntSceneUI : MonoBehaviour
{

    private Transform damagePool;
    private List<DamageText> ldamagetext;
    private float time = 0f;
    private static HuntSceneUI s_instance = null; //싱글톤
    public static HuntSceneUI Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance //= new CGame();
                    = FindObjectOfType(typeof(HuntSceneUI)) as HuntSceneUI;
            }
            return s_instance;
        }
    }

    public void Awake()
    {
        if (s_instance != null)
        {
            Debug.LogError("Cannot have two instances of CGame.");
            return;
        }
        s_instance = this;
        
    }

    public void SetDamageText(GameObject target, float damage)
    {
        Debug.Log("damtext");
        Debug.Log(damage);
        Vector3 targetpos = Camera.main.WorldToScreenPoint(target.transform.localPosition);
        targetpos = new Vector3(targetpos.x/2, targetpos.y/2 );
        GameObject damtext = CGame.Instance.GameObject_from_prefab("DamageText");
        //damtext.transform.parent = gameObject.transform.Find("Canvas");
        //damtext.transform.SetParent(transform.Find("Canvas"));
        damtext.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        MoveText(damtext, targetpos);
        

    }
    public void MoveText(GameObject damtext , Vector3 targetpos)
    {
        time = 0f;
        damtext.transform.localPosition = targetpos;
        while (time <= 2.0f)
        {
            time += Time.deltaTime;
            damtext.transform.Translate(transform.localPosition.x, transform.localPosition.y + 4, 0);
        }
       // Destroy(damtext);
    }

    
}