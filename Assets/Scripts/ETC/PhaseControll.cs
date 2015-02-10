using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhaseControll : MonoBehaviour
{
  /**************************************************
   * field
   **************************************************/
  [SerializeField, Tooltip("ステージで使用するフェーズのプレハブ")]
  private List<GameObject> phasePrefabs = default(List<GameObject>);

  private bool nextTrigger = default(bool);
  private int currentInstantiateIndex = 0;

  /**************************************************
   * method
   **************************************************/

  /// <summary>
  /// 次のフェーズを開始するトリガーをセットする
  /// </summary>
  public void SetTrigger()
  {
    nextTrigger = true;
  }

  public void Awake()
  {
  }

  private void Update()
  {
    if(nextTrigger)
    {
      nextTrigger = false;
      if(currentInstantiateIndex < phasePrefabs.Count)
      {
        Instantiate(phasePrefabs[currentInstantiateIndex]);
        ++currentInstantiateIndex;
      }
      else
      {
        //全てのフェーズをインスタンス化したので、事後処理
        Destroy(gameObject);
      }
    }
  }

  private void OnGUI()
  {
    if(GUILayout.Button("次のフェーズを実行"))
      SetTrigger();
    if(currentInstantiateIndex >= phasePrefabs.Count)
      GUILayout.Label("フェーズはもうありません。未実装です。");
  }
}
