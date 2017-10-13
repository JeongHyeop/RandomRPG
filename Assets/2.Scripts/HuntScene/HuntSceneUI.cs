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
        Vector3 targetpos = transform.position; //메인카메라.WorldToScreenPoint(target.transform.localPosition)으로 변경;
        targetpos = new Vector3(targetpos.x, targetpos.y + 200);
        GameObject damtext = CGame.Instance.GameObject_from_prefab("DamageText");
        damtext.transform.localPosition = targetpos;
        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            damtext.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 4, 0);
        }
        Destroy(damtext);

    }
}