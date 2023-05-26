using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState<T>
{
    protected T mController;
    public List<FSMTransition<T>> Transitions;

    public FSMState(T controller)
    {
        mController = controller;
        Transitions = new List<FSMTransition<T>>();
    }

    public abstract void OnEnter();
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnExit();
}
