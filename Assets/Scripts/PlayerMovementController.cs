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
        // Пример анимации или движения к лифту
        Debug.Log("Вход в лифт...");

        // Добавьте сюда анимацию или дополнительную логику для входа в лифт

        SetRunningAnimation(false); // Отключаем анимацию бега
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