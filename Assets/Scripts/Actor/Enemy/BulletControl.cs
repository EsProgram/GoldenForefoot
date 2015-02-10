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

  private bool shotFlag = true;
  private EnemyControl enemyCtrl;

  public void Awake()
  {
    enemyCtrl = GetComponent<EnemyControl>();
  }

  public void Update()
  {
    if(shotFlag)
    {
      StartCoroutine(CreateBullet());
      shotFlag = false;
    }
  }

  private IEnumerator CreateBullet()
  {
    yield return new WaitForSeconds(initWaitTime);
    for(int i = 0; i < bulletPrefab.Count; ++i)
    {
      if(i != 0)
        yield return new WaitForSeconds(repeatRate);
      if(enemyCtrl.state == State.Play)
        BulletInstantiate(i);
    }
    shotFlag = true;
  }

  private void BulletInstantiate(int index)
  {
    Instantiate(bulletPrefab[index], transform.position, Quaternion.identity);
  }
}
