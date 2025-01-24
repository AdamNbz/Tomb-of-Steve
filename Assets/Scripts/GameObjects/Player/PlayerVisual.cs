using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Animator))]
public class PlayerVisual : InitializeVisual
{
    #region INITIALIZE COMPONENTS
    [SerializeField] private PlayerScript player;

    public const string START_ANIM = "Start";
    public const string IDLE_ANIM = "Idle";
    public const string DEATH_ANIM = "Death";
    public const string JUMP_ANIM = "Jump";

    public const string TWEEN = "_FADE_";

    #endregion

    #region FUNCTIONS

    private void OnDisable()
    {
        DOTween.Kill(this.GetInstanceID() + TWEEN);
    }

    private void LateUpdate()
    {
        this.RecheckMovementAnimation();
    }

    private void RecheckMovementAnimation()
    {
        if (!this.objAnimator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_ANIM) && !this.objAnimator.GetCurrentAnimatorStateInfo(0).IsName(JUMP_ANIM))
            return;

        if (this.player.IsMoving() && !this.objAnimator.GetCurrentAnimatorStateInfo(0).IsName(JUMP_ANIM))
        {
            this.objAnimator.Play(JUMP_ANIM);
        }

        if (this.player.IsMoving() && !this.objAnimator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_ANIM))
        {
            this.objAnimator.Play(IDLE_ANIM);
        }
    }

    public void PlayStartAnimation()
    {
        this.objSR.DOFade(1.0f, 2.0f).SetId(this.GetInstanceID() + TWEEN);
        this.objAnimator.Play(START_ANIM);
    }

    public void PlayAnimation(string key)
    {
        this.objAnimator.Play(key);
    }

    public void OnPlayerDieAnimCompleted()
    {
        this.player.gameObject.SetActive(false);
    }

    #endregion

}
