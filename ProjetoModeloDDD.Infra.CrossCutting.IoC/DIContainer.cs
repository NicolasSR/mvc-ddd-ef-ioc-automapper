using ProjetoModeloDDD.Application;
using ProjetoModeloDDD.Application.Interface;
using ProjetoModeloDDD.Domain.Interfaces.Repositories;
using ProjetoModeloDDD.Domain.Interfaces.Services;
using ProjetoModeloDDD.Domain.Services;
using ProjetoModeloDDD.Infra.Data.Repositories;
using SimpleInjector;

namespace ProjetoModeloDDD.Infra.CrossCutting.IoC
{
    public static class DIContainer
    {
        public static Container RegisterDependencies()
        {
            var container = new Container();

            container.Register(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            container.Register<IClienteRepository, ClienteRepository>();
            container.Register<IProdutoRepository, ProdutoRepository>();

            container.Register(typeof(IServiceBase<>), typeof(ServiceBase<>));
            container.Register<IClienteService, ClienteService>();
            container.Register<IProdutoService, ProdutoService>();

            container.Register(typeof(IAppServiceBase<>), typeof(AppServiceBase<>));
            container.Register<IClienteAppService, ClienteAppService>();
            container.Register<IProdutoAppService, ProdutoAppService>();

            return container;
        }
    }
}
