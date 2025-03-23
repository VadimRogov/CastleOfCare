using UnityEngine;
using System.Collections;
using System;

// Класс для управления перемещением кабины лифта
public class MoveLift : MonoBehaviour
{
    public event Action OnLiftMovementComplete; // Событие завершения перемещения лифта
    public bool isCabin = false; // Флаг, указывающий, найдена ли кабина
    [SerializeField] private Cell[] listLift; // Массив ячеек лифта
    public GameObject cellWithCabin; // Ячейка с кабиной лифта
    private Transform cabinTransform; // Трансформ кабины лифта
    private Rigidbody liftRigidbody; // Rigidbody лифта

    // Параметры для ручного регулирования позиционирования
    [SerializeField] private float xOffset = 0f; // Смещение по оси X
    [SerializeField] private float yOffset = 0f; // Смещение по оси Y
    [SerializeField] private float zOffset = 0f; // Смещение по оси Z

    void Awake()
    {
        // Находим кабину лифта при старте
        FindCabineLift();
        liftRigidbody = GetComponent<Rigidbody>(); // Получаем компонент Rigidbody

        if (liftRigidbody == null)
        {
            Debug.LogError("Rigidbody не найден на объекте " + gameObject.name);
            return;
        }

        liftRigidbody.isKinematic = true; // Устанавливаем isKinematic в true для контроля через скрипт
    }

    // Метод для поиска кабины лифта
    public bool FindCabineLift()
    {
        foreach (Cell cell in listLift)
        {
            foreach (Transform room in cell.transform)
            {
                if (room.CompareTag("Lift"))
                {
                    GameObject roomCell = room.gameObject;
                    foreach (Transform cabin in roomCell.transform)
                    {
                        if (cabin.CompareTag("Cabin"))
                        {
                            cellWithCabin = cabin.gameObject;
                            cabinTransform = cabin; // Сохраняем ссылку на трансформ кабины
                            isCabin = true;
                            return isCabin;
                        }
                    }
                }
            }
        }
        return isCabin;
    }

    // Метод для перемещения кабины лифта на целевой этаж
    public void MoveCabin(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= listLift.Length)
        {
            Debug.LogWarning("Целевой индекс вне диапазона.");
            return;
        }
        else if (targetIndex == 0)
        {
            Debug.LogWarning("Лифт на нужном этаже");
        }

        Cell targetCell = listLift[targetIndex];

        if (HasLift(targetCell))
        {
            Debug.Log($"Перемещение кабины лифта на этаж {targetIndex + 1}");
            StartCoroutine(MoveToFloor(targetCell));
        }
        else
        {
            Debug.LogWarning($"Нет лифта в ячейке {targetIndex + 1}");
        }
    }

    // Корутина для перемещения кабины лифта на целевой этаж
    private IEnumerator MoveToFloor(Cell targetCell)
    {
        Vector3 targetPosition = targetCell.transform.position;
        Vector3 startPosition = cabinTransform.position;

        targetPosition.x += xOffset;
        float cabinHeight = cabinTransform.localScale.y / 2;
        targetPosition.y += cabinHeight + yOffset;
        targetPosition.z += zOffset;

        float duration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Используем плавное перемещение с помощью Lerp
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            liftRigidbody.MovePosition(newPosition);
            yield return null;
        }

        // Устанавливаем финальную позицию
        liftRigidbody.MovePosition(targetPosition);
        cabinTransform.position = targetPosition;

        Transform newRoom = null;
        foreach (Transform room in targetCell.transform)
        {
            if (room.CompareTag("Lift"))
            {
                newRoom = room;
            }
        }

        if (newRoom != null)
        {
            cabinTransform.SetParent(newRoom);
            Debug.Log($"Кабина лифта теперь является дочерним объектом комнаты: {newRoom.name}");
            
            // Вызываем событие завершения перемещения
            OnLiftMovementComplete?.Invoke();
        }
        else
        {
            Debug.LogError("Комната не найдена в целевой ячейке.");
        }
    }

    // Метод для проверки наличия лифта в ячейке
    private bool HasLift(Cell cell)
    {
        for (int j = 0; j < cell.transform.childCount; j++)
        {
            Transform room = cell.transform.GetChild(j);
            if (room.CompareTag("Lift"))
            {
                return true; // Лифт существует в этой ячейке
            }
        }
        return false; // Лифт не найден в этой ячейке
    }

    // Метод для отрисовки Gizmos (для отладки)
    void OnDrawGizmos()
    {
        if (cabinTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(cabinTransform.position, 0.1f); // Отображение позиции кабины
            Gizmos.color = Color.green;

            foreach (var lift in listLift)
            {
                Gizmos.DrawSphere(lift.transform.position, 0.1f); // Отображение позиции каждой ячейки лифта
            }
        }
    }

    // Свойство для доступа к массиву ячеек лифта
    public Cell[] Cells => listLift;
}