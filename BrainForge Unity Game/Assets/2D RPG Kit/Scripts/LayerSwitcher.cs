using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerSwitcher : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;

    private string stateOneSortingLayer = "Foreground";
    private int stateOneOrderInLayer = 0;
    private string stateTwoSortingLayer = "Background";
    private int stateTwoOrderInLayer = 2;

    private bool isStateOne = true;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        if (tilemapRenderer == null)
        {
            Debug.LogError("TilemapRenderer component not found on the GameObject.");
        }
        else
        {
            SetToForeground();
        }
    }

    public void ToggleState()
    {
        if (isStateOne)
        {
            SetToBackground();
        }
        else
        {
            SetToForeground();
        }
        isStateOne = !isStateOne;
    }

    public void SetToForeground()
    {
        tilemapRenderer.sortingLayerName = stateOneSortingLayer;
        tilemapRenderer.sortingOrder = stateOneOrderInLayer;
    }

    public void SetToBackground()
    {
        tilemapRenderer.sortingLayerName = stateTwoSortingLayer;
        tilemapRenderer.sortingOrder = stateTwoOrderInLayer;
    }
}
