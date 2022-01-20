using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class FindLobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _lobbySlotPrefab;
    [SerializeField] private Transform _lobbyListParent;
    [SerializeField] private NetworkManager _networkManager;

    private List<GameObject> _spawnedSlots = new List<GameObject>();

    public void RefreshLobby()
    {
        // Destroy previously spawned slots
        if (_spawnedSlots.Count > 0)
        {
            for (int i = 0; i < _spawnedSlots.Count; i++)
            {
                Destroy(_spawnedSlots[i]);
            }
        }

        // Spawn lobby slots to list
        if (PhotonNetwork.InLobby && _networkManager.CachedRoomList != null)
        {
            foreach (var lobby in _networkManager.CachedRoomList.Values)
            {
                Debug.Log("Running foreach loop...");

                if (lobby.IsOpen)
                {
                    Debug.Log("Lobby Is Open! Spawning the stuff");

                    GameObject obj = Instantiate(_lobbySlotPrefab, _lobbyListParent);
                    obj.transform.SetParent(_lobbyListParent);
                    obj.GetComponent<SetLobbySlot>().UpdateValues(lobby.Name, lobby.PlayerCount, lobby.MaxPlayers, _networkManager);

                    _spawnedSlots.Add(obj);
                }
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"Room List Updated!   Time: {Time.time} Amount: {roomList.Count}");

        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                Debug.Log($"Removed Room: {room.Name}");
            }
            else
            {
                Debug.Log($"Added Room: {room.Name}");
            }
        }

        UpdateCachedRoomList(roomList);
        RefreshLobby();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                _networkManager.CachedRoomList.Remove(info.Name);
            }
            else
            {
                _networkManager.CachedRoomList[info.Name] = info;
            }
        }
    }

    public override void OnLeftLobby()
    {
        Debug.LogWarning("Left lobby!");
    }

    public override void OnConnected()
    {
        RefreshLobby();
    }
}
