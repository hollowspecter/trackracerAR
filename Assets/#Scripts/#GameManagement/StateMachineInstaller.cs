using Zenject;

public class StateMachineInstaller : MonoInstaller
{
    public override void InstallBindings ()
    {
        RootStateMachine root = new RootStateMachine ();
        Container.Bind<RootStateMachine> ().FromInstance ( root );

        // layer 1
        BuildState buildState = new BuildState ();
        root.AddState ( StateName.TRACKBUILDER_BUILD, buildState );
        //Container.Bind<BuildState> ().FromInstance ( buildState );

        ARStateMachine aRStateMachine = new ARStateMachine ();
        root.AddState ( StateName.AR_SM, aRStateMachine );
        //Container.Bind<ARStateMachine> ().FromInstance ( aRStateMachine );

        // layer 2
        RaceSetupState raceSetupState = new RaceSetupState ();
        aRStateMachine.AddState ( StateName.TRACKRACER_SETUP, raceSetupState );
        //Container.Bind<RaceSetupState> ().FromInstance ( raceSetupState );

        RacingState racingState = new RacingState ();
        aRStateMachine.AddState ( StateName.TRACKRACER_RACING, racingState );
        //Container.Bind<RacingState> ().FromInstance ( racingState );

        RaceOverState raceOverState = new RaceOverState ();
        aRStateMachine.AddState ( StateName.TRACKRACER_OVER, raceOverState );
        //Container.Bind<RaceOverState> ().FromInstance ( raceOverState );
    }
}