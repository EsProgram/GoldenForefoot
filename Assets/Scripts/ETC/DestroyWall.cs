using Es.Actor;
using System.Collections;
using UnityEngine;

/// <summary>
/// 破壊壁
/// </summary>
public class DestroyWall : MonoBehaviour
{
  [SerializeField]
  private bool hpCheck = false;

  /// <summary>
  /// 接触者がPlayer以外のActor
  /// かつHPが0の場合爆破
  /// </summary>
  public void OnCollisionEnter2D(Collision2D coll)
  {
    var villain = coll.gameObject.GetComponent<VillainBase>();
    if(villain != null)
    {
      if(hpCheck && villain.HP < 1)
        coll.gameObject.SendMessage("DeadOnSilent");
      if(!hpCheck)
        coll.gameObject.SendMessage("DeadOnSilent");
    }
  }
}
