using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Interfaces;
using Diary.LoadingScreen;
using Diary.ReportBugDioalog;
using Diary.Repositories;
using Diary.Repositories.Models;
using EventAggregator = Diary.Framework.Infrastructure.Common.EventAggregator;
using WindowManager = Diary.Framework.Infrastructure.Common.WindowManager;

namespace Diary
{
    public class AppBootstrapper : BootstrapperBase
    {
        public CompositionContainer container;
        

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var catalog =
                new AggregateCatalog(
                    AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>());
            container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();

            WindowManager.Current = new Caliburn.Micro.WindowManager();
            batch.AddExportedValue(WindowManager.Current);

            EventAggregator.Current = new Caliburn.Micro.EventAggregator();
            batch.AddExportedValue(EventAggregator.Current);
            batch.AddExportedValue(container);


            container.Compose(batch);
        }


        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            try
            {
                var assemblies = new List<Assembly>
                {
                    Assembly.GetExecutingAssembly(),
                    Assembly.Load("Diary.Framework"),
                    Assembly.Load("Diary.Controls"),
                    Assembly.Load("Diary.Admin"),
                    Assembly.Load("Diary.Student"),
                    Assembly.Load("Diary.Teacher"),
                    Assembly.Load("Diary.Resources"),
                    Assembly.Load("Diary.Repositories"),
                    Assembly.Load("Diary.Common")
                };
                // get external plugins
                var executingPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var path = executingPath + DllDynamicHelper._externalPluginsFolder;
                string[] externalAsseblies = null;
                if (Directory.Exists(path))
                {
                    externalAsseblies = Directory.GetFiles(path, "*.dll");
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                if (externalAsseblies != null && externalAsseblies.Length > 0)
                {
                    foreach (var ext in externalAsseblies)
                    {
                        var dll = Assembly.LoadFile(ext);
                        assemblies.Add(dll);
                    }
                }

                return assemblies;

            }
            catch (Exception ex)
            {

            }
            return new[]
            {
                Assembly.GetExecutingAssembly()
            };
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-GB");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            try
            {
                var loadingScreen = IoC.Get<ILoadingScreen>();

                loadingScreen.CloseLoadingScreen += () =>
                {
                    var login = IoC.Get<ILogin>();

                    login.LoginFinished += loged =>
                    {
                        var logedAccount = loged as Account;

                        if (true)
                        {
                            var loginVm = (LoginViewModel)login;
                            var loginView = loginVm.GetView() as Window;
                            loginView?.Hide();

                            var map = IoC.Get<IMap>();
                            var mapVm = (MapViewModel)map;
                            mapVm.LogedUser = logedAccount?.User;
                            SimpleService.LoggedUser = logedAccount?.User;
                            mapVm.AccountType = logedAccount?.AccountType.Type;

                            bool? result;

                            result = WindowManager.Current.ShowDialog(mapVm);

                            if (result == true)
                                loginView?.Show();
                            else
                            {
                                loginVm.TryClose();
                            }
                        }

                        return false;
                    };

                    var loginVM = (LoginViewModel)login;
                    try
                    {
                        WindowManager.Current.ShowDialog(loginVM);
                    }
                    catch (Exception ex) { }
                };

                WindowManager.Current.ShowDialog(loadingScreen);
            }
            catch (Exception exception)
            {
                CallBugsDialog(exception);
            }
        }

        private void CallBugsDialog(Exception exception)
        {
            BugReportDialogViewModel bugsDialog = new BugReportDialogViewModel();
            bugsDialog.Exception = exception.Message;

            dynamic settings = new ExpandoObject();
            settings.Title = "Bugs";
            settings.ResizeMode = ResizeMode.NoResize;
            settings.SizeToContent = SizeToContent.WidthAndHeight;
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            WindowManager.Current.ShowDialog(bugsDialog, null, settings);
        }
    }
}