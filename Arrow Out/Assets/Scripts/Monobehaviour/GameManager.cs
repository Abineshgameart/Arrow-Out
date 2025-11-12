using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Public
    public static GameManager instance;
    public int currentLevel = 1;
    public int numberOfTiles;
    public List<GameObject> clearedArrows = new List<GameObject>();

    // Private
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject winPanelUI;
    [SerializeField] private TextMeshProUGUI winTxt;
    [SerializeField] private GameObject losePanelUI;
    [SerializeField] private TextMeshProUGUI timerTxt;
    private float remainingTime;
    private bool timerStatus = true;
    [SerializeField] private List<GameObject> levelTileSet = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        SetLevelDetails();
    }

    private void Update()
    {
        if (timerStatus)
        {
            Timer();
        }
        
    }

    public void SetLevelDetails()
    {
        switch (currentLevel)
        {
            case 1: 
                numberOfTiles = 15;
                remainingTime = 20;
                SetTimerInUI();
                break;
            case 2: 
                numberOfTiles = 16;
                remainingTime = 10;
                SetTimerInUI();
                break;
            case 3: 
                numberOfTiles = 16;
                remainingTime = 10;
                SetTimerInUI();
                break;
            case 4: 
                numberOfTiles = 16;
                remainingTime = 10;
                SetTimerInUI();
                break;
            case 5: 
                numberOfTiles = 16;
                remainingTime = 10;
                SetTimerInUI();
                break;
        }
    }

    private void Timer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            
            if (remainingTime > 0)
            {
                SetTimerInUI();
            } else
            {
                timerStatus = false;
                LosePanel();
            }
            
        }
        
    }

    private void SetTimerInUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerTxt.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    private void ClearUI()
    {
        if (winPanelUI.activeSelf)
        {
            winPanelUI.SetActive(false);
        }
        if (losePanelUI.activeSelf)
        {
            losePanelUI.SetActive(false);
        }
        if (pauseMenuUI.activeSelf)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    public void NextLevel()
    {
        levelTileSet[currentLevel - 1].SetActive(false);
        currentLevel++;
        SetLevelDetails();
        levelTileSet[currentLevel - 1].SetActive(true);
        ClearUI();
    }

    public void RetryLevel()
    {
        foreach(GameObject arrow in clearedArrows)
        {
            if (arrow != null)
            {
                arrow.gameObject.SetActive(true);
            }
        }

        clearedArrows.Clear();

        ClearUI();

        SetLevelDetails();
        
        timerStatus = true;
    }

    public void PauseMenu()
    {
        if (!pauseMenuUI.activeSelf)
        {
            pauseMenuUI.SetActive(true);
            // Freeze time
            Time.timeScale = 0f;
        } else
        {
            pauseMenuUI.SetActive(false);

            // Unfreeze (resume normal speed)
            Time.timeScale = 1f;
        }
    }

    public void WinPanel()
    {
        winTxt.text = "Lv." + currentLevel.ToString() + " Clear";
        winPanelUI.SetActive(true);
        timerStatus = false;
    }

    public void LosePanel()
    {
        losePanelUI.SetActive(true);
    }
}
