using Infastructure;
using Models;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private WindowService _windowService;
        [SerializeField] private SyncManager _syncManager;

        public override void InstallBindings()
        {
            InjectService.Instance.SetContainer(Container);
            
            Container.Bind<WindowService>().FromInstance(_windowService).AsSingle();
            Container.Bind<SyncManager>().FromInstance(_syncManager).AsSingle();

            BindService();
            BindModel();
        }
    
        private void BindModel()
        {
            Container.Bind<GridModel>().AsSingle();
            Container.Bind<IServerModel>().To<LocalServerModel>().FromComponentInHierarchy().AsSingle();
        }
    
        private void BindService()
        {
            Container.Bind<StaticDataService>().AsSingle();
            Container.Bind<EventsService>().AsSingle();
        }
    }
}