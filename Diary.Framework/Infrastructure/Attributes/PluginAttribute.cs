using System;
using System.ComponentModel.Composition;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Framework.Infrastructure.Attributes
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : ExportAttribute, IPluginMetaData
    {
        public PluginAttribute(
            string name,
            object id,
            object parentId,
            int priority = default(int),
            object icon = null) :
            base(typeof(IDPlugin))
        {
            Icon = icon;
            Id = id.ToString();
            Name = name;
            ParentId = parentId.ToString();
            Priority = priority;
        }

        public string Id { get; }
        public object Icon { get; }
        public string Name { get; }
        public string ParentId { get; }
        public int Priority { get; }
    }
}