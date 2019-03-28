/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IBuildPaintState
{
    void OnCancel();
    void OnDone();
}

public class BuildPaintState : State, IBuildPaintState
{
    private Point3DFactory.Factory m_point3DFactory;
    private PointRecorder.Factory m_pointRecorderFactory;
    private IBuildStateMachine m_buildSM;
    private PointRecorder m_pointRecorder;
    private Transform m_trackTransform;

    #region Di

    [Inject]
    protected void Construct( Point3DFactory.Factory _point3DFactory,
                             PointRecorder.Factory _pointRecorderFactory,
                             [Inject ( Id = "TrackParent" )]Transform _trackTransform )
    {
        m_point3DFactory = _point3DFactory;
        m_pointRecorderFactory = _pointRecorderFactory;
        m_trackTransform = _trackTransform;
    }

    #endregion

    #region State Lifecycle

    protected override void Initialise()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState ();
        m_buildSM.m_touchDetected += OnTouchDetected;

        // TODO Create a Point Recorder and Call the Record Function for every Touch Input
        m_pointRecorder = m_pointRecorderFactory.Create ();
        m_pointRecorder.ThrowIfNull ( nameof ( m_pointRecorder ) );
    }

    public override void ExitState()
    {
        base.ExitState ();
        // Dispose of the Point Recorder here
        m_pointRecorder = null;
        m_buildSM.m_touchDetected -= OnTouchDetected;
    }

    #endregion

    #region Callbacks

    public void OnCancel()
    {
        if ( !m_active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void OnDone()
    {
        if ( !m_active ) return;

        // Dump the Points of the Point Recorder
        Vector3 [] points;
        m_pointRecorder.DumpPoints ( out points );
        Debug.LogFormat ("Recorded {0} points with the point recorder!", points.Length);

        // Create the Prefabs with the Factory at the Positions to a certain injected root object
        Transform currentPoint;
        for (int i =0; i< points.Length;++i )
        {
            currentPoint = m_point3DFactory.Create ( new Point3DFactory.Params () );
            currentPoint.position = points [ i ];
            currentPoint.parent = m_trackTransform;
        }

        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }

    private void OnTouchDetected( float x, float y )
    {
        m_pointRecorder.RecordPoint ();
    }

    #endregion
}
