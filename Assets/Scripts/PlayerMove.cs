using System;
using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public CellManager cellManager;
    public MoveLift moveLift;
    public Cell currentCell; 
    public Cell targetCell;
    private Stage currentStage;
    private Stage targetStage;
    private bool isInLift = false; // Флаг, указывающий, что персонаж вошел в лифт

    public void Move()
    {
        if (IsStage())
        {
            Debug.LogWarning("Move in current stage");
            MoveCurrentStage(targetCell);
        }
        else if (!IsStage())
        {
            Debug.LogWarning("Move in other stage");
            MoveOtherStage(targetCell);

            // Вызываем лифт на этаж, на котором находится персонаж
            int currentFloorIndex = getIndexStage(currentStage);
            moveLift.MoveCabin(currentFloorIndex);
            
            Cell currentLift = FindLiftCell(currentStage);
            Lift liftRoom = null;
            foreach (Transform lift in currentLift.transform)
            {
                if(lift.CompareTag("Lift"))
                {
                    Debug.LogWarning("Лифт в ячейке найден");
                    liftRoom = lift.GetComponent<Lift>();
                    break;
                }
            }
            if (liftRoom != null)
            {
                Debug.LogWarning("Лифт найден! Двери открываются");
                liftRoom.SetOpenDoor();
            }
            else {
                Debug.LogWarning("Компонент Lift не найден");
            }
        }
        
    }

    public bool IsStage()
    {
        foreach (Stage stage in cellManager.Stages)
        {
            foreach (Cell cell in stage.Cells)
            {
                if (currentCell == cell)
                {
                    Debug.LogWarning("Текущая ячейка найдена");
                    currentStage = stage;
                }
                else if (targetCell == cell)
                {
                    Debug.LogWarning("Целевая ячейка найдена");
                    targetStage = stage;
                }
            }
        }
        return currentStage == targetStage;
    }

    public int getIndexStage(Stage stage)
    {
        for (int i = 0; i < cellManager.Stages.Length; i++)
        {
            if(stage == cellManager.Stages[i])
            {
                Debug.LogWarning("Этаж №" + i + 1);
                return i;
            }
        }
        return -1;
    }

    public Cell FindLiftCell(Stage stage)
    {
        foreach (Cell cell in stage.Cells)
        {
            foreach (Transform room in cell.transform)
            {
                if(room.CompareTag("Lift"))
                {
                    Debug.LogWarning("Найдена комната с лифтом");
                    return cell; // Возвращаем родительскую ячейку, а не дочерний объект
                }
            }
        }
        return null;
    }

    // Перемещение на другие этажи
    public void MoveOtherStage(Cell targetCell)
    {
        // Текущий этаж
        Cell currentLiftCell = FindLiftCell(currentStage);
        if (currentLiftCell != null)
        {
            MoveChangeStage(currentLiftCell);
        }

        // Целевой этаж
        Cell targetLiftCell = FindLiftCell(targetStage);
        if (targetLiftCell != null)
        {
            MoveChangeStage(targetLiftCell);
        }
    }

    public void MoveChangeStage(Cell targetCell)
    {
        StartCoroutine(SmoothMoveOnStage(targetCell.transform.position));
    }

    private IEnumerator SmoothMoveOnStage(Vector3 targetPosition)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        // Сохраняем текущее значение Y
        float currentY = startPosition.y;

        // Перемещение к целевой позиции лифта по оси X
        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / Mathf.Abs(targetPosition.x - startPosition.x);

            // Обновление позиции X, оставляя Y неизменным
            transform.position = new Vector3(
                Mathf.Lerp(startPosition.x, targetPosition.x, fractionOfJourney),
                currentY,
                startPosition.z
            );

            yield return null;
        }

        // Убедитесь, что персонаж точно достиг целевой позиции по X
        transform.position = new Vector3(targetPosition.x, currentY, startPosition.z);

        // Сразу же начинаем центрироваться по Z
        startTime = Time.time;
        startPosition = transform.position;

        // Центрируем по Z
        while (Mathf.Abs(transform.position.z - targetPosition.z) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / Mathf.Abs(targetPosition.z - startPosition.z);

            // Обновление позиции Z, оставляя Y неизменным и используя целевую позицию по X
            transform.position = new Vector3(
                targetPosition.x,
                currentY,
                Mathf.Lerp(startPosition.z, targetPosition.z, fractionOfJourney)
            );

            yield return null;
        }

        // Убедитесь, что персонаж точно достиг целевой позиции по X и Z
        transform.position = new Vector3(targetPosition.x, currentY, targetPosition.z);

        // Если персонаж вошел в лифт, перемещаем его на целевой этаж
        if (isInLift)
        {
            int targetFloorIndex = GetTargetFloorIndex();
            moveLift.MoveCabin(targetFloorIndex);
        }
    }

    private int GetTargetFloorIndex()
    {
        for (int i = 0; i < moveLift.Cells.Length; i++)
        {
            if (ReferenceEquals(targetCell, moveLift.Cells[i]))
            {
                return i;
            }
        }
        return -1; // Если ячейка не найдена
    }

    // Перемещение по текущему этажу
    public void MoveCurrentStage(Cell targetCell)
    {
        StartCoroutine(SmoothMove(targetCell.transform.position));
        currentCell = targetCell;
    }

    private IEnumerator SmoothMove(Vector3 targetPosition)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / Vector3.Distance(startPosition, targetPosition);

            transform.position = new Vector3(
                Mathf.Lerp(startPosition.x, targetPosition.x, fractionOfJourney),
                transform.position.y,
                transform.position.z
            );
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
    }

    // Метод, вызываемый, когда персонаж заходит в лифт
    public void EnterLift()
    {
        isInLift = true;
    }
}