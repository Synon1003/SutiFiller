using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Win32;
using SutiFiller.Admin.Model;
using SutiFiller.Admin.Persistence;
using SutiFiller.Admin.View;
using SutiFiller.Admin.ViewModel;

namespace SutiFiller.Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceModel _model;
        private MainViewModel _mainViewModel;
        private SutiEditorWindow _sutiEditorView;
        private OrderEditorWindow _orderEditorView;
        private CheckOrderWindow _checkOrderView;
        private CheckOrdersByMonthsWindow _checkOrdersByMonthsView;
        private HelpWindow _helpView;
        private MainWindow _mainView;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new ServiceModel(new ServicePersistence("https://localhost:7120/"));

            _mainViewModel = new MainViewModel(_model);
            _mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _mainViewModel.SelectedOrderShown += new EventHandler(MainViewModel_SelectedOrderShown);
            _mainViewModel.OrdersShownByMonths += new EventHandler(MainViewModel_OrdersShownByMonths);
            _mainViewModel.SutiEditingStarted += new EventHandler(MainViewModel_SutiEditingStarted);
            _mainViewModel.SutiEditingFinished += new EventHandler(MainViewModel_SutiEditingFinished);
            _mainViewModel.OrderEditingStarted += new EventHandler(MainViewModel_OrderEditingStarted);
            _mainViewModel.OrderEditingFinished += new EventHandler(MainViewModel_OrderEditingFinished);
            _mainViewModel.ImageEditingStarted += new EventHandler<SutiEventArgs>(MainViewModel_ImageEditingStarted);
            _mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _mainViewModel.OpenHelp += new EventHandler(ViewModel_OpenHelp);

            _mainView = new MainWindow();
            _mainView.DataContext = _mainViewModel;
            _mainView.Show();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Service", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_SelectedOrderShown(object sender, EventArgs e) {
            _checkOrderView = new CheckOrderWindow();
            _checkOrderView.DataContext = _mainViewModel;
            _checkOrderView.Show();
        }

        private void MainViewModel_OrdersShownByMonths(object sender, EventArgs e)
        {
            _checkOrdersByMonthsView = new CheckOrdersByMonthsWindow();
            _checkOrdersByMonthsView.DataContext = _mainViewModel;
            _checkOrdersByMonthsView.Show();
        }

        private void MainViewModel_SutiEditingStarted(object sender, EventArgs e)
        {
            _sutiEditorView = new SutiEditorWindow();
            _sutiEditorView.DataContext = _mainViewModel;
            _sutiEditorView.Show();
        }

        private void MainViewModel_OrderEditingFinished(object sender, EventArgs e)
        {
            _orderEditorView.Close();
        }

        private void MainViewModel_OrderEditingStarted(object sender, EventArgs e)
        {
            _orderEditorView = new OrderEditorWindow();
            _orderEditorView.DataContext = _mainViewModel;
            _orderEditorView.Show();
        }

        private void MainViewModel_SutiEditingFinished(object sender, EventArgs e)
        {
            _sutiEditorView.Close();
        }

        private void ViewModel_OpenHelp(object sender, EventArgs e)
        {
            _helpView = new HelpWindow();
            _helpView.DataContext = _mainViewModel;
            _helpView.Show();
        }

        private void ViewModel_ExitApplication(object sender, System.EventArgs e)
        {
            _mainView.Close();
            Shutdown();
        }

        private void MainViewModel_ImageEditingStarted(object sender, SutiEventArgs e)
        {
            if (e.SutiId == -1)
            {
                return;
            }
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.CheckFileExists = true;
                dialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Boolean? result = dialog.ShowDialog();

                if (result == true)
                {
                    _model.CreateImage(e.SutiId,
                                       ImageHandler.OpenAndResize(dialog.FileName, 120),
                                       ImageHandler.OpenAndResize(dialog.FileName, 720));
                }
            }
            catch { }
        }

    }

}
