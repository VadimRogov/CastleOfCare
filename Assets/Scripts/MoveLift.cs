using UnityEngine;

public class MoveLift : MonoBehaviour
{
    public bool isCabin = false;
    [SerializeField] private Cell[] listLift;

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
                                isCabin = true;
                                return isCabin;
                        }
                    }
                    
                }
            }
        }
        return isCabin;
    }
}