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
     * method
     **************************************************/

    protected override void Move()
    {
      transform.Translate(-Vector2.right * speed * Time.deltaTime);
    }
  }
}
