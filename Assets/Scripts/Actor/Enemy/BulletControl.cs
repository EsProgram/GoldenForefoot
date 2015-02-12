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

  private float time;
  private int currentIndex;

  public void Awake()
  {
  }

  public void Start()
  {
    time = 0;
    currentIndex = 0;
  }

  public void Update()
  {
    time += Time.deltaTime;

    if(currentIndex == 0)
    {
      if(time > initWaitTime)
      {
        BulletInstantiate(currentIndex++);
        time = 0;
      }
    }

    if(currentIndex > 0)
    {
      if(time > repeatRate)
      {
        BulletInstantiate(currentIndex++);
        time = 0;
      }
    }

    if(currentIndex >= bulletPrefab.Count)
      currentIndex = 0;
  }

  private void BulletInstantiate(int index)
  {
    Instantiate(bulletPrefab[index], transform.position, Quaternion.identity);
  }
}
