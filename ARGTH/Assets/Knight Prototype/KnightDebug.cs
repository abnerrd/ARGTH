using UnityEngine;
using System.Collections;

public class KnightDebug : MonoBehaviour {


    private static KnightDebug instance = null;

    public KnightContext context = null;
    public GameObject DebugMonsterUnitPrefab;

	// Use this for initialization
	void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (context == null)
        {
            Debug.LogError("Please put an instance of a GameContext");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Generate Grid
            context.GenerateGrid();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Add/create Player
            context.AddPlayerUnit();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Add Debug Unit
            context.AddUnit(GameObject.Instantiate(DebugMonsterUnitPrefab).GetComponent<UnitScript>());
        }
    }
}
