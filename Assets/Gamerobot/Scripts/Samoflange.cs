using UnityEngine;
using System.Collections;

public class Samoflange : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f;
    public Transform grenadePrefab;
    public Transform explosion;
    [SerializeField]
    private Transform target;
    float range = 20.0f;
    Transform clonegrenadeexplode;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {
        InvokeRepeating("Attack", 5.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //  transform.Rotate(new Vector3(0, 30 * Time.deltaTime, 0));
        //transform.LookAt(target);
      
        {
            Quaternion targetRotate = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotate, Time.deltaTime * 1.2f);

            Vector3 forward = transform.forward;
            Vector3 targetDir = target.position - transform.position;
            float angle = Vector3.Angle(targetDir, forward);
            /*  if (angle < 10.0)
              {
                  StartCoroutine(SubstractHealth());
                  Player.Health -= 0.2f;

              }
              else
              {

              }*/
        }
    }

    IEnumerator SubstractHealth()
    {
        yield return new WaitForSeconds(20);


    }

    bool CanAttackTarget()
    {
        if (Vector3.Distance(transform.position, target.position) > range)
        {
           // print("out of range");
//            Disengage();
            return false;
        }
        RaycastHit hit;

        if (Physics.Linecast(transform.position, target.position, out hit))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
            //    print("item in the way" + hit.collider.gameObject.name);
              //  Disengage();
            }
            else
            {
              //  print("player detected");
                return true;
            }
        }
        //Disengage();
        return false;

    }
    void Attack()
    {
        Transform newgrenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
        newgrenade.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
       StartCoroutine(GrenadeExplode(newgrenade));
       
    }
    IEnumerator GrenadeExplode(Transform newgrenade)
    {
        yield return new WaitForSeconds(2.0f);
        //print("explode");
            clonegrenadeexplode = Instantiate(explosion, newgrenade.position, Quaternion.identity);
        Destroy(newgrenade.gameObject);
         

    }
}
