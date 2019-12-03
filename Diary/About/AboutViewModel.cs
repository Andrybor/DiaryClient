using System.IO;
using System.Reflection;
using Diary.Framework.Infrastructure.Base;

namespace Diary.About
{
    public class AboutViewModel : DScreen
    {
        public string Text { get; set; }

        private void ReadLicenses()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = "Diary.About.licenses.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string result = reader.ReadToEnd();
                        Text = result;
                    }
                }
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            ReadLicenses();
        }
    }
}
