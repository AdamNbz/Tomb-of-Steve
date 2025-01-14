using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public bool debug;

        public AudioTrack[] tracks;

        private PlayerMovement _movement;
        private PlayerInput _input;

        private Collider2D _col;
        [SerializeField] private Animator _animator;

    }
}
