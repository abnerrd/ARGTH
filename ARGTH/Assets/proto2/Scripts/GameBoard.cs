using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//  This script will handle the Grid and events on each tile.
//  Contains the Grid Database.
//  Contains Grid Visuals.

public class GameBoard : MonoBehaviour
{
    /// <summary>
    /// The root for the entire GameWorld
    /// </summary>
    public GameObject GridRoot;

    public GameObject _tilePrefab;

    protected BaseTile[,] m_GridArray;
    [SerializeField]
    private int m_rows = 1, m_columns = 1;
    public int Rows { get { return m_rows; } }
    public int Columns { get { return m_columns; } }

    void Awake()
    {
        if(GridRoot == null)
        {
            GridRoot = new GameObject("Grid Root");
        }
    }

    // Use this for initialization
    void Start ()
    {
        GenerateGrid();
        PositionGrid();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void GenerateGrid(int rows = -1, int columns = -1)
    {
        ClearGrid();

        if (rows > 0)
            m_rows = rows;
        if (columns > 0)
            m_columns = columns;

        m_GridArray = new BaseTile[m_rows, m_columns];

        for (int r = 0; r < m_rows; r++)
        {
            for (int c = 0; c < m_columns; c++)
            {
                GameObject newTile = GameObject.Instantiate(_tilePrefab, GridRoot.transform);
                newTile.name = "Tile " + r + ", " + c;

                BaseTile tile_ref = newTile.GetComponent<BaseTile>();
                tile_ref.InitializeTile(r, c);
                m_GridArray[r, c] = tile_ref;
            }
        }
    }

    public void ClearGrid()
    {
        if (m_GridArray != null)
        {
            for (int r = 0; r < m_rows; r++)
            {
                for (int c = 0; c < m_columns; c++)
                {
                    GameObject.Destroy(m_GridArray[r, c].gameObject);
                }
            }
            Array.Clear(m_GridArray, 0, m_GridArray.Length);
        }
    }

    public void PositionGrid(float tileSpacing = 4f)
    {
        for (int r = 0; r < m_rows; r++)
        {
            for (int c = 0; c < m_columns; c++)
            {
                BaseTile tile_reference = m_GridArray[r, c];

                Vector3 newPosition = GridRoot.transform.position;
                newPosition.x += r * (tileSpacing);
                newPosition.z += c * (tileSpacing);

                tile_reference.transform.position = newPosition;
            }
        }
    }

    //  Record a Unit into the Gameboard; Link events and delegates to listen to.
    public void RegisterUnit(/*UnitScript?*/)
    {

    }

    public void UnregisterUnit(/*UnitScript*/)
    {

    }

}
