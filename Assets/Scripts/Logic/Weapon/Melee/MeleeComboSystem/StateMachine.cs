using Aarthificial.Reanimation;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string customName;

    private State mainStateType;

    public State CurrentState { get; private set; }
    public Animator animator { get; private set; }

    private State nextState;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        if (nextState != null)
        {
            SetState(nextState);
        }

        CurrentState?.OnUpdate();
    }

    private void SetState(State _newState)
    {
        nextState = null;
        CurrentState?.OnExit();
        CurrentState = _newState;
        CurrentState.OnEnter(this);
    }

    public void SetNextState(State _newState)
    {
        if (_newState != null)
        {
            nextState = _newState;
        }
    }

    private void LateUpdate()
    {
        CurrentState?.OnLateUpdate();
    }

    private void FixedUpdate()
    {
        CurrentState?.OnFixedUpdate();
    }

    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }

    private void Awake()
    {
        SetNextStateToMain();

    }


    private void OnValidate()
    {
        if (mainStateType == null)
        {
            if (customName == "Combat")
            {
                mainStateType = new IdleCombatState();
            }
        }
    }
}