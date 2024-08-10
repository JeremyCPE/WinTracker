using System.Windows;
using Wpf.Ui;

namespace WinTracker.Services
{
    public class PageService : IPageService
    {
        /// <summary>
        /// Service which provides the instances of pages.
        /// </summary>
        public readonly IServiceProvider _serviceProvider;

        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public T? GetPage<T>() where T : class
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException("The page should be a WPF control");
            }

            return (T?)_serviceProvider.GetService(typeof(T));
        }

        /// <inheritdoc />
        public FrameworkElement? GetPage(Type pageType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
            {
                throw new InvalidOperationException("The page should be a WPF control");
            }

            return _serviceProvider.GetService(pageType) as FrameworkElement;
        }
    }
}

