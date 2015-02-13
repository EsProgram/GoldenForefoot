using System.Collections;
using UnityEngine;

public class AddEnemySpeed : MonoBehaviour
{
  [SerializeField]
  private float speed = 2f;

  public void OnTriggerStay2D(Collider2D other)
  {
    if(other.tag == "Enemy")
      other.rigidbody2D.velocity = -Vector2.right * speed;
  }
}
