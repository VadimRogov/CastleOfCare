using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isEmpty = true;
    public GameObject openObject; // Объект, который отображается, когда ячейка свободна и активна для строительства
    public GameObject closedObject; // Объект, который отображается, когда ячейка занята

    public bool isOpenForBuilding = false; // Флаг, чтобы отслеживать, активна ли ячейка для строительства

    public void SetOpenForBuilding(bool isOpen)
    {
        isOpenForBuilding = isOpen;
        UpdateCellState();
    }

    public void UpdateCellState()
    {
        if (isEmpty)
        {
            openObject.SetActive(isOpenForBuilding);
            closedObject.SetActive(false);
        }
        else
        {
            openObject.SetActive(false);
            closedObject.SetActive(true);
        }
    }
}