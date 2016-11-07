using UnityEngine;
using System.Collections;
using System;

// Leaving this as a MonoBehaviour so that it can be a component

public class GridManager : MonoBehaviour
{
    public GameObject GridRoot;

    public GameObject _tilePrefab;
    public TileScript[,] m_Grid;
    [SerializeField]
    private int m_rows = 1, m_columns = 1;
    public int Rows { get { return m_rows; } }
    public int Columns { get { return m_columns; } }

    
    public float _debugSpacing = 1;

    void Awake()
    {
        if(GridRoot == null)
        {
            GridRoot = new GameObject();
            GridRoot.name = "Grid Root";
        }
    }

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    public void GenerateGrid(int rows = -1, int columns = -1)
    {
        ClearGrid();

        if(rows > 0)
            m_rows = rows;
        if(columns > 0)
            m_columns = columns;

        m_Grid = new TileScript[m_rows, m_columns];

        for (int r = 0; r < m_rows; r++)
        {
            for (int c = 0; c < m_columns; c++)
            {
                GameObject newTile = GameObject.Instantiate(_tilePrefab);
                newTile.name = "Tile " + r + ", " + c;
                newTile.transform.SetParent(GridRoot.transform);

                TileScript tile_ref = newTile.GetComponent<TileScript>();
                tile_ref.InitializeTile(r, c);
                m_Grid[r, c] = tile_ref;
            }
        }

    }

    public void ClearGrid()
    {
        if (m_Grid != null)
        {
            for (int r = 0; r < m_rows; r++)
            {
                for (int c = 0; c < m_columns; c++)
                {
                    GameObject.Destroy(m_Grid[r, c].gameObject);
                }
            }
            Array.Clear(m_Grid, 0, m_Grid.Length);
        }
    }

    public void PositionGrid(Vector3 grid_origin, float tileSpacing = 4f)
    {
        for (int r = 0; r < m_rows; r++)
        {
            for (int c = 0; c < m_columns; c++)
            {
                TileScript tile_reference = m_Grid[r, c];

                Vector3 newPosition = grid_origin;
                newPosition.x += r * (tileSpacing);
                newPosition.z += c * (tileSpacing);

                tile_reference.transform.position = newPosition;
            }
        }
    }

    public TileScript getTile(int row, int column)
    {
        TileScript retVal = null;
        if (row <= m_rows - 1 && column <= m_columns - 1)
        {
            retVal = m_Grid[row, column];
        }
        return retVal;
    }

    /// <summary>
    /// Condition(s) to entering other Tiles
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="tile"></param>
    /// <returns></returns>
    private bool CanUnitMoveToTile(UnitScript unit, TileScript tile)
    {
        return tile.TileAvailable;
    }

    /// <summary>
    /// Use to set GameObject position
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="tile"></param>
    /// <returns></returns>
    private bool setOntoTile(UnitScript unit, TileScript tile)
    {
        bool retVal = false;

        if (tile != null && CanUnitMoveToTile(unit, tile))
        {
            unit.transform.position = tile.transform.position;
            tile.enterTile(unit.BlockTile, unit);

            retVal = true;
        }
        else
            Debug.Log("Unable to set " + unit.name + " onto tile " + tile.row.ToString() + tile.column.ToString());

        return retVal;
    }

    /// <summary>
    /// Use to set GameObject position
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    private bool setOntoTile(UnitScript unit, int row, int column)
    {
        if (row < m_rows && column < m_columns && row >= 0 && column >= 0)
            return setOntoTile(unit, m_Grid[row, column]);

        return false;
    }

    /// <summary>
    /// Use to introduce a unit into the Grid
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public bool enterGrid(UnitScript unit, int row, int column)
    {
        bool retVal = false;

        if (row <= m_rows && column <= m_columns)
        {
            // TODO aherrera : add check to see if this Unit has been already introduced to this Grid?
            TileScript tile_ref = m_Grid[row, column];
            if (tile_ref.TileAvailable)
            {
                retVal = setOntoTile(unit, tile_ref);
            }
            else
            {
                Debug.Log("Tile " + tile_ref.row.ToString() + tile_ref.column.ToString() + " is unavailable.");
            }
        }
        else
            Debug.LogWarning("GameContext::changeIndex() -- Invalid tile!");

        return retVal;
    }

    /// <summary>
    /// Use to change the coordinates of a Unit at specified coordinates
    /// @r TRUE if unit was moved
    /// </summary>
    /// <param name="oldPosition"></param>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    public bool changeIndex(UnitScript unit, Vector2 oldPosition, Vector2 newPosition)
    {
        bool retVal = false;

        if (oldPosition.x <= m_rows && oldPosition.y <= m_columns)
        {
            TileScript tile_ref = m_Grid[(int)oldPosition.x, (int)oldPosition.y];
            if (tile_ref.isOccupied && tile_ref.OccupiesTile(unit))
            {
                // TODO aherrera : could this possibly reset tile unintentionally?
                tile_ref.exitTile(unit);
                retVal = setOntoTile(unit, (int)newPosition.x, (int)newPosition.y);
                if(!retVal)
                {
                    // Re-enter old tile -- Something's blocking the way!
                    // TODO aherrera : could this bring a bug? Set a tile w/ unit as passable for special reason, IF anything exits/enters, will it be reset?
                    tile_ref.enterTile(unit.BlockTile, unit);
                }
            }
            else
            {
                Debug.Log("Tile " + tile_ref.row.ToString() + tile_ref.column.ToString() + " unoccupied.");
            }
        }
        else
            Debug.LogWarning("GameContext::changeIndex() -- Invalid tile!");

        return retVal;
    }
    
}
