using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TotalManager : MonoBehaviour
{
    public static TotalManager instance;

    public Button gameStartButton;
    
    public Image fadeScreen;
    public Image optionScreen;
    
    public Button optionGameExitButton;
    public Button optionResumeButton;
    private bool optionEnabled = false;
    
    public TextMeshProUGUI volumeText;
    public AudioMixer mixer;
    public Slider volumeSlider;
    private float perVolume;

    private float readyTimer = 1f;
    private float readyTime = 0f;
    private int startTime = 0;
    
    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        volumeSlider.onValueChanged.AddListener(SetLevel);
        Ready();
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

    public void MoveScene(string sceneName)
    {
        StartCoroutine(MoveSceneWithFade(sceneName));
    }

    private IEnumerator MoveSceneWithFade(int id)
    {
        yield return StartCoroutine(FadeScreen(true));// yield return이 Coroutine이 끝날때까지 기다림
        SceneManager.LoadScene(id);
        yield return StartCoroutine(FadeScreen(false));
        
    }
    
    private IEnumerator MoveSceneWithFade(string sceneName)
    {
        yield return StartCoroutine(FadeScreen(true));// yield return이 Coroutine이 끝날때까지 기다림
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(FadeScreen(false));
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

    public void ButtonSFX()
    {
        AudioSource buttonSFX = GetComponent<AudioSource>();
        buttonSFX.Play();
    }

    public void GoToIngame()
    {
        int gameNumber = Random.Range(1, 3);
        TotalManager.instance.MoveScene(gameNumber);
        gameStartButton.interactable = false;
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
}