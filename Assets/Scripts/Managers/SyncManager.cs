using Data;
using Models;
using Photon.Pun;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class SyncManager : MonoBehaviourPunCallbacks
{
    [Inject] private GridModel _gridModel;

    private void Start()
    {
        gameObject.GetComponent<PhotonView>().ViewID = PhotonNetwork.AllocateViewID(999);
    }

    public void SyncGridData(CellData[,] matrix)
    {
        string jsonMatrix = JsonConvert.SerializeObject(matrix);
        photonView.RPC("UpdateGridDataRPC", RpcTarget.AllBuffered, jsonMatrix);
    }


    [PunRPC]
    public void UpdateGridDataRPC(string matrixJSON)
    {
        CellData[,] matrix = JsonConvert.DeserializeObject<CellData[,]>(matrixJSON);
        _gridModel.SetDataMatrix(matrix);
        Debug.Log("Grid data updated");
    }
}
