/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using Zenject;
using UnityEngine;

public class StateMachineInstaller : Installer<StateMachineInstaller>
{
    public override void InstallBindings ()
    {
        // StateMachineInstaller
        Container.BindInterfacesTo<StateManager> ().AsSingle ().NonLazy ();

        // Root StateMachine
        RootStateMachine root = new RootStateMachine ();
        Container.Bind<IRootStateMachine> ().To<RootStateMachine> ().FromInstance ( root );

        CalibrateState calibrateState = new CalibrateState ();
        root.AddState (StateName.CALIBRATE_STATE, calibrateState);
        Container.Bind<ICalibrateState> ().To<CalibrateState> ().FromInstance (calibrateState);
        Container.QueueForInject (calibrateState);

        ARStateMachine aRStateMachine = new ARStateMachine ();
        root.AddState ( StateName.AR_SM, aRStateMachine );
        Container.Bind<IARStateMachine> ().To<ARStateMachine> ().FromInstance ( aRStateMachine );

        // AR StateMachine
        BuildStateMachine buildStateMachine = new BuildStateMachine ();
        aRStateMachine.AddState ( StateName.BUILD_SM, buildStateMachine );
        Container.Bind<IBuildStateMachine> ().To<BuildStateMachine> ().FromInstance ( buildStateMachine );
        Container.QueueForInject (buildStateMachine);

        RaceStateMachine raceStateMachine = new RaceStateMachine ();
        aRStateMachine.AddState ( StateName.RACE_SM, raceStateMachine );
        Container.Bind<IRaceStateMachine> ().To<RaceStateMachine> ().FromInstance ( raceStateMachine );

        // Build StateMachine
        BuildDialogState buildDialogState = new BuildDialogState ();
        buildStateMachine.AddState ( StateName.BUILD_DIALOG_STATE, buildDialogState );
        Container.Bind<IBuildDialogState> ().To<BuildDialogState> ().FromInstance ( buildDialogState );
        Container.QueueForInject (buildDialogState);

        BuildPaintState buildPaintState = new BuildPaintState ();
        buildStateMachine.AddState ( StateName.BUILD_PAINT_STATE, buildPaintState );
        Container.Bind<IBuildPaintState> ().To<BuildPaintState> ().FromInstance ( buildPaintState );
        Container.QueueForInject ( buildPaintState );

        BuildEditorState buildEditorState = new BuildEditorState ();
        buildStateMachine.AddState ( StateName.BUILD_EDITOR_STATE, buildEditorState );
        Container.Bind<IBuildEditorState> ().To<BuildEditorState> ().FromInstance ( buildEditorState );
        Container.QueueForInject ( buildEditorState );

        BuildSaveState buildSaveState = new BuildSaveState ();
        buildStateMachine.AddState ( StateName.BUILD_SAVE_STATE, buildSaveState );
        Container.Bind<IBuildSaveState> ().To<BuildSaveState> ().FromInstance ( buildSaveState );

        BuildLoadState buildLoadState = new BuildLoadState ();
        buildStateMachine.AddState ( StateName.BUILD_LOAD_STATE, buildLoadState );
        Container.Bind<IBuildLoadState> ().To<BuildLoadState> ().FromInstance ( buildLoadState );
        Container.QueueForInject ( buildLoadState );

        BuildObserveDialogState buildObserveDialogState = new BuildObserveDialogState ();
        buildStateMachine.AddState (StateName.BUILD_OBSERVE_DIALOG_STATE, buildObserveDialogState);
        Container.Bind<IBuildObserveDialogState> ().To<BuildObserveDialogState> ().FromInstance (buildObserveDialogState);

        BuildObserveState buildObserveState = new BuildObserveState ();
        buildStateMachine.AddState (StateName.BUILD_OBSERVE_STATE, buildObserveState);
        Container.Bind<IBuildObserveState> ().To<BuildObserveState> ().FromInstance (buildObserveState);
        Container.QueueForInject (buildObserveState);

        // Race StateMachine
        RaceSetupState raceSetupState = new RaceSetupState ();
        raceStateMachine.AddState ( StateName.RACE_SETUP, raceSetupState );
        Container.Bind<IRaceSetupState> ().To<RaceSetupState> ().FromInstance ( raceSetupState );
        Container.QueueForInject (raceSetupState);

        RacingState racingState = new RacingState ();
        raceStateMachine.AddState ( StateName.RACE_RACING, racingState );
        Container.Bind<IRacingState> ().To<RacingState> ().FromInstance ( racingState );
        Container.QueueForInject (racingState);

        RaceOverState raceOverState = new RaceOverState ();
        raceStateMachine.AddState ( StateName.RACE_OVER, raceOverState );
        Container.Bind<IRaceOverState> ().To<RaceOverState> ().FromInstance ( raceOverState );
        Container.QueueForInject (raceOverState);
    }
}