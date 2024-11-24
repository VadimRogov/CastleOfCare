using UnityEngine;
using UnityEngine.UI;

public class FamilySpawner : MonoBehaviour
{
    public GameObject[] listFamily; 
    public GameObject reception; 
    private int currentFamilyIndex = 0; 

    private float timer = 0f; // Таймер для отслеживания времени
    private float interval = 20f; // Интервал в секундах

    public Button buttonPrefab; // Префаб кнопки с вашей текстурой

    public Button smail;
    public GameObject imageNewPatient;
    public Button sendDoctor;

    public void CreatedFamily()
    {
        if (currentFamilyIndex >= listFamily.Length)
        {
            Debug.LogWarning("Все семьи уже созданы.");
            return; 
        }

        bool hasFamilyMembers = false;
        foreach (Transform child in reception.transform)
        {
            if (child.CompareTag("FamilyKira") || 
                child.CompareTag("FamilyTosha") || 
                child.CompareTag("FamilyVitya"))
            {
                hasFamilyMembers = true;
                break; 
            }
        }

        if (!hasFamilyMembers)
        {
            Vector3 receptionPosition = reception.transform.position;

            GameObject familyInstance = Instantiate(listFamily[currentFamilyIndex]);
            familyInstance.transform.SetParent(reception.transform);

            Renderer familyRenderer = familyInstance.GetComponent<Renderer>();
            if (familyRenderer != null)
            {
                Vector3 familySize = familyRenderer.bounds.size;
                Vector3 familyPosition = new Vector3(receptionPosition.x, receptionPosition.y, receptionPosition.z - (familySize.z / 2));
                familyInstance.transform.position = familyPosition; 

                
            }
            else
            {
                Debug.LogWarning("У объекта семьи нет Renderer для расчета размеров.");
            }

            currentFamilyIndex++;
        }
        else 
        {
            Debug.LogWarning("Комната не пуста");
        }
    }

    public void Start()
    {
        CreatedFamily(); 
    }

    public void Update()
    {
       timer += Time.deltaTime;

       if (timer >= interval)
       {
           CreatedFamily(); 
           timer = 0f; 
       }
    }
}