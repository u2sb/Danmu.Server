using System.Collections.ObjectModel;
using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace Danmu.Utils.UI
{
    public class BindingEditableTabBase : ComponentBase
    {
        protected ObservableCollection<TabOption> Models = new ObservableCollection<TabOption>
        {
            new TabOption
            {
                Name = "tab1",
                Title = "概览",
                Content = "内容1",
                IsClosable = true
            }
        };

        protected BTab Tab;

        protected void OnAddingTabAsync()
        {
            Models.Add(new TabOption
            {
                Content = "内容" + Models.Count,
                IsClosable = true,
                Title = "标题" + Models.Count,
                IsActive = true
            });
            Tab.MarkAsRequireRender();
        }
    }
}
