using UnityEngine;
using UnityEngine.Events;

public class ObserverSubject : MonoBehaviour
{
    public UnityEvent<string> observers;

    protected void NotifyObservers(GameUpdates message, int value = 0)
    {
        var payload = $"{message}:{value}";
        print($"NOTIFY OBSERVERS {payload}; Observers: {observers}");
        observers?.Invoke(payload);
    }
}