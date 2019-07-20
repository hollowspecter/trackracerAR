/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using Zenject;

/// <summary>
/// Copy of StateMachineInstaller but only with the racing states
/// </summary>
public class RaceTestStateMachineInstaller : Installer<RaceTestStateMachineInstaller>
{
    public override void InstallBindings()
    {
        // StateMachineInstaller
        Container.BindInterfacesTo<StateManager> ().AsSingle ().NonLazy ();

        // Root StateMachine
        RootStateMachine root = new RootStateMachine ();
        Container.Bind<IRootStateMachine> ().To<RootStateMachine> ().FromInstance (root);

        ARStateMachine aRStateMachine = new ARStateMachine ();
        root.AddState (StateName.AR_SM, aRStateMachine);
        Container.Bind<IARStateMachine> ().To<ARStateMachine> ().FromInstance (aRStateMachine);

        // AR StateMachine
        RaceStateMachine raceStateMachine = new RaceStateMachine ();
        aRStateMachine.AddState (StateName.RACE_SM, raceStateMachine);
        Container.Bind<IRaceStateMachine> ().To<RaceStateMachine> ().FromInstance (raceStateMachine);

        // Race StateMachine
        RaceSetupState raceSetupState = new RaceSetupState ();
        raceStateMachine.AddState (StateName.RACE_SETUP, raceSetupState);
        Container.Bind<IRaceSetupState> ().To<RaceSetupState> ().FromInstance (raceSetupState);
        Container.QueueForInject (raceSetupState);

        RacingState racingState = new RacingState ();
        raceStateMachine.AddState (StateName.RACE_RACING, racingState);
        Container.Bind<IRacingState> ().To<RacingState> ().FromInstance (racingState);
        Container.QueueForInject (racingState);

        RaceOverState raceOverState = new RaceOverState ();
        raceStateMachine.AddState (StateName.RACE_OVER, raceOverState);
        Container.Bind<IRaceOverState> ().To<RaceOverState> ().FromInstance (raceOverState);
        Container.QueueForInject (raceOverState);
    }
}
