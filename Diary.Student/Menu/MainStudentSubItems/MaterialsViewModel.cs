using System;
using System.Collections.Generic;
using System.Diagnostics;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;

namespace Diary.Student.Menu.MainStudentSubItems
{
    [Plugin("Materials", DPlugin.Materials, DPlugin.MainStudent, 4, PackIconKind.FileDocument)]
    public class MaterialsViewModel : DScreen
    {
        private List<Materials> _materials;

        public List<Materials> Materials
        {
            get => _materials;
            set
            {
                _materials = value;
                NotifyOfPropertyChange(() => Materials);
            }
        }

        private async void InitMaterials()
        {
            var materials = await SimpleService.Get<List<Materials>>(Controller.Material, Method.GetAll);
            if (materials != null)
            {
                Materials = materials;
            }

        }

        public void FileSearch(object item)
        {
            try
            {
                if (item is Materials material)
                    Process.Start(material.Text);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            InitMaterials();

        }
    }
}