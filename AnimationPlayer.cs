using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    ///<summary>Triggers animations to appear</summary>
    public void PlayAppearAnimation()
    {
        _animator.SetTrigger("Appear");
    }
    ///<summary>Triggers animations to disappear</summary>
    public void PlayDisappearAnimation()
    {
        _animator.SetTrigger("Disappear");
    }
}
