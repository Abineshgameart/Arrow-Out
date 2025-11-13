using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Public
    public static GameManager instance;
    public int currentLevel = 1;
    public int numberOfTiles;
    public int nextTileDist;
    public List<GameObject> clearedArrows = new List<GameObject>();

    // Private
    private AudioManager audioManager;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject winPanelUI;
    [SerializeField] private GameObject congratulationPanelUI;
    [SerializeField] private TextMeshProUGUI winTxt;
    [SerializeField] private GameObject losePanelUI;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private InterstitialAds interstitialAds;
    [SerializeField] private BannerAds bannerAds;
    [SerializeField] private List<GameObject> levelTileSet = new List<GameObject>();
    

    private SceneManagerScript sceneManagerScript;
    private float remainingTime;
    private bool timerStatus = true;
    private Color orangeColor;
    private GameObject arrowGameObj;
    private Image arrowImg;
    private List<GameObject> childObjects = new List<GameObject>();

    private void Awake()
    {
        // Check if there's already an instance
        if (instance != null)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        instance = this;

        // Getting Audio Manager
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        SetLevelDetails();  // Setting details of the level on the start
        bannerAds.ShowBannerAd();  // To show Banner ads at the bottom
        sceneManagerScript = GetComponent<SceneManagerScript>(); // Getting SceneManager Script
        ColorUtility.TryParseHtmlString("#FB8500", out orangeColor); // getting color from hexcode
    }

    private void Update()
    {
        // To update time every frame
        if (timerStatus)
        {
            Timer();
        }
        
    }

    // Setting Levels Details by using Switch case
    public void SetLevelDetails()
    {
        switch (currentLevel)
        {
            case 1: 
                numberOfTiles = 15; // number of arrow tiles in the Level
                remainingTime = 20; // Timer Duration
                nextTileDist = 185; // For Checking next Tiles distance
                SetTimerInUI();     // Setting Timer in UI
                break;
            case 2: 
                numberOfTiles = 34;
                remainingTime = 30;
                nextTileDist = 125;
                SetTimerInUI();
                break;
            case 3: 
                numberOfTiles = 33;
                remainingTime = 40;
                nextTileDist = 125;
                SetTimerInUI();
                break;
            case 4:
                numberOfTiles = 62;
                remainingTime = 50;
                nextTileDist = 93;
                SetTimerInUI();
                break;
            case 5: 
                numberOfTiles = 63;
                remainingTime = 60;
                nextTileDist = 93;
                SetTimerInUI();
                break;
        }
    }

    // Timer Function
    private void Timer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // reducing time by deltatime in time
            
            // Base case for timer to not go in negative value
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

    // Method to Set Timer in Canva UI
    private void SetTimerInUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerTxt.text = string.Format("{0:00} : {1:00}", minutes, seconds);// Updating on UI
    }

    // Methos to Hide Menu or Panel if it is On
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

    // Function to Set the scene for next Level
    public void NextLevel()
    {
        audioManager.ButtonClick();  // Playing Button Sound
        levelTileSet[currentLevel - 1].SetActive(false); // hideing current level tileset
        currentLevel++;
        SetLevelDetails();
        levelTileSet[currentLevel - 1].SetActive(true); // activing the next level tile set
        ClearUI(); // Clearing the UI menus if anythi is on
        timerStatus = true; // Setting time to start running
    }

    // Method to Retry or reload the Scene
    public void RetryLevel()
    {
        audioManager.ButtonClick();
        
        // truning active the disable cleared arrow tiles
        foreach (GameObject arrow in clearedArrows)
        {
            if (arrow != null)
            {
                arrow.gameObject.SetActive(true);
            }
        }
        
        childObjects.Clear(); // clearing the child object list

        foreach (Transform child in levelTileSet[currentLevel - 1].transform)
        {
            childObjects.Add(child.gameObject);  // Adding Every Arrow in the TileSet
        }

        // Changing the arrow colors its default Orange color 
        foreach(GameObject arrow in childObjects)
        {
            if (arrow == null) continue;

            if (arrow.transform.childCount > 0)
            {
                arrowGameObj = arrow.transform.GetChild(0).gameObject;

                if (arrowGameObj.activeSelf)
                {
                    arrowImg = arrowGameObj.GetComponent<Image>();
                    if (arrowImg != null)
                    {
                        arrowImg.color = orangeColor; // Changing color to orane
                    }
                }
            }
            
                
        }

        clearedArrows.Clear();

        ClearUI();

        SetLevelDetails();
        
        timerStatus = true;

        // Unfreeze the Scene
        Time.timeScale = 1f;
    }

    // Method to pause the Game and showint ehpause menu
    public void PauseMenu()
    {
        audioManager.ButtonClick();
        if (!pauseMenuUI.activeSelf)
        {
            pauseMenuUI.SetActive(true);
            // Freeze time
            Time.timeScale = 0f;
        } else
        {
            pauseMenuUI.SetActive(false);

            // Unfreeze the time
            Time.timeScale = 1f;
        }
    }

    // Methos to Enable the win Panel
    public void WinPanel()
    {
        audioManager.PlaySFX(audioManager.levelPass);
        winTxt.text = "Lv." + currentLevel.ToString() + " Clear";
        winPanelUI.SetActive(true);
        timerStatus = false;
        interstitialAds.ShowAd();
    }

    // Method to enable LosePanel
    public void LosePanel()
    {
        audioManager.PlaySFX(audioManager.levelFail);
        losePanelUI.SetActive(true);
        timerStatus = false;
        interstitialAds.ShowAd();
    }

    //Method to Enable the Final Lv.5 Cleared Congratulation panel
    public void CongratulationPanel()
    {
        audioManager.PlaySFX(audioManager.congratulation);
        congratulationPanelUI.SetActive(true);
        timerStatus = false;
        interstitialAds.ShowAd();
    }

    // Method to call Home Scene
    public void HomeScr()
    {
        audioManager.ButtonClick();
        if (bannerAds.bannerAdsStatus == true)
        {
            bannerAds.HideBannerAd();
        }

        sceneManagerScript.HomeScene();
    }
}
