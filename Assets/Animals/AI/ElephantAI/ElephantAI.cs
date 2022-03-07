using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElephantAI : MonoBehaviour
{
    public enum ElephantState
    {
        Idle,
        Wander,
        WanderPoint
    }

    private ElephantState currentState;
    private float currentTime;
    [SerializeField] private float stateTime;
    private Vector3 currentPos;
    private Animator animator;
    private NavMeshAgent agent;
    private Quaternion rotate;
    [SerializeField] public List<Transform> point;
    public bool IsPoint = true;
    public float speed;
    public int currentPoint = 0;
    public GameObject warningIcon;


    void Start()
    {
        currentState = ElephantState.Idle;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        stateTime = 5f;

        Transform[] wanderPoint = GameObject.Find("ElephantWander").GetComponentsInChildren<Transform>();
        foreach (Transform go in wanderPoint)
        {
            point.Add(go);
        }
        point.RemoveAt(0);
    }
    void Update()
    {
        switch (currentState)
        {
            case ElephantState.Idle:
                IdleState();
                break;
            case ElephantState.Wander:
                NavMeshAgentWanderState();
                break;
            case ElephantState.WanderPoint:
                WanderState();
                break;
        }
    }

    private void IdleState()
    {
        currentTime += Time.deltaTime;
        if (currentTime > stateTime)
        {
            currentTime = 0;
            animator.SetInteger("State", 1);
            if (IsPoint)
            {
                ChosePoint();
                currentState = ElephantState.WanderPoint;
                currentPos = point[currentPoint].position;
                StartCoroutine(WarningActive());

            }
            else
            {
                currentState = ElephantState.Wander;
                currentPos = RandomNavSphere(transform.position, 50, -1);
                agent.SetDestination(currentPos);
            }
        }
    }
    private void NavMeshAgentWanderState()
    {
        Vector3 dist = currentPos - transform.position;
        rotate = Quaternion.LookRotation(dist);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, 0.1f);
        float dist2 = (transform.position - currentPos).magnitude;

        if (dist2 < 1f)
        {
            currentTime = 0;
            currentState = ElephantState.Idle;
            animator.SetInteger("State", 0);
        }

    }

    private void WanderState()
    {

        Vector3 dist = currentPos - transform.position;
        rotate = Quaternion.LookRotation(dist, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 0.35f);

        transform.position = Vector3.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);
        float dist2 = (transform.position - currentPos).magnitude;

        if (dist2 < 1f)
        {
            currentTime = 0;
            currentState = ElephantState.Idle;
            if (stateTime > 7f)
            {
                animator.SetInteger("State", 2);
            }
            else
            {
                animator.SetInteger("State", 0);
            }
        }
    }

    private void ChosePoint()
    {
        switch (currentPoint)
        {
            case 0:
                currentPoint = 1;
                stateTime = Random.Range(5f, 10f);
                break;
            case 1:
                stateTime = Random.Range(5f, 10f);
                currentPoint = 2;
                break;
            case 2:
                stateTime = Random.Range(2f, 5f);
                currentPoint = 3;
                break;
            case 3:
                stateTime = Random.Range(2f, 5f);
                currentPoint = 4;
                break;
            case 4:
                stateTime = Random.Range(2f, 5f);
                currentPoint = 0;
                break;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {

        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void SetAnimator()
    {
        animator.SetInteger("State", 0);
    }

    IEnumerator WarningActive()
    {
        warningIcon.transform.position = currentPos + new Vector3(0,1.5f,0);
        warningIcon.SetActive(true);
        yield return new WaitForSeconds(3f);
        warningIcon.SetActive(false);
    }

    public AudioSource audioSource;
    public void AudioPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public LevelControl levelControl;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            levelControl.MinusScorePos(collision.transform.position);
            levelControl.GenTotalScore(5);
        }
    }
}
