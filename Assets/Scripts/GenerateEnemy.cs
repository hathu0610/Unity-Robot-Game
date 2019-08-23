using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemy : MonoBehaviour
{

    public GameObject theEnemy;
    int xPos;
    int zPos;
    int xPos1;
    int zPos1;
    int xPos2;
    int zPos2;
    public int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
    
                StartCoroutine(EnemyDrop());

        
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            xPos = Random.Range(4, 20);
            zPos = Random.Range(1, 13);
            Instantiate(theEnemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
            enemyCount += 1;

            yield return new WaitForSeconds(5f);
        }
    }

}
