using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MaterialDesignThemes.Wpf;

namespace Diary.Student.Menu.MainStudentSubItems
{
    [Plugin("Homework",DPlugin.Homework,DPlugin.MainStudent,3,PackIconKind.AnimationOutline)]
    public class HomeWorkViewModel : DScreen
    {
        private List<Homework> _homeworks;

        public List<Homework> Homeworks
        {
            get => _homeworks;
            set
            {
                _homeworks = value;
                NotifyOfPropertyChange(()=> Homeworks);
            }
        }

        private async void Init()
        {
            var homeworks =
                await SimpleService.Get<List<Homework>>(Controller.Homework, Method.Get, SimpleService.LoggedUser.Id);
            if (homeworks != null)
            {
                Homeworks = homeworks;
            }
        }

        public void Download(object item)
        {
            if (item is Homework homework)
            {
                ByteArrayToFile(homework.Theme,homework.Task);
            }
        }
        public void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                string strPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                if (!File.Exists(strPath + @"\" + $"{fileName}"))
                {
                    // Create a file to write to.
                    using (StreamWriter streamWriter = File.CreateText(strPath + @"\" + $"{fileName}.txt"))
                    {
                        streamWriter.BaseStream.Write(byteArray, 0, byteArray.Length);
                    }
                }
            }
            catch(Exception ex) { }
        }
        protected override void OnActivate()
        {
            Init();

            base.OnActivate();
        }
    }
}
