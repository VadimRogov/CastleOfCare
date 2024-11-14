using UnityEngine;
using System.Collections;

public class MoveInStage : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zOffset = 1.5f;
    public CellManager cellManager;
    public MoveLift moveLift;
    public Cell currentCell; 
    public Cell targetCell;
    public Animator animator;
    private Stage currentStage;
    private Stage targetStage;
    private Lift roomLift;
    private bool isInLift = false;
    private bool isRun = false;

    private void Start()
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
    }

    public void IsMove()
    {
        if (IsStage())
        {
            Debug.LogWarning("Перемещение на текущем этаже");
            MoveCurrentStage(targetCell); // Этот метод должен быть объявлен
        }
        else
        {
            Debug.LogWarning("Перемещение на другом этаже");
            Move();
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
                    currentStage = stage;
                }
                else if (targetCell == cell)
                {
                    targetStage = stage;
                }
            }
        }
        return currentStage == targetStage;
    }

    public void Move()
    {
        Cell currentLiftCell = FindLiftCell(currentStage);
        if (currentLiftCell != null)
        {
            int currentLift = GetIndexStage(currentStage);
            moveLift.MoveCabin(currentLift);
            roomLift.SetOpenDoor();
            EnterInRoomLift(currentLiftCell);
        }
    }

    public Cell FindLiftCell(Stage stage)
    {
        foreach (Cell cell in stage.Cells)
        {
            foreach (Transform room in cell.transform)
            {
                if (room.CompareTag("Lift"))
                {
                    roomLift = room.GetComponent<Lift>();
                    Debug.LogWarning("Найдена комната с лифтом");
                    return cell;
                }
            }
        }
        return null;
    }

    public int GetIndexStage(Stage stage)
    {
        for (int i = 0; i < cellManager.Stages.Length; i++)
        {
            if (stage == cellManager.Stages[i])
            {
                return i;
            }
        }
        return -1;
    }

    public void EnterInRoomLift(Cell currentLiftCell)
    {
        StartCoroutine(SmoothMoveOnStage(currentLiftCell.transform.position));
    }

    private IEnumerator SmoothMoveOnStage(Vector3 targetPosition)
    {
        SetRunningAnimation(true); 

        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        float currentY = startPosition.y;

        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / Mathf.Abs(targetPosition.x - startPosition.x);

            transform.position = new Vector3(
                Mathf.Lerp(startPosition.x, targetPosition.x, fractionOfJourney),
                currentY,
                startPosition.z
            );

            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, currentY, startPosition.z);
        SetRunningAnimation(false); 

        Cell currentLift = FindLiftCell(currentStage);
        Lift liftRoom = null;

        foreach (Transform lift in currentLift.transform)
        {
            if (lift.CompareTag("Lift"))
            {
                liftRoom = lift.GetComponent<Lift>();
                break;
            }
        }

        EnterElevator(liftRoom);
    }

    public void EnterElevator(Lift liftRoom)
    {
        foreach (Transform cabin in liftRoom.transform)
        {
            if (cabin.CompareTag("Cabin"))
            {
                StartCoroutine(MoveToCabin(cabin));
                break; 
            }
        }
    }

    private IEnumerator MoveToCabin(Transform cabin)
    {
        Vector3 targetPosition = cabin.position;
        Vector3 startPosition = transform.position;
        float currentY = startPosition.y;
        float startTime = Time.time;

        SetRunningAnimation(true); 

        while (Mathf.Abs(transform.position.z - targetPosition.z) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / Mathf.Abs(targetPosition.z - startPosition.z);

            transform.position = new Vector3(
                startPosition.x,
                currentY,
                Mathf.Lerp(startPosition.z, targetPosition.z, fractionOfJourney)
            );

            yield return null;
        }

        transform.position = new Vector3(startPosition.x, currentY, targetPosition.z);
        transform.SetParent(cabin);

        isInLift = true; 
        roomLift.SetCloseDoor();
        moveLift.MoveCabin(GetIndexStage(targetStage));

        currentCell = FindLiftCell(targetStage);

        roomLift.SetOpenDoor();

        SetRunningAnimation(false); 

        yield return new WaitForSeconds(1);
        OutLift();
    }

    private void OutLift()
    {
        if (!isInLift) return;

        Vector3 exitPosition = transform.position + transform.forward * 1.5f;
        exitPosition.z += zOffset;

        StartCoroutine(MoveOutOfLift(exitPosition));
    }

    private IEnumerator MoveOutOfLift(Vector3 targetPosition)
    {
        
        Vector3 startPosition = transform.position;
        float startTime = Time.time;

        SetRunningAnimation(true); 

        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f || Mathf.Abs(transform.position.z - targetPosition.z) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / Vector3.Distance(startPosition, targetPosition);

            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }

        transform.position = targetPosition; 
        transform.SetParent(null); 

        isInLift = false; 

        SetRunningAnimation(false); 

        MoveToTargetRoom();
    }

    private void MoveToTargetRoom()
    {
        roomLift.SetCloseDoor();
        StartCoroutine(MoveToTargetCellPosition(targetCell.transform.position));
    }

    private IEnumerator MoveToTargetCellPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;

        SetRunningAnimation(true); 

        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / Mathf.Abs(targetPosition.x - startPosition.x);

            transform.position = new Vector3(
                Mathf.Lerp(startPosition.x, targetPosition.x, fractionOfJourney),
                startPosition.y,
                startPosition.z
            );

            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, startPosition.y, startPosition.z); 
        SetRunningAnimation(false); 
    }


    private void SetRunningAnimation(bool isRunning)
    {
        isRun = isRunning;
        animator.SetBool("isRun", isRunning);
    }

    // Объявление метода MoveCurrentStage
    public void MoveCurrentStage(Cell targetCell)
    {
        StartCoroutine(SmoothMove(targetCell.transform.position));
        currentCell = targetCell;
    }

    private IEnumerator SmoothMove(Vector3 targetPosition)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        SetRunningAnimation(true); 

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

        SetRunningAnimation(false); 
        Debug.Log("Персонаж достиг целевой комнаты на оси X.");
    }
}
