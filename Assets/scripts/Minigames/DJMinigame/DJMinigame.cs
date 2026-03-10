using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class DJMinigame : MonoBehaviour, IMinigame
{
    [Header("Core settings")]
    public GameObject notePrefab;
    public RectTransform notesParent;
    public RectTransform spawnPoint;
    public RectTransform hitLine;

    [Header("Spawn")]
    public RectTransform spawnLeft;
    public RectTransform spawnDown;
    public RectTransform spawnUp;
    public RectTransform spawnRight;

    [Header("Difficulty")]
    public float spawnInterval = 1.2f;
    public float noteSpeed = 300f;
    public float hitTolerance = 60f;

    [Header("HP")]
    public int playerHP;
    public int djHP;

    public int playerMaxHP = 5;
    public int djMaxHP = 10;
    public TMP_Text hpText;
    public Slider hpSlider;


    [Header("Audio")]
    public AudioSource musicSource;

    [Header("Prize item")]
    public Sprite bckstagePassSprite;

    private bool isRunning = false;

    public bool IsRunning => isRunning;
    public bool IsFinished { get; private set; }
    public bool PlayerCheated { get; private set; }

    private float startTime;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void ClearNotes()
    {
        foreach (Transform child in notesParent)
        {
            Destroy(child.gameObject);
        }
    }
    public void StartGame()
    {
        gameObject.SetActive(true);

        isRunning = true;
        IsFinished = false;
        PlayerCheated = false;

        UpdateHPUI();
        ClearNotes();
        StartRhythm();
    }


    void StartRhythm()
    {
        SetupDifficulty();
        startTime = Time.time;

        if (musicSource != null)
            musicSource.Play();

        InvokeRepeating(nameof(SpawnNote), 1f, spawnInterval);
    }

    void SetupDifficulty()
    {
        Difficulty difficulty = GameState.Instance.currentDifficulty;

        switch (difficulty)
        {
            case Difficulty.Easy:
                spawnInterval = 1.2f;
                noteSpeed = 250f;
                playerMaxHP = 8;
                djMaxHP = 6;
                break;

            case Difficulty.Medium:
                spawnInterval = 0.9f;
                noteSpeed = 320f;
                playerMaxHP = 5;
                djMaxHP = 8;
                break;

            case Difficulty.Hard:
                spawnInterval = 0.6f;
                noteSpeed = 420f;
                playerMaxHP = 3;
                djMaxHP = 10;
                break;
        }

        playerHP = playerMaxHP;
        djHP = djMaxHP;
    }


    void Update()
    {
        if (!isRunning)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            TryHit(NoteDirection.Left);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            TryHit(NoteDirection.Down);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            TryHit(NoteDirection.Up);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            TryHit(NoteDirection.Right);
    }

    void UpdateHPUI()
    {
        hpText.text = "Ty: " + playerHP + " | DJ: " + djHP;
    }

    void UpdateHPBar()
    {
        float playerRatio = (float)playerHP / playerMaxHP;
        float djRatio = (float)djHP / djMaxHP;

        float balance = playerRatio - djRatio;

        hpSlider.value = balance;
    }


    void SpawnNote()
    {
        int lane = Random.Range(0, 4);

        RectTransform spawn;

        switch (lane)
        {
            case 0:
                spawn = spawnLeft;
                break;

            case 1:
                spawn = spawnDown;
                break;

            case 2:
                spawn = spawnUp;
                break;

            default:
                spawn = spawnRight;
                break;
        }

        GameObject obj = Instantiate(notePrefab, spawn.position, Quaternion.identity, notesParent);

        RhythmNote note = obj.GetComponent<RhythmNote>();

        note.speed = noteSpeed;
        note.direction = (NoteDirection)lane;
        note.minigame = this;
    }

    void TryHit(NoteDirection direction)
    {
        foreach (Transform child in notesParent)
        {
            RhythmNote note = child.GetComponent<RhythmNote>();

            if (note == null)
                continue;

            if (note.direction != direction)
                continue;

            float distance = Mathf.Abs(
                child.GetComponent<RectTransform>().position.y -
                hitLine.position.y
            );

            if (distance < hitTolerance)
            {
                Debug.Log("HIT " + direction);

                note.wasHit = true;
                Destroy(note.gameObject);

                djHP--;
                UpdateHPUI();
                UpdateHPBar();

                CheckGameState();

                return;
            }
        }

        Debug.Log("MISS " + direction);
    }

    public void NoteMissed(RhythmNote note)
    {
        Debug.Log("MISS");

        if (note.wasHit)
        {
            return;
        }

        playerHP--;
        UpdateHPUI();
        UpdateHPBar();

        CheckGameState();
    }
    void CheckGameState()
    {
        if (djHP <= 0)
        {
            PlayerWin();
        }

        if (playerHP <= 0)
        {
            PlayerLose();
        }
    }
    void PlayerWin()
    {
        Debug.Log("PLAYER WIN");

        IsFinished = true;
        InventoryManager.Instance.AddItem(ItemType.StagePass, bckstagePassSprite);

        EndGame();
    }
    void PlayerLose()
    {
        Debug.Log("PLAYER LOSE");

        PlayerCheated = true;

        EndGame();
    }


    void EndGame()
    {
        isRunning = false;
        IsFinished = true;

        if (musicSource != null)
            musicSource.Stop();

        gameObject.SetActive(false);
        CancelInvoke();
    }
}
