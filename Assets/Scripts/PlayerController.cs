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
    private float applySpeed;                   // 적용되는 이동 속도
    private bool isRunning = false;
    private bool isGround = false;
    private bool isCrouching = false;

    [SerializeField] private Camera camera;
    private GunController gunController;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        gunController = FindFirstObjectByType<GunController>();

        applySpeed = walkSpeed;
        originalPosY = camera.transform.localPosition.y;
        applyCrouchPosY = originalPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void TryRun()
    {
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
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
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
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRunning = false;
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
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
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
