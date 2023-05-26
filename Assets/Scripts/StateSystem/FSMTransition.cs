using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FSMTransition<T>
{
    public FSMTransition(
        Func<bool> isValid, 
        Func<FSMState<T>> getNextState
    ){
        IsValid = isValid;
        GetNextState = getNextState;
    }

    public Func<bool> IsValid { private set; get; }
    public Func<FSMState<T>> GetNextState { private set; get; }
}
