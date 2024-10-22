using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EasyTransition;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TotalManager : MonoBehaviourPunCallbacks
{
    public static TotalManager instance;

    public GameObject[] prefabDB;
    public GameObject[] obprefabDB;
    public int playerPrefabNumber = 0;
    public GameObject playerPrefab => prefabDB[playerPrefabNumber];
    public GameObject obplayerPrefab => obprefabDB[playerPrefabNumber];

    public Sprite[] fadeimgaedb;
    public Image fadeimage;
    public Image fadeScreen;
    public Image optionScreen;
    public Image waitScreen;
    public Sprite[] waitSpriteDB;
    public Image waitImage;
    
    public Button optionGameExitButton;
    public Button optionResumeButton;
    private bool optionEnabled = false;
    
    public TextMeshProUGUI BGMVolumeText;
    public TextMeshProUGUI SFXVolumeText;
    public AudioMixer mixer;
    public Slider BGMVolumeSlider;
    private float perBGMVolume;
    public Slider SFXVolumeSlider;
    private float perSFXVolume;
    private bool isSceneStarted;
    
    public GameObject gameManager;
    public PhotonView PV;
    private int isGameEnd = 0;
    private int isGameStart = 0;
    public WaitForSeconds waitHalfSecond = new WaitForSeconds(0.5f);
    public WaitForSeconds waitTwoSecond = new WaitForSeconds(2f);
    public WaitForSeconds waitFiveSeconds = new WaitForSeconds(5f);
    public AudioSource BGM;

    public int gameRound=0;
    public List<int> NextgameNum;

    public Texture2D cursurIcon;
    private TransitionManager manager;

    public TransitionSettings transition;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMLevel);
        SFXVolumeSlider.onValueChanged.AddListener(SetSFXLevel);
        BGM = GetComponent<AudioSource>();
        BGM.Play();
        Cursor.SetCursor(cursurIcon,Vector2.zero, CursorMode.Auto);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionEnabled)
            {
                optionScreen.gameObject.SetActive(false);
                optionEnabled = false;
            }
            else
            {
                optionScreen.gameObject.SetActive(true);
                optionEnabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(PhotonNetwork.NetworkClientState);
        }
    }

    public void MoveScene(int id)
    {
        MoveFadeScene(id); //  warning CS4014: Because this call is not awaited,
                           // execution of the current method continues before the call is completed.
                           // Consider applying the 'await' operator to the result of the call.
    }
    public void MoveScene(string str)
    {
        MoveFadeScene(str); //  warning CS4014: Because this call is not awaited,
                            // execution of the current method continues before the call is completed.
                            // Consider applying the 'await' operator to the result of the call.
    }
    

    #region UniTask 로 로드씬구현

    public void GoToGameScene()
    {
        PV.RPC("rpcGoToGameScene",RpcTarget.All);
    }

    [PunRPC]
    public void rpcGoToGameScene()
    {
        UniTaskGoToGameScene(); //  warning CS4014: Because this call is not awaited,
                                // execution of the current method continues before the call is completed.
                                // Consider applying the 'await' operator to the result of the call.
    }
    private async UniTask UniTaskGoToGameScene()
    {
        gameRound++;
        await MoveFadeScene(NextgameNum[gameRound]);
        gameManager = GameObject.Find("GameManager");
        BGM.Stop();
        await ReadyForStart();
        gameManager.GetComponent<WholeGameManager>().SpawnObsPlayer();
        SendGameStart();
    }

  
    #region RPC Call

    public void CallCountForStart()
    {
        PV.RPC("rpcCountForStart",RpcTarget.All);
    }
    

    [PunRPC]
    private void rpcCountForStart()
    {
        UniTaskCountForStart(); //  warning CS4014: Because this call is not awaited,
                                // execution of the current method continues before the call is completed.
                                // Consider applying the 'await' operator to the result of the call.
    }
    private async UniTask UniTaskCountForStart()
    {
        await UniTask.WaitForSeconds(2f); // *** need count 
        for (int i = 1; i < 4; i++)
        {
            waitImage.sprite = waitSpriteDB[i]; // 3 2 1
            SoundManager.instance.PlaySound((4-i).ToString());
            await UniTask.WaitForSeconds(0.5f);
        }
        waitImage.sprite = waitSpriteDB[4]; //Go
        SoundManager.instance.PlaySound("Go");
        await UniTask.WaitForSeconds(0.5f);
        waitScreen.gameObject.SetActive(false);
        gameManager.GetComponent<WholeGameManager>().GameStart();
    }
    public async UniTask UniReadyCount()
    {
        waitImage.sprite = waitSpriteDB[0]; //Ready
        gameManager.GetComponent<WholeGameManager>().ReadyForStart();
        waitScreen.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(1f); // *** need count 
        for (int i = 1; i < 4; i++)
        {
            waitImage.sprite = waitSpriteDB[i]; // 3 2 1
            SoundManager.instance.PlaySound((4-i).ToString());
            await UniTask.WaitForSeconds(0.5f);
        }
        waitImage.sprite = waitSpriteDB[4]; //Go
        SoundManager.instance.PlaySound("Go");
        await UniTask.WaitForSeconds(0.5f);
        waitScreen.gameObject.SetActive(false);
 
    }
    public void SendGameStart()
    {
        PV.RPC("rpcSendGameStart",RpcTarget.MasterClient);
    }
    [PunRPC]
    void rpcSendGameStart()
    {
        isGameStart++;
        
        if (isGameStart == PhotonNetwork.PlayerList.Length)
        {
            CallCountForStart();
            isGameStart = 0;
        }
    }

    #endregion
    
    #region MoveFade
    private async UniTask MoveFadeScene(int id)
    {
        fadeimage.sprite = fadeimgaedb[id-1];
        await FadeScreenTask(true);
        fadeimage.gameObject.SetActive(true);
        await FadeScreenTask(false);
        await UniTask.WaitForSeconds(5f);
        await FadeScreenTask(true);
        fadeimage.gameObject.SetActive(false);

        isSceneStarted = false;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(id);
        }
        
        await UniTask.WaitUntil(() => isSceneStarted);
        await FadeScreenTask(false);
    }
    
    public void SendMessageSceneStarted()
    {
        isSceneStarted = true;
    }
    
    private async UniTask MoveFadeScene(string str)
    {
        await FadeScreenTask(true);
        PhotonNetwork.LoadLevel(str);
        await FadeScreenTask(false);
    }
    private async UniTask ReadyForStart()
    {
        waitScreen.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(2f);
        waitImage.sprite = waitSpriteDB[0]; //Ready
        gameManager.GetComponent<WholeGameManager>().ReadyForStart();
    }

    private async UniTask FadeImageTask(bool fadeOut)
    {
        var fadeTimer = 0f;
        const float FadeDuration = 1f;

        var initialValue = fadeOut ? 0f : 1f;
        var fadeDir = fadeOut ? 1f : -1f;
        
        while (fadeTimer < FadeDuration)
        {
            await UniTask.NextFrame(); //한 프레임만 기다려서 업데이트처럼 프레임당 움직임
            fadeTimer += Time.deltaTime;

            var color = fadeimage.color;

            initialValue += fadeDir * Time.deltaTime;
            color.a = initialValue;

            fadeimage.color = color;
        }
    }
    private async UniTask FadeScreenTask(bool fadeOut)
    {
        var fadeTimer = 0f;
        const float FadeDuration = 1f;

        var initialValue = fadeOut ? 0f : 1f;
        var fadeDir = fadeOut ? 1f : -1f;
        
        while (fadeTimer < FadeDuration)
        {
            await UniTask.NextFrame(); //한 프레임만 기다려서 업데이트처럼 프레임당 움직임
            fadeTimer += Time.deltaTime;

            var color = fadeScreen.color;

            initialValue += fadeDir * Time.deltaTime;
            color.a = initialValue;

            fadeScreen.color = color;
        }
    }

    #endregion
    
    
    #endregion

    #region TestSceneField //test씬 만들기
    
    public void MoveTestScene()
    {
        StartCoroutine(GoTestScene());
    }
    public IEnumerator GoTestScene()
    {
        yield return StartCoroutine(FadeScreen(true));
        SceneManager.LoadScene(1);
        yield return StartCoroutine(FadeScreen(false));
        gameManager = GameObject.Find("GameManager");
    }
    public void GoToTestgame()
    {
        int gameNumber = Random.Range(1, 2);
        MoveTestScene();
        BGM.Stop();
        StartCoroutine(CountBeforeStart()); // sendgameend 방식을 응용해서 플레이어가 준비 되면 start하는걸 고민
    }
    #endregion

    private IEnumerator FadeScreen(bool fadeOut)
    {
        var fadeTimer = 0f;
        const float FadeDuration = 1f;

        var initialValue = fadeOut ? 0f : 1f;
        var fadeDir = fadeOut ? 1f : -1f;
        
        while (fadeTimer < FadeDuration)
        {
            yield return null; //한 프레임만 기다려서 업데이트처럼 프레임당 움직임
            fadeTimer += Time.deltaTime;

            var color = fadeScreen.color;

            initialValue += fadeDir * Time.deltaTime;
            color.a = initialValue;

            fadeScreen.color = color;
        }
    }

    public void SetBGMVolumeText()
    {
        perBGMVolume = BGMVolumeSlider.value * 100.0f;
        BGMVolumeText.text = perBGMVolume.ToString("N0") + "%";
    }
    
    public void SetSFXVolumeText()
    {
        perSFXVolume = SFXVolumeSlider.value * 100.0f;
        SFXVolumeText.text = perSFXVolume.ToString("N0") + "%";
    }

    public void SetBGMLevel(float sliderVal) {
        mixer.SetFloat("BGM", Mathf.Log10(sliderVal)*20);
    }
    
    public void SetSFXLevel(float sliderVal) {
        mixer.SetFloat("SFX", Mathf.Log10(sliderVal)*20);
    }
    
    private IEnumerator CountBeforeStart()
    {
        waitScreen.gameObject.SetActive(true);
        yield return waitTwoSecond;
        waitImage.sprite = waitSpriteDB[0]; //Ready
        yield return waitTwoSecond;           // *** need count 
        for (int i = 1; i < 4; i++)
        {
            waitImage.sprite = waitSpriteDB[i]; // 3 2 1
            SoundManager.instance.PlaySound((4-i).ToString());
            yield return waitHalfSecond;
        }
        waitImage.sprite = waitSpriteDB[4]; //Go
        SoundManager.instance.PlaySound("Go");
        yield return waitHalfSecond;
        waitScreen.gameObject.SetActive(false);
        gameManager.GetComponent<WholeGameManager>().GameStart();
    }

    public void StartFinish()
    {
        StartCoroutine(CountBeforeFinish());
    }
    private IEnumerator CountBeforeFinish()
    {
        gameManager.GetComponent<WholeGameManager>().GetScore();
        yield return waitHalfSecond;
        waitScreen.gameObject.SetActive(true);
        waitImage.sprite = waitSpriteDB[5]; //Finish
        yield return waitTwoSecond;
        waitScreen.gameObject.SetActive(false);
        waitImage.sprite = waitSpriteDB[0];
        isSceneStarted = false;
        SendGameEnd();
    }
    
    public void ResumeGame()
    {
        optionScreen.gameObject.SetActive(false);
        optionEnabled = false;
    }

    public void ExitGame()
    {
#if  UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    void SendGameEnd()
    {
        PV.RPC("rpcSendgameEnd",RpcTarget.MasterClient);
    }
    [PunRPC]
    void rpcSendgameEnd()
    {
        isGameEnd++;
        
        if (isGameEnd == PhotonNetwork.PlayerList.Length)
        {
            
            PV.RPC("TransGameEnd",RpcTarget.All);
        }
    }

    [PunRPC]
    void TransGameEnd()
    {
        manager = TransitionManager.Instance();
        manager.onTransitionCutPointReached += TransScore;
        manager.Transition(transition,0.01f);
    }

    public void TransScore()
    {
        UniTransScore();

    }

    async UniTask UniTransScore()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("ScoreBoard");
        }
        isGameEnd = 0;
        await UniTask.WaitUntil(() => isSceneStarted = true);
        manager.onTransitionCutPointReached -= TransScore;
    }
}