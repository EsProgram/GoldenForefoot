using System.Collections;
using UnityEngine;

namespace Es.Charactor
{
  /// <summary>
  /// 全てのキャラクターの基本となるクラス
  /// </summary>
  public abstract class CharactorBase : MonoBehaviour
  {
    /**************************************************
     * キャラクターステータス
     **************************************************/
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected float moveSpeed;

    /**************************************************
     * クラスフィールド
     **************************************************/
  }
}
