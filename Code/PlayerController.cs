using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls the logic of the player character.
public class PlayerController : MonoBehaviour
{
    // Global identifiers - many are public despite not needing to be, this is so they can be viewed in the inspector.

    // Movement and vector related variables.
    public float speed;
    public float speedLimit;
    public float speedInc;
    public float speedDelta;
    public float tempRightVecX;
    public float tempRightVecY;
    public float gravConst;
    public float gravY;
    public float entryAngleLenience;
    public float seaLevel;
    public float sandLevel;
    public Vector3 added;
    public float smoothRightVecX;
    public float smoothRightVecY;

    // GameObjects and scripts.
    public GameObject tail;
    public TailController tailController;
    public GameObject splash1, splash2;
    public SplashController splashController1, splashController2;
    public GameObject scoreDetails;
    public TrickTrackController trickTrackController;

    public int difficulty;

    // Timers.
    public int enterTimer;
    public int inEnterTimer;
    public int enterTimerSet1;
    public int enterTimerSet2;
    public int timer;
    public int maxTimer;
    public int scoreResetTimer;

    // Input and rotation variables.
    public float degrees;
    public int degInc;
    public float exitDegrees;
    public float degDifference;
    public bool leftTouched;
    public bool rightTouched;

    // Track if orca is above sea level
    public bool isAbove;
    // Track recent waveCollision
    public bool waveCollision;
    // True once game is played - used to play audio scource on start
    public bool hasBeenPlayed;
    public bool stop;

    // Holds planets and Moon.
    public bool[] spaceObjects;

    // Animation related identifiers.
    public Animator playerAnim;
    public float playerAnimSpeed;

    // Audio identifiers.
    public AudioSource splash_source;
    public AudioSource outside_splash_source;
    public AudioSource underwater_ambience;
    public AudioSource outside_ambience;
    public AudioSource space_ambience;

    // Start contains code initializing the script variables, and is called before the first frame
    void Start()
    {
        // Default speed is 10. speedDelta is 0 to ensure start speed is 0
        speed = 10;
        speedDelta = 0;
        speedLimit = 146;

        PlayerPrefs.SetInt("Score", 0);

        // Initialize tail with reference to Tail GameObject, then get components with this reference
        tail = GameObject.Find("Tail");
        tailController = tail.GetComponent<TailController>();
        playerAnim = tail.GetComponent<Animator>();

        // Get SplashController components of both splash objects, to use their methods to set splash positions
        splash1 = GameObject.Find("Splash1");
        splash2 = GameObject.Find("Splash2");
        splashController1 = splash1.GetComponent<SplashController>();
        splashController2 = splash2.GetComponent<SplashController>();

        // Again, get object reference followed by object component
        scoreDetails = GameObject.Find("ScoreDetails");
        trickTrackController = scoreDetails.GetComponent<TrickTrackController>();

        degrees = 0;
        degInc = 6;

        // Set acceleration and lenience based on difficulty
        difficulty = PlayerPrefs.GetInt("Difficulty");
        switch (difficulty)
        {
            case 0:
                speedInc = 12;
                entryAngleLenience = 15;
                PlayerPrefs.SetFloat("end_multiplier", 1);
                break;
            case 1:
                speedInc = 10;
                entryAngleLenience = 35;
                break;
            case 2:
                speedInc = 8;
                entryAngleLenience = 25;
                break;
            case 3:
                speedInc = 6;
                entryAngleLenience = 15;
                break;
            case 4:
                speedInc = 5;
                entryAngleLenience = 10;
                break;
        }

        spaceObjects = new bool[10];

        // Values for sea and sand boundary, and gravity
        gravConst = 0.3f;
        seaLevel = 20.5f;
        sandLevel = -44f;

        // Timer set to 25 so acceleration occurs at beginning
        timer = 25;
        maxTimer = 75;
        enterTimerSet1 = 5;
        enterTimerSet2 = 10;

        leftTouched = false;
        rightTouched = false;

        // Set all audio source variables to AudioSource components of respective GameObjects
        splash_source = GameObject.Find("Inside_Splash").GetComponent<AudioSource>();
        outside_splash_source = GameObject.Find("Outside_Splash").GetComponent<AudioSource>();
        underwater_ambience = GameObject.Find("Underwater_Ambience").GetComponent<AudioSource>();
        outside_ambience = GameObject.Find("Outside_Ambience").GetComponent<AudioSource>();
        space_ambience = GameObject.Find("Space_Ambience").GetComponent<AudioSource>();

        outside_ambience.Play();
        outside_ambience.volume = 0;
    }

    // Called once per frame, input is controlled here.
    void Update()
    {
        // Start playing underwater ambience sounds initially
        if (!hasBeenPlayed)
        {
            hasBeenPlayed = true;
            underwater_ambience.Play();
        }
        // User input handled in Update as FixedUpdate would result in input loss
        HandleKeys();
        HandleTouch();
    }

    // Called once per physics frame - movement handling function called here to ensure orca speed doesn't change based on framerate
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Limit the framerate to 60 for power consumption and to ensure the speed consistency across devices
    // vSync must also be disabled for limit to be set
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Handle key input.
    void HandleKeys()
    {
        if (Input.GetKey("a") && Input.GetKey("d"))
            return;

        // Check inputs, a for left d for right
        if (Input.GetKey("a"))
        {
            // Increment degrees by incremental constant, set degrees to 0 on reaching 360
            // This is to ensure exit degrees are recorded correctly
            // If degrees aren't reset to 0, they simply keep increasing / decreasing based on which direction the player rotates
            degrees += degInc;
            degrees %= 360;
            tailController.LeftTurn();
            playerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("eccojr-cclock_0");
        }

        if (Input.GetKey("d"))
        {
            degrees -= degInc;
            degrees %= 360;
            playerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("eccojr-clock_0");
            if (degrees == 0)
            {
                degrees = 360;
            }
            tailController.RightTurn();
        }

        // If key released, play forward animation.
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            playerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("eccojr_forward_0");
            playerAnim.Play(0, -1, 0.5f);
        }

    }

    // Handles touch input.
    void HandleTouch()
    {
        int i = 0;
        if (i < Input.touchCount)
        {
            // If touch detected on left side of screen, turn left, if touch detected on right side of screen, turn right.
            if (Input.GetTouch(i).position.x < Screen.width / 2)
            {
                degrees += degInc;
                degrees %= 360;
                tailController.LeftTurn();
                if (!leftTouched)
                {
                    playerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("eccojr-cclock_0");
                }
                leftTouched = true;
            }
            if (Input.GetTouch(i).position.x >= Screen.width / 2)
            {
                degrees -= degInc;
                degrees %= 360;
                if (degrees <= 0)
                {
                    degrees = 360;
                }
                tailController.RightTurn();

                if (!rightTouched)
                {
                    playerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("eccojr-clock_0");
                }
                rightTouched = true;
            }
            i++;
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("eccojr_forward_0");
            playerAnim.Play(0, -1, 0.5f);
            //touched = false;
            leftTouched = false;
            rightTouched = false;
        }

    }

    // Handles movement of player.
    void MovePlayer()
    {
        // If this orca is above sea, and isAbove is false, this must be the orca's first frame above the water.
        if ((transform.position.y > seaLevel) && (!isAbove))
        {
            splashController2.SetPos(transform.position.x);
            splashController2.Active();
            
            playerAnim.speed = 0;
            playerAnim.Play(0, -1, 0.5f);

            speedDelta = 0;
            gravY = transform.right.y * speed;
            isAbove = true;
            outside_splash_source.PlayOneShot(outside_splash_source.clip, 1f);
            underwater_ambience.volume = 0;
            outside_ambience.volume = 0.85f;

            exitDegrees = degrees;
            waveCollision = false;
            timer = 0;
            
            transform.position += new Vector3(transform.right.x * speed * Time.deltaTime, gravY * Time.deltaTime);
        }
        // If orca is above sea, and has been for at least 1 frame, constantly reduce y velocity (applying gravity).
        else if ((transform.position.y > seaLevel))
        {
            gravY -= gravConst;
            transform.position += new Vector3(transform.right.x * speed * Time.deltaTime, gravY * Time.deltaTime);
            tempRightVecX = transform.right.x;
            tempRightVecY = gravY / speed;
            // Following if statements check the y position of the player, and change music accordingly
            if (transform.position.y >= 70)
            {
                outside_ambience.volume = (110 - transform.position.y) / 50;
            }

            if (transform.position.y > 110)
            {
                if (!space_ambience.isPlaying)
                {
                    space_ambience.Play();
                } 

                space_ambience.volume = (transform.position.y - 110) / 30;
            }

            // Following if statements check if y position of player is above a certain point, then if planet reaching trick associated with that point has been added.
            // If not, the trick is added.
            if ((transform.position.y > 150) && !spaceObjects[0])
            {
                trickTrackController.AddElement("Moon Man!", 10000);
                spaceObjects[0] = true;
            }

            if ((transform.position.y > 300) && !spaceObjects[1])
            {
                trickTrackController.AddElement("Musk!", 20000);
                spaceObjects[1] = true;
            }

            if ((transform.position.y > 450) && !spaceObjects[2])
            {
                trickTrackController.AddElement("Red Eye!", 50000);
                spaceObjects[2] = true;
            }

            if ((transform.position.y > 600) && !spaceObjects[3])
            {
                trickTrackController.AddElement("The Ring!", 75000);
                spaceObjects[3] = true;
            }

            if ((transform.position.y > 720) && !spaceObjects[4])
            {
                trickTrackController.AddElement("The Bounty!", 150000);
                spaceObjects[4] = true;
            }

        }
        // If entrance to water occured this frame.
        else if ((transform.position.y <= seaLevel) && (isAbove))
        {
            // Indicate orca is below sea level
            isAbove = false;
            
            // Underwater ambience should play below the water, set volumes accordingly
            outside_ambience.volume = 0;
            underwater_ambience.volume = 1;
            
            degDifference = 360 - degrees;

            playerAnim.speed = playerAnimSpeed;

            splashController1.SetPos(transform.position.x);
            splashController1.Active();
            splash_source.PlayOneShot(splash_source.clip, 1f);

            spaceObjects = new bool[10];

            // If incorrect entrance angle, reset speed, cancel current tricks, reset speed, and set timer
            if ((exitDegrees > (degDifference + entryAngleLenience)) || (exitDegrees < (degDifference - entryAngleLenience)) || ((degrees > 0) && (degrees < 180)))
            {
                timer = 75;
                speed = 10;
                speedDelta = speed;
                trickTrackController.ResetListSplash();
                waveCollision = true;
                transform.position += new Vector3(tempRightVecX * speed * Time.deltaTime, tempRightVecY * speed * Time.deltaTime);
            }
            // If correct angle, increase speed and enact score increase
            else
            {
                speed += speedInc;
                trickTrackController.AddElement("Smooth entrance!", 500);
                scoreResetTimer = 70;
                if (speed > speedLimit)
                    speed = speedLimit;

                if (speed > 50)
                {
                    enterTimer = enterTimerSet1; inEnterTimer = enterTimer;
                }
                else
                {
                    enterTimer = enterTimerSet2; inEnterTimer = enterTimer; 
                }

                transform.position += new Vector3(tempRightVecX * speed * Time.deltaTime, tempRightVecY * speed * Time.deltaTime);
            }

        }
        // If the orca's current position indicates it is in water, between the sea and the sand
        else if ((sandLevel < transform.position.y) && (transform.position.y <= seaLevel))
        {
            // Ensure orca stops if timer has reach 0 and game is about to end
            if (stop)
            {
                playerAnim.speed = 0;
                // Add score from current tricks if smooth entrance was made but timer for score addition not yet zero (scoreResetTimer only set in case of smooth entrance)
                if (scoreResetTimer > 0) 
                {
                    trickTrackController.ResetListSmooth();
                }
                return;
            }

            // If timer is above 25, the orca is slowed.
            if (timer > 25)
            {
                speedDelta -= speed / 50;
                playerAnimSpeed = (float)(maxTimer - timer) / (float)maxTimer;
                playerAnim.speed = playerAnimSpeed;

                timer--;
                if (timer == 25)
                {
                    speedDelta = 0;
                }

                // Increment position with entrance vector components and decreasing speed if incorrect angle entrance (wave collision) has been made
                if (waveCollision)
                {
                    transform.position += new Vector3(tempRightVecX * speedDelta * Time.deltaTime, tempRightVecY * speedDelta * Time.deltaTime);
                }
                else
                {
                    transform.position += added * speedDelta * Time.deltaTime;
                }
            }
            // If timer is equal to or below 25, the orca is speed back up.
            else if (timer > 0)
            { 
                transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
                playerAnimSpeed = (float)(maxTimer - timer) / (float)maxTimer;
                playerAnim.speed = playerAnimSpeed;
                speed = 10;
                speedDelta += speed / 26;
                timer--;
                transform.position += new Vector3(transform.right.x * speedDelta * Time.deltaTime, transform.right.y * speedDelta * Time.deltaTime);
                if (timer == 0)
                {
                    waveCollision = false;
                }
            }
            // If enterTimer is above 0, the orca has recently entered and its movement vector must be changed slowly.
            else if (enterTimer > 0)
            {
                // Rotate as normal.
                transform.rotation = Quaternion.Euler(Vector3.forward * degrees);

                // x and y components of to hold incremental change to orca direction based on enterTimer progression.
                smoothRightVecX = ((tempRightVecX * (float)enterTimer) + (transform.right.x) * ((float)inEnterTimer - (float)enterTimer)) / (float)inEnterTimer;
                smoothRightVecY = ((tempRightVecY * (float)enterTimer) + (transform.right.y) * ((float)inEnterTimer - (float)enterTimer)) / (float)inEnterTimer;
                transform.position += new Vector3(smoothRightVecX * speed * Time.deltaTime, smoothRightVecY * speed * Time.deltaTime);

                // If the orca is in the sand, it should be moved above the sand, then continue as if it had collided with the sand.
                if (transform.position.y <= sandLevel)
                {
                    SandCollision();
                }

                //
                exitDegrees = degrees;
                enterTimer--;

                // Timer to reset score should be decreased after newly entering sea - if 0, reset the trick list and add score
                if (scoreResetTimer > 0)
                {
                    scoreResetTimer--;
                }
                else
                {
                    // Only reset if trick has been added
                    if (trickTrackController.Exists())
                    {
                        trickTrackController.ResetListSmooth();
                    }
                }
            }
            // In water, no current timers, so move normally.
            else if (enterTimer == 0)
            {
                // Ensure rotation changes according to degree change that occurs due to player input.
                transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
                transform.position += transform.right * speed * Time.deltaTime;
                if (transform.position.y <= sandLevel)
                {
                    SandCollision();
                }

                // Timer to reset score should be decreased - if 0, reset the trick list and add score
                if (scoreResetTimer > 0)
                {
                    scoreResetTimer--;
                }
                else
                {
                    // Only reset if trick has been added
                    if (trickTrackController.Exists())
                    {
                        trickTrackController.ResetListSmooth();
                    }
                }
            }
        }
    }

    // Called when the object this script is attached to collides.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Resets trick list without increasing score.
        trickTrackController.ResetListSplash();
        // Vector storing reflection of the negative of current Orca direction, with respect to the x-axis.
        added = Vector3.Reflect(-transform.right, Vector3.right);
        // Add 'added' once for this frame.
        transform.position += added * speed * Time.deltaTime;
        // Reset speed & speedDelta, set timer up.
        speed = 10;
        speedDelta = speed;
        timer = 75;
    }

    // Manually called version of previous function, that also sets position of orca above sand.
    void SandCollision()
    {
        Vector3 tempVec = new Vector3(transform.position.x, -43.5f, transform.position.z);
        transform.position = tempVec;
        trickTrackController.ResetListSplash();
        added = Vector3.Reflect(-transform.right, Vector3.right);
        transform.position += added * speed * Time.deltaTime;
        speed = 10;
        speedDelta = speed;
        timer = 75;
    }

    // Called when game about to stop.
    public void StopCounting()
    {
        stop = true;
    }
}