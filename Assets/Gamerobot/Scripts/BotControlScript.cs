using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]
public class BotControlScript : MonoBehaviour
{
    public Joystick leftjoystick;
    public Joystick rightjoystick;
    protected float CameraAngle;
    protected float CameraAngleSpeed = 2f;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    public float VInput;
    public float HInput;
	[System.NonSerialized]					
	public float lookWeight;					// the amount to transition when using head look
	
	[System.NonSerialized]
	public Transform enemy;						// a transform to Lerp the camera to during head look
	
	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
	public float lookSmoother = 3f;				// a smoothing setting for camera motion
	public bool useCurves;						// a setting for teaching purposes to show use of curves

	
	private Animator anim;							// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	private AnimatorStateInfo layer2CurrentState;	// a reference to the current state of the animator, used for layer 2
	private CapsuleCollider col;                    // a reference to the capsule collider of the character
   public static bool laseron = false;
   
    Transform head;
    static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int jumpState = Animator.StringToHash("Base Layer.Jump");				// and are used to check state for various actions to occur
	static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");		// within our FixedUpdate() function below
	static int fallState = Animator.StringToHash("Base Layer.Fall");
	static int rollState = Animator.StringToHash("Base Layer.Roll");
	static int waveState = Animator.StringToHash("Layer2.Wave");
	

	void Start ()
	{
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        // joystick = FindObjectOfType<Joystick>();
        // initialising reference variables
        anim = GetComponent<Animator>();					  
		col = GetComponent<CapsuleCollider>();				
		enemy = GameObject.Find("Enemy").transform;	
		if(anim.layerCount ==2)
			anim.SetLayerWeight(1, 1);
        head = anim.GetBoneTransform(HumanBodyBones.Head);
    }
   void Update()
    {

        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 5;
        Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, (forward), out hit))
        {
            if (hit.distance < 8f && hit.collider.gameObject.tag == "Door")
            {
                Animator anim1;
                anim1 = hit.collider.GetComponent<Animator>();
                anim1.SetBool("Door", true);
            }
        }
    }

    void FixedUpdate ()
	{
        CameraAngle += rightjoystick.Horizontal * CameraAngleSpeed;
        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(0, 3, 4);
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
        //m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        float h = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
        	float v = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
       // float h = leftjoystick.Horizontal;
        //float v = leftjoystick.Vertical;
       
        anim.SetFloat("Speed", v);							// set our animator's float parameter 'Speed' equal to the vertical input axis				
		anim.SetFloat("Direction", h); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
		anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
		//anim.SetLookAtWeight(lookWeight);					// set the Look At Weight - amount to use look at IK vs using the head's animation
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation
		
		if(anim.layerCount ==2)		
			layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);   // set our layer2CurrentState variable to the current state of the second Layer (1) of animation

        // LOOK AT ENEMY
       
        // if we hold Alt..
        if (Input.GetButton("Fire2") && GamePlayerControl.LASER > 0)
		{
            // ...set a position to look at with the head, and use Lerp to smooth the look weight from animation to IK (see line 54)
            // anim.SetLookAtPosition(enemy.position);
            if (laseron == true )
            {
                StartCoroutine(lookfor2seconds());
               
            }

        }
		// else, return to using animation for the head by lerping back to 0 for look at weight
		else if (Input.GetButton("Fire2") && GamePlayerControl.LASER <= 0f)
		{
			lookWeight = Mathf.Lerp(lookWeight,0f,Time.deltaTime*lookSmoother);
        
		}
		
		// STANDARD JUMPING
		
		// if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
		if (currentBaseState.nameHash == locoState)
		{
			if(Input.GetButtonDown("Jump"))
			{
				anim.SetBool("Jump", true);
			}
		}
		
		// if we are in the jumping state... 
		else if(currentBaseState.nameHash == jumpState)
		{
			//  ..and not still in transition..
			if(!anim.IsInTransition(0))
			{
				if(useCurves)
					// ..set the collider height to a float curve in the clip called ColliderHeight
					col.height = anim.GetFloat("ColliderHeight");
				
				// reset the Jump bool so we can jump again, and so that the state does not loop 
				anim.SetBool("Jump", false);
			}
			
			// Raycast down from the center of the character.. 
			Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
			RaycastHit hitInfo = new RaycastHit();
			
			if (Physics.Raycast(ray, out hitInfo))
			{
				// ..if distance to the ground is more than 1.75, use Match Target
				if (hitInfo.distance > 1.75f)
				{
					
					// MatchTarget allows us to take over animation and smoothly transition our character towards a location - the hit point from the ray.
					// Here we're telling the Root of the character to only be influenced on the Y axis (MatchTargetWeightMask) and only occur between 0.35 and 0.5
					// of the timeline of our animation clip
					anim.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
				}
			}
		}
		
		
		// JUMP DOWN AND ROLL 
		
		// if we are jumping down, set our Collider's Y position to the float curve from the animation clip - 
		// this is a slight lowering so that the collider hits the floor as the character extends his legs
		else if (currentBaseState.nameHash == jumpDownState)
		{
			col.center = new Vector3(0, anim.GetFloat("ColliderY"), 0);
		}
		
		// if we are falling, set our Grounded boolean to true when our character's root 
		// position is less that 0.6, this allows us to transition from fall into roll and run
		// we then set the Collider's Height equal to the float curve from the animation clip
		else if (currentBaseState.nameHash == fallState)
		{
			col.height = anim.GetFloat("ColliderHeight");
		}
		
		// if we are in the roll state and not in transition, set Collider Height to the float curve from the animation clip 
		// this ensures we are in a short spherical capsule height during the roll, so we can smash through the lower
		// boxes, and then extends the collider as we come out of the roll
		// we also moderate the Y position of the collider using another of these curves on line 128
		else if (currentBaseState.nameHash == rollState)
		{
			if(!anim.IsInTransition(0))
			{
				if(useCurves)
					col.height = anim.GetFloat("ColliderHeight");
				
				col.center = new Vector3(0, anim.GetFloat("ColliderY"), 0);
				
			}
		}
		// IDLE
		
		// check if we are at idle, if so, let us Wave!
		else if (currentBaseState.nameHash == idleState)
		{
			if(Input.GetButtonUp("Jump"))
			{
				anim.SetBool("Wave", true);
			}
		}
		// if we enter the waving state, reset the bool to let us wave again in future
		if(layer2CurrentState.nameHash == waveState)
		{
			anim.SetBool("Wave", false);
		}
	}
    private void OnAnimatorIK()
    {
        anim.SetLookAtWeight(lookWeight);
        if (enemy != null)
        {
            anim.SetLookAtPosition(enemy.position);
        }
    }
    IEnumerator lookfor2seconds()
    {
        lookWeight = Mathf.Lerp(lookWeight, 1f, Time.deltaTime * lookSmoother);
        print(lookWeight);
        yield return new WaitForSeconds(1.5f);
        if (GamePlayerControl.LASER > 0f)
        {
            GamePlayerControl.LASER -= 0.5f;
        }
    }
}
