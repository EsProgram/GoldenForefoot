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
  public enum PlayerState
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
    private Transform attackOrigin = default(Transform);
    [SerializeField]
    private Transform arrowOrigin = default(Transform);
    [SerializeField]
    private Transform dirHelper = default(Transform);
    [SerializeField, Range(0, 10)]
    private float moveSpeed = 5f;
    [SerializeField, Range(0, 1)]
    private float slowSpeedRate = 0.5f;
    [SerializeField, Range(0, 999)]
    private float attackPower = 999f;
    [SerializeField, Range(0, 10)]
    private float attackRadius = 0.5f;

    private float horizontalInput;
    private float verticalInput;
    private bool leftHandAttackInput;//"左手"での攻撃
    private bool rightHandAttackInput;//"右手"での攻撃
    private bool slowSpeedInput;//低速移動
    private bool slowTrriger;//ボタンを押す度低速とノーマル入れ替え

    private Vector2 moveDir;//移動方向

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
          moveDir = new Vector2(horizontalInput, verticalInput);
          if(moveDir.magnitude > 1f)
            moveDir = moveDir.normalized;
          if(slowSpeedInput)
            slowTrriger = !slowTrriger;
          if(slowTrriger)
            moveDir *= slowSpeedRate;
          transform.Translate(Time.deltaTime * moveDir * moveSpeed, Space.World);
          Debug.Log(Camera.main.WorldToViewportPoint(transform.position));

          var arrowDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
          var angle = Mathf.Atan2(arrowDir.y, arrowDir.x) * Mathf.Rad2Deg;
          arrowOrigin.transform.eulerAngles = new Vector3(0f, 0f, angle);
          #endregion 移動

          #region 攻撃

          DebugExtension.DrawCircle(attackOrigin.position, attackRadius, Color.green);
          if(leftHandAttackInput || rightHandAttackInput)
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
            var exprDir = dirHelper.position - arrowOrigin.position;
            if(leftHandAttackInput)
              AddForce(cols, exprDir, attackPower);
            if(rightHandAttackInput)
              AddForce(cols, exprDir, attackPower);

            //対象のステート変更とダメージ処理
            foreach(var col in cols)
            {
              col.GetComponent<Animator>().StopPlayback();
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
      leftHandAttackInput = Input.GetButtonDown("Fire1");
      rightHandAttackInput = Input.GetButtonDown("Fire2");
      slowSpeedInput = Input.GetButtonDown("Fire3");
    }
  }
}
