using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour
{
    public delegate void MoveUnitCallback(UnitScript unit);
    public event MoveUnitCallback onUnitMoved;


    private int m_currentRowCoordinate = -1, m_currentColumnCoordinate = -1;
    public int Row { get { return m_currentRowCoordinate; } }
    public int Column { get { return m_currentColumnCoordinate; } }
    public Vector2 Coordinates { get { return new Vector2(Row, Column); } }

    [Tooltip("Cooldown rate at which this Unit can do another action")]
    public float _updateFrequency = 1.5f;
    protected bool m_canUpdateUnit = true;

    //  TODO aherrera : should this variable be replaced with the overall manager bool? One that states whether Units should be autonomous or rougelike?
    protected bool m_doUpdate = false;

    protected bool m_hasMovedThisFrame = false;

    [SerializeField]
    [Tooltip("TRUE has Unit blocking Tile if occupying it")]
    private bool m_blocksTile = false;
    public bool BlockTile { get { return m_blocksTile; } }

    private FSMScript m_stateMachine;

    public BoxCollider _detectionCollider;
    public BoxCollider _bodyCollider;



    //  CAN UPDATE VARIABLES
    float m_canUpdateTimestamp;
    public float m_autonomousCooldownWait = 0;


	// Use this for initialization
	void Start ()
    {
        InitializeUnit();

        //  TODO aherrera : remove this check later... or refine error
        if(_detectionCollider == null || _bodyCollider == null)
        {
            Debug.LogError("UnitScript -- detection || body collider missing");
        }
    }
	
    protected virtual void InitializeUnit()
    {
        m_canUpdateTimestamp = Time.time;

        if (m_stateMachine == null)
        {
            m_stateMachine = this.GetComponent<FSMScript>();
        }

        m_stateMachine.ReInitialize();
    }

	// Update is called once per frame
	protected virtual void Update ()
    {
        UpdateInput();
        
        if (m_canUpdateUnit && m_doUpdate && (KnightContext.instance.AutonomousUnits ? true : !m_hasMovedThisFrame))
        {
            UpdateUnit();
            m_doUpdate = false;
            m_canUpdateUnit = false;
            m_hasMovedThisFrame = true;
            m_canUpdateTimestamp = Time.time;
        }
    }

    private void LateUpdate()
    {
        m_hasMovedThisFrame = false;
    }

    protected virtual void UpdateInput()
    {
        //  TODO aherrera : replace this condition with a function... I do not like it here
        m_doUpdate = KnightContext.instance.AutonomousUnits ? true : KnightContext.instance.updateUnits;
        m_canUpdateUnit = CanUpdate();
    }

    protected bool CanUpdate()
    {
        return (Time.time - m_canUpdateTimestamp > m_autonomousCooldownWait);
    }
    
    protected virtual void UpdateUnit()
    {
        if(m_stateMachine != null)
        {
            m_autonomousCooldownWait = m_stateMachine.UpdateActiveState();
        }
    }


    public void UpdateCoordinates(int row, int column)
    {
        m_currentRowCoordinate = row;
        m_currentColumnCoordinate = column;
        callMoveUnitEvent();
    }

    #region Events
    protected void callMoveUnitEvent()
    {
        if(onUnitMoved != null)
        {
            onUnitMoved(this);
        }
    }

    #endregion

    public void SetCanUpdate(bool value)
    {
        m_canUpdateUnit = value;
    }

    public void SetBlock(bool value)
    {
        m_blocksTile = value;
    }

    protected float GetActionReturnTime(float actionCD)
    {
        return (KnightContext.instance.AutonomousUnits ? actionCD : 0);
    }

    public void AddState(StateDelegate state)
    {
        m_stateMachine.Push(state);
    }

    public void PopState(StateDelegate specificState = null)
    {
        m_stateMachine.Pop(specificState);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject != this.gameObject)
        {
            TriggerEnter(collider);
        }
    }

    protected virtual void TriggerEnter(Collider collider)
    {

    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject != this.gameObject)
        {
            TriggerStay(collider);
        }
    }

    protected virtual void TriggerStay(Collider collider)
    {

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject != this.gameObject)
        {
            TriggerExit(collider);
        }
    }

    protected virtual void TriggerExit(Collider collider)
    {

    }

    public bool ColliderIsBody(Collider colliderInQuestion)
    {
        return (colliderInQuestion == _bodyCollider);
    }

}
