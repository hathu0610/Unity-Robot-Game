using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonInput : MonoBehaviour
{
    public Camera cam1;
    public FixedButton LaserButton;
    public FixedButton JumpButton;
    public FixedButton HealthButton;
    public FixedJoystick LeftJoystick;
    private Animator anim;
    protected ThirdPersonUserControl Control;
    public FixedTouchField TouchField;
    protected float CameraAngleX;
    protected float CameraAngleY;
    protected float CameraAngleSpeed = 0.2f;
    public float lookSmoother = 3f;
    [System.NonSerialized]
    public float lookWeight;                    // the amount to transition when using head look
    public static bool laseron = false;
    [System.NonSerialized]
    public Transform enemy;
    private bool pressedlaserbutton = false;
    public static bool pressedhealthbutton = false;
    public static bool pressedexitbutton = false;
    // Start is called before the first frame update
    void Start()
    {
       Control = GetComponent<ThirdPersonUserControl>();
        anim = GetComponent<Animator>();
     //   enemy = GameObject.Find("Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Control.m_Jump = JumpButton.Pressed;
        pressedlaserbutton = LaserButton.Pressed;
        pressedhealthbutton = HealthButton.Pressed;
        Control.Hinput = LeftJoystick.Horizontal;
        Control.Vinput = LeftJoystick.Vertical;
        CameraAngleY += TouchField.TouchDist.y * CameraAngleSpeed;
        CameraAngleX += TouchField.TouchDist.x * CameraAngleSpeed;
        cam1.transform.position = transform.position + Quaternion.AngleAxis(CameraAngleX, Vector3.up) * new Vector3(0, 3, 4) + Quaternion.AngleAxis(CameraAngleY, Vector3.right)* new Vector3(1, 0, 1)  ;
        cam1.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up *2f  - cam1.transform.position, Vector3.up);

    }
    /*void FixedUpdate()
    {
        if (pressedlaserbutton == true && GamePlayerControl.LASER > 0)
        {
            // ...set a position to look at with the head, and use Lerp to smooth the look weight from animation to IK (see line 54)
            // anim.SetLookAtPosition(enemy.position);
            if (laseron == true)
            {

                StartCoroutine(lookfor2seconds());

            }


        }
        // else, return to using animation for the head by lerping back to 0 for look at weight
        else if (pressedlaserbutton == true && GamePlayerControl.LASER <= 0f || laseron == false)
        {
            lookWeight = Mathf.Lerp(lookWeight, 0f, Time.deltaTime * lookSmoother);
        }
    }*/

  /*  private void OnAnimatorIK()
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
        yield return new WaitForSeconds(1.5f);
        if (GamePlayerControl.LASER > 0f)
        {
            GamePlayerControl.LASER -= 0.5f;
            GamePlayerControl.ENEMYHEALTH -= 0.5f;
        }
    }*/
}
