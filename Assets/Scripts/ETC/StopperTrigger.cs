using System.Collections;
using UnityEngine;

public class StopperTrigger : MonoBehaviour
{
  public void OnTriggerExit2D(Collider2D other)
  {
    if(other.tag == "Enemy")
      other.gameObject.layer = LayerMask.NameToLayer("StopEnemy");
  }
}
