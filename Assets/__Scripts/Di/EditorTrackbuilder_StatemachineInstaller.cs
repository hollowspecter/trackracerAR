using UnityEngine;
using Zenject;

public class EditorTrackbuilder_StatemachineInstaller : MonoInstaller
{
    public override void InstallBindings ()
    {
        InstallMachines ();
    }

    private void InstallMachines ()
    {
        // Root StateMachine
        RootStateMachine root = new RootStateMachine ();
        Container.Bind<IRootStateMachine> ().To<RootStateMachine> ().FromInstance ( root );

        // AR StateMachine
        BuildStateMachine buildStateMachine = new BuildStateMachine ();
        root.AddState ( StateName.BUILD_SM, buildStateMachine );
        Container.Bind<IBuildStateMachine> ().To<BuildStateMachine> ().FromInstance ( buildStateMachine );

        // Build StateMachine
        BuildDialogState buildDialogState = new BuildDialogState ();
        buildStateMachine.AddState ( StateName.BUILD_DIALOG_STATE, buildDialogState );
        Container.Bind<IBuildDialogState> ().To<BuildDialogState> ().FromInstance ( buildDialogState );

        BuildStartState buildStartState = new BuildStartState ();
        buildStateMachine.AddState ( StateName.BUILD_PAINT_STATE, buildStartState );
        Container.Bind<IBuildStartState> ().To<BuildStartState> ().FromInstance ( buildStartState );

        BuildEditorState buildEditorState = new BuildEditorState ();
        buildStateMachine.AddState ( StateName.BUILD_EDITOR_STATE, buildEditorState );
        Container.Bind<IBuildEditorState> ().To<BuildEditorState> ().FromInstance ( buildEditorState );

        BuildSaveState buildSaveState = new BuildSaveState ();
        buildStateMachine.AddState ( StateName.BUILD_SAVE_STATE, buildSaveState );
        Container.Bind<IBuildSaveState> ().To<BuildSaveState> ().FromInstance ( buildSaveState );

        BuildLoadState buildLoadState = new BuildLoadState ();
        buildStateMachine.AddState ( StateName.BUILD_LOAD_STATE, buildLoadState );
        Container.Bind<IBuildLoadState> ().To<BuildLoadState> ().FromInstance ( buildLoadState );
    }
}