#define DEBUG

using Es.Charactor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Es.Charactor
{
  /// <summary>
  /// プレイヤーの状態
  /// </summary>
  internal enum PlayerState
  {
    Idle,//強制的に動けない状態
    Play,//ゲームプレイ中の状態
    Dead,//死亡状態
  }

  /// <summary>
  /// プレイヤー制御
  /// </summary>
  public class PlayerController : CharactorBase
  {
    /**************************************************
     * field
     **************************************************/

    [SerializeField]
    private PlayerState state = PlayerState.Play;
    [SerializeField]
    public Transform attackOrigin;
    [SerializeField, Range(0, 999)]
    private float attackPower = 999f;
    [SerializeField, Range(0, 10)]
    private float attackRadius = 0.5f;

    private float horizontalInput;
    private float verticalInput;
    private bool leftHandAttackInput;//"左手"での攻撃(右に飛ばす)
    private bool rightHandAttackInput;//"右手"での攻撃(左に飛ばす)
    private bool forwardLeftHandAttackInput;//"左手"での攻撃(前に飛ばす)
    private bool forwardRightHandAttackInput;//"右手"での攻撃(前に飛ばす)
    private Vector2 dir;

    private const string LEFT_HAND = "LeftHand";
    private const string RIGHT_HAND = "RightHand";

    /**************************************************
     * override
     **************************************************/

    public void Update()
    {
      //ユーザー入力情報の取得
      GetUserInput();

      //State別処理
      switch(state)
      {
        case PlayerState.Idle:
          Debug.Log(name + ":Idle状態に遷移しました");
          break;

        case PlayerState.Play:

          #region 移動
          dir = new Vector2(horizontalInput, verticalInput);
          transform.Translate(Time.deltaTime * moveSpeed * dir, Space.World);
          #endregion 移動

          #region 攻撃

          //Debug.DrawLine(attackOrigin.position - Vector3.right * attackRadius, attackOrigin.position + Vector3.right * attackRadius);
          if(leftHandAttackInput || rightHandAttackInput || forwardLeftHandAttackInput || forwardRightHandAttackInput)
          {
            /**************************************************
             * Enemyタグをもつ目の前のコライダーの取得と
             * 攻撃対象(力を与える対象)の選定
             **************************************************/

            var cols = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius)
              .Where(c => { return c.tag == "Enemy"; })//tagがEnemy
              .Where(c =>
              {
                var component = c.GetComponent<CharactorBase>();
                if(component == null)
                  return false;
                return component.GetHP() > 0;
              })//HPが1以上
              .Where(c =>
              {
                var component = c.GetComponent<EnemyControll>();
                if(component == null)
                  return false;
                return component.State == EnemyState.Play;
              });//Play状態

            //それぞれの攻撃による吹き飛ばし
            if(leftHandAttackInput)
              AddForce(cols, new Vector2(1, -1), attackPower);
            else if(rightHandAttackInput)
              AddForce(cols, new Vector2(1, 1), attackPower);
            else if(forwardLeftHandAttackInput || forwardRightHandAttackInput)
              AddForce(cols, new Vector2(1, 0), attackPower);

            //対象のステート変更とダメージ処理
            foreach(var col in cols)
            {
              col.GetComponent<EnemyControll>().State = EnemyState.Panched;
              col.gameObject.SendMessage("Damaged");
            }
          }
          #endregion 攻撃

          break;

        case PlayerState.Dead:
          Debug.Log("Dead状態に遷移しました");
          break;

        default:
          break;
      }
    }

    /**************************************************
     * method
     **************************************************/

    /// <summary>
    /// ユーザーからの入力を取得する
    /// </summary>
    private void GetUserInput()
    {
      horizontalInput = Input.GetAxis("Horizontal");
      verticalInput = Input.GetAxis("Vertical");
      leftHandAttackInput = Input.GetButtonDown("LeftHandAttack");
      rightHandAttackInput = Input.GetButtonDown("RightHandAttack");
      forwardLeftHandAttackInput = Input.GetButtonDown("ForwardLeftHandAttack");
      forwardRightHandAttackInput = Input.GetButtonDown("ForwardRightHandAttack");
    }
  }
}
