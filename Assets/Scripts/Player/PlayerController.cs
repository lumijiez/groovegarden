using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private PlayerPusher playerPusher;

    private SoundEngine soundEngine;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;

    protected override void Awake()
    {
        base.Awake();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        playerPusher = GetComponentInChildren<PlayerPusher>() ?? GameObject.FindFirstObjectByType<PlayerPusher>();
        myTrailRenderer = GetComponentInChildren<TrailRenderer>() ?? GameObject.FindFirstObjectByType<TrailRenderer>();
        soundEngine = GetComponentInChildren<SoundEngine>();

        if (playerPusher == null || myTrailRenderer == null || soundEngine == null)
        {
            Debug.LogError("PlayerController: Missing essential components.");
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.playerControls.Combat.Dash.performed += OnDashPerformed;
        InputManager.Instance.playerControls.Combat.Push.performed += OnPushPerformed;
    }

    private void OnDisable()
    {
        InputManager.Instance.playerControls.Combat.Dash.performed -= OnDashPerformed;
        InputManager.Instance.playerControls.Combat.Push.performed -= OnPushPerformed;
    }

    private void OnDashPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (this != null) 
        {
            Dash();
        }
    }

    private void OnPushPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (this != null) 
        {
            PushAll();
        }
    }

    private void Start()
    {
        startingMoveSpeed = moveSpeed;
        ActiveInventory.Instance.EquipStartingWeapon();
        InputManager.Instance.playerControls.Combat.Dash.performed += OnDashPerformed;
        InputManager.Instance.playerControls.Combat.Push.performed += OnPushPerformed;
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PushAll()
    {
        if (playerPusher == null)
        {
            InitializeComponents(); 
        }
        playerPusher?.PushAll();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        movement = InputManager.Instance.playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            if (myTrailRenderer == null)
            {
                InitializeComponents();
            }
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
