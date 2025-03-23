using UnityEngine;
using System.Linq;  // Добавьте это для использования LINQ
using System.Collections;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator; // Ссылка на Animator
    public CellManager cellManager; // Менеджер ячеек, должен быть назначен в инспекторе
    public MoveLift moveLift; // Компонент лифта, должен быть назначен в инспекторе
    public Cell currentCell;
    public Cell targetCell;
    
    private bool isRunning = false; // Для анимации бега
    private bool isInLift = false;
    private bool isMovingToTarget = false;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>(); // Получаем Animator, если не назначен в инспекторе

        if (cellManager == null)
            Debug.LogError("CellManager не назначен в инспекторе!");

        if (moveLift == null)
            Debug.LogError("MoveLift не назначен в инспекторе!");

        if (currentCell == null || targetCell == null)
            Debug.LogError("Текущая или целевая ячейка не назначены!");

        // Подписываемся на событие завершения перемещения лифта
        moveLift.OnLiftMovementComplete += OnLiftMovementComplete;
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        if (moveLift != null)
            moveLift.OnLiftMovementComplete -= OnLiftMovementComplete;
    }

    private void OnLiftMovementComplete()
    {
        if (isInLift)
        {
            StartCoroutine(CompleteLiftMovement());
        }
    }

    private IEnumerator CompleteLiftMovement()
    {
        // Находим лифт на целевом этаже
        Stage targetStage = GetStageForCell(targetCell);
        Cell targetLiftCell = FindLiftCell(targetStage);
        
        if (targetLiftCell == null)
        {
            Debug.LogError("Лифт не найден на целевом этаже!");
            yield break;
        }

        // Перемещаем персонажа к лифту на целевом этаже
        Vector3 targetPosition = targetLiftCell.transform.position;
        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        currentCell = targetLiftCell;
        isInLift = false;
        SetRunningAnimation(false);
    }

    public void MoveToTarget(Cell newTargetCell)
    {
        if (newTargetCell == null)
        {
            Debug.LogError("Целевая ячейка не может быть пустой!");
            return;
        }

        targetCell = newTargetCell; // Обновляем целевую ячейку
        if (IsSameFloor())
        {
            MoveOnCurrentFloor();
        }
        else
        {
            MoveToLift();
        }
    }

    private bool IsSameFloor()
    {
        Stage currentStage = GetStageForCell(currentCell);
        Stage targetStage = GetStageForCell(targetCell);

        return currentStage != null && targetStage != null && currentStage == targetStage;
    }

    private Stage GetStageForCell(Cell cell)
    {
        if (cellManager == null)
            return null;

        foreach (Stage stage in cellManager.Stages)
        {
            if (stage.Cells.Contains(cell)) // Теперь это работает, так как используется LINQ
                return stage;
        }

        return null;
    }

    private void MoveOnCurrentFloor()
    {
        StartCoroutine(SmoothMove(targetCell.transform.position));
    }

    private void MoveToLift()
    {
        // Поиск лифта на текущем этаже
        Cell liftCell = FindLiftCell(GetStageForCell(currentCell));

        if (liftCell != null)
        {
            StartCoroutine(MoveToLiftAndEnter(liftCell));
        }
        else
        {
            Debug.LogError("Лифт не найден на этом этаже.");
        }
    }

    private Cell FindLiftCell(Stage stage)
    {
        if (stage == null)
        {
            Debug.LogError("Этап не найден.");
            return null;
        }

        foreach (Cell cell in stage.Cells)
        {
            if (cell.transform.CompareTag("Lift"))
            {
                return cell;
            }
        }
        return null;
    }

    private IEnumerator SmoothMove(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);

        SetRunningAnimation(true); // Включаем анимацию бега

        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }

        transform.position = targetPosition; // Завершаем движение

        SetRunningAnimation(false); // Отключаем анимацию
        currentCell = targetCell; // Обновляем текущую ячейку
    }

    private IEnumerator MoveToLiftAndEnter(Cell liftCell)
    {
        SetRunningAnimation(true); // Включаем анимацию бега

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = liftCell.transform.position;

        // Двигаем персонажа к лифту
        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        Debug.Log("Персонаж достиг лифта.");

        // Здесь логика для входа в лифт (например, воспроизведение анимации входа в лифт)
        yield return EnterLift(liftCell);
    }

    private IEnumerator EnterLift(Cell liftCell)
    {
        isInLift = true;
        Debug.Log("Вход в лифт...");

        // Находим целевой этаж
        Stage targetStage = GetStageForCell(targetCell);
        if (targetStage == null)
        {
            Debug.LogError("Целевой этаж не найден!");
            yield break;
        }

        // Находим индекс целевого этажа
        int targetIndex = -1;
        for (int i = 0; i < cellManager.Stages.Length; i++)
        {
            if (cellManager.Stages[i] == targetStage)
            {
                targetIndex = i;
                break;
            }
        }
        
        if (targetIndex == -1)
        {
            Debug.LogError("Не удалось найти индекс целевого этажа!");
            yield break;
        }

        // Перемещаем кабину лифта
        moveLift.MoveCabin(targetIndex);
        
        // Ждем завершения перемещения лифта через событие OnLiftMovementComplete
        yield return null;
    }

    private void SetRunningAnimation(bool isRunning)
    {
        if (animator != null)
        {
            this.isRunning = isRunning;
            animator.SetBool("isRunning", isRunning);
        }
    }
}
