using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent( typeof( Rigidbody ) )]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private float acceleration = 15f;
	[SerializeField] private float maxSpeed = 15f;
	[SerializeField] private float turnSpeed = 100f;
	[SerializeField] private float centerOfMassY = -0.5f;
	[SerializeField] private float groundCheckDistance = 0.6f;
	[SerializeField] private LayerMask groundLayer = ~0;

	private Rigidbody _rb;
	private InputAction _moveAction;
	private bool _isGrounded;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_rb.centerOfMass = new Vector3( 0f, centerOfMassY, 0f );
		_moveAction = InputSystem.actions.FindAction( "Player/Move" );
	}

	private void FixedUpdate()
	{
		_isGrounded = Physics.Raycast( transform.position, Vector3.down, groundCheckDistance, groundLayer );

		Vector2 input = _moveAction.ReadValue<Vector2>();

		if ( _isGrounded )
		{
			_rb.AddForce( transform.forward * input.y * acceleration, ForceMode.Acceleration );

			float forwardSpeed = Vector3.Dot( _rb.linearVelocity, transform.forward );
			if ( Mathf.Abs( forwardSpeed ) > 0.5f )
			{
				float speedFactor = Mathf.Clamp01( Mathf.Abs( forwardSpeed ) / maxSpeed );
				float turn = input.x * turnSpeed * speedFactor * Time.fixedDeltaTime * Mathf.Sign( forwardSpeed );
				_rb.MoveRotation( _rb.rotation * Quaternion.Euler( 0f, turn, 0f ) );
			}
		}

		Vector3 flatForward = new Vector3( transform.forward.x, 0f, transform.forward.z ).normalized;
		Vector3 forwardVel = Vector3.Dot( _rb.linearVelocity, flatForward ) * flatForward;
		_rb.linearVelocity = new Vector3( forwardVel.x, _rb.linearVelocity.y, forwardVel.z );

		if ( forwardVel.magnitude > maxSpeed )
		{
			forwardVel = forwardVel.normalized * maxSpeed;
			_rb.linearVelocity = new Vector3( forwardVel.x, _rb.linearVelocity.y, forwardVel.z );
		}
	}
}
