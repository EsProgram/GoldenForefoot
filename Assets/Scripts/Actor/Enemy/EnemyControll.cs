using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Es.Actor
{
  public class EnemyControll : VillainBase
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
        case State.Idle:
          break;

        case State.Play:

          //HPが0になったら爆発
          if(HP <= 0)
            state = State.Dead;

          break;

        case State.Attacked:

          //物理演算処理との時間差を吸収できていれば
          if(timeFixFlagOnPanched)
          {
            //吹き飛びの速度が一定以下になった
            if(rigidbody2D.velocity.magnitude < Vector2.one.magnitude)
            {
              //HPが0になっていたら爆発する(周りに力を与え、ダメージメッセージを送信)
              if(hp <= 0)
              {
                #region 爆発処理

                ExprDead();
                #endregion 爆発処理
              }
              state = State.Play;
              timeFixFlagOnPanched = false;
            }
          }

          break;

        case State.Dead:
          ExprDead();

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
      if(hp <= 0 && state == State.Attacked)
        rigidbody2D.velocity = Vector2.zero;
    }
  }
}
