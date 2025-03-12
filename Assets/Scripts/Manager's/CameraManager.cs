using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float moveSpeed = 10f; // Базовая скорость движения
    public float fastMoveSpeed = 20f; // Скорость движения с ускорением
    public float minHeight = 5f; // Минимальная высота камеры
    public float maxHeight = 50f; // Максимальная высота камеры
    [SerializeReference] internal Vector2 mapBoundsX;
    [SerializeReference] internal Vector2 mapBoundsZ;
    public float mouseSensitivity = 100f; // Чувствительность мыши
    public BuildingsGrid grid;
    private bool isreged = false;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        HandleMovement();
        HandleRotation();
        ClampPosition();
    }

    private void Start()
    {
        transform.position = Vector3.zero;
        if (isreged == false)
        {
            grid = FindFirstObjectByType<BuildingsGrid>();
            isreged = true;
        }
        UpdateMapBounds(grid);
    }


    private void UpdateMapBounds(BuildingsGrid grid)
    {
        mapBoundsX.x = -(grid.GridSize.y * grid.CellSize)/2;
        mapBoundsX.y = (grid.GridSize.x * grid.CellSize)/2;
        mapBoundsZ = mapBoundsX;
    }
    private void HandleMovement()
    {
        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? fastMoveSpeed : moveSpeed;

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.D))
            moveDirection += transform.right;

        //moveDirection.y = 0; // Игнорируем движение по оси Y
        moveDirection.Normalize();

        transform.position += moveDirection * currentMoveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            rotationY += mouseX;
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minHeight, maxHeight);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, mapBoundsX.x, mapBoundsX.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, mapBoundsZ.x, mapBoundsZ.y);
        transform.position = clampedPosition;
    }
}