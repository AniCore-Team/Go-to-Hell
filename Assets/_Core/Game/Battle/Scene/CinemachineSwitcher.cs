using System;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    public enum CinemachineState
    {
        Battle,
        Enemy,
        Player
    }

    private CinemachineState currentState = CinemachineState.Battle;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SwitchState(currentState);
    }

    public void SwitchState(CinemachineState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        animator.Play(currentState.ToString());
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentState = (CinemachineState)((int)(++currentState) % Enum.GetValues(typeof(CinemachineState)).Length);
            SwitchState(currentState);
            animator.Play(currentState.ToString());
        }
    }
#endif
}
