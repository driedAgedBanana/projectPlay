using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("Game");
    }

    public void options()
    {

    }

    public void quit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
}
