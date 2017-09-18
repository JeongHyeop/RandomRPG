using UnityEngine;
using System.Collections;

public class MoblieSetting : MonoBehaviour {
    private bool exit = false;
    private float time = 0.0f;

    void Awake () {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.SetResolution(720, 1280, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update () {
        ExitApplication();
    }

    void ExitApplication () {
        if (exit) {
            time = +Time.deltaTime;
            if(time >= 1f) {
                exit = false;
                time = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!exit) {
                exit = true;
            }
            else {
                //팝업 불러서 종료 할 건지 물어보고 종료
                CGame.Instance.CallExit();
            }
        }
    }

}
