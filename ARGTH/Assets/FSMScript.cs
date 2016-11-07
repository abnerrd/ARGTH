using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Sources for this type of FSM
/// https://gamedevelopment.tutsplus.com/tutorials/finite-state-machines-theory-and-implementation--gamedev-11867
/// http://answers.unity3d.com/questions/925943/storing-a-method-in-a-variable.html
/// 
/// This class will serve as the brain for the AI.
/// It will execute functions that are set to the top of it's "states" stack.

//  Note: should always return GetActionReturnTime()
public delegate float StateDelegate();

public class FSMScript : MonoBehaviour
{
    List<StateDelegate> m_stateStack;

    public bool blockStateMachine = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void ReInitialize()
    {
        if(m_stateStack != null)
        {
            m_stateStack.Clear();

        }

        m_stateStack = new List<StateDelegate>();
    }

    public virtual float UpdateActiveState()
    {
        float thefloat = -1;

        if(!IsEmpty() && !blockStateMachine)
        {
            StateDelegate active_state = m_stateStack[m_stateStack.Count - 1];
            bool resumeLoop = true;
            while(resumeLoop)
            {
                resumeLoop = false;
 
                thefloat = active_state();

                if(active_state != m_stateStack[m_stateStack.Count - 1] && KnightContext.instance.AutonomousUnits)
                {
                    //  TODO aherrera : is there some sort of "exit state" function I should call?

                    Debug.Log("FSM -- Resuming FSM loop");
                    active_state = GetActiveState();
                    resumeLoop = true;
                }
            }
        }

        return thefloat;
    }

    public void Pop(StateDelegate specificState = null)
    {
        if(!IsEmpty())
        {
            if(specificState == null)
            {
                Debug.Log("FSM -- Popping off Active State");
                m_stateStack.RemoveAt(m_stateStack.Count - 1);
            }
            else
            {
                if(m_stateStack.Contains(specificState))
                {
                    Debug.Log("Popping off specific state: " + specificState.ToString());
                    m_stateStack.Remove(specificState);
                }
                else
                {
                    Debug.LogWarning("FSM -- Attempting to pop nonexistent State");
                }
            }
        }
        
    }

    /// <summary>
    /// Push a new state to the Top of the stack
    /// </summary>
    /// <param name="newActiveState"></param>
    public void Push(StateDelegate newActiveState)
    {
        if(GetActiveState() != newActiveState)
        {
            Debug.Log("FSM -- Adding new ActiveState: " + newActiveState.ToString());
            m_stateStack.Add(newActiveState);
        }
    }

    /// <summary>
    /// Insert a state a num of indexes away from the top. If 0, adds to the top
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="rangeFromTop"></param>
    public void Insert(StateDelegate newState, int rangeFromTop = 0)
    {
        int index = Mathf.Min(0, m_stateStack.Count - rangeFromTop - 1);
        m_stateStack.Insert(index, newState);
    }

    public bool IsEmpty()
    {
        return m_stateStack.Count <= 0;
    }

    public StateDelegate GetActiveState()
    {
        StateDelegate returnVal = null;

        if(m_stateStack.Count > 0)
        {
            returnVal = m_stateStack[m_stateStack.Count - 1];
        }

        if (returnVal == null) Debug.LogWarning("FSM -- returning a null ActiveState");
        return returnVal;
    }

}
