using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TotalManager : MonoBehaviourPunCallbacks
{
    public static TotalManager instance;
    
    public Image fadeScreen;
    public Image optionScreen;
    public Image waitScreen;
    public Text waitText;

    
    public Button optionGameExitButton;
    public Button optionResumeButton;
    private bool optionEnabled = false;
    
    public TextMeshProUGUI volumeText;
    public AudioMixer mixer;
    public Slider volumeSlider;
    private float perVolume;

    public GameObject gameManager;
    public WaitForSeconds waitOneSecond = new WaitForSeconds(1f);
    public WaitForSeconds waitFiveSeconds = new WaitForSeconds(5f);

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        volumeSlider.onValueChanged.AddListener(SetLevel);
        AudioSource BGM = GetComponent<AudioSource>();
        BGM.Play();
        Debug.Log("BGM");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionEnabled)
            {
                optionScreen.GameObject().SetActive(false);
                optionEnabled = false;
            }
            else
            {
                optionScreen.GameObject().SetActive(true);
                optionEnabled = true;
            }
        }
    }
    
    public void MoveScene(int id)
    {
        StartCoroutine(MoveSceneWithFade(id));
    }
    

    private IEnumerator MoveSceneWithFade(int id)
    {
        yield return StartCoroutine(FadeScreen(true));
        PhotonNetwork.LoadLevel(id);
        yield return StartCoroutine(FadeScreen(false));
        gameManager = GameObject.Find("GameManager");
    }

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

    public void SetVolumeText()
    {
        perVolume = volumeSlider.value * 100.0f;
        volumeText.text = perVolume.ToString("N0") + "%";
    }

    public void SetLevel(float sliderVal) {
        mixer.SetFloat("BGM", Mathf.Log10(sliderVal)*20);
    }

    public void GoToIngame()
    {
        int gameNumber = Random.Range(1, 2);
        MoveScene(gameNumber);
        StartCoroutine(CountBeforeStart());
    }
    
    private IEnumerator CountBeforeStart()
    {
        waitScreen.GameObject().SetActive(true);
        yield return waitOneSecond;
        for (int i = 5; i >= 1; i--)
        {
            waitText.text = i.ToString();
            yield return waitOneSecond;
        }
        waitText.text= "GAME START!";
        yield return waitOneSecond;
        waitScreen.GameObject().SetActive(false);
        gameManager.GetComponent<WholeGameManager>().GameStart();
    }

    private IEnumerator CountBeforeFinish()
    {
        waitScreen.GameObject().SetActive(true);
        waitText.text = "FINISH!";
        yield return waitFiveSeconds;
        waitText.text = "";
        waitScreen.GameObject().SetActive(false);
        MoveScene(2);
    }
    
    public void ResumeGame()
    {
        optionScreen.GameObject().SetActive(false);
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

    public void ScoreBoardTest()
    {
        StartCoroutine(CountBeforeFinish());
    }
}