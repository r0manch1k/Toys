using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent( typeof( Rigidbody ) )]
public class WagonPlayer : Wagon
{
	private WagonModule _nearbyWagon;
	private WagonModule _tailWagon;
	private InputAction _interactAction;

	private void Awake()
	{
		_interactAction = InputSystem.actions.FindAction( "Player/Interact" );
		_interactAction.performed += OnInteract;
	}

	private void OnEnable()
	{
		_interactAction?.Enable();
	}

	private void OnDisable()
	{
		_interactAction?.Disable();
	}

	private void OnDestroy()
	{
		_interactAction.performed -= OnInteract;
	}

	public void SetNearbyWagon(WagonModule wagon)
	{
		_nearbyWagon = wagon;

		Debug.Log( "Nearby wagon set" );
	}

	public void ClearNearbyWagon(WagonModule wagon)
	{
		if ( _nearbyWagon == wagon )
			_nearbyWagon = null;

		Debug.Log( "Nearby wagon cleared" );
	}

	private void OnInteract(InputAction.CallbackContext ctx)
	{
		if ( _nearbyWagon == null || _nearbyWagon.IsAttached ) return;

		Wagon tailWagon = _tailWagon != null ? _tailWagon : this;

		_nearbyWagon.AttachTo( tailWagon );

		_tailWagon = _nearbyWagon;
		_nearbyWagon = null;
	}
}
