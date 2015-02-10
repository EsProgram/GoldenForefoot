﻿using Es.Actor;
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
  [SerializeField, Tooltip("クリアアニメーションに移行させるオブジェクト")]
  private List<GameObject> clearObjects = null;

  private bool nextTrigger = false;
  private int currentInstantiateIndex = 0;
  private GameObject currentPhase;

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
      if(clearObjects.Any(o => { return o.tag == "Player" && o.GetComponent<PlayerControl>().HP > 0; }))
        Clear();

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
  }

  /// <summary>
  /// 次のフェーズを開始するトリガーをセットする
  /// </summary>
  private void SetTrigger()
  {
    nextTrigger = true;
  }

  /// <summary>
  /// クリアオブジェクトのClearメソッドを呼び出す
  /// </summary>
  private void Clear()
  {
    Debug.Log("クリア");
    foreach(var obj in clearObjects)
      obj.SendMessage("Clear");
  }
}