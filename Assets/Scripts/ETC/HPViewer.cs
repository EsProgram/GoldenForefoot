using Es.Actor;
using System.Collections;
using UnityEngine;

public class HPViewer : MonoBehaviour
{
  [SerializeField]
  private GameObject player = null;

  private PlayerControl ctrl;
  private float defaultScaleX;

  public void Awake()
  {
    ctrl = player.GetComponent<PlayerControl>();
  }

  private void Start()
  {
    defaultScaleX = transform.localScale.x;
  }

  private void Update()
  {
    transform.localScale = new Vector3(defaultScaleX * ctrl.HP / ctrl.MaxHP, transform.localScale.y, transform.localScale.z);
  }
}
