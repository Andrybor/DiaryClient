using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Framework.Infrastructure.Base
{
    public class DependencyData : IDependencyData
    {
        public object Icon { get; set; }
        public string Id { get; set; }

        public void SetData(string id, object icon)
        {
            Id = id;
            Icon = icon;
        }
    }
}