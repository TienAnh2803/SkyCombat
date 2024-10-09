using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Linq;

public class NetworkHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefap;

    NetworkRunner networkRunner;
    void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefap);
        networkRunner.name = "Network runer";

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
        Debug.Log("server started");
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;
        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "test room",
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
