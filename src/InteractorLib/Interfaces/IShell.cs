using InteractorLib.ViewModels;

namespace InteractorLib.Interfaces
{
    public interface IShell
    {
        void Show(string message);
        void ShowHtml(string page);
        void ShowHal(HalViewModel viewModel);

    }
}