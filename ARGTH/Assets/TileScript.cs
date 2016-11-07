using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {

    private int m_rowIndex, m_columnIndex;
    public int row { get { return m_rowIndex; } }
    public int column { get { return m_columnIndex; } }

    [SerializeField]
    // TODO aherrera : change from UnitScript type to id?
    private List<UnitScript> m_occupants;
    public bool isOccupied { get { return (m_occupants.Count > 0); } }

    // TODO aherrera : think of a better name for this one..
    [SerializeField]
    [Tooltip("Can this tile be traveled upon?")]
    private bool m_tileAvailable = true;
    public bool TileAvailable { get { return m_tileAvailable; } }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitializeTile(int row, int column, UnitScript inhabitant = null)
    {
        m_occupants = new List<UnitScript>();
        m_rowIndex = row;
        m_columnIndex = column;
        m_occupants.Add(inhabitant);
    }

    public void enterTile(bool lockTile, UnitScript newOccupant = null)
    {
        m_tileAvailable = !lockTile;
        m_occupants.Add(newOccupant);
        newOccupant.UpdateCoordinates(m_rowIndex, m_columnIndex);
    }

    public void exitTile(UnitScript unit, bool unlockTile = true)
    {
        // TODO aherrera : add check for unit NOT in list?
        m_occupants.Remove(unit);
        m_tileAvailable = unlockTile;
    }

    public bool OccupiesTile(UnitScript unit)
    {
        return m_occupants.Contains(unit);
    }

    public void SetTileAvailability(bool value)
    {
        m_tileAvailable = value;
    }

    

}
