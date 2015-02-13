﻿using System.Collections;
using UnityEngine;

/// <summary>
/// 背景スクロール
/// </summary>
public class Scroll : MonoBehaviour
{
  /// <summary>
  /// スクロールの状態を表す
  /// </summary>
  public enum ScrollState
  {
    Run,
    Stop,
  }

  [SerializeField]
  private float speed = 1f;

  private SpriteRenderer sr;
  private static ScrollState state;

  /// <summary>
  /// スクロールを開始する
  /// </summary>
  public static void Run()
  {
    state = ScrollState.Run;
  }

  /// <summary>
  /// スクロールを止める
  /// </summary>
  public static void Stop()
  {
    state = ScrollState.Stop;
  }

  private void Awake()
  {
    sr = GetComponent<SpriteRenderer>();
  }

  private void Start()
  {
    state = ScrollState.Run;
  }

  private void Update()
  {
    switch(state)
    {
      case ScrollState.Run:
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        break;

      case ScrollState.Stop:
        break;

      default:
        break;
    }
  }

  private void OnBecameInvisible()
  {
    transform.position += Vector3.right * sr.bounds.size.x * 2;
  }
}
