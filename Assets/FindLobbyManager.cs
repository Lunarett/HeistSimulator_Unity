using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class FindLobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _lobbySlotPrefab;
    [SerializeField] private Transform _lobbyListParent;
    [SerializeField] private NetworkManager _networkManager;

    public void RefreshLobby()
    {
        // Destroy previously spawned slots
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_lobbyListParent.GetChild(i).gameObject);
        }

        // Spawn lobby slots to list
        if (PhotonNetwork.InLobby && _networkManager.CachedRoomList != null)
        {
            foreach (var lobby in _networkManager.CachedRoomList.Values)
            {
                if (lobby.IsOpen)
                {
                    GameObject obj = Instantiate(_lobbySlotPrefab, _lobbyListParent);
                    obj.transform.SetParent(_lobbyListParent);
                    obj.GetComponent<SetLobbySlot>().UpdateValues(lobby.Name, lobby.PlayerCount, lobby.MaxPlayers, _networkManager);
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
}
