using UnityEngine;
using System.Collections;

public class BaseContext : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        InitializeContext();
	}

    protected virtual void InitializeContext()
    {

    }

    // Update is called once per frame
   void Update ()
    {
        UpdateUserInput();
	}
    
    protected virtual void UpdateUserInput()
    {

    }

    protected virtual void UpdateContext()
    {

    }

}
