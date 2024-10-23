using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�÷��̾��� ������ �ӵ��� �����ϴ� ���� 
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;             //�̵� �ӵ�
    public float jumpForce = 5.0f;             //���� ��
    public float rotationSpeed = 10.0f;        //ȸ�� �ӵ�

    //ī�޶� ���� ����
    [Header("Camera Setting")]
    public Camera firstPersonCamera;            //1��Ī ī�޶�
    public Camera thirdPersonCamera;            //3��Ī ī�޶�

    public float mouseSenesitivity = 200.0f;      //���콺 ����

    public float cameraDistance = 5.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;

    private float currentX = 0.0f;
    private float currentY = 45.0f;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public float yMinLimit = -90;               //ī�޶� ���� ȸ�� �ּҰ�
    public float yMaxLimit = 90;                //ī�޶� ���� ȸ�� �ִ밢

    public float radius = 5.0f;                 //3��Ī ī��[��� �÷��̾� ���� �Ÿ�
    public float minRadius = 1.0f;              //ī�޶� �ּ� �Ÿ�
    public float maxRadius = 10.0f;             //ī�޶� �ִ� �Ÿ�

    private float theta = 0.0f;                    //ī�޶��� ���� ȸ�� ����
    private float phi = 0.0f;                      //ī�޶��� ���� ȸ�� ����
    private float targetVericalRotation = 0;      //��ǥ ���� ȸ�� ����
    private float verticalRoatationSpeed = 240f;   //���� ȸ�� ����

    //���� ������
    public bool isFirstPerson = true;         //1��Ī ��� ���� ����
    //private bool isGrounded;                   //�÷��̾ ���� �ִ��� ����
    private Rigidbody rb;                      //�÷��̾��� Rigidbody

    public float falllingThreshold = -0.1f;    // �������� ������ ������ ���� �ӵ� �Ӱ谪

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();       //Rigidbody ������Ʈ�� �����´�.

        Cursor.lockState = CursorLockMode.Locked;     //���콺 Ŀ���� ��װ� �����.
        SetupCameras();
        SetActiveCamera();
    }

    void Update()
    {
        HandleRotation();
        //HandleCameraToggle();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
    }
    void FixedUpdate()
    {
        HandleMovement();
    }

    //Ȱ��ȭ�� ī�޶� �����ϴ� �Լ�
    void SetActiveCamera()
    {
        firstPersonCamera.gameObject.SetActive(isFirstPerson);   //1��Ī ī�޶� Ȱ��ȭ ����
        thirdPersonCamera.gameObject.SetActive(!isFirstPerson);  //3��Ī ī�޶� Ȱ��ȭ ����
    }
    //ī�޶� �� ĳ���� ȸ�� ó���ϴ� �Լ�
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity;         //���콺 �¿��Է�
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity;         //���콺 �����Է�

        if (isFirstPerson)
        {
            //1��Ī ī�޶� ������ ����
            transform.rotation = Quaternion.Euler(0.0f, currentX, 0.0f);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(currentY, 0.0f, 0.0f);
        }
        else
        {
            //3��Ī ī�޶� ���� ����
            currentX += mouseX;
            currentY -= mouseY;         //���콺 Y�� ����

            //���� ȸ�� ����
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            //ī�޶� ��ġ �� ȸ�� ���
            Vector3 dir = new Vector3(0,0, -cameraDistance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            thirdPersonCamera.transform.position = transform.position + rotation * dir;
            thirdPersonCamera.transform.LookAt(transform.position);     //LookAt �ٶ󺸴� ���� ��� (ĳ���͸� ��� ����)

            //�� ó�� 
            cameraDistance = Mathf.Clamp(cameraDistance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);
        }
    }

    //ī�޶� �ʱ� ��ġ �� ȸ���� �����ϴ� �Լ�
    void SetupCameras()
    {
        firstPersonCamera.transform.localPosition = new Vector3(0.0f, 0.6f, 0.0f);      //1��Ī ī�޶� ��ġ
        firstPersonCamera.transform.localRotation = Quaternion.identity;                //1��Ī ī�޶� ȸ�� �ʱ�ȭ
    }

    //�÷��̾� ������ ó���ϴ� �Լ�
    public void HandleJump()
    {
        if (isGrounded())
        {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  //�������� ���� ���� ����
        }
    }

    //�÷��̾��� �̵��� ó���ϴ� �Լ� 
    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");            //�¿� �Է� (-1~1)
        float moveVertical = Input.GetAxis("Vertical");                //�յ� �Է� (1~-1)

        Vector3 movement;

        if(!isFirstPerson) //3��Ī ����϶�, ī�޶� �������� �̵� ó�� 
        {
            Vector3 cameraFoward = thirdPersonCamera.transform.forward;   //ī�޶� �� ����
            cameraFoward.y = 0.0f;
            cameraFoward.Normalize();   //���� ���� ����ȣ�� (0~1)���� ������ ������ش�.

            Vector3 cameraRight = thirdPersonCamera.transform.right;      //ī�޶� ������ ����
            cameraRight.y = 0.0f;
            cameraRight.Normalize();

            movement = cameraRight * moveHorizontal + cameraFoward * moveVertical;
        }
        else
        {
            //ĳ���� �������� �̵�
            movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        }

        //�̵� �������� ĳ���� ȸ��
        if (movement.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

    }

    public bool isGrounded()    // �� üũ Ȯ��
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.0f);
    }

    public bool isFalling()   //�������� �� Ȯ��
    {
        return rb.velocity.y < falllingThreshold && !isGrounded();
    }

    public float GetVerticalVelocity() // �÷��̾��� Y�� �ӵ� Ȯ��
    {
        return rb.velocity.y;
    }
}
