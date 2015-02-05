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
    Idle,
    Play,

    //以下、遷移不可時間が存在する
    Damaged,
    Dead,
  }

  /// <summary>
  /// プレイヤー制御
  /// </summary>
  public class PlayerController : CharactorBase
  {
    /**************************************************
     * Field
     **************************************************/
    private bool possibleChangeState;
    private PlayerState state;
    private float horizontalInput;
    private float verticalInput;
    private bool leftAttackInput;
    private bool rightAttackInput;
    private Vector2 dir;
    private Dictionary<string, Animator> handAnimators;

    private const string LEFT_HAND = "LeftHand";
    private const string RIGHT_HAND = "RightHand";

    private PlayerController()
    {
      possibleChangeState = true;
      state = PlayerState.Idle;
    }

    public void Awake()
    {
      //ゲームオブジェクト名にhandを含むオブジェクトのAnimatorコンポーネントをHashで取得
      //キーはゲームオブジェクト名
      handAnimators = transform.GetComponentsInChildren<Animator>().Where(a =>
      {
        return a.name.ToLower().Contains("hand");
      }).ToDictionary(a => a.name);
    }

    public void Update()
    {
      //ユーザーが状態遷移可能なら
      if(possibleChangeState)
      {
        //ユーザー入力情報の取得
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        leftAttackInput = Input.GetButtonDown("LeftAttack");
        rightAttackInput = Input.GetButtonDown("RightAttack");

        //PlayerStateの変更
      }

      //State別処理
      switch(state)
      {
        case PlayerState.Idle:
          break;

        case PlayerState.Play:

          #region 移動
          dir = new Vector2(horizontalInput, verticalInput);
          transform.Translate(Time.deltaTime * moveSpeed * dir);
          #endregion 移動

          #region 攻撃
          if(leftAttackInput)
            handAnimators[LEFT_HAND].SetTrigger("Attack");
          if(rightAttackInput)
            handAnimators[RIGHT_HAND].SetTrigger("Attack");
          #endregion 攻撃

          break;

        case PlayerState.Damaged:
          break;

        case PlayerState.Dead:
          break;

        default:
          break;
      }
    }

#if DEBUG

    public void OnGUI()
    {
      if(GUILayout.Button("Idle"))
        state = PlayerState.Idle;
      if(GUILayout.Button("Play"))
        state = PlayerState.Play;
      if(GUILayout.Button("Damaged"))
        state = PlayerState.Damaged;
      if(GUILayout.Button("Dead"))
        state = PlayerState.Dead;
    }

#endif
  }
}
