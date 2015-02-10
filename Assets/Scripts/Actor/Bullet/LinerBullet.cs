using System.Collections;
using UnityEngine;

namespace Es.Actor
{
  /// <summary>
  /// 単発直線軌道の弾
  /// </summary>
  public class LinerBullet : BulletBase
  {
    /**************************************************
     * field
     **************************************************/

    [SerializeField]
    private Vector2 dir = new Vector2(-1, 0);

    /**************************************************
     * method
     **************************************************/

    protected override void Move()
    {
    }
  }
}
