using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour {
    public Transform stick;
    Vector3 axis;
    float radius;
    Vector3 defaultCenter;
    Touch myTouch;
    Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        radius = GetComponent<RectTransform>().sizeDelta.y / 2;
        defaultCenter = stick.position;
    }
    public void Move()
    {
        Vector3 touchPos = Vector3.zero;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {            
            Touch[] touches = Input.touches;
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (touches[i].position.x <= CGame.Instance.width / 2)
                {
                    touchPos = touches[i].position;
                    break;
                }
            }
            if (touchPos == Vector3.zero)
                return;
        }
        else
            touchPos = Input.mousePosition;

        axis = (touchPos - defaultCenter).normalized;

        //if(axis.x >= -0.75f && axis.x <= 0.75 && axis.y > 0)
        //    axis = new Vector3(0, 1, 0);
        //else if (axis.x < 0 && axis.y < 0.75f && axis.y > -0.75f)
        //    axis = new Vector3(-1, 0, 0);
        //else if (axis.x >= -0.75f && axis.x <= 0.75f && axis.y < 0)
        //    axis = new Vector3(0, -1, 0);
        //else if (axis.x > 0 && axis.y < 0.75f && axis.y > -0.75f)
        //    axis = new Vector3(1, 0, 0);

        stick.position = defaultCenter + axis * radius;

        float dist = Vector3.Distance(touchPos, defaultCenter);
        if (dist > radius)
            stick.position = defaultCenter + axis * radius;
        else
            stick.position = defaultCenter + axis * dist;        

        //플레이어 무브
        CGame.Instance.player.playerCharacter.moveAxis = axis;
        CGame.Instance.player.playerCharacter.eAct = eCharacterAct.run;
    }
    public void End()
    {
        axis = Vector3.zero;
        stick.position = defaultCenter;
        CGame.Instance.player.playerCharacter.moveAxis = axis;
        CGame.Instance.player.playerCharacter.eAct = eCharacterAct.idle;
    }

}
