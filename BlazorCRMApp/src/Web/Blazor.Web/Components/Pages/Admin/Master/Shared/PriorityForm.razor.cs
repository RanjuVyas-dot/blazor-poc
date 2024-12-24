using Blazor.Web.Resources;
using Blazor.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Lib.Dto;

namespace Blazor.Web.Components.Pages.Admin.Master.Shared
{
    public partial class PriorityForm : ComponentBase
    {
        #region [Inject Service]
        [Inject] IMasterService MasterService { get; set; }
        [Inject] CustomNotificationService CustomNotification { get; set; } = default!;
        [Inject] DialogService DialogService { get; set; } = default!;
        #endregion
        [Parameter] public EventCallback OnSave { get; set; }

        [Parameter] public EventCallback OnCancel { get; set; }

        [Parameter]
        public PriorityResponseDto? editPriority { get; set; }


        public PriorityRequestDto priorityModal = new PriorityRequestDto();

        protected override void OnInitialized()
        {
            if (editPriority != null)
            {
                priorityModal.Id = editPriority.Id;
                priorityModal.Title = editPriority.Title;
                priorityModal.Color = editPriority.Color;
                priorityModal.IsActive = editPriority.IsActive;
            }
            else
            {
                priorityModal.Color = "#000000";
            }
            base.OnInitialized();
        }

        public async void Save()
        {
            try
            {
                var result = await DialogService.Confirm(
                   priorityModal.Id > 0 ? Resource.PRIORITY_UPDATE_CONFIRM_MESSGAE : Resource.PRIORITY_ADD_CONFIRM_MESSGAE,
                   priorityModal.Id > 0 ? Resource.PRIORITY_UPDATE_CONFIRM_TITLE : Resource.PRIORITY_ADD_CONFIRM_TITLE
                    );

                if (result.HasValue && result.Value) // User clicked "Yes"
                {
                    var response = await MasterService.ManagePriority(priorityModal);
                    if (response.IsSuccess)
                    {

                        CustomNotification.ShowNotification(NotificationSeverity.Success,
                        priorityModal.Id > 0 ? Resource.PRIORITY_UPDATE_SUCCESS_MESSAGE : Resource.PRIORITY_ADD_SUCCESS_MESSAGE
                        );
                        await OnSave.InvokeAsync();
                    }
                    else
                        CustomNotification.ShowNotification(NotificationSeverity.Error, response.Message);
                }
            }
            catch (Exception ex)
            {
                CustomNotification.ShowNotification(NotificationSeverity.Error, ex.Message);
            }
        }

        public async void CancelForm()
        {
            try
            {
                await OnCancel.InvokeAsync();
            }
            catch (Exception ex)
            {
                CustomNotification.ShowNotification(NotificationSeverity.Error, ex.Message);
            }
        }
    }
}
