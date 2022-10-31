using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used as a cheap alternative to the animator state machine
//stuff which we haven't learned yet
public class Animations {

    private Animator _animator; //Animator to manipulate
    private string currentAnimation; //The name of the animation the animator is currently playing

    public Animations(Animator _animator, string initialAnimation) {
        this._animator = _animator;
        this.currentAnimation = initialAnimation;

        //Play the initial animation given
        _animator.Play(initialAnimation);
    }

    public void SetAnimation(string animation) {
        if(currentAnimation.Equals(animation)) { return; }
        //Only if the animation we want to play is different from
        //the animation currently playing, play the given animation
        //and set it as the current animation
        currentAnimation = animation;
        _animator.Play(animation);
    }

}
