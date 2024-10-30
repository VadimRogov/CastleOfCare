using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private bool isEmpty = true;
    public GameObject closed; // Сделаем закрытый объект публичным для доступа из CellManager
    [SerializeField] private Canvas open;
    [SerializeField] private Button roomButton;

    public bool IsEmpty
    {
        get { return isEmpty; }
        private set { isEmpty = value; }
    }

    void Start()
    {
        if (roomButton != null)
        {
            roomButton.onClick.AddListener(OnRoomButtonClicked);
        }
    }

    public void SetCondition()
    {
        if (isEmpty)
        {
            if (closed != null) closed.SetActive(false);
            if (open != null) open.gameObject.SetActive(true);
            if (roomButton != null) roomButton.gameObject.SetActive(true);
        }
    }

    public void SetReturn(bool isBuild)
    {
        if (!isBuild)
        {
            if (closed != null) closed.SetActive(true);
            if (open != null) open.gameObject.SetActive(false);
            if (roomButton != null) roomButton.gameObject.SetActive(false);
        }
    }

    public void SetEmpty(bool value)
    {
        isEmpty = value;

    }

    public void SetClosed(bool value)
    {
        closed.SetActive(value);
    }

    private void OnRoomButtonClicked()
    {
        CellManager cellManager = FindObjectOfType<CellManager>();
        if (cellManager != null)
        {
            ShopPanelControll shopPanelControll = FindObjectOfType<ShopPanelControll>();
            if (shopPanelControll != null)
            {
                ProductCard selectedProductCard = shopPanelControll.GetSelectedProductCard();
                if (selectedProductCard != null && selectedProductCard.productPrefab != null)
                {
                    cellManager.CreateRoomInEmptyCell(selectedProductCard.productPrefab);
                    SetEmpty(false);
                    SetReturn(false);
                }
            }
        }
    }
}