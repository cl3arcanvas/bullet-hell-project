using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Pause : MonoBehaviour
{

    public GameObject pauseMenu;


    [HideInInspector]
    public bool justUnpausedOrPaused = false;

    // Update is called once per frame
    private void Start()
    {
        pauseMenu.SetActive(false);
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Resume();


            }
            else
            {
                //Debug.Log("Paused");
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                justUnpausedOrPaused = true;
            }


        }
    }

    public void Resume()
    {
        //Debug.Log("Unpaused");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        justUnpausedOrPaused = true;
        new WaitForSeconds(0.2f);
        justUnpausedOrPaused = false;

    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }









}
