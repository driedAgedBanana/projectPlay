using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauze;
    public GameObject options;
    public GameObject check;
    public GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pauze();
        }
        
    }

    public void Pauze()
    {
        if (!background.activeInHierarchy)
        {
            pauze.SetActive(true);
            background.SetActive(true);
        }
        else if (background.activeInHierarchy)
        {
            pauze.SetActive(false);
            check.SetActive(false);
            options.SetActive(false);
            background.SetActive(false);
        }
    }

    public void Continue()
    {
        pauze.SetActive(false);
        background.SetActive(false);
    }
    public void Options()
    {
        pauze.SetActive(false);
        options.SetActive(true);
    }
    public void Back()
    {
        options.SetActive(false);
        pauze.SetActive(true);
    }
    public void Menu()
    {
        check.SetActive(true);
    }
    public void yes()
    {
        SceneManager.LoadScene("Start");
    }
    public void no()
    {
        check.SetActive(false);
    }
}
