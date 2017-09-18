using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoScene : MonoBehaviour {
    public Text logoText;
    Outline textOutLine;
    float time = 0;
    bool bFalg = false;
    Animator animator;
    float fColorValue = 0;
    float sceneTime = 0;

	void Start () {        
        textOutLine = logoText.gameObject.GetComponent<Outline>();
        animator = logoText.GetComponent<Animator>();
        animator.SetBool("LogoActive",true);
        CGame.Instance.PlaySound((int)eSound.eSound_GameIntro, GameObject.Find("Main Camera"), false);
    }
	
	void Update () {
        time += Time.deltaTime;
        sceneTime += Time.deltaTime;

        if (time > 0.5f)
        {
            logoText.gameObject.GetComponent<Outline>().enabled = bFalg;
            bFalg = bFalg == false ? true : false;
            time = 0;
        }
        if (sceneTime > 3.0f)
            NextScene();
    }
    public void NextScene()
    {
        CGame.Instance.SceneChange(1);
    }
}
