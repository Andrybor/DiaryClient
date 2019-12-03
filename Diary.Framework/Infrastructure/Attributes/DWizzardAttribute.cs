using System;
using System.ComponentModel.Composition;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Framework.Infrastructure.Attributes
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class DWizzardAttribute : ExportAttribute, IWizzardMetaData
    {
        public DWizzardAttribute(
            string name,
            string id,
            string parentId,
            int priority = default(int)) : base(typeof(IWizzardDPlugin))
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            Priority = priority;
        }

        public string Id { get; }
        public string Name { get; }
        public string ParentId { get; }
        public int Priority { get; }
    }
}