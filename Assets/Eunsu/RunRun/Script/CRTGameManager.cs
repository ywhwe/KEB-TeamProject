using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using EPOOutline;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Random = UnityEngine.Random;

public class CRTGameManager : WholeGameManager
{
    public static CRTGameManager instance;
    
    [SerializeField] private AudioSource audioSource;
    
    [Header("Canvas")]
    public GameObject canvas;
    private Transform canvasTrans;
    
    [Header("Score Board")]
    public TextMeshProUGUI scoreText;
    
    [Header("Background")]
    public GameObject backComp;
    public GameObject backgroundPrefab;
    
    private GameObject currentBg;
    private GameObject nextBg;
    
    private readonly Vector3 initPos = new(0f, 0f, 0f);
    private readonly Vector3 nextPos = new(1920f, 0f, 0f);

    private readonly Vector3 movePos = new(-5f, 0f, 0f);

    private readonly Quaternion defaultAngle = new(0f, 0f, 0f, 1f);

    private const float moveSpeed = 10f;
    
    [Header("UpNote")]
    public GameObject UpNotePrefab;
    private GameObject upNote;
    private readonly Vector3 upNotePos = new (910f, 100f, 0f);
    
    [Header("DownNote")]
    public GameObject DownNotePrefab;
    private GameObject downNote;
    private readonly Vector3 downNotePos = new (910f, -100f, 0f);
    
    private const float musicBPM = 117f;
    private float musicTempo = 4f;
    private const float stdBPM = 60f;
    private float stdTempo = 4f;

    private float genTime = 0f;

    [HideInInspector] public int rand, noteNumber, getHigher;
    [HideInInspector] public bool isFinished, isTimedOut, isPlayed;
    [HideInInspector] public int noteCount = 0;
    
    public PhotonView pvTest;
    
    public GameObject[] playerposdb; // 각 플레이어 pos 데이터
    private GameObject playerpref; // local 플레이어의 프리펩
    private GameObject playerpos; // local 플레이어의 pos위치
    
    private void Awake()
    {
        instance = this;
        TotalManager.instance.SendMessageSceneStarted();
        
        canvasTrans = canvas.transform;

        isTimedOut = false;
        isFinished = false;
        isPlayed = false;
    }

    private void Start()
    {
        NetworkManager.instance.isDescending = true;
        isGameEnd = false;
        
        BackgroundMove().Forget();
        scoreText.text = 0.ToString();
    }

    private void Update()
    {
        // This is for level up SFX
        if (isPlayed && score is >= 2950 and < 3050 or >= 4950 and < 5050 or >= 6950 and < 7050)
            isPlayed = false;
        
        genTime = (stdBPM / musicBPM) * (musicTempo / stdTempo);
        
        scoreText.text = score.ToString();
    }
    
    private void LateUpdate()
    {
        if (noteCount < 1 && isTimedOut)
            isFinished = true;
    }
    
    private async UniTask GenNotes()
    {
        while (!isFinished)
        {
            await UniTask.WaitForSeconds(genTime);
            
            rand = Random.Range(0, 101);

            switch (rand)
            {
                case > 51 and <= 100:
                    upNote = Instantiate(UpNotePrefab, upNotePos, Quaternion.identity);
                    upNote.transform.SetParent(canvasTrans, false);
                    noteCount++;
                    noteNumber++;
                    break;
                
                case > 0 and <= 50:
                    downNote = Instantiate(DownNotePrefab, downNotePos, Quaternion.identity);
                    downNote.transform.SetParent(canvasTrans, false);
                    noteCount++;
                    noteNumber++;
                    break;
                
                default:
                    Debug.Log("Unexpected Range");
                    break;
            }

            if (noteNumber <= 115) continue;

            isTimedOut = true;
            break;
        }

        isGameEnd = true;

        await UniTask.WaitForSeconds(4f);
        StartCoroutine(EndScene());
    }
    
    public void SetScore(int number)
    {
        score += number;

        if (isPlayed) return; 
        
        switch (score)
        {
            case >= 7000:
                SoundManagerForCRT.instance.PlaySound("LevelUp");
                isPlayed = true;
                stdTempo = 8f;
                break;
            case >= 5000:
                SoundManagerForCRT.instance.PlaySound("LevelUp");
                isPlayed = true;
                getHigher = 3;
                break;
            case >= 3000:
                SoundManagerForCRT.instance.PlaySound("LevelUp");
                isPlayed = true;
                getHigher = 2;
                break;
            case >= 1000:
                SoundManagerForCRT.instance.PlaySound("LevelUp");
                isPlayed = true;
                getHigher = 1;
                break;
            default:
                getHigher = 0;
                break;
        }
    }
    
    private async UniTask BackgroundMove()
    {
        while (Application.isPlaying)
        {
            await UniTask.Yield();
            
            currentBg ??= Instantiate(backgroundPrefab, initPos, defaultAngle);
            nextBg ??= Instantiate(backgroundPrefab, nextPos, defaultAngle);
            
            currentBg.transform.SetParent(backComp.transform, false);
            nextBg.transform.SetParent(backComp.transform, false);
            
            currentBg.transform.Translate(movePos * (moveSpeed * Time.deltaTime));
            nextBg.transform.Translate(movePos * (moveSpeed * Time.deltaTime));

            if (!(nextBg.transform.position.x < -200f)) continue;
            
            var bg = Instantiate(backgroundPrefab, nextPos, defaultAngle);
            
            Destroy(currentBg);

            currentBg = nextBg;
            nextBg = bg;
        }
    }

    public override void GameStart()
    {
        CharacterMotionController.instance.isTwoKey = true;
        
        GenNotes().Forget();
        
        audioSource.Play();
    }

    public override void SpawnObsPlayer()
    {
        playerpref = TotalManager.instance.obplayerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        Debug.Log(index);
        playerpos= playerposdb[index];
        var obj = PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.identity);
        // obj.transform.SetParent(playerpos.transform);
        obj.GetComponent<Outlinable>().enabled = true;
        obj.transform.localScale = new Vector3(2.1f,2.1f,2.1f);
    }
    public override void ReadyForStart()
    {
        
    }
    private IEnumerator EndScene()
    {
        CharacterMotionController.instance.isTwoKey = false;
        yield return new WaitForSeconds(1f);
        TotalManager.instance.StartFinish();
    }
}
