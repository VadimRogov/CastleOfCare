using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public CellManager cellManager;
    public MoveLift moveLift;
    public Cell currentCell; 
    public Cell targetCell;
    public MoveInStage moveInStage;

    private Stage currentStage;
    private Stage targetStage;

    // Добавляем Animator и булевую переменную isRun
    private Animator animator;
    private bool isRun;

    private void Start()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();
        isRun = false; // Изначально персонаж не бежит
    }

    public void Move()
    {
        if (IsStage())
        {
            Debug.LogWarning("Перемещение на текущем этаже");
            MoveCurrentStage(targetCell);
        }
        else if (!IsStage())
        {
            Debug.LogWarning("Перемещение на другом этаже");
            moveInStage.Move();
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

    public Cell FindLiftCell(Stage stage)
    {
        foreach (Cell cell in stage.Cells)
        {
            foreach (Transform room in cell.transform)
            {
                if(room.CompareTag("Lift"))
                {
                    Debug.LogWarning("Найдена комната с лифтом");
                    return cell;
                }
            }
        }
        return null;
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

        isRun = true; // Персонаж начинает двигаться
        animator.SetBool("isRun", isRun); // Включаем анимацию бега

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

        isRun = false; // Персонаж остановился
        animator.SetBool("isRun", isRun); // Выключаем анимацию бега
        Debug.Log("Персонаж достиг целевой комнаты на оси X.");
    }
}