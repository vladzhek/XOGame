using Data;
using Models;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using View;
using Zenject;

namespace Infastructure
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        
        [Inject] private EventsService _eventsService;
        [Inject] private WindowService _windowService;
        [Inject] private StaticDataService _dataService;
        [Inject] private GridModel _gridModel;
        
        public GameLoopState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public void Enter()
        {
            InjectService.Instance.Inject(this);

            _eventsService.OnEndGame += EndGame;
        }

        public void Exit()
        {
            _eventsService.OnEndGame -= EndGame;
        }
        
        private void EndGame()
        {
            SceneManager.LoadSceneAsync("Game").completed += ReloadGame;
        }

        private void ReloadGame(AsyncOperation obj)
        {
            _windowService.CloseWindow(WindowType.EndGame);
            _windowService.OpenWindow(WindowType.Lobby);
            _dataService.RebutData();
            _gridModel.InitializeGridCells();
        }
    }
}