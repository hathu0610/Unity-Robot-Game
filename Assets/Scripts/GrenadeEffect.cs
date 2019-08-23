using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEffect : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    float range = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        CheckIfPlayerInRange();
    }
    // Update is called once per frame
    void Update()
    {
      
    }
    void CheckIfPlayerInRange()
    {
        if (Vector3.Distance(transform.position, target.position) > range)
        {
            print("no health deducted");
        }
        RaycastHit hit;

        if (Physics.Linecast(transform.position, target.position, out hit))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                print("item in the way of health" + hit.collider.gameObject.name);
                //  Disengage();
            }
            else
            {
                print("player detected to deduct health");
                GamePlayerControl.PHEALTH -= 10f;
              // print(GamePlayerControl.HEALTH);
            }
        }

    }
}
