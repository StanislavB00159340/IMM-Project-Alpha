using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    public float speed = 50f;
    public float acceleration = 20f;
    public float deceleration = 30f;
    public float minInitialVelocity = 1f;
    public float peakVelocityMultiplier = 2f;
    public float dashDepleteRate = 20f; 
    public float dashSpeedMultiplier = 3f; 

    private Vector3 targetPosition;
    private float currentVelocity = 0f;
    private bool isTransitioning = false;
    private bool canSwitchDirection = true;
    private bool isDashing = false; 

    public Slider dashBar; 
    public float dashFillAmount = 0.2f; 

    private Animator animator;

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        if (!isDashing)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && canSwitchDirection)
            {
                targetPosition = new Vector3(-70, 2, 5);
                if (!isTransitioning) StartCoroutine(Transition("TransitionLeft", "TransitionLeftLanding"));
                else canSwitchDirection = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && canSwitchDirection)
            {
                targetPosition = new Vector3(-70, 2, -5);
                if (!isTransitioning) StartCoroutine(Transition("TransitionRight", "TransitionRightLanding"));
                else canSwitchDirection = false;
            }
        }

        
        if (Input.GetKeyDown(KeyCode.X) && dashBar.value == 1)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                targetPosition = new Vector3(-70, 2, 5);
                StartCoroutine(DashToTarget());
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                targetPosition = new Vector3(-70, 2, -5);
                StartCoroutine(DashToTarget());
            }
        }
    }

    private IEnumerator Transition(string transitionAnim, string landingAnim)
    {
        isTransitioning = true;
        currentVelocity = minInitialVelocity;
        animator.SetTrigger(transitionAnim);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            if (currentVelocity < speed * peakVelocityMultiplier)
            {
                currentVelocity += acceleration * Time.deltaTime;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentVelocity * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                currentVelocity -= deceleration * Time.deltaTime;
                if (currentVelocity < 0f) currentVelocity = 0f;
            }

            yield return null;
        }

        transform.position = targetPosition;
        currentVelocity = 0f;
        animator.SetTrigger(landingAnim);

        yield return new WaitForSeconds(0.5f);

        isTransitioning = false;
        canSwitchDirection = true;
    }

    private IEnumerator DashToTarget()
    {
        isDashing = true;
        float dashSpeed = speed * dashSpeedMultiplier;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f && dashBar.value > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);

            dashBar.value -= dashDepleteRate * Time.deltaTime; // Deplete dash bar

            yield return null;
        }

        transform.position = targetPosition;
        isDashing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pointEnable"))
        {
            dashBar.value += dashFillAmount; // Fill dash bar
            Debug.Log("sss");
            
        }
    }
}
