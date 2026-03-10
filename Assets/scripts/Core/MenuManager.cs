using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject difficultyPanel;



    void Start()
    {
        mainMenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }
    public void StartButtonPressed()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void SetEasy()
    {
        GameState.Instance.currentDifficulty = Difficulty.Easy;
        LoadGame();
    }

    public void SetMedium()
    {
        GameState.Instance.currentDifficulty = Difficulty.Medium;
        LoadGame();
    }

    public void SetHard()
    {
        GameState.Instance.currentDifficulty = Difficulty.Hard;
        LoadGame();
    }

    void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}