using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class GamePlayerControl : MonoBehaviour
{
    public Sprite h00, h10, h20, h30, h40, h50, h60, h70, h80;
    public GameObject phealthdisplay;
    public GameObject laserdisplay;
    public GameObject coindisplay;
    public GameObject enemydisplay;
    public static Text cointext;
    public static float LASER = 10f;
    public static float PHEALTH = 80f;
    public static int NUMOFENEMY;
    public static float COIN;

    [SerializeField]
    GameObject GameoverPanel;

    [SerializeField]
    GameObject CutSceneBonusBoost;

    // Start is called before the first frame update
    GameObject mainCam;
    GameObject Door;
    void Start()
    {
        cointext = coindisplay.GetComponent<Text>();
        mainCam = GameObject.FindWithTag("MainCamera");
        Door = GameObject.FindWithTag("Door");
    }

    // Update is called once per frame
    void Update()
    {


        TrackHealth();
        if (LASER > 70f)
        {
            laserdisplay.GetComponent<Image>().sprite = h80;
            ThirdPersonInput.laseron = true;

            return;
        }
        else if (LASER > 60f)
        {
            laserdisplay.GetComponent<Image>().sprite = h70;
            ThirdPersonInput.laseron = true;

            return;

        }
        else if (LASER > 50f)
        {
            laserdisplay.GetComponent<Image>().sprite = h60;
            ThirdPersonInput.laseron = true;

            return;

        }
        else if (LASER > 40f)
        {
            laserdisplay.GetComponent<Image>().sprite = h50;
            ThirdPersonInput.laseron = true;

            return;
        }
        else if (LASER > 30f)
        {
            laserdisplay.GetComponent<Image>().sprite = h40;
            ThirdPersonInput.laseron = true;

            return;
        }
        else if (LASER > 20f)
        {
            laserdisplay.GetComponent<Image>().sprite = h30;
            ThirdPersonInput.laseron = true;

            return;
        }
        else if (LASER > 10f)
        {
            laserdisplay.GetComponent<Image>().sprite = h20;
            ThirdPersonInput.laseron = true;

            return;
        }
        else if (LASER > 5f)
        {
            laserdisplay.GetComponent<Image>().sprite = h10;
            ThirdPersonInput.laseron = true;
            return;
        }
        else if (LASER <= 0f)
        {
            laserdisplay.GetComponent<Image>().sprite = h00;
            ThirdPersonInput.laseron = false;
//            print("laseronfalse");
            return;
        }
        //enemy's health


        //display health


}

    void OnCollisionEnter(Collision collision)
    {
        /*if (hit.gameObject.tag == "Door")
        {
            Animator anim;
            anim = hit.gameObject.GetComponent<Animator>(); 
            anim.SetBool("Door", true);
        }*/
        if (collision.gameObject.tag == "Laser")
        {
            //destro the amo box
            Destroy(collision.gameObject);
            //add amo to inventory
            LASER += 5f;
        }

        if (collision.gameObject.tag == "Coin")
        {
            //destro the amo box
            Destroy(collision.gameObject);
            //add amo to inventory
            COIN += 20f;
            cointext.text = COIN.ToString();
        }
        if (collision.gameObject.tag == "BoostUp")
        {
            CutSceneBonusBoost.SetActive(true);
            mainCam.SetActive(false);
            Destroy(collision.gameObject);
            StartCoroutine(wait2s());
        }

    }
    IEnumerator wait2s()
    {
        yield return new WaitForSeconds(4f);
        Destroy(Door);
    }
    void TrackHealth()
    {
        if (PHEALTH > 70f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h80;

            return;
        }
        else if (PHEALTH > 60f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h70;
            return;
        }
        else if (PHEALTH > 50f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h60;
            return;
        }
        else if (PHEALTH > 40f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h50;
            return;
        }
        else if (PHEALTH > 30f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h40;
            return;
        }
        else if (PHEALTH > 20f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h30;
            return;
        }
        else if (PHEALTH > 10f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h20;
            return;
        }
        else if (PHEALTH > 5f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h10;
            return;
        }
        else if (PHEALTH <= 0f)
        {
            phealthdisplay.GetComponent<Image>().sprite = h00;
            GameoverPanel.SetActive(true);
            Time.timeScale = 0;
            return;
        }
    }
}
