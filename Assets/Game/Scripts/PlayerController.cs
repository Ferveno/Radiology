using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct CharacterConfig
{
    public RuntimeAnimatorController animatorController;
    public Vector3 targetScale;
    public Vector2 colliderOffset;
    public Vector2 colliderSize;
    public Action onCompleteCallback;
}

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

    // Now using the runtime type
    private RuntimeAnimatorController MyAnimatorController;

    [Header("Character Configs by Key Tag")]
    [SerializeField] private List<string> keyTags;
    [SerializeField] private List<CharacterConfig> configs;

    private BoxCollider2D _col;
    private Dictionary<string, CharacterConfig> _configMap;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();

        // Build quick lookup map
        _configMap = new Dictionary<string, CharacterConfig>();
        for (int i = 0; i < keyTags.Count; i++)
        {
            var cfg = configs[i];
            // Capture the callback with the proper GameManager call
            switch (i)
            {
                case 0: cfg.onCompleteCallback = GameManager.instance.OnGame1Start; break;
                case 1: cfg.onCompleteCallback = GameManager.instance.OnGame2Start; break;
                case 2: cfg.onCompleteCallback = GameManager.instance.OnGame3Start; break;
                case 3: cfg.onCompleteCallback = GameManager.instance.OnGame4Start; break;
                case 4: cfg.onCompleteCallback = GameManager.instance.OnGame5Start; break;
                case 5: cfg.onCompleteCallback = GameManager.instance.OnGame6Start; break;
            }
            _configMap[keyTags[i]] = cfg;
        }
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigiRigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // Store the current runtime animator controller
        MyAnimatorController = _animator.runtimeAnimatorController;
    }

    private void FixedUpdate()
    {
        _rigiRigidbody.velocity = new Vector2(
            _joystick.Horizontal * _moveSpeed,
            _joystick.Vertical * _moveSpeed
        );

        if (_rigiRigidbody.velocity.x > 0)
            _spriteRenderer.flipX = false;
        else if (_rigiRigidbody.velocity.x < 0)
            _spriteRenderer.flipX = true;

        bool isMoving = _rigiRigidbody.velocity.sqrMagnitude > 0.01f;
        _animator.SetBool("isWalk", isMoving);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_configMap.TryGetValue(collision.gameObject.tag, out var cfg))
            return;

        collision.gameObject.SetActive(false);
        DoCharacterChangeAnimation(cfg);
    }

    private void DoCharacterChangeAnimation(CharacterConfig cfg)
    {
        var seq = DOTween.Sequence();

        // 1) Shrink away
        seq.Append(transform
            .DOScale(0.1f, 0.25f)
            .SetEase(Ease.InBack)
        );

        // 2) Swap animator + apply new scale & collider
        seq.AppendCallback(() =>
        {
            _animator.runtimeAnimatorController = cfg.animatorController;
            transform.localScale = cfg.targetScale;
            _col.offset = cfg.colliderOffset;
            _col.size = cfg.colliderSize;
        });

        // 3) Bounce back
        seq.Append(transform
            .DOScale(cfg.targetScale, 0.5f)
            .SetEase(Ease.OutBounce)
        );

        // 4) Flash sprite in parallel
        if (_spriteRenderer)
        {
            seq.Join(_spriteRenderer
                .DOColor(Color.white, 0.1f)
                .SetLoops(2, LoopType.Yoyo)
            );
        }

        // 5) Finally, trigger the game‐start callback
        seq.OnComplete(() =>
        {
            cfg.onCompleteCallback?.Invoke();
        });

        seq.Play();
    }

#if UNITY_EDITOR
    // Example editor-only code can go here, e.g. validator or asset-loader.
#endif
}