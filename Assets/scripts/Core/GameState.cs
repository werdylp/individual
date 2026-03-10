using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public Difficulty currentDifficulty = Difficulty.Easy;

    [Header("DEBUG")]
    public bool unlockStage = false;

    //location: wc
    public bool talkedToCurator = false;
    public bool curatorReceivedDrink = false;

    //location: bar
    public bool bartenderMinigamePlayed = false;
    public bool bartenderCheated = false;

    void Start()
    {
        if (unlockStage)
        {
            progress = LocationProgress.Stage;
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Location currentLocation = Location.Toilet;

    public enum LocationProgress
    {
        WC,
        Bar,
        Stage
    }


    public LocationProgress progress = LocationProgress.WC;

    public bool IsBarUnlocked()
    {
        return progress >= LocationProgress.Bar;
    }

    public bool IsStageUnlocked()
    {
        return progress >= LocationProgress.Stage;
    }

    public void AdvanceProgress()
    {
        if (progress == LocationProgress.WC)
            progress = LocationProgress.Bar;
        else if (progress == LocationProgress.Bar && curatorReceivedDrink)
            progress = LocationProgress.Stage;
        Debug.Log("New progress:" + progress);
    }
}
