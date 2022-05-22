using System.Reflection;

using ReactiveUI;

using Splat;

using UEFI.View.Views;

namespace UEFI.View.Services;

public static class DI
{
    public static T GetRequiredService<T>()
    {
        var service = Locator.Current.GetService<T>();

        if (service is null)
        {
            throw new InvalidOperationException($@"No service for type {typeof(T)} has been registered.");
        }

        return service;
    }

    public static void Register()
    {
        Locator.CurrentMutable.InitializeSplat();
        Locator.CurrentMutable.InitializeReactiveUI(RegistrationNamespace.Wpf);

        Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        Locator.CurrentMutable.RegisterConstant(new UEFIVariableService());
        Locator.CurrentMutable.RegisterConstant(new MainWindow());
    }
}