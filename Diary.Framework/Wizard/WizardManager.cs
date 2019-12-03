using System.ComponentModel.Composition;
using Caliburn.Micro;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Framework.Wizard
{
    [Export(typeof(WizardManager))]
    public class WizardManager
    {
        [Import(typeof(IWizard))] private WizardViewModel _wizardViewModel;

        public bool? Show(string parentId, object dataContext, string title = "")
        {
            return _wizardViewModel.CreateAndShowWizardDialog(parentId, title, dataContext);
        }

        public IScreen ShowInside(string parentId, string title = "")
        {
            return _wizardViewModel.GetWizardScreen(parentId, title);
        }
    }
}