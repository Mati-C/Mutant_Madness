using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class SceneManagement : MonoBehaviour {

    public GameObject pauseMenu;
    FirstPersonController playerComp;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            playerComp = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            if (Input.GetKeyDown(KeyCode.Escape))
                Pause();
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Cursor.visible = pauseMenu.activeSelf;
        playerComp.enabled = !pauseMenu.activeSelf;
        var allAudios = FindObjectsOfType<AudioSource>();

        if (pauseMenu.activeSelf)
        {
            foreach (var i in allAudios)
                i.Pause();
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            foreach (var i in allAudios)
                i.UnPause();
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
