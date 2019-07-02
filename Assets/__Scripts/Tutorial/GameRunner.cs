using UnityEngine;
using Zenject;

public class GameRunner : ITickable
{
    readonly Ship _ship;

    public GameRunner ( Ship ship )
    {
        _ship = ship;
    }

    public void Tick ()
    {
        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            _ship.TakeDamage ( 10 );
        }
    }
}