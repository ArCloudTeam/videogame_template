using UnityEngine;

public static class GameManager
{
    public static bool isGameOver { get; private set; }
    public static bool isVictory { get; private set; }

    public static void Win()
    {
        isGameOver = true;
        isVictory = true;
        Time.timeScale = 0f;
    }

    public static void Lose()
    {
        isGameOver = true;
        isVictory = false;
        Time.timeScale = 0f;
    }

    public static void Restart()
    {
        isGameOver = false;
        isVictory = false;
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public static void Reset()
    {
        isGameOver = false;
        isVictory = false;
        Time.timeScale = 1f;
    }
}
