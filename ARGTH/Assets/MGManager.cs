using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MGManager : MonoBehaviour
{
    public static MGManager instance = null;

    // TODO aherrera : replace this with an enum, and access to several pre-made prefabs
    public GameObject _MGWidgetPrefab;

    protected List<MGWidget> m_activeMGs;

    public Transform _MGPanelRoot;

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
    }

	// Use this for initialization
	void Start ()
    {
        Initialize();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    /// <summary>
    /// Reset to start-up conditions
    /// </summary>
    void Initialize()
    {
        if (m_activeMGs != null)
            m_activeMGs.Clear();
        m_activeMGs = new List<MGWidget>();
    }


    public MGWidget AddMG()
    {
        // TODO aherrera : create system of enums for premade Widgets?

        GameObject mgObject = GameObject.Instantiate(_MGWidgetPrefab);
        MGWidget mg_ref = mgObject.GetComponent<MGWidget>();
        m_activeMGs.Add(mg_ref);
        mg_ref.transform.SetParent(_MGPanelRoot);
        mgObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        mgObject.GetComponent<RectTransform>().localScale = Vector3.one;

        return mg_ref;
    }

    public void RemoveAllMG()
    {
        foreach(MGWidget mg in m_activeMGs)
        {
            mg.CloseMG();
        }

        m_activeMGs.Clear();
    }

}
