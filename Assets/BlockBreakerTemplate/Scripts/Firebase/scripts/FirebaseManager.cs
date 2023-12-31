using System;
using System.Threading.Tasks;
using BlockBreakerTemplate.Scripts.observer.scripts;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FirebaseManager : ObserverSubject
{
    private async UniTaskVoid Start()
    {
        Debug.Log("Fetching & Listening to data...");

        var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);

        await fetchTask.ContinueWithOnMainThread(FetchComplete);
        await UniTask.SwitchToMainThread();
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError(
                $"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
            .ContinueWithOnMainThread(
                task =>
                {
                    print($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                    var configGameOverValue = FirebaseRemoteConfig.DefaultInstance.GetValue("GameOverScreen");
                    var configWinValue = FirebaseRemoteConfig.DefaultInstance.GetValue("WinScreen");
                    NotifyObservers(ConfigUpdatesEnum.GameOverScreen, configGameOverValue);
                    NotifyObservers(ConfigUpdatesEnum.WinScreen, configWinValue);

                    // FIXME: Listener does not work
                    FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener += ConfigUpdateListenerEventHandler;
                });
    }

    private void ConfigUpdateListenerEventHandler(object sender, ConfigUpdateEventArgs args)
    {
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;

        if (args.Error != RemoteConfigError.None)
        {
            Debug.Log($"Error occurred while listening: {args.Error}");
            return;
        }

        Debug.Log("Updated keys: " + string.Join(", ", args.UpdatedKeys));

        // Activate all fetched values and then display a welcome message.
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log($"Remote data loaded and ready (last fetch time {remoteConfig.Info.FetchTime}).");
            Debug.Log(FirebaseRemoteConfig.DefaultInstance.GetValue("GameOverScreen").StringValue);
        });
    }

    // Stop the listener.
    private void OnDestroy()
    {
        FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
            -= ConfigUpdateListenerEventHandler;
    }
}