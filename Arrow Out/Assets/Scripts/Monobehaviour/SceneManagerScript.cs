using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // For Loading Home Scene
    public void HomeScene()
    {
        SceneManager.LoadScene(0);
    }

    // To Load Game or Level Scene
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    // Quit Application
    public void QuitApplication()
    {
        Application.Quit();
    }
}
