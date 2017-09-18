using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {    	
    public void OkButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        Application.Quit();                
    }
    public void CancelButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        Destroy(this.gameObject);
    }
}
