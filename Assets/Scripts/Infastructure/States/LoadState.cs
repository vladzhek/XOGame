using Data;
using Models;
using Services;
using Zenject;

namespace Infastructure
{
    public class LoadState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        private StaticDataService _dataService;
        private GridModel _gridModel;
        private WindowService _windowService;

        [Inject]
        private void Construct(StaticDataService staticDataService, GridModel gridModel, WindowService windowService)
        {
            _dataService = staticDataService;
            _gridModel = gridModel;
            _windowService = windowService;
        }

        public LoadState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string scene)
        {
            InjectService.Instance.Inject(this);
            
            _gridModel.InitializeGridCells();
            
            _windowService.OpenWindow(WindowType.Lobby);
            _sceneLoader.Load(scene, OnLoaded);
        }

        private void OnLoaded()
        {
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {

        }
    }
}