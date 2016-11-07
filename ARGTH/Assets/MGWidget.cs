using UnityEngine;
using System.Collections;

// The individual minigame itself 
// Controls UI, input, and Win Conditions

public class MGWidget : MonoBehaviour
{
    public delegate void MGCompleteCallback(bool mgResult);
    public event MGCompleteCallback onMGFinished;

    public enum MGState
    {
        NOT_STARTED = 0,
        IN_PROGRESS,
        COMPLETE
    }
    protected MGState m_mgState;
    public bool Finished { get { return (m_mgState == MGState.COMPLETE); } }

    protected bool m_winConditionMet = false;
    public bool MGWon { get { return m_winConditionMet; } }

    [Tooltip("in seconds, how long until the MG is auto-closed")]
    public float _timeLimit = 5.0f;

    [Tooltip("in seconds, how long the MG should wait to close out")]
    public float _closingTime = 2.0f;

	// Use this for initialization
	void Start ()
    {
        InitializeMG();
        StartMG();
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateMG();
	}

    protected void InitializeMG()
    {
        m_mgState = MGState.NOT_STARTED;
    }

    protected void StartMG()
    {
        m_mgState = MGState.IN_PROGRESS;
        StartCoroutine("StartMGCountdown");
    }

    protected virtual IEnumerator StartMGCountdown()
    {
        yield return new WaitForSeconds(_timeLimit);
        if(!Finished)
            EndMG(false);
    }

    protected virtual void UpdateMG()
    {
        // TODO aherrera : this is where the win condition goes, and eventual cal to EndMG(result)

        if(Input.GetKeyDown(KeyCode.A))
        {
            EndMG(true);
        }

    }

    public virtual void EndMG(bool won = false)
    {
        m_winConditionMet = won;
        m_mgState = MGState.COMPLETE;
        StartCoroutine("StartMGClosing");
    }

    IEnumerator StartMGClosing()
    {
        float closingTime = ClosingOutAnimation();
        yield return new WaitForSeconds(closingTime);
        CloseMG();
    }

    /// <summary>
    /// Returns length of animation
    /// </summary>
    /// <returns></returns>
    protected virtual float ClosingOutAnimation()
    {
        float retVal = _closingTime;

        // play animation?

        if(!MGWon)
        {
            retVal = 0f;
        }

        return retVal;
    }

    public virtual void CloseMG()
    {
        // TODO aherrera : is this too late to restore input? or just right time?
            //  + what if this is not the right input...
        callMGCompleteEvent();

        // TODO aherrera : destroy I guess...
        DestroyObject(this.gameObject);

    }

    public void RewardPlayer(ref PlayerUnit player)
    {
        // TODO aherrera : hugs and kisses!
            //  event call?
    }

    public void PunishPlayer(ref PlayerUnit player)
    {
        // TODO aherrera : nails and death!
            //  event call?
    }
    #region Events
    protected void callMGCompleteEvent()
    {
        if(onMGFinished != null)
        {
            onMGFinished(m_winConditionMet);
        }
    }
    #endregion

}
