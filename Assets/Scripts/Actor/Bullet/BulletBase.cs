using System.Collections;
using System.Linq;
using UnityEngine;

namespace Es.Actor
{
  /// <summary>
  /// Bulletの基本クラス
  /// 派生クラスではMoveメソッドをオーバーライドするだけでいい
  /// </summary>
  public abstract class BulletBase : VillainBase
  {
    /**************************************************
     * field
     **************************************************/
    [SerializeField, Range(-10, 10)]
    protected float speed = 3f;

    /**************************************************
     * method
     **************************************************/

    public void Update()
    {
      switch(state)
      {
        case State.Play:
          DestroyOutOfCamera();
          Move();
          if(HP <= 0)
            state = State.Dead;

          break;

        case State.Dead:
          DeadOnSilent();
          break;

        default:
          state = State.Play;
          break;
      }
    }

    /// <summary>
    /// カメラ外に存在する場合に破棄する
    /// </summary>
    private void DestroyOutOfCamera()
    {
      Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
      if(pos.magnitude > 2f)
        Destroy(gameObject);
    }

    /// <summary>
    /// Update内で呼び出される移動処理
    /// </summary>
    protected abstract void Move();
  }
}
