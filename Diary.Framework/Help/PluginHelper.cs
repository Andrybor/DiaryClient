using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Diary.Common;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Framework.Help
{
    public static class PluginHelper
    {
        public static void BuildPlugins(IEnumerable<Lazy<IDPlugin, IPluginMetaData>> _plugins,
            IObservableCollection<IDPlugin> items, string id)
        {
            items.Clear();

            var plugins = _plugins.Where(x => x.Metadata.ParentId == id)
                .OrderBy(x => x.Metadata.Priority);

            foreach (var v in plugins)
            {
                var screen = v.Value;
                if (screen is IDPlugin iScreen)
                {
                    iScreen.DisplayName = v.Metadata.Name.Translate();
                    iScreen.Panel.Icon = v.Metadata.Icon;
                    iScreen.Panel.Id = v.Metadata.Id;
                }

                items.Add(v.Value);
            }
        }

        public static string ValueOfIdAttribute<T>()
        {
            var attribute = typeof(T)
                .GetCustomAttributes(typeof(PluginAttribute), true)
                .FirstOrDefault() as PluginAttribute;

            if (attribute != null) return attribute.Id;

            return null;
        }

        public static IEnumerable<Lazy<IWizzardDPlugin, IWizzardMetaData>> GetPluginsByParentId(
            this IEnumerable<Lazy<IWizzardDPlugin, IWizzardMetaData>> _plugins, string id)
        {
            return _plugins.Where(x => x.Metadata.ParentId == id);
        }

        public static IEnumerable<Lazy<IDPlugin, IPluginMetaData>> GetPluginsByParentId(
            this IEnumerable<Lazy<IDPlugin, IPluginMetaData>> _plugins, string id)
        {
            return _plugins.Where(x => x.Metadata.ParentId == id);
        }
    }
}