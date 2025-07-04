using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;

    [SerializeField] private float lookSensitivity;
    [SerializeField] private float cameraRotationLimit;
    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchPosY;
    private float originalPosY;                 // 카메라 Y 초기값
    private float applyCrouchPosY;              // 적용되는 카메라 Y 위치(crouch)

    private float currentCameraRotationX = 0f;  // 카메라 X 회전값
    private float applySpeed;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isGround = false;
    private bool isCrouching = false;
    private Vector3 lastPos;

    [SerializeField] private Camera camera;
    private GunController gunController;
    private Crosshair crosshair;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;
    private StatusController statusController;
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        gunController = FindFirstObjectByType<GunController>();
        crosshair = FindFirstObjectByType<Crosshair>();
        statusController = FindFirstObjectByType<StatusController>();

        applySpeed = walkSpeed;
        originalPosY = camera.transform.localPosition.y;
        applyCrouchPosY = originalPosY;
        lastPos = transform.position;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        if (!Inventory.inventoryActivated)
        {
            CameraRotation();
            CharacterRotation();
        }
        
    }
    private void FixedUpdate()
    {
        MoveCheck();
    }

    private void TryRun()
    {
        if (statusController.GetCurrentSp() <= 0)
        {
            RunningCancel();
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && statusController.GetCurrentSp() > 0)
        {
            Jump();
        }
    }
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouching = !isCrouching;
        crosshair.CrouchingAnimation(isCrouching);

        if (isWalking)
        {
            isWalking = false;
            crosshair.WalkingAnimation(isWalking);
        }
        if (isCrouching)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originalPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }
    IEnumerator CrouchCoroutine()
    {
        float _posY = camera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchPosY)
        {
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.5f);
            camera.transform.localPosition = new Vector3(0f, _posY, 0f);

            count++;
            if (count > 15) break;
            yield return null;
        }
        camera.transform.localPosition = new Vector3(0f, applyCrouchPosY, 0f);
    }
    private void Jump()
    {
        if (isCrouching)
        {
            Crouch();
        }
        statusController.DecreaseStamina(100);
        myRigid.linearVelocity = transform.up * jumpForce;
    }
    private void Running()
    {
        if (isCrouching)
        {
            Crouch();
        }
        gunController.CancelFineSight();
        isRunning = true;
        crosshair.RunningAnimation(isRunning);
        statusController.DecreaseStamina(10);
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRunning = false;
        crosshair.RunningAnimation(isRunning);
        applySpeed = walkSpeed;
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxis("Horizontal");
        float _moveDirZ = Input.GetAxis("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(myRigid.position + _velocity * Time.deltaTime);
    }
    private void MoveCheck() // 걷는 중인지 체크(크로스헤어 애니메이션을 위해)
    {
        if (!isRunning && !isCrouching && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
            crosshair.WalkingAnimation(isWalking);
            lastPos = transform.position;
        }
        
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        crosshair.JumpingAnimation(!isGround);
    }
    private void CameraRotation()
    {
        float _XRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _XRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    private void CharacterRotation()
    {
        float _YRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _YRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
