using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;

        public OnSelectedCounterChangedEventArgs(ClearCounter selectedCounter)
        {
            this.selectedCounter = selectedCounter;
        }
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking = false;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than one Player instance");
        }
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update() {
        HandleMovement();
        ScanForInteractableObjects();
    }

    private void ScanForInteractableObjects()
    {
        // Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        // Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactionDistance = 2f;
        RaycastHit targetedObject;
        bool foundAnObject = Physics.Raycast(
            transform.position, 
            transform.forward, 
            out targetedObject, 
            interactionDistance, 
            countersLayerMask
        );
        if(foundAnObject)
        {
            if(targetedObject.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if(selectedCounter != clearCounter)
                {
                    UpdateSelectedCounter(clearCounter);
                }
            }
            else
            {
                UpdateSelectedCounter(null);

            }
        }
        else
        {
            UpdateSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDirection,
            moveDistance
        );

        // If we're colliding with something, try moving only on the x axis
        if(!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0);
            canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirectionX,
                moveDistance
            );

            // If we were successful, update our move direction to only include
            // the X component. Otherwise, repeat the process for the Z axis
            if(canMove) moveDirection = moveDirectionX.normalized;
            else
            {
                Vector3 moveDirectionZ = new Vector3(moveDirection.z, 0, 0);
                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirectionZ,
                    moveDistance
                );
                if(canMove) moveDirection = moveDirectionZ.normalized;
                else isWalking = false;
            }
        }

        // Final movement commitment
        if(canMove)
        {
            if(moveDirection != Vector3.zero) isWalking = true;
            else isWalking = false;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }


        float rotateSpeed = 10f;
        Vector3 threeDeeInputVector = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.forward = Vector3.Slerp(transform.forward, threeDeeInputVector, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void UpdateSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs(selectedCounter));
    }
}
