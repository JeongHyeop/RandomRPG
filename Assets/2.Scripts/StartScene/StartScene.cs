using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour {
    public GameObject loadGameButton;

	void Start () {        
        if (CGame.Instance.player.bPlayerData == false)
            loadGameButton.SetActive(false);
        else
            loadGameButton.SetActive(true);
        CGame.Instance.PlaySound((int)eSound.eSound_MainMenuBGM, GameObject.Find("Main Camera"), true);
	}
    public void NewGameButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        CGame.Instance.CallPopup();        
    }
    public void LoadGameButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        CGame.Instance.SceneChange(3);
    }
    public void ExitButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        CGame.Instance.CallExit();
    }
}
