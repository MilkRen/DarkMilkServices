using LauncherDM.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LauncherDM.Infastructure.Commands;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.Views.UserControls;
using System.Windows.Media.Animation;
using System;

namespace LauncherDM.ViewModels
{
    internal class StoreUserControlViewModel : ViewModel.Base.ViewModel
    {

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        #endregion

        #region Binding

        public string ProgramText => _resourcesHelper.LocalizationGet("Programs");

        public string GamesText => _resourcesHelper.LocalizationGet("Games");

        #region ProgramsViewModel

        private ObservableCollection<ProgramsViewModel> _programsListView;

        public ObservableCollection<ProgramsViewModel> ProgramsListView
        {
            get => _programsListView;
            set => Set(ref _programsListView, value);
        }

        #endregion

        #region SelectItem

        private string _titleItem;

        public string TitleItem
        {
            get => _titleItem;
            set => Set(ref _titleItem, value);
        }

        private string _imageItem;

        public string ImageItem
        {
            get => _imageItem;
            set => Set(ref _imageItem, value);
        }

        private string _descItem;

        public string DescItem
        {
            get => _descItem;
            set => Set(ref _descItem, value);
        }

        private ObservableCollection<ProgramItemViewModel> _itemListView;

        public ObservableCollection<ProgramItemViewModel> ItemListView
        {
            get => _itemListView;
            set => Set(ref _itemListView, value);
        }


        #endregion

        #endregion

        // Todo: надо убрать это безобразие 
        public static Border ItemProgram;

        public StoreUserControlViewModel(ResourcesHelperService resourcesHelper)
        {
            _resourcesHelper = resourcesHelper;
            ProgramsListView = new ObservableCollection<ProgramsViewModel>();
            ItemListView = new ObservableCollection<ProgramItemViewModel>();
            int a = 0;
            while (a < 10)
            {
                var prog = new Programs()
                {
                   Title = "StrikeJo",
                   ImageMainPath = "https://static1.cbrimages.com/wordpress/wp-content/uploads/2024/02/every-joestar-in-jojo-s-bizarre-adventure.jpg?q=50&fit=contain&w=1140&h=&dpr=1.5",
                   Price = "4999",
                   Description = "Во все большем количестве российских изданий \u2212 как печатных, так и онлайновых \u2212 появляются объемные материалы особого типа, за которыми в журналистской среде закрепилось название «длинные тексты» (англ. – long forms) или лонгриды (от англ. \u2212 long read – материал, предназначенный для длительного прочтения, в отличие от маленькой заметки).\r\n\r\nСразу же следует оговориться, что объем материала – хотя и наиболее заметная, но не ключевая характеристика лонгрида. Объемными могут быть и материалы других жанров, поэтому сам по себе большой объем текста вовсе не означает, что перед нами лонгрид. Как будет показано в исследовании, лонгриды отличает также особый подход к выбору темы, требования к качеству собранной информации и способ подачи материала.\r\n\r\nВ исследовании предпринята попытка описать типологические характеристики лонгридов, разобрать особенности их подготовки, а также выявить распространенность лонгридов в современной российской прессе. Еще одной целью исследования является оценка перспектив этого жанра, о котором можно говорить если не как о сложившемся (в принятых на сегодняшний день в научной среде жанровых классификациях лонгрид отсутствует), то как о складывающемся и проникающем во все большее количество изданий."
                };

                ProgramsListView.Add(new ProgramsViewModel(prog, new LambdaCommand(o =>
                    {
                        TitleItem = prog.Title;
                        ImageItem = prog.ImageMainPath;
                        DescItem = prog.Description;

                        var buttonAnimation = new DoubleAnimation();
                        buttonAnimation.From = ItemProgram.ActualWidth;
                        buttonAnimation.To = 400;
                        buttonAnimation.Duration = TimeSpan.FromSeconds(1);
                        ItemProgram.BeginAnimation(Button.WidthProperty, buttonAnimation);

                    }, o => true)));
                a++;
            }

            a = 0;
            while (a < 10)
            {
                var prog = new Programs()
                {
                    Title = "StrikeJo",
                    ImageMainPath = "https://newcdn.igromania.ru/mnt/articles/9/3/c/8/9/a/32337/html/more/_fdd363ee92996e66c33571d_1920xH.webp",
                    Price = "4999",
                    Description = "Во все большем количестве российских изданий \u2212 как печатных, так и онлайновых \u2212 появляются объемные материалы особого типа, за которыми в журналистской среде закрепилось название «длинные тексты» (англ. – long forms) или лонгриды (от англ. \u2212 long read – материал, предназначенный для длительного прочтения, в отличие от маленькой заметки).\r\n\r\nСразу же следует оговориться, что объем материала – хотя и наиболее заметная, но не ключевая характеристика лонгрида. Объемными могут быть и материалы других жанров, поэтому сам по себе большой объем текста вовсе не означает, что перед нами лонгрид. Как будет показано в исследовании, лонгриды отличает также особый подход к выбору темы, требования к качеству собранной информации и способ подачи материала.\r\n\r\nВ исследовании предпринята попытка описать типологические характеристики лонгридов, разобрать особенности их подготовки, а также выявить распространенность лонгридов в современной российской прессе. Еще одной целью исследования является оценка перспектив этого жанра, о котором можно говорить если не как о сложившемся (в принятых на сегодняшний день в научной среде жанровых классификациях лонгрид отсутствует), то как о складывающемся и проникающем во все большее количество изданий."
                };

                if (ItemListView.Count < 5)
                    ItemListView.Add(new ProgramItemViewModel(prog.ImageMainPath, new LambdaCommand(o =>
                    {
                        ImageItem = prog.ImageMainPath;
                    }, o => true)));

                ProgramsListView.Add(new ProgramsViewModel(prog, new LambdaCommand(o =>
                {
                    TitleItem = prog.Title;
                    ImageItem = prog.ImageMainPath;
                    DescItem = prog.Description;

                    var buttonAnimation = new DoubleAnimation();
                    buttonAnimation.From = ItemProgram.ActualWidth;
                    buttonAnimation.To = 400;
                    buttonAnimation.Duration = TimeSpan.FromSeconds(1);
                    ItemProgram.BeginAnimation(Button.WidthProperty, buttonAnimation);

                }, o => true)));
                a++;
            }
        }
    }
}
