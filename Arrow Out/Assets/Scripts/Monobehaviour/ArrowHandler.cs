using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowHandler : MonoBehaviour
{
    // Private
    private GameManager gameManager;

    private Vector3 arrowTilePosition;
    private float arrowTileRotationZ;
    private float nextTileCheckPos;
    private float nextTilePos;

    private Image arrowImg;

    // Public
    public GraphicRaycaster UIRaycaster;
    public EventSystem eventSystem;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void nextTileCheck(GameObject arrowTile)
    {
        arrowTilePosition = arrowTile.transform.position;
        arrowTileRotationZ = arrowTile.transform.rotation.z;

        if (arrowTileRotationZ == 1)
        {
            // LEFT
            Debug.Log("Left");
            RayCastingNextTile(arrowTile, new Vector3(arrowTilePosition.x - 185, arrowTilePosition.y, arrowTilePosition.z));
        }
        else if (arrowTileRotationZ > 0.70)
        {
            // UP
            Debug.Log("Up");
            RayCastingNextTile(arrowTile, new Vector3(arrowTilePosition.x, arrowTilePosition.y + 185, arrowTilePosition.z));
        }
        else if (arrowTileRotationZ == 0)
        {
            // RIGHT
            Debug.Log("right");
            RayCastingNextTile(arrowTile, new Vector3(arrowTilePosition.x + 185, arrowTilePosition.y, arrowTilePosition.z));
        }
        else if (arrowTileRotationZ < 0)
        {
            // DOWN
            Debug.Log("Down");
            RayCastingNextTile(arrowTile, new Vector3(arrowTilePosition.x, arrowTilePosition.y - 185, arrowTilePosition.z));
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
            Debug.Log($"Touched UI: {nextTileArrow.tag}");

            if (nextTileArrow.tag == "Tile")
            {
                ClearArrow(arrowTile);
            }
            else
            {
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

        Debug.Log(gameManager.numberOfTiles);

        if (gameManager.numberOfTiles == 0 )
        {
            gameManager.WinPanel();
        }
    }

}
