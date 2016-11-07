using UnityEngine;
using System.Collections;

public class MonsterScript : UnitScript
{
    public float _MoveSelfWaitTime = 1.0f;

    public UnitScript unit_target;
    Vector2 pasttarget_coordinates;

    protected override void InitializeUnit()
    {
        base.InitializeUnit();
        AddState(MoveSelf);
    }

    //  STATE DELEGATE
    protected float MoveSelf()
    {
        KnightContext.instance.MoveUnit(this, 1, 0);

        if(unit_target != null)
        {
            AddState(MoveTowardsTarget);
        }

        return GetActionReturnTime(_MoveSelfWaitTime);
    }

    protected float MoveTowardsTarget()
    {
        if(unit_target == null)
        {
            PopState();
        }
        else
        {
            Vector2 targetCoords = pasttarget_coordinates;
            pasttarget_coordinates = unit_target.Coordinates;

            int row_change = 0;
            int col_change = 0;
            int coinToss = Random.Range(0, 2);
            if(coinToss == 0)
            {
                //  Prevent moving diagonally for now?
                col_change = (int)targetCoords.y - this.Column;
                
                if (col_change == 0)
                {
                    row_change = (int)targetCoords.x - this.Row;
                }
            }
            else
            {
                //  Prevent moving diagonally for now?
                row_change = (int)targetCoords.x - this.Row;

                if (row_change == 0)
                {
                    col_change = (int)targetCoords.y - this.Column;
                    
                }
            }

            row_change = Mathf.Clamp(row_change, -1, 1);
            col_change = Mathf.Clamp(col_change, -1, 1);


            KnightContext.instance.MoveUnit(this, row_change, col_change);
        }

        return GetActionReturnTime(1.5f);
    }

    protected override void TriggerEnter(Collider collider)
    {
        base.TriggerEnter(collider);

        //  TODO aherrera : please for the love of god rewrite this condition
        if (collider.tag == "Player" && collider.GetComponent<UnitScript>().ColliderIsBody(collider))
        {
            unit_target = collider.GetComponent<UnitScript>();
            pasttarget_coordinates = unit_target.Coordinates;
        }

    }

    protected override void TriggerExit(Collider collider)
    {
        base.TriggerExit(collider);

        if (collider.tag == "Player" && collider.GetComponent<UnitScript>().ColliderIsBody(collider))
        {
            unit_target = null;
            pasttarget_coordinates = Vector2.zero;
        }
    }


}
