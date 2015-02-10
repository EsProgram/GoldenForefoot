using Es.Actor;
using System.Collections;
using UnityEngine;

/// <summary>
/// 破壊壁
/// </summary>
public class DestroyWall : MonoBehaviour
{
  /// <summary>
  /// 接触者がPlayer以外のActor
  /// かつHPが0の場合爆破
  /// </summary>
  public void OnCollisionEnter2D(Collision2D coll)
  {
    var actor = coll.gameObject.GetComponent<ActorBase>();
    if(actor != null && actor.tag != "Player" && actor.HP < 1)
      coll.gameObject.SendMessage("ExprDead");
  }
}
