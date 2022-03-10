using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private PlayerMovement pm;
    private PlayerData data;

    private int pid;

    public float transAmt;
    public float rotAmt;
    Vector3 currentPos;
    Vector3 targetPos;
    [SerializeField] private bool isDash = false;
    [SerializeField] public bool holdingItem = false;
    [SerializeField] private float setDashTime;
    [SerializeField] private float remainDashTime;

    AnimatorController anim;
    bool takePressed;
    bool takePressUp;
    float holdDownStartTime = 0f;
    [SerializeField] float pressedTime;
    [SerializeField] bool inTreeArea;

    //UI
    public GameObject throwSlider;
    public Slider slider;
    public bool showArrow = false;

    private void Awake()
    {
        anim = GetComponent<AnimatorController>();
        anim.animator = GetComponent<Animator>();
        anim.Init();
        pm = GetComponent<PlayerMovement>();
        data = GetComponent<PlayerData>();
        inTreeArea = data.inTree;
    }

    void Start()
    {
        setDashTime = pm.setDashTime;
        pid = data.pid;
    }

    public bool takePressDown;
    float takePressTimer;
    public float pressTimeSaver = 0.0f;

    void Update()
    {
        if (throwSlider == null)
        {
            throwSlider = GameObject.Find("ThrowSlider" + pid);
            if (throwSlider != null)
            {
                slider = throwSlider.GetComponent<Slider>();
                slider.value = 0f;

                throwSlider.transform.position = this.transform.position + this.transform.forward;
                throwSlider.transform.position += new Vector3(0, 0.5f, 0);
                throwSlider.transform.forward = this.transform.forward;
                throwSlider.transform.localRotation *= Quaternion.Euler(90, 0, 0);
            }
        }
        else
        {
            Debug.Log($"slider input {pid} value {slider.value}");
        }

        inTreeArea = data.inTree;

        string aniClip = "none";

        //rotAmt = Input.GetAxis("Horizontal" + pid);
        //transAmt = Input.GetAxis("Vertical" + pid);
        takePressed = Input.GetButtonDown("Take" + pid);
        takePressUp = Input.GetButtonUp("Take" + pid);
        //bool moved = (rotAmt != 0 || transAmt != 0);
        //Vector3 movementDirection = new Vector3(rotAmt, 0.0f, transAmt);
        //movementDirection.Normalize();

        bool allowMove = anim.CheckAniPlayingOrNot();

        if (data.item != null)
        {
            holdingItem = true;
        }
        else
        {
            holdingItem = false;
        }

        if (isDash == false && (Input.GetAxis("Vertical" + pid) != 0 || Input.GetAxis("Horizontal" + pid) != 0))
        {
            if (allowMove)
            {
                anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
                anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
                transAmt = Input.GetAxis("Vertical" + pid);
                rotAmt = Input.GetAxis("Horizontal" + pid);

                if (transAmt <= 0.2f && transAmt >= -0.2f)
                {
                    transAmt = 0.0f;
                    anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
                }

                if (rotAmt <= 0.2f && rotAmt >= -0.2f)
                {
                    rotAmt = 0.0f;
                    anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
                }

                if (transAmt != 0 || rotAmt != 0)
                {
                    aniClip = pm.MoveAndRotate(transAmt, rotAmt, Camera.main);
                }
                
                Debug.Log("Move1" + aniClip + transAmt + "/" + rotAmt);
                anim.ChangeAnimationState(aniClip, transAmt, rotAmt);
            }
        }
        //else if (!moved)
        //{
        //    anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
        //    anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
        //}

        if(Input.GetAxis("Vertical" + pid) == 0 && Input.GetAxis("Horizontal" + pid) == 0)
        {
            anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
            anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
        }

        if(Input.GetButtonDown("Take" + pid) && isDash == false)
        {
            takePressDown = true;
            
            if ((data.item != null && (data.item.tag == "RockModel" || data.item.tag == "Box" || data.item.tag == "Bag")))
            {
                showArrow = true;
            }
        }

        if(Input.GetButtonUp("Take" + pid) && isDash == false)
        {
            takePressDown = false;
            showArrow = false;
            slider.value = 0;
        }

        if(takePressDown == true)
        {
            takePressTimer += Time.deltaTime;

            if (showArrow == true)
            {
                slider.value = Mathf.Pow(takePressTimer * 2f, 5f);
            }
        }

        if(Input.GetButtonUp("Take" + pid) && isDash == false)
        {
            if (holdingItem == false)
            {
                aniClip = pm.Take();

                if (aniClip == "none")
                {
                    return;
                }

                Debug.Log(aniClip);
                anim.ChangeAnimationState(aniClip, 0, 0);
            }
            else
            {
                string itemInhand = data.item.gameObject.tag;

                if(takePressTimer >= 0.2f && (itemInhand == "RockModel" || itemInhand == "Box" || itemInhand == "Bag"))
                {
                    aniClip = pm.Throw(takePressTimer);
                }
                else
                {
                    if (itemInhand == "Chop")
                    {
                        aniClip = pm.UseChop();
                    }
                    else if (itemInhand == "Bucket")
                    {
                        //aniName = pm.UseBucket();
                    }
                    else
                    {
                        aniClip = pm.Drop();
                    }
                }

                anim.ChangeAnimationState(aniClip, 0, 0);
            }

            pressTimeSaver = takePressTimer;
            takePressTimer = 0.0f;
        }

        
        if (Input.GetButtonDown("Dash" + pid) && isDash == false && anim.animator.GetBool(anim.animRoling) == false)
        {
            if (isDash == false)
            {
                isDash = true;
                remainDashTime = setDashTime;
            }
        }

        if (remainDashTime <= 0)
        {
            isDash = false;
        }
        else
        {
            bool allowSpeedRun =
                !anim.animator.GetBool(anim.animPickedHash) && isDash
                && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_UsingTable)
                && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_Cheer);

            if (allowSpeedRun)
            {

                remainDashTime = pm.Dash(remainDashTime);
                aniClip = "SpeedRun";
                anim.ChangeAnimationState(aniClip, 0, 0);
            }
            else
            {
                isDash = false;
            }
                
        }

        if (Input.GetButtonDown("Use" + pid) && isDash == false)
        {
            if (data.item != null)
            {
                holdingItem = true;
            }
            else
            {
                holdingItem = false;
            }

            if(holdingItem == false)
            {
                aniClip = pm.UseBench();

                if (aniClip == "none")
                {
                    return;
                }

                anim.ChangeAnimationState(aniClip, 0, 0);
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            anim.ChangeAnimaEventState(anim.Player_Fear);
        }
    }

    //for pInWater
    public Vector3 GetPlayerPos()
    {
        return this.gameObject.transform.position;
    }

    //force slider arrow
    private void LateUpdate()
    {
        if (throwSlider != null)
        {
            Vector3 temp = this.transform.position + this.transform.forward;
            temp.y += 0.5f;
            throwSlider.transform.position = temp;
            throwSlider.transform.forward = this.transform.forward;
            throwSlider.transform.localRotation *= Quaternion.Euler(90, 0, 0);
        }
    }

}