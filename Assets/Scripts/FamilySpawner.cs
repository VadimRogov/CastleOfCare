using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using JetBrains.Annotations;

public class FamilySpawner : MonoBehaviour
{
    public CellManager cellManager;
    public List<GameObject> familyPrefabs; // Список префабов для разных семей
    public int familyStayDuration = 10; // Время, сколько семья будет оставаться в комнате (в секундах)

    public Button face;

    public Vector3 positionOffset; // Смещение позиции семьи, редактируемое в инспекторе
    public Vector3 centerOffset; // Смещение для центрирования семьи в комнате, редактируемое в инспекторе

    private GameObject currentFamily;  // Текущая семья, которая находится в комнате
    private bool isFamilyInRoom = false; // Флаг, который проверяет, есть ли семья в комнате
    private int currentFamilyIndex = 0; // Индекс текущей семьи в списке

    private Cell currtCell;
    private Cell targetCell;

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

        if (!isFamilyInRoom)
        {
            // Получаем позицию центра комнаты и добавляем смещение
            Vector3 receptionPosition = receptionCenter.transform.position + positionOffset;
            Debug.Log("Создаем семью: " + familyPrefabs[currentFamilyIndex].name);

            // Создаем семью из списка префабов
            currentFamily = Instantiate(familyPrefabs[currentFamilyIndex], receptionPosition, Quaternion.identity);
            currentFamily.transform.SetParent(receptionCenter.transform);

            // Центрируем семью в комнате с учетом смещения центрирования
            currentFamily.transform.localPosition += centerOffset;

            // Помечаем, что семья находится в комнате
            isFamilyInRoom = true;
            Debug.Log("Семья успешно помещена в комнату.");

            face.enabled = true;

            TargetMove();
        }
    }


    public void TargetMove()
    {
        foreach (GameObject family in familyPrefabs)
        {
            if (family != null)
            {
                if (family.CompareTag("Vitya"))
                {
                    
                    MoveInStage moveInStage = Character(family);
                    moveInStage.targetCell = TargetCell("Psyh");
                    moveInStage.currentCell = family.GetComponent<Cell>();
                }
                else if (family.CompareTag("Kira"))
                {
                    MoveInStage moveInStage = Character(family);
                    moveInStage.targetCell = TargetCell("Psyh");
                    moveInStage.currentCell = family.GetComponent<Cell>();
                }
                else if (family.CompareTag("Tosha"))
                {
                    MoveInStage moveInStage = Character(family);
                    moveInStage.targetCell = TargetCell("LFK");
                    moveInStage.currentCell = family.GetComponent<Cell>();
                }
            }
        }
    }

    public MoveInStage Character(GameObject family)
    {
        foreach (Transform character in family.transform)
        {
            if (character != null)
            {
                return character.gameObject.GetComponent<MoveInStage>();
            }
        }
        return null;
    }

    public Cell TargetCell(string tag)
    {
        foreach (Stage stage in cellManager.Stages)
        {
            foreach (Cell cell in stage.Cells)
            {
                if (cell != null)
                {
                    if (cell.CompareTag(tag))
                    {
                        targetCell = cell;
                        return targetCell;
                    }
                }
            }
        }
        return null;
    }


}