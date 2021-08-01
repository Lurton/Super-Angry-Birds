using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTracker : MonoBehaviour
{
    public Transform[] pigs = new Transform[0];

    private static bool gameOver = false;
    private static string message = "";
    
    public static int score = 0;

    public void Awake()
    {
        gameOver = false;
        score = 0;
    }

    public void OnGUI()
    {
        if (gameOver)
            DrawGameOver();
        else
            DrawScore();
    }

    private void DrawScore()
    {
        Rect scoreRect = new Rect(Screen.width - 100, 0, 100, 30);
        GUI.Label(scoreRect, "Score: " + score);
    }

    private void DrawGameOver()
    {
        Rect boxRect = new Rect(0, 0, Screen.width, Screen.height);
        GUI.Box(boxRect, "Game Over\n" + message);

        Rect scoreRect = new Rect(0, Screen.height / 2, Screen.width, 30);
        GUI.Label(scoreRect, "Score: " + score);

        Rect exitRect = new Rect(0, Screen.height / 2 + 50, Screen.width, 50);

        if(GUI.Button(exitRect, "Return to Level Select"))
        {
            SceneManager.LoadScene(0);
            SaveScore();
        }
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }

    private void CheckPigs()
    {
        for(int i=0; i < pigs.Length; i++)
        {
            if (pigs[i] != null) return;
        }

        gameOver = true;
        message = "You destroyed the  pigs";
    }

    public static void OutOfBirds()
    {
        if (gameOver) return;

        gameOver = true;
        message = "You ran out of birds!";
    }

    private void SaveScore()
    {
        string key = "LevelScore" + SceneManager.GetActiveScene().name;

        int previousScore = PlayerPrefs.GetInt(key, 0);
        if (previousScore < score)
            PlayerPrefs.SetInt(key, score);
    }

    public void LateUpdate()
    {
        if (!gameOver)
            CheckPigs();
    }
}
