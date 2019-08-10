/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;
using Baguio.Splines;

/// <summary>
/// Interface for <see cref="RaceSetupState"/>
/// </summary>
public interface IRaceSetupState {

    /// <summary>
    /// Transitions to the <see cref="BuildStateMachine"/>
    /// </summary>
    void OnBack();

    /// <summary>
    /// Transitions to the <see cref="RacingState"/>
    /// </summary>
    void OnStart();
}

/// <summary>
/// Manages the setup for the race and spawns a vehicle.
/// </summary>
public class RaceSetupState : State, IRaceSetupState
{
    private ISplineManager m_splineMgr;
    private StreetView m_streetView;
    private DialogBuilder.Factory m_dialogBuilderFactory;
    private VehicleSpawnManager m_vehicleManager;
    private TouchInput m_input;
    private SignalBus m_signalBus;
    private IBuildStateMachine m_buildSM;

    #region DI

    [Inject]
    private void Construct( [Inject (Id = "TrackParent")] ISplineManager _splineMgr,
                            [Inject (Id = "TrackParent")] StreetView _streetView,
                            DialogBuilder.Factory _dialogBuilderFactory,
                            VehicleSpawnManager _vehicleManager,
                            TouchInput _input,
                            SignalBus _signalBus,
                            IBuildStateMachine _buildSM )
    {
        m_streetView = _streetView;
        m_splineMgr = _splineMgr;
        m_dialogBuilderFactory = _dialogBuilderFactory;
        m_vehicleManager = _vehicleManager;
        m_input = _input;
        m_signalBus = _signalBus;
        m_buildSM = _buildSM;
    }

    #endregion

    #region State methods

    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceSetupState" );
        base.EnterState ();

        // Rebuild track once more before the race
        m_streetView.ToggleAppearance (false, () =>
        {
            m_splineMgr.GenerateTrackFromTrackData ();
            m_vehicleManager.SpawnVehicleAtStart ();
            m_streetView.ToggleAppearance (true, null);
        });

        // Reset the speed value;
        m_input.SetValue (0f);
    }

    #endregion

    #region Button callbacks

    public void OnBack()
    {
        if ( !Active ) return;

        m_dialogBuilderFactory.Create ()
            .SetTitle ("Exit the race?")
            .SetMessage ("Are you sure you would like to exit the race mode?")
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .AddButton ("Yes", () => {
                m_signalBus.Fire<DestroyVehicleSignal> ();
                m_buildSM.ReturnToPreviousStateFlag = true;
                m_stateMachine.TransitionToState (StateName.BUILD_SM);
            })
            .AddButton ("Keep Racing!")
            .Build ();
    }

    public void OnStart()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.RACE_RACING);
    }

    #endregion
}
