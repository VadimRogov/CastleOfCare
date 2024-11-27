using UnityEngine;

public class Patient : MonoBehaviour
{
    public string doctor;
    public Cell chamber;
    public Cell roomDoctor;
    public int progress;
    public CellManager cellManager;

    public Cell FindCharacter(Patient character)
    {
        foreach (Stage stage in cellManager.Stages)
        {
            foreach (Cell cell in stage.Cells)
            {
                foreach (Transform room in cell.transform)
                {
                    foreach (Transform family in room.transform)
                    {
                        foreach (Transform patient in family.transform)
                        {
                            if (patient.gameObject.GetComponent<Patient>() == character)
                            {
                                return cell;
                            }
                        }
                    }
                }
            }
        }
        return null;
    }
}
