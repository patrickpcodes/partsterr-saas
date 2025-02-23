using CommunityToolkit.Mvvm.Input;
using PartsterrSaas.Maui.App.Models;

namespace PartsterrSaas.Maui.App.PageModels;

public interface IProjectTaskPageModel
{
	IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
	bool IsBusy { get; }
}