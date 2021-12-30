using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rollSpeed = 4f;
    [SerializeField] private float _rollTime = 0.5f;
    [SerializeField] private float _ladderSpeed = 3f;
    [SerializeField] private float _smoothHandSnap = 11f;
    [SerializeField] private float _jumpHeight = 23f;
    [SerializeField] private float _gravity = 10f;
    [SerializeField] private bool _isGrounded;

    private bool _isJumping = false;
    private bool _isHanging = false;
    private bool _isClimbing = false;
    private bool _isRolling = false;
    private float _yVelocity;

    [SerializeField] private int _coins = 0;

    private CharacterController _controller;
    private Animator _animator;

    private Vector3 _velocity;
    private Vector3 _handPos;
    private LedgeChecker _activeLedge;
    private LadderChecker _activeLadder;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
            print("Controller is null");

        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
            print("Animator is null");
    }

    private void Update()
    {
        PlayerMovement();

        if (_isHanging &&  Input.GetKeyDown(KeyCode.W))
        {
            _isHanging = false;
            _animator.SetTrigger("climbUp");
            _animator.SetBool("canLedgeGrab", false);
        }

        if (_isHanging)
        {
            transform.position = Vector3.MoveTowards(transform.position, _handPos, _smoothHandSnap * Time.deltaTime);
        }

        if (_isClimbing)
        {
            float y = Input.GetAxisRaw("Vertical");
            _velocity = new Vector3(0, y, 0) * _ladderSpeed;
            _animator.SetFloat("ySpeed", Mathf.Abs(_velocity.y));
            //_animator.SetFloat("zSpeed", 0);
            _animator.SetBool("usingLadder", true);
            _animator.SetBool("isJumping", false);
        }
    }

    private void PlayerMovement()
    {
        _isGrounded = _controller.isGrounded;
        _animator.SetBool("isGrounded", _isGrounded);

        if (_isGrounded)
        {
            if (_isJumping)
            {
                _isJumping = false;
                _animator.SetBool("isJumping", _isJumping);
            }

            float z = Input.GetAxisRaw("Horizontal");

            _velocity = new Vector3(0, 0, z) * _speed;
            _animator.SetFloat("zSpeed", Mathf.Abs(z));

            if (z != 0)
            {
                Vector3 getFacing = transform.localEulerAngles;
                getFacing.y = _velocity.z > 0 ? 0 : 180;
                transform.localEulerAngles = getFacing;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _isClimbing = false;
                _isJumping = true;
                _animator.SetBool("isJumping", _isJumping);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && !_isRolling)
            {
                StartCoroutine(SprintRollRoutine());
            }
        }
        else if (!_isGrounded)
        {
            if (!_isClimbing)
                _yVelocity -= _gravity;
        }

        if (!_isClimbing)
        {
            _velocity.y = _yVelocity;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Ladder"))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.blue);
            ClimbLadder();
        }

        if(_isClimbing && hit.collider.tag != "Ladder")
        {
            _isClimbing = false;
            _animator.SetBool("usingLadder", false);
            _animator.SetFloat("ySpeed", 0);
        }
    }

    public void GrabLedge(Vector3 handPos, LedgeChecker currentLedge)
    {
        print("Is Ledge Grabbing!");
        _controller.enabled = false;
        _isHanging = true;
        _animator.SetBool("canLedgeGrab", true);
        _animator.SetFloat("zSpeed", 0.0f);
        _animator.SetBool("isJumping", false);

        //Update Position to hand pos
        _handPos = handPos;

        _activeLedge = currentLedge;
    }

    public void ClimbLadder()
    {
        print("Climbing Ladder");
        _isClimbing = true;
        _animator.SetBool("usingLadder", true);
        _animator.SetFloat("zSpeed", 0);
    }

    public void ClimbingLedgeDone()
    { 
        _isHanging = false;

        if(_activeLedge != null)
            gameObject.transform.position = _activeLedge.GetStandingPosition();

        _animator.SetBool("canLedgeGrab", false);
        _controller.enabled = true;
    }

    public void ClimbingLadderDone()
    {
        _isClimbing = false;

        gameObject.transform.position = _activeLadder.GetStandingPosition();

        _animator.SetBool("usingLadder", false);
        _animator.SetFloat("ySpeed", 0);
        StartCoroutine(EnableControllerRoutine());
    }

    public void SetClimbingOffLadderTrigger(LadderChecker currentLadder)
    {
        _activeLadder = currentLadder;
        _animator.SetTrigger("climbLadderTop");
        _controller.enabled = false;
    }

    public void AddCoin()
    {
        _coins += 1;
    }

    public int GetCoins() => _coins;
    public bool IsClimbing() => _isClimbing;

    IEnumerator EnableControllerRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        _controller.enabled = true;
    }

    IEnumerator SprintRollRoutine()
    {
        if (_velocity != Vector3.zero)
        {
            _isRolling = true;
            _animator.SetBool("isRolling", true);

            _speed += _rollSpeed;

            yield return new WaitForSeconds(_rollTime);

            _speed -= _rollSpeed;
            _isRolling = false;
            _animator.SetBool("isRolling", false);
        }
    }
}
