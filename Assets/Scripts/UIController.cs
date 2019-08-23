using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject IAPPanel;
    [SerializeField]
    GameObject StartPanel;
    [SerializeField]
    GameObject PlayButton;
    [SerializeField]
    GameObject PausePanel;
    int counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Restart()
    {
        GamePlayerControl.PHEALTH = 80f;
        GamePlayerControl.LASER = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameStart()
    {
        Time.timeScale = 1;
        StartPanel.SetActive(false);
    }
    public void IAPButton()
    {
        IAPPanel.SetActive(true);
    }

    public void IAPButtonExit()
    {
        IAPPanel.SetActive(false);
    }
    public void PauseGame()
    {
        counter++;
        if (counter % 2 == 1)
        {
            Time.timeScale = 0;
            PlayButton.SetActive(true);
            PausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            PlayButton.SetActive(false);
            PausePanel.SetActive(false);
        }


    }

}
