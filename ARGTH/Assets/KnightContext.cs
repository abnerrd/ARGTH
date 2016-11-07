using UnityEngine;
using System.Collections;

// Prototype context to practice coding games

/// KnightContext is simple knight rougelike. Player can explore, fight monsters.

public class KnightContext : BaseContext
{
    public static KnightContext instance = null;

    public GameObject _playerPrefab = null;
    private PlayerUnit m_playerReference = null;
    public PlayerUnit Player { get { return m_playerReference; } }

    [Header("Grid Variables")]
    private GridManager m_gridManager;

    private enum eMovementScheme
    {
        ROGUELIKE,
        AUTONOMOUS
    }
    [SerializeField]
    eMovementScheme m_move_scheme;
    public bool AutonomousUnits { get { return (m_move_scheme == eMovementScheme.AUTONOMOUS); } }

    //  TODO aherrera : this boolean will be replaced w/ "UnitsUpdate", which will be set to TRUE for autonomous, or be attached to PlayerMove event for roguelike
    public bool updateUnits = false;

    protected override void InitializeContext()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        base.InitializeContext();

        if (m_gridManager == null)
        {
            GridManager grid_ref = this.gameObject.GetComponent<GridManager>();
            if (grid_ref == null)
            {
                m_gridManager = this.gameObject.AddComponent<GridManager>();
            }
            else
            {
                m_gridManager = grid_ref;
            }
        }
    }

    protected override void UpdateUserInput()
    {
        base.UpdateUserInput();
    }

    protected override void UpdateContext()
    {
        base.UpdateContext();
    }

    #region Grid Functions

    public void GenerateGrid()
    {
        m_gridManager.GenerateGrid();
        // TODO aherrera : take out that stupid parameter
        m_gridManager.PositionGrid(Vector3.zero, m_gridManager._debugSpacing);
    }

    public TileScript GetTile(int rowIndex, int columnIndex)
    {
        TileScript retVal = m_gridManager.getTile(rowIndex, columnIndex);

        return retVal;
    }

    // TODO aherrera : add a UnitFactory/Manager? Use to define Units, where the Context will grab info from to create a specific one?
    public void AddUnit(UnitScript unit, int row = -1, int column = -1)
    {
        int selected_row = row;
        int selected_column = column;
        if (selected_row < 0)
        {
            selected_row = Random.Range(0, m_gridManager.Rows);
        }
        if (selected_column < 0)
        {
            selected_column = Random.Range(0, m_gridManager.Columns);
        }

        SetUnit(unit, selected_row, selected_column, true);

    }

    /// <summary>
    /// Use to explicitly set a Unit onto any Tile
    /// @p firstEntry: Specify if this unit is being set into the Grid for 1st time
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="desiredRow"></param>
    /// <param name="desiredColumn"></param>
    /// <param name="firstEntry"></param>
    /// <returns>@r TRUE if Unit was set successfully</returns>
    public bool SetUnit(UnitScript unit, int desiredRow, int desiredColumn, bool firstEntry = false)
    {
        bool retVal = false;

        if (firstEntry)
        {
            retVal = m_gridManager.enterGrid(unit, desiredRow, desiredColumn);
        }
        else
        {
            retVal = m_gridManager.changeIndex(unit, unit.Coordinates, new Vector2(desiredRow, desiredColumn));
        }


        return retVal;
    }

    public bool MoveUnit(UnitScript unit, int row_adjustment, int column_adjustment)
    {
        bool retVal = false;

        Vector2 oldCoordinates = unit.Coordinates;
        Vector2 newCoordinates = new Vector2(oldCoordinates.x + row_adjustment, oldCoordinates.y + column_adjustment);

        retVal = m_gridManager.changeIndex(unit, oldCoordinates, newCoordinates);

        return retVal;
    }

    #endregion

    public void AddPlayerUnit()
    {
        if (m_playerReference == null)
        {
            GameObject newPlayer = GameObject.Instantiate(_playerPrefab);
            m_playerReference = newPlayer.GetComponent<PlayerUnit>();
            AddUnit(m_playerReference);

            // TODO aherrera : have CameraScript be a singleton in a way that allows for multiple cameras.. maybe enum titles? Tags???
            CameraScript main_cam = Camera.main.GetComponent<CameraScript>();
            main_cam.SetFollowTarget(m_playerReference.transform);

            if(!AutonomousUnits)
            {
                AddListenerToPlayerInput(onPlayerMoved);
            }

        }
    }

    public void onPlayerMoved(UnitScript unit)
    {
        StartCoroutine("PlayerInputCoroutine");
    }

    protected IEnumerator PlayerInputCoroutine()
    {
        updateUnits = true;
        yield return new WaitForFixedUpdate();
        updateUnits = false;
    }

    protected void AddListenerToPlayerInput(UnitScript.MoveUnitCallback callback)
    {
        if(m_playerReference != null)
        {
            m_playerReference.onUnitMoved += callback;
        }
    }

    protected void RemoveListenerToPlayerInput(UnitScript.MoveUnitCallback callback)
    {
        if(m_playerReference != null)
        {
            m_playerReference.onUnitMoved -= callback;
        }
    }


}
