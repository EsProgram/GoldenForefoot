using System.Collections;
using UnityEngine;

namespace Es.Actor
{
  /// <summary>
  /// Y軸方向の追尾機能がついた弾
  /// </summary>
  public class TrackingBullet : BulletBase
  {
    [SerializeField, Range(0, 10), Tooltip("Y軸方向の追尾速度")]
    private float trackingSpeed = 2f;

    private Transform player;
    private Vector2 move;

    public override void Awake()
    {
      player = GameObject.FindGameObjectWithTag("Player").transform;
      base.Awake();
    }

    protected override void Move()
    {
      move = new Vector2(-1f * speed, Tracking() * trackingSpeed) * Time.deltaTime;
      transform.Translate(move);
    }

    /// <summary>
    /// 追尾するための相対的なy座標を-1～1の間で取得
    /// </summary>
    private float Tracking()
    {
      var y = (player.position - transform.position).y;
      y = Mathf.Min(1f, Mathf.Max(y, -1f));
      return y;
    }
  }
}
