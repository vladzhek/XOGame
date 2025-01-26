using System;
using System.Linq;
using Infastructure;
using Photon.Pun;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace View
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private Button _gotoLobbyBtn;

        [Inject] private StaticDataService _dataService;
        [Inject] private EventsService _eventsService;
        private void Awake()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            _gotoLobbyBtn.onClick.AddListener(EndSession);
        }
        
        private void OnDisable()
        {
            _gotoLobbyBtn.onClick.RemoveListener(EndSession);
        }

        private void Start()
        {
            var playerWithMaxPoints = _dataService.PlayerPoints
                .OrderByDescending(kvp => kvp.Value)
                .FirstOrDefault();

            _titleText.text = "WIINER\n" + playerWithMaxPoints.Key + " : " + playerWithMaxPoints.Value;
        }

        private void EndSession()
        {
            PhotonNetwork.LeaveRoom();
            _eventsService.OnEndGameInvoke();
        }
    }
}