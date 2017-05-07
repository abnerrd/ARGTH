using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Handles GAMEOBJECT visuals.
//  Contains information on the Tile.

public class BaseTile : MonoBehaviour
{
    private int m_rowIndex, m_columnIndex;
    public int row { get { return m_rowIndex; } }
    public int column { get { return m_columnIndex; } }

    // TODO aherrera : think of a better name for this one..
    [SerializeField]
    [Tooltip("Can this tile be traveled upon?")]
    private bool m_tileAvailable = true;
    public bool IsAvailable { get { return m_tileAvailable; } }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void InitializeTile(int row, int column)
    {
        m_rowIndex = row;
        m_columnIndex = column;
    }

    public void SetTileAvailability(bool value)
    {
        m_tileAvailable = value;
    }



}
