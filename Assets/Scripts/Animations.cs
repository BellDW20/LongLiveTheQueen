using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations {

    private Animator _animator;
    private string currentAnimation;
    public Animations(Animator _animator, string initialAnimation) {
        this._animator = _animator;
        this.currentAnimation = initialAnimation;
        _animator.Play(initialAnimation);
    }

    public void SetAnimation(string animation) {
        if(currentAnimation.Equals(animation)) { return; }
        currentAnimation = animation;
        _animator.Play(animation);
    }

}
