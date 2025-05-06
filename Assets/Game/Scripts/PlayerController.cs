using System.Collections;
using System.Collections.Generic;
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


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigiRigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

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
       if(collision.gameObject.tag.Equals("Enemy"))
       {
            SceneManager.LoadScene("Game 1");
       }
       else if(collision.gameObject.tag.Equals("SolveQuestion"))
       {
            Debug.LogError("Solve Question");
       }  
    }
}
