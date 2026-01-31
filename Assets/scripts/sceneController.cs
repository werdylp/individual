using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour 
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
