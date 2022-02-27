using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
// video 19, time to tweak
// correcting way point = 27, send messgae (koi fayda ni way point correct krne ka)
// enemy script, parent
// version control 38

// **************************************************IDEA**********ROLLER COASTER***************************************************** 

// tip: NO 10 SEC SHOULD BE THE SAME
public class PlayerController : MonoBehaviour
{
    float xThrow, yThrow;
    [Header("General")]
    [Tooltip("In m/s")] [SerializeField] float xSpeed = 20f;
    [Tooltip("In m")] [SerializeField] float xRange = 5f;
    [Tooltip("In m")] [SerializeField] float yRange = 3f;
    [SerializeField] GameObject[] guns;


    [Header("Screen Position and Controll")]
    [SerializeField] float positonPitchFactor = -5f;
    [SerializeField] float controlPitchFactor = -20f;

    [SerializeField] float positonYawFactor = 5f;

    [SerializeField] float controlRollFactor = -30f;

    bool isControlEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    void OnPlayerDeath() // called by string method in controller script
    {
        Debug.Log("Received");
        isControlEnabled = false;
    }

    private void ProcessRotation()
    {
        float pitch = (transform.localPosition.y * positonPitchFactor) + (yThrow * controlPitchFactor);
        float yaw = transform.localPosition.x * positonYawFactor;
        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll); // in other sense they are just x,y,z
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * xSpeed * Time.deltaTime;
        float yOffset = yThrow * xSpeed * Time.deltaTime;

        float rawXPos = (transform.localPosition.x + xOffset);
        float rawYPos = (transform.localPosition.y + yOffset);

        float clampXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float clampYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampXPos, clampYPos, transform.localPosition.z);


        /*float xyy = Input.GetAxis("Horizontal");
        float xOffset = xyy * xSpeed * Time.deltaTime;
        transform.Translate(Vector3.right * Time.deltaTime * xSpeed * xyy);

        if((transform.localPosition.x+xOffset) < -5f)
        {
            transform.localPosition = new Vector3(-5, (transform.localPosition.y), (transform.localPosition.z));
        }*/
    }

    private void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
        }
        else
        {
            SetGunsActive(true);
        }
    }

    void SetGunsActive(bool isActive)
    {
        foreach(GameObject gun in guns)
        {
            var emmisionModule = gun.GetComponent<ParticleSystem>().emission;
            emmisionModule.enabled = isActive;
        }

    }
}
