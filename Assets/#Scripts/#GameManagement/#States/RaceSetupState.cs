﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceSetupState : State
{
    protected override void Initialise ()
    {
    }

    public override void UpdateActive ( double _deltaTime )
    {
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceSetupState" );
    }

    public override void ExitState ()
    {
    }
}
