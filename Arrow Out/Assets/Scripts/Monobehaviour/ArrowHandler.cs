using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowHandler : MonoBehaviour
{
    // Private
    private GameManager gameManager;
    private AudioManager audioManager;

    private Vector3 arrowTilePosition;
    private float arrowTileRotationZ;

    private Image arrowImg;

    // Public
    public GraphicRaycaster UIRaycaster;
    public EventSystem eventSystem;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        gameManager = GetComponent<GameManager>();
    }

    public void nextTileCheck(GameObject arrowTile)
    {
        arrowTilePosition = arrowTile.transform.position;
        arrowTileRotationZ = arrowTile.transform.rotation.z;

        if (arrowTileRotationZ == 1)
        {
            // LEFT
            RayCastingNextTile(arrowTile, new Vector3(arrowTilePosition.x - gameManager.nextTileDist, arrowTilePosition.y, arrowTilePosition.z));
        }
        else if (arrowTileRotationZ > 0.70)
        {
            // UP
            RayCastingNextTile(arrowTile, new Vector3(arrowTilePosition.x, arrowTilePosition.y + gameManager.nextTileDist, arrowTilePosition.z));
        }
        else if (arrowTileRotationZ == 0)
        {
            // RIGHT
            RayCastingNextTile(arrowTile, new Vector3(arrowTilePosition.x + gameManager.nextTileDist, arrowTilePosition.y, arrowTilePosition.z));
        }
        else if (arrowTileRotationZ < 0)
        {
            // DOWN
            RayCastingNextTile(arrowTile, new Vector3(arrowTilePosition.x, arrowTilePosition.y - gameManager.nextTileDist, arrowTilePosition.z));
        }

        // Debug.Log(arrowTilePosition);
    }

    private void RayCastingNextTile(GameObject arrowTile, Vector3 tilePos)
    {
        // Create a new PointerEventData object
        PointerEventData pointerEventData = new PointerEventData(eventSystem);

        // Set the Pointer Event Position to the current mouse position
        pointerEventData.position = tilePos;

        // Create a list to store the raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast using the GraphicRaycaster and the pointer event data
        UIRaycaster.Raycast(pointerEventData, results);
        if (results.Count > 0)
        {
            GameObject nextTileArrow = results[0].gameObject;

            if (nextTileArrow.tag == "Tile")
            {
                ClearArrow(arrowTile);
            }
            else
            {
                audioManager.PlaySFX(audioManager.wroungArrowClick);
                arrowImg = arrowTile.GetComponent<Image>();
                arrowImg.color = Color.red;
            }
        }  
    }

    private void ClearArrow(GameObject currentArrowTile)
    {
        gameManager.clearedArrows.Add(currentArrowTile);

        currentArrowTile.SetActive(false);
        gameManager = GetComponent<GameManager>();
        gameManager.numberOfTiles--;

        if (gameManager.numberOfTiles == 0)
        {
            if (gameManager.currentLevel == 5)
            {
                gameManager.CongratulationPanel();
            } else
            {
                gameManager.WinPanel();
            }
        }
    }
}


