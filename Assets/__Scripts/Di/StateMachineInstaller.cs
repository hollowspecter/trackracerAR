/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
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

        ARStateMachine aRStateMachine = new ARStateMachine ();
        root.AddState ( StateName.AR_SM, aRStateMachine );
        Container.Bind<IARStateMachine> ().To<ARStateMachine> ().FromInstance ( aRStateMachine );

        // AR StateMachine
        BuildStateMachine buildStateMachine = new BuildStateMachine ();
        aRStateMachine.AddState ( StateName.BUILD_SM, buildStateMachine );
        Container.Bind<IBuildStateMachine> ().To<BuildStateMachine> ().FromInstance ( buildStateMachine );

        RaceStateMachine raceStateMachine = new RaceStateMachine ();
        aRStateMachine.AddState ( StateName.RACE_SM, raceStateMachine );
        Container.Bind<IRaceStateMachine> ().To<RaceStateMachine> ().FromInstance ( raceStateMachine );

        // Build StateMachine
        BuildDialogState buildDialogState = new BuildDialogState ();
        buildStateMachine.AddState ( StateName.BUILD_DIALOG_STATE, buildDialogState );
        Container.Bind<IBuildDialogState> ().To<BuildDialogState> ().FromInstance ( buildDialogState );

        BuildPaintState buildPaintState = new BuildPaintState ();
        buildStateMachine.AddState ( StateName.BUILD_PAINT_STATE, buildPaintState );
        Container.Bind<IBuildPaintState> ().To<BuildPaintState> ().FromInstance ( buildPaintState );
        Container.QueueForInject ( buildPaintState );

        BuildEditorState buildEditorState = new BuildEditorState ();
        buildStateMachine.AddState ( StateName.BUILD_EDITOR_STATE, buildEditorState );
        Container.Bind<IBuildEditorState> ().To<BuildEditorState> ().FromInstance ( buildEditorState );

        BuildSaveState buildSaveState = new BuildSaveState ();
        buildStateMachine.AddState ( StateName.BUILD_SAVE_STATE, buildSaveState );
        Container.Bind<IBuildSaveState> ().To<BuildSaveState> ().FromInstance ( buildSaveState );

        BuildLoadState buildLoadState = new BuildLoadState ();
        buildStateMachine.AddState ( StateName.BUILD_LOAD_STATE, buildLoadState );
        Container.Bind<IBuildLoadState> ().To<BuildLoadState> ().FromInstance ( buildLoadState );

        // Race StateMachine
        RaceSetupState raceSetupState = new RaceSetupState ();
        raceStateMachine.AddState ( StateName.RACE_SETUP, raceSetupState );
        Container.Bind<IRaceSetupState> ().To<RaceSetupState> ().FromInstance ( raceSetupState );

        RacingState racingState = new RacingState ();
        raceStateMachine.AddState ( StateName.RACE_RACING, racingState );
        Container.Bind<IRacingState> ().To<RacingState> ().FromInstance ( racingState );

        RaceOverState raceOverState = new RaceOverState ();
        raceStateMachine.AddState ( StateName.RACE_OVER, raceOverState );
        Container.Bind<IRaceOverState> ().To<RaceOverState> ().FromInstance ( raceOverState );
    }
}