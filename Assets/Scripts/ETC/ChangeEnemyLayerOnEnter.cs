using System.Collections;
using UnityEngine;

public class ChangeEnemyLayerOnEnter : MonoBehaviour
{
  [SerializeField]
  private string layerName = null;

  public void OnTriggerEnter2D(Collider2D other)
  {
    if(other.gameObject.tag == "Enemy")
      other.gameObject.layer = LayerMask.NameToLayer(layerName);
  }
}
