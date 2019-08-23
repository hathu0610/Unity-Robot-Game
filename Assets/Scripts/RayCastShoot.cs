using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastShoot : MonoBehaviour
{
    public FixedButton LaserButton;
    public int gunDamage = 1;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    private bool pressedlaserbutton = false;
    public Camera cam;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    private float nextFire;
    private int enemyHealth = 3;
    Animator anim;
    public Text enemytext;
    // Start is called before the first frame update
    void Start()
    {
    
        laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {//&& GamePlayerControl.LASER > 0 && Time.time > nextFire
        pressedlaserbutton = LaserButton.Pressed;
        if (pressedlaserbutton == true && GamePlayerControl.LASER > 0 && Time.time > nextFire)
        {

            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            laserLine.SetPosition(0, gunEnd.position);

            if(Physics.Raycast(rayOrigin, cam.transform.forward,out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);
                anim = hit.collider.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("Take Damage");
                    enemyHealth -= 1;
                    if (enemyHealth == 0)
                    {
                        anim.SetTrigger("Die");
                        StartCoroutine(waitfor2(hit));

                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (cam.transform.forward * weaponRange));
            }

        }
    }
    private IEnumerator waitfor2(RaycastHit hit)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(hit.transform.gameObject);
        GamePlayerControl.NUMOFENEMY += 1;
        enemytext.text = GamePlayerControl.NUMOFENEMY.ToString();
        enemyHealth = 3;
    }
    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
