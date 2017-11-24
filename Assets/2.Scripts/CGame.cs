using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CGame : MonoBehaviour
{

    public string sDevice = "";
    public string sLanguage = "English";	//language 0: korean 1: english 2: japanese 3: chinese
    public string sCountry = "A1";          
    public string sMarket = "google";
    public string sMarket_id = "market_id";
    public string sMarket_token = "market_token";

    public bool bGameInit = false;
    public int nSceneNumber_cur = 0;

    //화면 해상도
    public int width = 720;
    public int height = 1280;

    //데이터 테이블
    public CGameTable dataTable;

    //플레이어 객체
    public Player player;

    //팝업 & 알림창
    public GameObject notice;
    public GameObject popup;    
    GameObject exitPopup;

    //아이템
    public ItemPanel itemPanel;
    public int nBuyItemindex;
    public bool bRandomCheck;
    //사운드
    public float effectSound = 0.5f;
    public float bgmSound = 0.5f;
    public GameObject bgmObject;
    GameObject sndObject;

    private static CGame s_instance = null; //싱글톤
    public static CGame Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance //= new CGame();
                    = FindObjectOfType(typeof(CGame)) as CGame;
            }
            return s_instance;
        }
    }

    void Awake()
    {
        if (s_instance != null)
        {
            Debug.LogError("Cannot have two instances of CGame.");
            return;
        }
        s_instance = this;

        //테이블 로드
        dataTable = (CGameTable)gameObject.AddComponent(typeof(CGameTable));
        
        //플레이어 로드
        player = (Player)gameObject.AddComponent(typeof(Player));

        //사운드 로드
        SoundLoad();

        DontDestroyOnLoad(this);        
    }


    void Start()
    {
        bGameInit = true; //게임 초기화 종료.
    }


    void Update()
    {
    }
    void OnApplicationQuit()
    {
        //Debug.Log(">>>>>>>>>>>>>>>>>>>> OnApplicationQuit");
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //Debug.Log("go to Background");
        }
        else
        {
            //Debug.Log("go to Foreground");
        }
    }


    //데이터 로드
    public void LocalDB_init(string _nickName) //클라 정보, 최초 1회.
    {
        Player player = CGame.Instance.player;

        player.playerData.nickName = _nickName;
        player.playerData.level = 1;
        player.playerData.exp = 0;
        player.playerData.gold = 100;        
        player.playerData.point = 3;          

        LocalDB_save();
    }

    public void LocalDB_save()
    {
        Player player = CGame.Instance.player;       

        player.bPlayerData = true;

        //Serialize -----------------------------
        string db =
            player.playerData.nickName + "\t" + 
            player.playerData.level + "\t" +
            player.playerData.exp + "\t" +
            player.playerData.gold + "\t" +
            player.playerData.point + "\t" +
 
            "";       

        PlayerPrefs.SetString("Player_data", db);  //save
    }

    public void LocalDB_load() //클라 정보, 초기화 로딩.
    {
        Player player = CGame.Instance.player;

        if (player.playerData == null)
            Debug.Log("Player Data Null");

        string db = PlayerPrefs.GetString("Player_data");  //load
        
        if (db.Length > 3)
            player.bPlayerData = true;
        else
            return;

        //로드 코드
        string[] Cells = db.Split("\t"[0]);        

        player.playerData.nickName = Cells[0];
        player.playerData.level = int.Parse(Cells[1]);
        player.playerData.exp = int.Parse(Cells[2]);
        player.playerData.gold = int.Parse(Cells[3]);
        player.playerData.point = int.Parse(Cells[4]);

        print("LoadDatabase : " + db.Length + " " + db);    
    }
    public void SoundSave()
    {
        string db =
            effectSound + "\t" +
            bgmSound + "\t" +            
            "";

        PlayerPrefs.SetString("Sound_Data", db);  //save
    }
    public void SoundLoad()
    {
        string db = PlayerPrefs.GetString("Sound_Data");  //load

        if (db.Length < 4)
        {
            SoundSave();
            return;
        }
        
        //로드 코드
        string[] Cells = db.Split("\t"[0]);

        effectSound = float.Parse(Cells[0]);
        bgmSound = float.Parse(Cells[1]);   
    }
    //------------------------------------------------------------------------------------------------
    // 씬 변경을 위한 호출함수.
    public void SceneChange(int _number)
    {
        if (nSceneNumber_cur == 0)
            LocalDB_load();

        if (nSceneNumber_cur != 0)
        {
            //로딩시 화면처리.
            //CGame.Instance.Show_Window("Prefab/WindowLoading", null);

            //GameObject loading = (GameObject)Instantiate(Resources.Load("prefab/screen_loading", typeof(GameObject)));
            //loading.transform.parent = Camera.main.transform;
            //loading.transform.localPosition = new Vector3( 0, 0, 0.5f ); //카메라 바로 앞.
        }
        itemPanel.ExitItemMenu();
        SceneManager.LoadScene(_number);

        nSceneNumber_cur = _number;

    }
    public void SetBGM(GameObject _bgm)
    {
        bgmObject = _bgm;
    }
    public GameObject PlayFx(int _nIndex, Vector3 _pos, float _lifetime = 3.0f)
    {
        GameObject fxCloneGO = GameObject_from_prefab("Fx/" + _nIndex);
        if (fxCloneGO == null) return null;

        Destroy(fxCloneGO, _lifetime);

        return fxCloneGO;
    }
    public GameObject PlayFx(string _strName, Vector3 _pos, float _lifetime = 3.0f)
    {
        GameObject fxCloneGO = GameObject_from_prefab("Fx/" + _strName);
        if (fxCloneGO == null) return null;

        Destroy(fxCloneGO, _lifetime);

        return fxCloneGO;
    }
    //사운드
    public GameObject PlaySound<T>(T _name , GameObject _pos, bool bLoop)
    {
        SoundLoad();        

        AudioClip obj = (AudioClip)Resources.Load("Sounds/" + _name, typeof(AudioClip));

        if (bLoop == true)
        {
            if (bgmObject == null)
            {
                bgmObject = new GameObject();
                bgmObject.AddComponent(typeof(AudioSource));
            }

            bgmObject.GetComponent<AudioSource>().clip = obj;
            bgmObject.transform.parent = _pos.transform; // AudioListener
            bgmObject.transform.position = _pos.transform.position;

            bgmObject.GetComponent<AudioSource>().loop = true;
            bgmObject.GetComponent<AudioSource>().volume = bgmSound;

            bgmObject.GetComponent<AudioSource>().playOnAwake = true;
            bgmObject.GetComponent<AudioSource>().Play();   

            return bgmObject;
        }
        else
        {
            if (sndObject == null)
            {
                sndObject = new GameObject();
                sndObject.AddComponent(typeof(AudioSource));
            }

            sndObject.GetComponent<AudioSource>().clip = obj;
            sndObject.transform.parent = _pos.transform; // AudioListener
            sndObject.transform.position = _pos.transform.position;

            sndObject.GetComponent<AudioSource>().loop = false;
            sndObject.GetComponent<AudioSource>().volume = effectSound;

            sndObject.GetComponent<AudioSource>().playOnAwake = true;
            sndObject.GetComponent<AudioSource>().Play();

            return sndObject;
        }        
    }
    public Sprite GetImage(string _imgName)
    {
        Sprite img = (Sprite)Resources.Load("Images/" + _imgName, typeof(Sprite));
        return img;
    }
    public Material GetMaterial(string _material)
    {
        Material mate = (Material)Resources.Load("Material/" + _material, typeof(Material));
        return mate;
    }
    //------------------------------------------------------------------------------------------------
    // 리소스 이미지 로드.
    public Texture2D GetResourceImage(string _imagename)
    {
        string imageName = _imagename; // "path/" + _imagename;
        Texture2D texture = (Texture2D)Resources.Load(imageName);
        return texture;
    }

    // GameObject 텍스처 변경.
    public void GameObject_set_texture(GameObject go, Texture2D _tx)
    {
      //  go.GetComponent<Renderer>().material.Texture = _tx;
        //go.GetComponent<Renderer>().material.color = new Color(1,1,1,1.0f);
    }

    // GameObject에 prefab을 로드하여 어태치하기
    public GameObject GameObject_from_prefab(string _prefab_name)
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/" + _prefab_name, typeof(GameObject)));

        return go;
    }

    // GameObject의 UI Image 의 sprite 변경
    public void GameObject_set_image(GameObject go, string _path) //"image/test"
    {
        //GameObject go = GameObject.FindGameObjectWithTag("userTag1");
        Image myImage = go.GetComponent<Image>();
        myImage.sprite = Resources.Load<Sprite>(_path) as Sprite;
    }

    // 객체의 이름을 통하여 자식 요소를 찾아서 리턴하는 함수 
    //UILabel _label = CGame.Instance.GameObject_get_child(obj, "_label").GetComponent<UILabel>();
    public GameObject GameObject_get_child(GameObject source, string strName)
    {
        Transform[] AllData = source.GetComponentsInChildren<Transform>(true); //비활성포함.

        GameObject target = null;

        foreach (Transform Obj in AllData)
        {
            if (Obj.name == strName)
            {
                target = Obj.gameObject;
                break;
            }
        }
        return target;
    }

    //객체에 붙은 Child를 제거
    public void GameObject_del_child(GameObject source)
    {
        Transform[] AllData = source.GetComponentsInChildren<Transform>(true); //비활성포함.
        foreach (Transform Obj in AllData)
        {
            if (Obj.gameObject != source) //자신 제외. 
            {
                Destroy(Obj.gameObject);
            }
        }
    }
    //팝업창 호출
    public void CallPopup()
    {
        if (popup == null)
        {
            popup = GameObject_from_prefab("Popup");
            popup.transform.parent = GameObject.Find("Canvas").transform;
            popup.transform.position = popup.transform.parent.position;            

            if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)                   
                popup.GetComponent<RectTransform>().sizeDelta = new Vector2(570, 300);
        }
    }
    public void CallExit()
    {
        if (exitPopup == null)
        {
            exitPopup = GameObject_from_prefab("ExitPopup");
            exitPopup.transform.parent = GameObject.Find("Canvas").transform;
            exitPopup.transform.position = exitPopup.transform.parent.position;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                popup.GetComponent<RectTransform>().sizeDelta = new Vector2(570 * 2, 300 * 2);
        }
    }
    public void CallNotice(string _str)
    {
        if (notice == null)
        {
            notice = GameObject_from_prefab("Notice");
            notice.transform.parent = GameObject.Find("Canvas").transform;
            Text text = GameObject_get_child(notice, "Text").GetComponent<Text>();
            text.text = _str;
            notice.transform.position = notice.transform.parent.position;
            notice.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(500, 100);
        }
    }
    public IEnumerator ICallNotice(string _str)
    {
        CallNotice(_str);
        yield return new WaitForSeconds(1.5f);
        if (CGame.Instance.notice != null)
            Destroy(CGame.Instance.notice);
    }
    //------------------------------------------------------------------------------------------------
    //스크린 좌표
    public Vector3 GetScreenPosition()
    {
        Camera camera = Camera.main;
        Vector3 p = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));
        return p;
    }

    //마우스 포인트에 타겟 피킹
    public GameObject GetRaycastObject()
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다.
        //마우스 근처에 오브젝트가 있는지 확인
        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
        {
            target = hit.collider.gameObject; //있으면 오브젝트를 저장한다.
        }
        return target;
    }
    public Vector3 GetRaycastObjectPoint()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    // 2D 유닛 히트처리 부분.  레이를 쏴서 처리합니다. 
    public GameObject GetRaycastObject2D()
    {
        GameObject target = null;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0);
        if (hit.collider != null)
        {
            //Debug.Log (hit.collider.name);  //이 부분을 활성화 하면, 선택된 오브젝트의 이름이 찍혀 나옵니다. 
            target = hit.collider.gameObject;  //히트 된 게임 오브젝트를 타겟으로 지정
        }
        return target;
    } 

}
