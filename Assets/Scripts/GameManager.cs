using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level;
    private int score;
    private int lives;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NewGame();
    }

    private void NewGame()
    {
        score = 0;
        lives = 3;

        LoadLevel(1);
    }

    public void CompleteLevel()
    {
        score += 100;

        int nextLevel = level + 1;
        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else
        {
            LoadLevel(1);
        }
    }

    public void FailLevel()
    {
        lives--;

        if (lives <= 0)
        {
            NewGame();
        }
        else
        {
            LoadLevel(level);
        }
    }

    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;

        if (camera != null)
        {
            camera.cullingMask = 0;
        }

        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }
}
