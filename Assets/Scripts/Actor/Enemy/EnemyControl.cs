using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Es.Actor
{
  public class EnemyControl : VillainBase
  {
    /**************************************************
     * field
     **************************************************/

    //パンチされた際の物理的挙動が適用された後かどうか
    private bool timeFixFlagOnPanched = false;

    /**************************************************
     * method
     **************************************************/

    public void Update()
    {
      switch(state)
      {
        case State.Play:

          //HPが0になったら爆発
          if(HP <= 0)
            state = State.Dead;

          break;

        case State.Attacked:

          //物理演算処理との時間差を吸収できていれば
          if(timeFixFlagOnPanched)
          {
            //爆発に巻き込まれてHPが無くなったらその場で爆発
            if(exprDamageTrigger && HP <= 0)
            {
              DeadOnExpr();
            }

            //吹き飛びの速度が一定以下になった
            if(rigidbody2D.velocity.magnitude < Vector2.one.magnitude)
            {
              if(HP <= 0)
                DeadOnExpr();
              state = State.Play;
              timeFixFlagOnPanched = false;
            }
          }

          break;

        case State.Dead:
          DeadOnExpr();

          break;

        default:
          state = State.Play;
          break;
      }
    }

    public void FixedUpdate()
    {
      // 物理パラメータ(velocity等)を参照するUpdateとのズレを考慮し、フラグを変更する
      if(state == State.Attacked)
        timeFixFlagOnPanched = true;
    }

    public override void OnCollisionEnter2D(Collision2D coll)
    {
      base.OnCollisionEnter2D(coll);

      // HP0,パンチされた状態で他の何かにぶつかったら、velocityをゼロに設定する
      if(HP <= 0 && state == State.Attacked)
        rigidbody2D.velocity = Vector2.zero;
    }
  }
}
