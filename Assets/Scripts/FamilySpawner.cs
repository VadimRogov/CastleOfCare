using UnityEngine;
using System.Collections.Generic;

public class FamilySpawner : MonoBehaviour
{
    public List<GameObject> familyPrefabs; // Список префабов для разных семей
    public int familyStayDuration = 10; // Время, сколько семья будет оставаться в комнате (в секундах)
    
    private GameObject currentFamily;  // Текущая семья, которая находится в комнате
    private bool isFamilyInRoom = false; // Флаг, который проверяет, есть ли семья в комнате
    private int currentFamilyIndex = 0; // Индекс текущей семьи в списке

    private void Start()
    {
        Debug.Log("Запуск процесса создания семей...");
        TrySpawnFamily(); // Попытка создать первую семью сразу при старте игры
    }

    // Метод для создания семьи, если в комнате нет другой
    private void TrySpawnFamily()
    {
        Debug.Log("Пытаемся создать семью...");

        // Находим объект "Reception", куда будет добавляться семья
        GameObject receptionCenter = GameObject.FindGameObjectWithTag("Reception");

        if (receptionCenter != null)
        {
            // Проверяем, есть ли семья в комнате
            if (receptionCenter.transform.childCount > 0)
            {
                Debug.Log("Семья уже находится в комнате. Дождитесь, пока она уйдет.");
                return; // Если семья уже в комнате, не создаем новую
            }

            // Проверяем, есть ли еще семьи в списке
            if (currentFamilyIndex < familyPrefabs.Count)
            {
                // Получаем позицию центра комнаты
                Vector3 receptionPosition = receptionCenter.transform.position;
                Debug.Log("Создаем семью: " + familyPrefabs[currentFamilyIndex].name);

                // Создаем семью из списка префабов
                currentFamily = Instantiate(familyPrefabs[currentFamilyIndex], receptionPosition, Quaternion.identity);
                currentFamily.transform.SetParent(receptionCenter.transform);

                // Центрируем семью в комнате
                currentFamily.transform.localPosition = Vector3.zero;

                // Создаем персонажей внутри семьи
                CreateFamilyMembers(currentFamily);

                // Помечаем, что семья находится в комнате
                isFamilyInRoom = true;
                Debug.Log("Семья успешно помещена в комнату.");

                // Запускаем таймер для удаления семьи через определённое время
                Invoke("RemoveFamily", familyStayDuration);

                // Переходим к следующей семье в списке
                currentFamilyIndex++;
                Debug.Log("Перехожу к следующей семье, если таковая имеется.");
            }
            else
            {
                Debug.Log("Все семьи были размещены.");
            }
        }
        else
        {
            Debug.LogError("Не найден объект с тегом 'Reception'. Убедитесь, что он существует на сцене.");
        }
    }

    // Метод для удаления семьи из комнаты
    private void RemoveFamily()
    {
        Debug.Log("Время прошло, удаляю текущую семью из комнаты.");

        if (currentFamily != null)
        {
            // Удаляем семью из комнаты
            Destroy(currentFamily);

            // Семья покинула комнату, теперь можем создать новую
            isFamilyInRoom = false;

            // Пытаемся создать следующую семью
            TrySpawnFamily();
        }
    }

    // Метод для создания персонажей внутри семьи
    private void CreateFamilyMembers(GameObject family)
    {
        // Предполагаем, что в каждом семейном префабе находятся персонажи
        Transform[] familyMembers = family.GetComponentsInChildren<Transform>();

        // Создаем каждого персонажа, если его нет в качестве дочернего объекта
        foreach (Transform member in familyMembers)
        {
            // Пропускаем сам объект семьи
            if (member == family.transform) continue;

            // Персонажи уже находятся в семейном префабе, и мы можем манипулировать их позициями
            member.gameObject.SetActive(true); // Делаем персонажа активным (если был выключен)
            member.localPosition = new Vector3(0, 0, 0); // Здесь можно дополнительно настроить позицию персонажа
            Debug.Log("Персонаж " + member.name + " стал активным в семье " + family.name);
        }
    }
}
