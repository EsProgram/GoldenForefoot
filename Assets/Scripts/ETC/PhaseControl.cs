using Es.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhaseControl : MonoBehaviour
{
  /**************************************************
   * field
   **************************************************/
  [SerializeField, Tooltip("ステージで使用するフェーズのプレハブ")]
  private List<GameObject> phasePrefabs = null;
  [SerializeField, Tooltip("クリアアニメーションに移行させるGameClearメソッドをもつオブジェクト")]
  private List<GameObject> clearObjects = null;

  private bool nextTrigger = false;
  private int currentInstantiateIndex = 0;
  private GameObject currentPhase;
  private bool calledlear;

  /**************************************************
   * method
   **************************************************/

  private void Update()
  {
    //Phaseをインスタンス化
    if(nextTrigger)
    {
      nextTrigger = false;
      if(currentInstantiateIndex < phasePrefabs.Count)
      {
        currentPhase = Instantiate(phasePrefabs[currentInstantiateIndex]) as GameObject;
        ++currentInstantiateIndex;
      }
    }

    //全てのフェーズを完了した時に呼び出される(クリア処理)
    if(currentPhase == null && currentInstantiateIndex == phasePrefabs.Count)
      if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().HP > 0)
        GameClear();

    //現在実行されているPhaseがなければトリガーをOnにする
    if(currentPhase == null)
      SetTrigger();
  }

  public void OnGUI()
  {
    if(GUILayout.Button("次のフェーズ"))
      SetTrigger();
    GUILayout.Label("現在のフェーズ:" + currentInstantiateIndex.ToString());
    if(currentInstantiateIndex >= phasePrefabs.Count)
      GUILayout.Label("これ以上のフェーズは存在しません");
    if(GUILayout.Button("HP全回復"))
      GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().Heal(9999);
    if(GUILayout.Button("タイトルへ戻る"))
      Application.LoadLevel("Title");
    if(GUILayout.Button("クリア画面に進む"))
      Application.LoadLevel("GameClear");
  }

  /// <summary>
  /// 次のフェーズを開始するトリガーをセットする
  /// </summary>
  private void SetTrigger()
  {
    nextTrigger = true;
  }

  /// <summary>
  /// クリアオブジェクトのGameClearメソッドを呼び出す
  /// 1回しか呼ばれない
  /// 一定秒後にクリアシーンに切り替える
  /// </summary>
  private void GameClear()
  {
    if(calledlear)
      return;
    foreach(var obj in clearObjects)
      obj.SendMessage("GameClear");
    Invoke("LoadGameClearScene", 3f);
    calledlear = true;
  }

  /// <summary>
  /// GameClearシーンを呼び出す
  /// </summary>
  private void LoadGameClearScene()
  {
    Application.LoadLevel("GameClear");
  }
}
