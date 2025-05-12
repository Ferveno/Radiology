using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigiRigidbody;
    [SerializeField] private DynamicJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public GameObject flameThrowerParticle_Ref;
    public float _moveSpeed;

    public int damageOnEnemy;

    public AnimatorController[] otherAnimatorControllers;

    private AnimatorController MyAnimatorController;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigiRigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        MyAnimatorController = _animator.GetComponent<AnimatorController>();

        // Check_EquippedGun();
    }
    private void FixedUpdate()
    {
        _rigiRigidbody.velocity = new Vector2(_joystick.Horizontal * _moveSpeed, _joystick.Vertical * _moveSpeed);

        if (_rigiRigidbody.velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_rigiRigidbody.velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }

        if (_rigiRigidbody.velocity.x != 0 || _rigiRigidbody.velocity.x != 0)
        {
            _animator.SetBool("isWalk", true);
        }
        else
        {
            _animator.SetBool("isWalk", false);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {       
        if (collision.gameObject.tag.Equals("Game1Key")) {
            //GameManager.instance.OnGame1Start();
            //_animator.runtimeAnimatorController = otherAnimatorControllers[0];
            //gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            //gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.027f, -0.326f);
            //gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.436f, 1.22f);

            // 1) Build a sequence
            var seq = DOTween.Sequence();

            // a) Shrink away quickly
            seq.Append(transform
                .DOScale(0.1f, 0.25f)
                .SetEase(Ease.InBack)
            );

            // b) On that callback, swap animator & collider
            seq.AppendCallback(() =>
            {
                _animator.runtimeAnimatorController = otherAnimatorControllers[0];

                // adjust scale & collider defaults
                transform.localScale = Vector3.one * 2f;
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.027f, -0.326f);
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.436f, 1.22f);
            });

            // c) Pop back out with a little bounce
            seq.Append(transform
                .DOScale(2f, 0.5f)
                .SetEase(Ease.OutBounce)
            );

            // (Optional) add a quick color flash on your sprite
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // flash white then back
                seq.Join(sr
                    .DOColor(Color.white, 0.1f)
                    .SetLoops(2, LoopType.Yoyo)
                );
            }

            // 2) Add a callback to the end of the sequence to start the respective Game
            seq.OnComplete(() =>
            {
                GameManager.instance.OnGame1Start();
            });

            // 3) Kick it off
            seq.Play();

        }
        else if (collision.gameObject.tag.Equals("Game2Key"))
        {
            GameManager.instance.OnGame2Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[1];
            gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.02013f, -0.3228f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.8018f, 1.7992f);

        }
        else if (collision.gameObject.tag.Equals("Game3Key"))
        {
            GameManager.instance.OnGame3Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[2];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.0461f, -0.2565f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.9620f, 0.9788f);

        }
        else if (collision.gameObject.tag.Equals("Game4Key"))
        {
            GameManager.instance.OnGame4Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[3];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005f, -0.398f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.453f, 0.562f);

        }
        else if (collision.gameObject.tag.Equals("Game5Key"))
        {
            GameManager.instance.OnGame5Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[4];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005f, -0.398f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.453f, 0.562f);

        }
        else if (collision.gameObject.tag.Equals("Game6Key"))
        {
            GameManager.instance.OnGame6Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[5];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005f, -0.398f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.453f, 0.562f);

        }
    }
}
