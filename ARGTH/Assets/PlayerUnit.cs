using UnityEngine;
using System.Collections;

public class PlayerUnit : UnitScript
{

	// Use this for initialization
	void Start ()
    {
	}

    protected override void UpdateUnit()
    {
    }

    protected override void UpdateInput()
    {
        m_canUpdateUnit = (KnightContext.instance.AutonomousUnits ? CanUpdate() : true);

        if (m_canUpdateUnit)
        {
            //  TODO aherrera : Should these functions be in a more generic "GameContext?"
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //GameContext.instance.MoveUnit(this, -1, 0);
                KnightContext.instance.MoveUnit(this, -1, 0);
                m_doUpdate = true;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // GameContext.instance.MoveUnit(this, 1, 0);
                KnightContext.instance.MoveUnit(this, 1, 0);
                m_doUpdate = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
               // GameContext.instance.MoveUnit(this, 0, -1);
                KnightContext.instance.MoveUnit(this, 0, -1);
                m_doUpdate = true;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //GameContext.instance.MoveUnit(this, 0, 1);
                KnightContext.instance.MoveUnit(this, 0, 1);
                m_doUpdate = true;
            }
        }
    }
}
