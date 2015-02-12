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
    private AnimationMove anim;

    /**************************************************
     * method
     **************************************************/

    public override void Awake()
    {
      base.Awake();
      anim = GetComponent<AnimationMove>();
    }

    public void Update()
    {
      switch(state)
      {
        case State.Play:

          //HPが0になったら爆発
          if(HP <= 0)
            state = State.Dead;

          //レイヤーがStopEnemyじゃないものは速度を一定にする
          if(gameObject.layer != LayerMask.NameToLayer("StopEnemy"))
          {
            //rigidbody2D.velocity = -Vector2.right * anim.speed;
            //加えている力を相殺
            rigidbody2D.AddForce(-anim.direction * anim.speed);

            //速度が遅すぎる場合(または逆移動の場合)は加速
            var speed = Mathf.Max(anim.speed, 1f);

            //一定速度での移動
            transform.Translate(-Vector2.right * Time.deltaTime * speed);
          }

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
              {
                DeadOnExpr();
              }
              state = State.Play;
              timeFixFlagOnPanched = false;
            }

            animator.SetTrigger("Damaged");
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

      //敵同士で接触した際に、巻き込まれた側はダメージを喰らう
      if(coll.gameObject.tag == "Enemy")
      {
        var other = coll.gameObject.GetComponent<EnemyControl>();
        if(state == State.Attacked && other.state == State.Play)
          other.SendMessage("Damaged");
      }

      // HP0,パンチされた状態で他の何かにぶつかったら、velocityをゼロに設定する
      if(HP <= 0 && state == State.Attacked)
        rigidbody2D.velocity = Vector2.zero;
    }

    /// <summary>
    /// 指定されたプレハブをインスタンス化する
    /// </summary>
    /// <param name="prefab">プレハブ</param>
    public void InstantiatePrefab(GameObject prefab)
    {
      if(prefab != null)
        Instantiate(prefab);
    }
  }
}
