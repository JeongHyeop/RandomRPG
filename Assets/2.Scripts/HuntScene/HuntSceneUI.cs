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
        string damagestr = damage.ToString();
        Debug.Log("damtext");
        Debug.Log(damage);
        Vector3 targetpos = Camera.main.WorldToScreenPoint(target.transform.localPosition);
        targetpos = new Vector3(targetpos.x -Screen.width/2 , targetpos.y - Screen.height/2 +200 );
        GameObject damtext = CGame.Instance.GameObject_from_prefab("DamageText");
        damtext.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        damtext.GetComponent<Text>().text = damagestr;
        
        MoveText(damtext, targetpos);
        StartCoroutine(Move(damtext, targetpos));

    }
    public void MoveText(GameObject damtext , Vector3 targetpos)
    {
        
       // Destroy(damtext);
    }
    IEnumerator Move (GameObject damtext , Vector3 targetpos)
    {
        float time = 0f;
        
        damtext.transform.localPosition = targetpos;
        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            damtext.transform.localPosition = new Vector3(damtext.transform.localPosition.x, damtext.transform.localPosition.y + 4, 0);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(damtext);
    }
    
}