using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {
    Text UI_Text;
    public void Init(string strDam)
    {
        gameObject.SetActive(true);

        if (UI_Text == null)
            UI_Text = GetComponent<Text>();

        UI_Text.text = strDam;

        StartCoroutine(setInActive());
    }

    IEnumerator setInActive()
    {
        float time = 0.0f;

        while (time <= 1.0f)
        {
            time += Time.deltaTime;

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 4, 0);

            yield return null;
        }

        gameObject.SetActive(false);

    }

}
