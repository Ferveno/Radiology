using System.Collections;
using System.Collections.Generic;
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
            GameManager.instance.OnGame1Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[0];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.027f, -0.326f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.436f, 1.22f);

        }
        else if (collision.gameObject.tag.Equals("Game2Key"))
        {
            GameManager.instance.OnGame2Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[1];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005f, -0.398f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.453f, 0.562f);

        }
        else if (collision.gameObject.tag.Equals("Game3Key"))
        {
            GameManager.instance.OnGame3Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[1];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005f, -0.398f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.453f, 0.562f);

        }
        else if (collision.gameObject.tag.Equals("Game4Key"))
        {
            GameManager.instance.OnGame4Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[1];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005f, -0.398f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.453f, 0.562f);

        }
        else if (collision.gameObject.tag.Equals("Game5Key"))
        {
            GameManager.instance.OnGame5Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[1];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005f, -0.398f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.453f, 0.562f);

        }
        else if (collision.gameObject.tag.Equals("Game6Key"))
        {
            GameManager.instance.OnGame6Start();
            _animator.runtimeAnimatorController = otherAnimatorControllers[1];
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005f, -0.398f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.453f, 0.562f);

        }
    }
}
