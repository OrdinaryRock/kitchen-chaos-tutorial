using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;

        public OnSelectedCounterChangedEventArgs(BaseCounter selectedCounter)
        {
            this.selectedCounter = selectedCounter;
        }
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectSpawnPosition;


    private bool isWalking = false;
    private float interactionDistance = 1f;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


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
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
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
            if(targetedObject.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(selectedCounter != baseCounter)
                {
                    UpdateSelectedCounter(baseCounter);
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

    private void UpdateSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs(selectedCounter));
    }

    public Transform GetKitchenObjectSpawnPosition()
    {
        return kitchenObjectSpawnPosition;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
