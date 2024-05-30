using LauncherDM.ViewModels.UserControlsVM;
using System;
using System.Windows.Input;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.ViewModels
{
    internal class ImageItemWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private Action _dragMoveAction;

        #endregion

        #region Services

        private readonly IDialogWindowService _dialogWindow;

        #endregion

        #region Binding

        #region ImageItem

        private string _imageItem;

        public string ImageItem
        {
            get => _imageItem;
            set => Set(ref _imageItem, value);
        }

        #endregion

        #region ToolBar

        private ToolbarToWindowViewModel _toolbarVM;

        public ToolbarToWindowViewModel ToolbarVM
        {
            get => _toolbarVM;
            set => Set(ref _toolbarVM, value);
        }

        #endregion

        #endregion

        #region Command

        #region MoveWindowCommand

        public Command MoveWindowCommand { get; }
        private bool CanMoveWindowCommandExecute(object p) => true;
        private void OnMoveWindowCommandExecuted(object p)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _dialogWindow.DragMoveAction = _dragMoveAction;
                _dialogWindow.DragMoveWindow();
            }
        }

        #endregion

        #endregion

        public ImageItemWindowViewModel(string imagePath ,Action dragMove, ToolbarToWindowViewModel toolbarViewModel)
        {
            ImageItem = imagePath;
            _dragMoveAction = dragMove;
            ToolbarVM = toolbarViewModel;
            _dialogWindow = new DialogWindowService();
            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
        }
    }
}
