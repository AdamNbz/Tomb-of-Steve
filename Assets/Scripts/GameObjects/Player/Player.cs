using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public enum Movement
{
    Up,
    Left,
    Down,
    Right
}

namespace Player
{
    public class Player : InitializeGameObject
    {

        private bool isPlaying, isMoving;

        public Transform raycastPosition1, raycastPosition2;

        private int currentAngle;

        private Queue<Movement> moveQueue;

        private Vector3 latestPosition;

        private const float raycast1Dist = 15.0f, raycast2Dist = 25.0f, raycast2PushOffset = -0.25f;

        #region Animations

        protected Animator animator;
        protected SpriteRenderer spriteRenderer;

        public const string START_ANIM = "Start";
        public const string IDLE_ANIM = "Idle";
        public const string JUMP_ANIM = "Jump";
        public const string DEATH_ANIM = "Death";
        private const string TWEEN = "Tween";

        private void PlayAnimation(string key)
        {
            this.animator.Play(key);
        }

        private void PlayStartAnimation()
        {
            this.spriteRenderer.DOFade(1.0f, 2.0f).SetId(this.GetInstanceID() + TWEEN);
            this.animator.Play(START_ANIM);
        }

        private IEnumerator GameStartAnimation()
        {
            this.PlayStartAnimation();

            yield return new WaitForSeconds(2.0f);

            this.isPlaying = true;

            this.PlayAnimation(IDLE_ANIM);
        }

        private void RecheckMovementAnimation()
        {
            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_ANIM) && !this.animator.GetCurrentAnimatorStateInfo(0).IsName(JUMP_ANIM))
            {
                return;
            }

            if (this.IsMoving() && !this.animator.GetCurrentAnimatorStateInfo(0).IsName(JUMP_ANIM))
            {
                this.animator.Play(JUMP_ANIM);
            }

            if (this.IsMoving() && !this.animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_ANIM))
            {
                this.animator.Play(IDLE_ANIM);
            }
        }

        void OnDisable()
        {
            DOTween.Kill(this.GetInstanceID() + TWEEN);
        }

        #endregion

        #region Movement

        bool IsMoving()
        {
            return this.isMoving;
        }

        public void OnDead()
        {
            this.isPlaying = false;

            //CGameSoundManager.Instance.PlayPlayerFx(GameDefine.PLAYER_DIE_FX_KEY);
            //CGameplayManager.Instance.OnPlayerDead();

            this.PlayAnimation(DEATH_ANIM);
        }

        #endregion

        void Start()
        {
            this.animator = this.GetComponent<Animator>();
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            StartCoroutine(GameStartAnimation());
        }

        void Update()
        {
            this.RecheckMovementAnimation();
        }

    }
}
