/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Baguio.Splines;

public interface IRaceSetupState { }

public class RaceSetupState : State, IRaceSetupState
{
    private ISplineManager m_splineMgr;
    private StreetView m_streetView;

    [Inject]
    private void Construct( [Inject (Id = "TrackParent")] ISplineManager _splineMgr,
                            [Inject (Id = "TrackParent")] StreetView _streetView)
    {
        m_streetView = _streetView;
        m_splineMgr = _splineMgr;
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceSetupState" );
        base.EnterState ();

        // Rebuild track once more before the race
        m_streetView.ToggleAppearance (false, () =>
        {
            m_splineMgr.GenerateTrack ();
            m_streetView.ToggleAppearance (true, null);
        });
    }
}
