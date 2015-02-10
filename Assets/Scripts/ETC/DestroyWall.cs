using System.Collections;
using UnityEngine;

/// <summary>
/// 破壊壁
/// </summary>
public class DestroyWall : MonoBehaviour
{
  public void OnCollisionEnter2D(Collision2D coll)
  {
    Destroy(coll.gameObject);
  }
}
