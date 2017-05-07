using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour {

    private static DebugScript instance = null;

    public KnightContext gameContext = null;
    public GameObject DebugUnitPrefab;

	// Use this for initialization
	void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if(gameContext == null)
        {
            Debug.LogError("Please put an instance of a GameContext");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Generate Grid
            gameContext.GenerateGrid();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            // Add/create Player
            gameContext.AddPlayerUnit();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            // Add Debug Unit
            gameContext.AddUnit(GameObject.Instantiate(DebugUnitPrefab).GetComponent<UnitScript>());
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // TODO aherrera : functionality to trigger a debug "mini-game" event
            MGManager.instance.AddMG();
        }

	}


}
