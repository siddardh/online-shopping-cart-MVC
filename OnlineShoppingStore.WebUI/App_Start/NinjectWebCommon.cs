[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(OnlineShoppingStore.WebUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(OnlineShoppingStore.WebUI.App_Start.NinjectWebCommon), "Stop")]

namespace OnlineShoppingStore.WebUI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Moq;
    using OnlineShoppingStore.Domain.Abstract;
    using OnlineShoppingStore.Domain.Entities;
    using System.Collections.Generic;
    using OnlineShoppingStore.Domain.Concrete;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            kernel.Bind<IAuthentication>().To<FormsAuthenticationProvider>();
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m=>m.Products).Returns(new List<Product>{
            //new Product{Name = "FootBall",Price = 23 },
            //new Product{Name = "Surf Board",Price = 178 },
            //new Product{Name = "Running Shoes",Price = 98 }
            //});
            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);
        }        
    }
}
