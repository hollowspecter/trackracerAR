/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Baguio.Splines;

public interface IRaceSetupState {
    void OnBack();
    void OnStart();
}

public class RaceSetupState : State, IRaceSetupState
{
    private ISplineManager m_splineMgr;
    private StreetView m_streetView;
    private DialogBuilder.Factory m_dialogBuilderFactory;
    private VehicleManager m_vehicleManager;
    private GameObject m_vehicle;
    private TouchInput m_input;

    [Inject]
    private void Construct( [Inject (Id = "TrackParent")] ISplineManager _splineMgr,
                            [Inject (Id = "TrackParent")] StreetView _streetView,
                            DialogBuilder.Factory _dialogBuilderFactory,
                            VehicleManager _vehicleManager,
                            TouchInput _input)
    {
        m_streetView = _streetView;
        m_splineMgr = _splineMgr;
        m_dialogBuilderFactory = _dialogBuilderFactory;
        m_vehicleManager = _vehicleManager;
        m_input = _input;
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceSetupState" );
        base.EnterState ();

        // Rebuild track once more before the race
        m_streetView.ToggleAppearance (false, () =>
        {
            m_splineMgr.GenerateTrackFromTrackData ();
            m_streetView.ToggleAppearance (true, null);
        });

        // Reset the speed value;
        m_input.ResetValue ();

        // Spawn the Vehicle
        m_vehicle = m_vehicleManager.SpawnVehicleAtStart ();
    }

    public void OnBack()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Exit the race?")
            .SetMessage ("Are you sure you would like to exit the race mode?")
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .AddButton ("Yes", () => {
                m_stateMachine.TransitionToState (StateName.BUILD_SM);
                if ( m_vehicle != null ) {
                    Object.Destroy (m_vehicle);
                }
            })
            .AddButton ("Keep Racing!")
            .Build ();
    }

    public void OnStart()
    {
        m_stateMachine.TransitionToState (StateName.RACE_RACING);
    }
}
