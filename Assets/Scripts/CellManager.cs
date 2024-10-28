using UnityEngine;

public class CellManager : MonoBehaviour
{
   [SerializeField] private Stage[] stages;
   private Cell firstCellEmpty;

   public Stage[] Stages
   {
        get { return stages; }
        private set { stages = value; }
   }

   public Cell FindFirstStage()
   {
    foreach(Stage stage in stages)
    {
        firstCellEmpty =  stage.FindFirstCell();
        if (firstCellEmpty != null)
        {
            return firstCellEmpty;
        }
    }
    return null;
   }
}