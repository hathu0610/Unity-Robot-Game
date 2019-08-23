using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine;

public class AdController : MonoBehaviour
{
    private string store_id = "3246245";
    private string video_ad = "video";
    private string rewarded_video_ad = "rewardedVideo";
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(store_id, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(ThirdPersonInput.pressedhealthbutton == true)
        {
            Advertisement.Show();
            GamePlayerControl.PHEALTH += 30f;
         //   Time.timeScale = 0;
            StartCoroutine(waitfor2s());

        }

    }
     
    IEnumerator waitfor2s()
    {
        yield return new WaitForSeconds(2f);
       
    }

}
