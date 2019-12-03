using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Diary.Framework;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.InfoSubItems
{
    [Plugin("AddMaterial", DPlugin.Library, DPlugin.Info, 1, PackIconModernKind.BookOpenText)]
    public class LibraryViewModel : DScreen
    {
        private Materials _material;
        private ObservableCollection<Materials> _materials;
        private ImageSource _photo;

        public ObservableCollection<Materials> Materials
        {
            get => _materials;
            set
            {
                _materials = value;
                NotifyOfPropertyChange(() => Materials);
            }
        }

        public Materials Material
        {
            get => _material;
            set
            {
                _material = value;
                NotifyOfPropertyChange(() => Material);
            }
        }

        public ImageSource Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                NotifyOfPropertyChange(nameof(Photo));
            }
        }

        public async void Create()
        {
            var photo = ImageHelper.ImageSourceToBytes(Photo);
            if (photo != null)
                Material.Image = photo;

            await SimpleService.Post(Controller.Material, Method.Create, Material);

            InitMaterials();
        }

        public async void Delete(object item)
        {
            if (item is Materials material)
            {
                await SimpleService.Post(Controller.Material, Method.Delete, material.Id);

                InitMaterials();
            }
        }

        private async void InitMaterials()
        {
            var allMaterials =
                await SimpleService.Get<ObservableCollection<Materials>>(Controller.Material, Method.GetAll);
            if (allMaterials != null) Materials = allMaterials;

            Material = new Materials();

            Photo = new BitmapImage();
        }

        protected override void OnActivate()
        {
            Photo = new BitmapImage();

            InitMaterials();

            base.OnActivate();
        }

        public void SetPhoto()
        {
            Photo = ImageHelper.OpenImageDialogRetImage();
        }
    }
}