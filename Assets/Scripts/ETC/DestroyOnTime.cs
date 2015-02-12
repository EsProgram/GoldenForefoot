using System.Collections;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
  [SerializeField]
  private float time = 3f;

  private void Start()
  {
    Destroy(gameObject, time);
  }
}
