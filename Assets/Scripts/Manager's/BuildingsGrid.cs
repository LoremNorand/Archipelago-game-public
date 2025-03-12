using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public float cellSize;
    public float CellSize;
    public Material GridMaterial;
    public Color GridColor = Color.gray;
    public GameManager gameManager;

    internal Building[,] grid;
    private Building flyingBuilding;
    private Building selectedBuilding;
    private Camera mainCamera;
    private List<GameObject> Lines = new List<GameObject>();


    public float holdTimeToMove = 1f; 
    private float holdTimer = 0f;
    private bool isHolding = false;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
        CellSize = cellSize * gameManager.voxelSize;
        CreateGrid();
    }

    private void CancelBuildingPlacement()
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
            flyingBuilding = null;
            selectedBuilding = null;
        }
    }

    private void CreateGrid()
    {
        for (int x = 0; x <= GridSize.x; x++)
        {
            float xPos = (x - GridSize.x / 2f) * CellSize;
            CreateLine(new Vector3(xPos, 0, -GridSize.y / 2f * CellSize), new Vector3(xPos, 0, GridSize.y / 2f * CellSize));
        }

        for (int z = 0; z <= GridSize.y; z++)
        {
            float zPos = (z - GridSize.y / 2f) * CellSize;
            CreateLine(new Vector3(-GridSize.x / 2f * CellSize, 0, zPos), new Vector3(GridSize.x / 2f * CellSize, 0, zPos));
        }
        foreach (GameObject line in Lines)
        {
            line.transform.SetParent(this.transform);
        }
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        Lines.Add(lineObj);
        LineRenderer line = lineObj.AddComponent<LineRenderer>();

        line.material = GridMaterial;
        line.startColor = GridColor;
        line.endColor = GridColor;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.useWorldSpace = true;

        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }
        if (buildingPrefab.cost.IsCanBeBought()) 
        {
            flyingBuilding = Instantiate(buildingPrefab);
        }
    }

    public bool IsReplacing()
    {
        if (flyingBuilding != null)
        {
            if(flyingBuilding.isPlaced == false)
            {
                flyingBuilding.cost.IsCanBeBought();
                return true;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    } 
    
    public void StartPlacingRoad(Road road)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }
        if (road.cost.IsCanBeBought())
        {
            flyingBuilding = Instantiate(road);
        }
    }

    private void PlaceBuilding()
    {
        if (flyingBuilding == null) return;
        if (IsReplacing())
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                int x = Mathf.FloorToInt((worldPosition.x + GridSize.x * 0.5f * CellSize) / CellSize);
                int z = Mathf.FloorToInt((worldPosition.z + GridSize.y * 0.5f * CellSize) / CellSize);

                bool available = true;
                if (x < 0 || x + flyingBuilding.Size.x > GridSize.x) available = false;
                if (z < 0 || z + flyingBuilding.Size.y > GridSize.y) available = false;
                if (available && IsPlaceTaken(x, z)) available = false;

                flyingBuilding.transform.position = new Vector3(
                    (x + flyingBuilding.Size.x * 0.5f - GridSize.x * 0.5f) * CellSize,
                    0,
                    (z + flyingBuilding.Size.y * 0.5f - GridSize.y * 0.5f) * CellSize
                );
                flyingBuilding.SetTransparent(available);
                if (Input.GetKeyDown(KeyCode.R))
                    flyingBuilding.Rotate();

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, z);
                }
            }
        }
        else
        {
            Destroy(flyingBuilding);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            CancelBuildingPlacement();
        }
    }
    private void Update()
    {
        if (flyingBuilding != null)
        {
            PlaceBuilding();
        }
        else
        {
            HandleBuildingSelection();
        }
    }
    private void HandleBuildingSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
            holdTimer = 0f;
        }

        if (Input.GetMouseButton(0) && isHolding)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTimeToMove)
            {
                SelectBuildingForMove();
                isHolding = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
        }

        if (selectedBuilding != null)
        {
            MoveSelectedBuilding();
        }
    }
    private void SelectBuildingForMove()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);
            int x = Mathf.FloorToInt((worldPosition.x + GridSize.x * 0.5f * CellSize) / CellSize);
            int z = Mathf.FloorToInt((worldPosition.z + GridSize.y * 0.5f * CellSize) / CellSize);
            if (x >= 0 && x < GridSize.x && z >= 0 && z < GridSize.y)
            {
                if (grid[x, z] != null)
                {
                    selectedBuilding = grid[x, z];
                    RemoveBuildingFromGrid(x, z);
                    flyingBuilding = selectedBuilding;
                    flyingBuilding.SetTransparent(true);
                }
            }
        }
    }




    private void RemoveBuildingFromGrid(int x, int z)
    {
        if (selectedBuilding != null)
        {
            for (int i = 0; i < selectedBuilding.Size.x; i++)
            {
                for (int j = 0; j < selectedBuilding.Size.y; j++)
                {
                    if (x + i < GridSize.x && z + j < GridSize.y)
                    {
                        grid[x + i, z + j] = null;
                    }
                }
            }
        }
    }


    private void MoveSelectedBuilding()
    {
        PlaceBuilding();
    }

    

    private bool IsPlaceTaken(int placeX, int placeZ)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int z = 0; z < flyingBuilding.Size.y; z++)
            {
                if (placeX + x >= GridSize.x || placeZ + z >= GridSize.y)
                {
                    return true; // Выход за границы сетки
                }
                if (grid[placeX + x, placeZ + z] != null && grid[placeX + x, placeZ + z] != flyingBuilding)
                {
                    return true; // Место занято другим зданием
                }
            }
        }
        return false;
    }


    private void PlaceFlyingBuilding(int placeX, int placeZ)
    {
        if (flyingBuilding.PreviousPosition != null)
        {
            ClearBuildingFromGrid(flyingBuilding.PreviousPosition.Value.x, flyingBuilding.PreviousPosition.Value.y);
            flyingBuilding.isPlaced = true;
        }

        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int z = 0; z < flyingBuilding.Size.y; z++)
            {
                grid[placeX + x, placeZ + z] = flyingBuilding;
            }
        }
        if (flyingBuilding.isPlaced == false)
        {
            flyingBuilding.cost.BuyBuilding();
        }
        else
        {
            flyingBuilding.isPlaced = true;
        }
        flyingBuilding.PreviousPosition = new Vector2Int(placeX, placeZ);
        flyingBuilding.SetNormal();
        flyingBuilding = null;
        selectedBuilding = null;
    }

    private void ClearBuildingFromGrid(int x, int z)
    {
        for (int i = 0; i < flyingBuilding.Size.x; i++)
        {
            for (int j = 0; j < flyingBuilding.Size.y; j++)
            {
                if (x + i < GridSize.x && z + j < GridSize.y)
                {
                    grid[x + i, z + j] = null;
                }
            }
        }
    }
}
