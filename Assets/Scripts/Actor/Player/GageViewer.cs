using Es.Actor;
using System.Collections;
using UnityEngine;

public class GageViewer : MonoBehaviour
{
  [SerializeField]
  private Color notAllow = default(Color);
  [SerializeField]
  private Color allow = default(Color);
  [SerializeField]
  private WhichHand which = WhichHand.Left;

  private PlayerControl ctrl;
  private SpriteRenderer sprite;
  private float defaultScaleX;

  public void Awake()
  {
    ctrl = GetComponentInParent<PlayerControl>();
    sprite = GetComponent<SpriteRenderer>();
  }

  private void Start()
  {
    defaultScaleX = transform.localScale.x;
  }

  private void Update()
  {
    switch(which)
    {
      case WhichHand.Left:
        transform.localScale = new Vector3(defaultScaleX * ctrl.LeftGage, transform.localScale.y, transform.localScale.z);
        if(ctrl.LeftGage < ctrl.MinAttackGage)
          sprite.color = notAllow;
        else
          sprite.color = allow;
        break;

      case WhichHand.Right:
        transform.localScale = new Vector3(defaultScaleX * ctrl.RightGage, transform.localScale.y, transform.localScale.z);
        if(ctrl.RightGage < ctrl.MinAttackGage)
          sprite.color = notAllow;
        else
          sprite.color = allow;
        break;

      default:
        break;
    }
  }
}
