using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    public Transform _followTarget;
    [Tooltip("How long it takes the camera to Lerp to this target")]
    public float _lerpTime = 1f;
    private bool m_isLerping = false;
    private float m_timeStartedLerping;
    private Vector3 m_startPosition;
    private Vector3 m_endPosition;

    public float _y_height = 15f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void FixedUpdate()
    {
        if(m_isLerping)
        {
            float timeSinceStarted = Time.time - m_timeStartedLerping;
            float percentComplete = timeSinceStarted / _lerpTime;

            transform.position = Vector3.Lerp(m_startPosition, m_endPosition, percentComplete);

            if(percentComplete >= 1.0f)
            {
                m_isLerping = false;
            }
        }
    }

    /// <summary>
    /// Maintain a view above the position
    /// </summary>
    /// <param name="newPosition"></param>
    public void LerpTo(Vector3 newPosition)
    {
        m_isLerping = true;
        m_startPosition = this.transform.position;
        m_endPosition = newPosition;
        m_endPosition.y = _y_height;
        m_timeStartedLerping = Time.time;
    }

    public void LerpTowardsFollowTarget()
    {
        LerpTo(_followTarget.position);
    }

    public void SetFollowTarget(Transform newTarget, bool autoLerpTo = true)
    {
        // TODO aherrera : add check to see what exactly we are looking at? SHould this be a different data type?
        UnitScript unit = (_followTarget != null ? _followTarget.GetComponent<UnitScript>() : null);

        ClearFollowTarget();

        _followTarget = newTarget;
        unit = _followTarget.GetComponent<UnitScript>();
        if(unit != null)
        {
            unit.onUnitMoved += onFollowTargetMoved;
        }

        if(autoLerpTo)
        {
            onFollowTargetMoved(unit);
        }

    }

    public void ClearFollowTarget()
    {
        if (_followTarget != null)
        {
            UnitScript unit = _followTarget.GetComponent<UnitScript>();
            if (unit != null)
            {
                unit.onUnitMoved -= onFollowTargetMoved;
            }
            _followTarget = null;
        }
    }

    private void onFollowTargetMoved(UnitScript unit)
    {
        LerpTowardsFollowTarget();
    }
}
