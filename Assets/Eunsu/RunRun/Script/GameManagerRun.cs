using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManagerRun : WholeGameManager // Need fix for inheritance
{
    public static GameManagerRun instance;
    
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
    
    [SerializeField]
    private AudioSource audioSource;
    
    private float playerScore;
    
    public PhotonView pvTest;
    
    public GameObject[] playerposdb; // 각 플레이어 pos 데이터
    private GameObject playerpref; // local 플레이어의 프리펩
    private GameObject playerpos; // local 플레이어의 pos위치
    
    private void Awake()
    {
        instance = this;
        playerpref = TotalManager.instance.obplayerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        Debug.Log(index);
        playerpos= playerposdb[index];

    }

    private void Start()
    {
        NetworkManager.instance.isDescending = true;
        isGameEnd = false;
        scoreText.text = 0.ToString();
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        
        if (!(Time.deltaTime > 1f)) return;
        Destroy(NoteController.instance.gameObject);
    }
    
    private void LateUpdate()
    {
        if (NoteController.instance.noteCount < 1 && NoteController.instance.IsTimedOut)
            NoteController.instance.IsFinished = true;
        
        if (!NoteController.instance.IsFinished) return;

        isGameEnd = true;
        StopCoroutine(NoteController.instance.GenNotes());
        StartCoroutine(EndScene());
    }
    
    public void SetScore(int number)
    {
        score += number;
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

            if (!(nextBg.transform.position.x < 0.3f)) continue;
            
            var bg = Instantiate(backgroundPrefab, nextPos, defaultAngle);
            
            Destroy(currentBg);

            currentBg = nextBg;
            nextBg = bg;
        }
    }

    public override void GameStart()
    {
        CharacterMotionController.instance.isTwoKey = true;
        BackgroundMove().Forget();
        StartCoroutine(NoteController.instance.GenNotes());
        audioSource.PlayOneShot(audioSource.clip);
    }

    public override void SpawnObsPlayer()
    {
        var obj = PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.identity);
        obj.transform.SetParent(playerpos.transform);
        obj.transform.localScale = Vector3.one;
    }
    private IEnumerator EndScene()
    {
        CharacterMotionController.instance.isTwoKey = false;
        yield return new WaitForSeconds(1f);
        TotalManager.instance.StartFinish();
    }
}
