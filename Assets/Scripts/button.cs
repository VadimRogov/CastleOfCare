using UnityEngine;

public class button : MonoBehaviour
{
   public Animator animator;

   private void OnTriggerEnter(Collider other) {
    if(other.tag=="but1") {
        animator.Play("DoorOpen");
    }
   }
}
