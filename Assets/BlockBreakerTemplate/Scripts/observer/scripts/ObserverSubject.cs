using BlockBreakerTemplate.Scripts.observer.scripts;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.Events;

public class ObserverSubject : MonoBehaviour
{
    public UnityEvent<string> observers;

    protected void NotifyObservers(GameUpdates message, int value = 0)
    {
        var payload = $"{message}::{value}";
        observers?.Invoke(payload);
    }

    protected void NotifyObservers(ConfigUpdatesEnum key, ConfigValue message)
    {
        var payload = $"{key}::{message.StringValue}";
        observers?.Invoke(payload);
    }
}