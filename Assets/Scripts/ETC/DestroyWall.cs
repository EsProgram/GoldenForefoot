using Es.Actor;
using System.Collections;
using UnityEngine;

/// <summary>
/// 破壊壁
/// </summary>
public class DestroyWall : MonoBehaviour
{
  [SerializeField, Tooltip("HPが0の場合にのみ爆破")]
  private bool hpCheck = false;
  [SerializeField, Tooltip("破壊する場合、爆発を起こすかどうかのフラグ")]
  private bool exprFlag = true;

  /// <summary>
  /// 接触者が敵
  /// </summary>
  public void OnCollisionEnter2D(Collision2D coll)
  {
    var villain = coll.gameObject.GetComponent<VillainBase>();
    if(villain != null)
    {
      if(exprFlag)
      {
        if(hpCheck && villain.HP <= 0)
          coll.gameObject.SendMessage("DeadOnSilent");
        if(!hpCheck)
          coll.gameObject.SendMessage("DeadOnSilent");
      }
      else
      {
        if(hpCheck && villain.HP <= 0)
          Destroy(coll.gameObject);
        if(!hpCheck)
          Destroy(coll.gameObject);
      }
    }
  }
}
