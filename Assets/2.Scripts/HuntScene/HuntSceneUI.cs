﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HuntSceneUI : MonoBehaviour
{
    private Transform damagePool;
    private List<DamageText> ldamagetext;
    private float time = 0f;
    private Camera mainCamera;
    private static HuntSceneUI s_instance = null; //싱글톤

    //정협 화살표 작업 2017.11.08
    public GameObject arrowObject;
    private Animator arrowAnimator;       
            
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

        //Arrow Anim 연결
        arrowAnimator = CGame.Instance.GameObject_get_child(arrowObject, "Arrow").GetComponent<Animator>();
        arrowAnimator.SetBool("ForwardMove", true);
        //카메라 연결
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void SetDamageText(GameObject target, float damage)
    {
        string damagestr = damage.ToString();
        int randompos = UnityEngine.Random.Range(-45, 45);
        

        Vector3 targetpos = Camera.main.WorldToScreenPoint(target.transform.localPosition);
        targetpos = new Vector3(targetpos.x -Screen.width/2 + randompos, targetpos.y - Screen.height/2 +300 );
        GameObject damtext = CGame.Instance.GameObject_from_prefab("DamageText");
        damtext.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        damtext.GetComponent<Text>().text = damagestr;
        if (target == CGame.Instance.player.playerCharacter.characterObject)
        {
            damtext.GetComponent<Text>().color = new Color(1f, 0.3f, 0.3f);
        }

        StartCoroutine(Move(damtext, targetpos));
        
    }
 
    IEnumerator Move (GameObject damtext , Vector3 targetpos)
    {
        float time = 0f;
        
        damtext.transform.localPosition = targetpos;
        while (time <= 0.4f)
        {
            time += Time.deltaTime;
            //damtext.transform.localPosition = new Vector3(damtext.transform.localPosition.x, damtext.transform.localPosition.y + 4, 0);
            damtext.GetComponent<Text>().fontSize = 200 - (int)(time * 270);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(damtext);
    }
    public void SetArrowSign(GameObject _playerCharacter, GameObject _enemyCharacter)
    {
        arrowObject.transform.position = new Vector3(_playerCharacter.transform.position.x, 2.0f, _playerCharacter.transform.position.z);        
        arrowObject.transform.LookAt(_enemyCharacter.transform);       
    }
    public void Shake()
    {
        StartCoroutine(CameraShake());
        Handheld.Vibrate();
    }
   IEnumerator CameraShake()
    {
        float _time = 0.0f;
        float _shakepower = 0f;
        Vector3 orininpos = mainCamera.transform.position;
        while (_time <= 0.4f)
        {
            _shakepower = Random.Range(-0.6f, 0.6f);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + _shakepower, mainCamera.transform.position.y, mainCamera.transform.position.z);
            _time += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = orininpos;
    }
}