using System.Collections;
using UnityEngine;

public class AddEnemySpeed : MonoBehaviour
{
  [SerializeField]
  private float speed = 2f;

  private Pause pause;

  public void Awake()
  {
    pause = GameObject.FindGameObjectWithTag("Pause").GetComponent<Pause>();
  }

  /// <summary>
  /// ポーズ状態でなければ力を加算
  /// </summary>
  /// <param name="other"></param>
  public void OnTriggerStay2D(Collider2D other)
  {
    if(other.tag == "Enemy" && !pause.IsPose())
      other.rigidbody2D.velocity = -Vector2.right * speed;
  }
}
