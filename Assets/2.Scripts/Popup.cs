using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Popup : MonoBehaviour {

    public ePopupState popupState;
	
	void Start () {
		
	}
    public void OkButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            CGame.Instance.SceneChange(1); 
            return;
        }
        CGame.Instance.SceneChange(CGame.Instance.nSceneNumber_cur + ((int)popupState));
    }
    public void CancelButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        Destroy(this.gameObject);
    }
}
public enum ePopupState
{
    ePopupState_PrevScene = -1,
    ePopupState_nextScene = 1,
}