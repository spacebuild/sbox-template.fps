namespace GameTemplate.Mechanics;

/// <summary>
/// AirMove decides how the player moves while in the air. Drives effects such as gravity, wind, etc.
/// </summary>
public partial class AirMoveMechanic : PlayerControllerMechanic
{
	public float AirControl => 30.0f;
	public float AirAcceleration => 35.0f;

	protected override void Simulate()
	{
		var ctrl = Controller;
		ctrl.Velocity += Player.Gravity * 0.5f * Time.Delta;
		ctrl.Velocity += new Vector3( 0, 0, ctrl.BaseVelocity.z ) * Time.Delta;
		ctrl.BaseVelocity = ctrl.BaseVelocity.WithZ( 0 );

		var groundedAtStart = GroundEntity.IsValid();
		if ( groundedAtStart ) 
			return;

		var wishVel = ctrl.GetWishVelocity( true );
		var wishdir = ctrl.MoveDir;
		var wishspeed = wishVel.Length;

		ctrl.Velocity += ctrl.Accelerate( wishdir, wishspeed, AirControl, AirAcceleration );
		ctrl.Velocity += ctrl.BaseVelocity;
		ctrl.Move();
		ctrl.Velocity -= ctrl.BaseVelocity;
		ctrl.Velocity += Player.Gravity * 0.5f * Time.Delta;
	}

	// AirMove should always simulate
	protected override bool ShouldStart() => true;
}
