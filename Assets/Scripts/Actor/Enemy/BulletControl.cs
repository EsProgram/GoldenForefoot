using Es.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> bulletPrefab = null;
  [SerializeField, Range(0, 5), Tooltip("初弾待ち時間")]
  private float initWaitTime = 1f;
  [SerializeField, Range(0, 5), Tooltip("弾打ち間隔")]
  private float repeatRate = 1f;

  private EnemyControl ctrl;
  private float time;
  private int currentIndex;

  public void Awake()
  {
    ctrl = GetComponent<EnemyControl>();
  }

  public void Start()
  {
    time = 0;
    currentIndex = 0;
  }

  public void Update()
  {
    time += Time.deltaTime;

    //インデックス0の弾を発射するとき
    if(currentIndex == 0)
    {
      if(time > initWaitTime)
      {
        if(gameObject.layer == LayerMask.NameToLayer("StopEnemy"))
          BulletInstantiate(currentIndex++);
        time = 0;
      }
    }

    //インデックス0以外の弾を発射するとき
    else
    {
      if(time > repeatRate)
      {
        BulletInstantiate(currentIndex++);
        time = 0;
      }
    }

    //全インデックスの弾を打ち終えた時
    if(currentIndex >= bulletPrefab.Count)
      currentIndex = 0;
  }

  /// <summary>
  /// 指定されたインデックスの弾をプレハブからインスタンス化する
  /// </summary>
  /// <param name="index">インデックス</param>
  private void BulletInstantiate(int index)
  {
    if(ctrl.state == State.Play)
      Instantiate(bulletPrefab[index], transform.position, Quaternion.identity);
  }
}
