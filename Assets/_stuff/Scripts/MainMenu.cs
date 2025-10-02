using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called when "Play" button is pressed
    public void PlayGame()
    {
        // Load the next scene in build settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Called when "Quit" button is pressed
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}