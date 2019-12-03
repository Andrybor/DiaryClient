namespace Diary.Framework.Infrastructure.Interfaces
{
    public interface IWizard
    {
        void GoTo(int index);

        void Next();

        void Back();
    }
}