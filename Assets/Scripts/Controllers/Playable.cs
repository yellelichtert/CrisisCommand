using System.Collections.Generic;
using UnityEngine;

public class Playable: MonoBehaviour
{
    private List<ICommand> commandQue = new List<ICommand>();

    
    public void AddCommandToQueue(ICommand command)
        => commandQue.Add(command);
    
    public void RemoveCommandFromQueue(ICommand command)
        => commandQue.Remove(command);

    
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        AddCommandToQueue(command);
    }

    
}