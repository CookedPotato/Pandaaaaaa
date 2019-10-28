using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GotoMainScene()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void OpenFridge()
    {
        SceneManager.LoadScene("MyFridge");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToKitchen()
    {
        SceneManager.LoadScene("Kitchen");
    }
}