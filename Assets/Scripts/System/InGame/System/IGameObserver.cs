using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObserver
{
    public void NotifyPreparation();
    public void NotifyGameStart();
    public void NotifyGameOver();
}
