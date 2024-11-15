using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 50f; // Max speed of the character movement
    public float acceleration = 20f; // Rate of acceleration (faster)
    public float deceleration = 30f; // Rate of deceleration (faster)
    public float minInitialVelocity = 1f; // Starting velocity
    public float peakVelocityMultiplier = 2f; // Multiplier for peak velocity
    private Vector3 targetPosition; // The position to move towards
    private float currentVelocity = 0f; // Current velocity during transition
    private bool isTransitioning = false; // Flag to check if the character is transitioning
    private bool canSwitchDirection = true; // Flag to check if direction switch is allowed

    private Animator animator; // Reference to the Animator component

    void Start()
    {
        targetPosition = transform.position; // Initialize starting position
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // Check for input to change target position
        if (Input.GetKeyDown(KeyCode.LeftArrow) && canSwitchDirection)
        {
            targetPosition = new Vector3(-70, 2, 5); // New target position to the left
            if (!isTransitioning) StartCoroutine(Transition("TransitionLeft", "TransitionLeftLanding"));
            else canSwitchDirection = false; // Disable direction switch until landing
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && canSwitchDirection)
        {
            targetPosition = new Vector3(-70, 2, -5); // New target position to the right
            if (!isTransitioning) StartCoroutine(Transition("TransitionRight", "TransitionRightLanding"));
            else canSwitchDirection = false; // Disable direction switch until landing
        }
        else if (!isTransitioning)
        {
            //animator.SetTrigger("Idle"); // Set idle animation when not transitioning
        }
    }

    private IEnumerator Transition(string transitionAnim, string landingAnim)
    {
        isTransitioning = true; // Set transitioning flag
        currentVelocity = minInitialVelocity; // Start with a low initial velocity

        animator.SetTrigger(transitionAnim); // Play transition animation

        // Continue transitioning until the character reaches the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Accelerate towards the target until peak velocity
            if (currentVelocity < speed * peakVelocityMultiplier)
            {
                currentVelocity += acceleration * Time.deltaTime; // Accelerate
            }

            // Move the character towards the target position with the current velocity
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentVelocity * Time.deltaTime);

            // If close to the target, start deceleration
            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                currentVelocity -= deceleration * Time.deltaTime; // Decelerate
                if (currentVelocity < 0f) currentVelocity = 0f; // Prevent negative velocity
            }

            yield return null; // Wait for the next frame
        }

        // Ensure the character snaps to the exact target position
        transform.position = targetPosition;
        currentVelocity = 0f; // Reset velocity

        // Play landing animation
        animator.SetTrigger(landingAnim);

        yield return new WaitForSeconds(0.5f); // Wait for landing animation to finish (adjust time as needed)

        isTransitioning = false; // Reset transitioning flag
        canSwitchDirection = true; // Allow direction switch again after landing
    }
}
