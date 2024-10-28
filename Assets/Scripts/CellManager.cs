using UnityEngine;

public class CellManager : MonoBehaviour
{
    public Cell[] cells;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public Cell HighlightFirstFreeCell()
    {
        foreach (Cell cell in cells)
        {
            if (cell.isEmpty)
            {
                cell.SetOpenForBuilding(true);
                return cell;
            }
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell != null && cell.isEmpty && cell.isOpenForBuilding)
                {
                    ShopPanelControll shopPanelControll = FindObjectOfType<ShopPanelControll>();
                    if (shopPanelControll != null)
                    {
                        ProductCard productCard = shopPanelControll.GetSelectedProductCard();
                        if (productCard != null && productCard.productPrefab != null)
                        {
                            RoomBuilder roomBuilder = FindObjectOfType<RoomBuilder>();
                            roomBuilder?.CreateRoomInCell(cell, productCard.productPrefab);
                            cell.SetOpenForBuilding(false); // Закрываем ячейку после строительства
                        }
                    }
                }
            }
        }
    }
}