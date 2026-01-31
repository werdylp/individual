using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public GameObject bar;
    public GameObject toilet;
    public GameObject stage;

    void Start()
    {
        SetLocation(Location.Toilet);
    }


    public void SetLocation(Location location)
    {
        GameState.Instance.currentLocation = location;

        bar.SetActive(location == Location.Bar);
        toilet.SetActive(location == Location.Toilet);
        stage.SetActive(location == Location.Stage);
    }

}
