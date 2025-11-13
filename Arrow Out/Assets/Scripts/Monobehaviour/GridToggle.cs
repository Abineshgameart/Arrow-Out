using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridToggle : MonoBehaviour
{
    // Private
    private AudioManager audioManager;
    [SerializeField] private Toggle myToggle;
    [SerializeField] private TextMeshProUGUI gridToggleTxt;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<GameObject> dotGrid;
    [SerializeField] private List<GameObject> lineGrid;

    private Color oceanBlueColor;
    private Color orangeYellowColor;


    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // Make sure the Toggle is assigned
        if (myToggle != null)
        {
            // Add listener for when the toggle changes
            myToggle.onValueChanged.AddListener(OnToggleChanged);
        }

        ColorUtility.TryParseHtmlString("#023047", out oceanBlueColor);
        ColorUtility.TryParseHtmlString("#FFB703", out orangeYellowColor);
    }

    // GridView Toggle Method
    private void OnToggleChanged(bool isOn)
    {
        audioManager.ButtonClick(); // To play Button Sound
        if (isOn)
        {
            // changing button color and toggling from dotted gid to line grid
            gridToggleTxt.color = oceanBlueColor;
            dotGrid[gameManager.currentLevel - 1].SetActive(false);
            lineGrid[gameManager.currentLevel - 1].SetActive(true);
        }
        else
        {
            // Toggling button color and toggling form line grid to dotte grid
            gridToggleTxt.color = orangeYellowColor;
            lineGrid[gameManager.currentLevel - 1].SetActive(false);
            dotGrid[gameManager.currentLevel - 1].SetActive(true);
        }
    }

}
