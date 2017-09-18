using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour {
    public Button backButton;
    public Button creditButton;
    public Button facebookButton;

    public Slider effectSound;
    public Slider bgmSound;

	// Use this for initialization
	void Start () {
        backButton.onClick.AddListener(BackButton);
        creditButton.onClick.AddListener(CreditButton);
        facebookButton.onClick.AddListener(FaceBookPageButton);
        effectSound.onValueChanged.AddListener(EffectSound);
        bgmSound.onValueChanged.AddListener(BgmSound);

        CGame.Instance.SoundLoad();
        EffectSound(CGame.Instance.effectSound);
        BgmSound(CGame.Instance.bgmSound);                
	}
	
	// Update is called once per frame
	void Update () {		
	}
    void BackButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        CGame.Instance.CallPopup();
        Popup popup = GameObject.FindGameObjectWithTag("Popup").GetComponent<Popup>();
        popup.popupState = ePopupState.ePopupState_PrevScene;
    }
    void CreditButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        StartCoroutine(CGame.Instance.ICallNotice("Coming Soon!"));
        Debug.Log("추후 작업");
    }
    void FaceBookPageButton()
    {
        CGame.Instance.PlaySound((int)eSound.eSound_Button, GameObject.Find("Main Camera"), false);
        StartCoroutine(CGame.Instance.ICallNotice("Coming Soon!"));
        Debug.Log("추후 작업");
    }
    void EffectSound(float _fValue)
    {
        effectSound.value = _fValue;
        CGame.Instance.effectSound = effectSound.value;        
        CGame.Instance.SoundSave();
    }
    void BgmSound(float _fValue)
    {
        bgmSound.value = _fValue;        
        CGame.Instance.bgmSound = bgmSound.value;
        CGame.Instance.bgmObject.GetComponent<AudioSource>().volume = bgmSound.value;
        CGame.Instance.SoundSave();
    }
}
