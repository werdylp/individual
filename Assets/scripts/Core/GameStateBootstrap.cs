using UnityEngine;

public class GameStateBootstrap : MonoBehaviour
{
    public GameObject gameStatePrefab;

    void Awake()
    {
        if (GameState.Instance == null)
        {
            Instantiate(gameStatePrefab);
        }
    }
}