using JetBrains.Annotations;
using Netcode;
using Netcode.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Movement.Components
{

    [RequireComponent(typeof(Rigidbody2D)),
     RequireComponent(typeof(Animator)),
     RequireComponent(typeof(NetworkObject))]
    public sealed class FighterMovement : NetworkBehaviour, IMoveableReceiver, IJumperReceiver, IFighterReceiver
    {
        public float speed = 1.0f;
        public float jumpAmount = 1.0f;

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private NetworkAnimator _networkAnimator;
        private Transform _feet;
        private LayerMask _floor;

        public static NetworkVariable<int> jugadores = new NetworkVariable<int>();
        public static SemaphoreSlim sumaJuadores = new SemaphoreSlim(1);
        private GameObject ganador;

        public const int maxHealth = 100;
        public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
        public Canvas canvas;
        public RectTransform healthBar;

        public Text nombre;
        private GameObject ganadorPanel;


        private Vector3 _direction = Vector3.zero;
        private bool _grounded = true;

        private static readonly int AnimatorSpeed = Animator.StringToHash("speed");
        private static readonly int AnimatorVSpeed = Animator.StringToHash("vspeed");
        private static readonly int AnimatorGrounded = Animator.StringToHash("grounded");
        private static readonly int AnimatorAttack1 = Animator.StringToHash("attack1");
        private static readonly int AnimatorAttack2 = Animator.StringToHash("attack2");
        private static readonly int AnimatorHit = Animator.StringToHash("hit");
        private static readonly int AnimatorDie = Animator.StringToHash("die");

        void Start()
        {
            //ganadorPanel = GameObject.FindGameObjectWithTag("Ganador");
            nombre.text = PlayerPrefs.GetString("nombre");
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _networkAnimator = GetComponent<NetworkAnimator>();

            _feet = transform.Find("Feet");
            _floor = LayerMask.GetMask("Floor");
        }

        void Update()
        {


            if (!IsServer) return;
            _grounded = Physics2D.OverlapCircle(_feet.position, 0.1f, _floor);
            _animator.SetFloat(AnimatorSpeed, this._direction.magnitude);
            _animator.SetFloat(AnimatorVSpeed, this._rigidbody2D.velocity.y);
            _animator.SetBool(AnimatorGrounded, this._grounded);

        }

        void FixedUpdate()
        {
            if (!IsServer) return;
            _rigidbody2D.velocity = new Vector2(_direction.x, _rigidbody2D.velocity.y);
        }

        public void Move(IMoveableReceiver.Direction direction)
        {
            UpdateMovementServerRpc(direction);

        }

        public void Jump(IJumperReceiver.JumpStage stage)
        {
            UpdateJumpServerRpc(stage);
        }

        [ServerRpc]
        public void UpdateJumpServerRpc(IJumperReceiver.JumpStage stage)
        {
            switch (stage)
            {
                case IJumperReceiver.JumpStage.Jumping:
                    if (_grounded)
                    {
                        float jumpForce = Mathf.Sqrt(jumpAmount * -2.0f * (Physics2D.gravity.y * _rigidbody2D.gravityScale));
                        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                    }
                    break;
                case IJumperReceiver.JumpStage.Landing:
                    break;
            }
        }

        [ServerRpc]
        public void UpdateMovementServerRpc(IMoveableReceiver.Direction direction)
        {
            if (direction == IMoveableReceiver.Direction.None)
            {
                this._direction = Vector3.zero;
                return;
            }

            bool lookingRight = direction == IMoveableReceiver.Direction.Right;
            _direction = (lookingRight ? 1f : -1f) * speed * Vector3.right;
            transform.localScale = new Vector3(lookingRight ? 1 : -1, 1, 1);
            canvas.transform.localScale = new Vector3(lookingRight ? -0.01f : 0.01f, 0.01f, 0f);
        }


        public void Attack1()
        {

            _networkAnimator.SetTrigger(AnimatorAttack1);

        }

        public void Attack2()
        {

            _networkAnimator.SetTrigger(AnimatorAttack2);
        }


        public void OnHealth()
        {
            if (IsServer)
            {
                currentHealth.Value -= 10;

            }
        }


        public override void OnNetworkSpawn()
        {
            sumaJuadores.Wait();
            currentHealth.Value = maxHealth;
            currentHealth.OnValueChanged += OncurrentHealthChanged;
            jugadores.Value++;
            sumaJuadores.Release();
        }


        private void OncurrentHealthChanged(int previous, int current)
        {
            
            if (current <= 0)
            {
                current = 0;
                Die();
            }
            healthBar.sizeDelta = new Vector2(current, healthBar.sizeDelta.y);

        }

        public void TakeHit()
        {
            OnHealth();
            _networkAnimator.SetTrigger(AnimatorHit);


        }

        public void Die()
        {
            _networkAnimator.SetTrigger(AnimatorDie);
            speed = 0.0f;
            jumpAmount = 0.0f;
            DieServerRpc();
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        public void DieServerRpc()
        {

            StartCoroutine(Wait());
            jugadores.Value--;
            if (jugadores.Value == 1)
            {
                ganador = GameObject.Find("Player(Clone)");
                PlayerNetworkConfig toprint = ganador.GetComponent<PlayerNetworkConfig>();
                //ganadorPanel.SetActive(true);
                Debug.Log(toprint);
            }


        }

    }
}