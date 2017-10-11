using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuntSceneUI : MonoBehaviour {

    private Transform DamagePool;
    private List<DamageText> lDamageText;

    void Start () {
		
	}
	

	void Update () {
		
	}
    
    public void SetDamageText(GameObject target , float damage)
    {
        Vector3 targetpos = transform.position; //메인카메라.WorldToScreenPoint(target.transform.localPosition)으로 변경;
          targetpos = new Vector3(targetpos.x, targetpos.y + 200);
        foreach (var DamText in lDamageText)
        {
            if (!DamText.gameObject.activeSelf)
            {
                //Debug.Log(newPos + " / " + _vec3.name);
                DamText.transform.localPosition = targetpos;
                DamText.Init(damage.ToString());
                return;

                //연결되면 연결 된 어택에 float Damage = (attack - enemy.defence);
                //SetDamageText(enemy , Damage);
            }
        }
    }

}
