using Blazor.Web.Components.Shared;
using Blazor.Web.Resources;
using Blazor.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Lib.Dto;
using Shared.Lib.Enums;

namespace Blazor.Web.Components.Pages.Admin.Master
{
    public partial class Priority : ComponentBase
    {
        #region [Inject Service]
        [Inject] IMasterService MasterService { get; set; } = default!;
        [Inject] CustomNotificationService CustomNotification { get; set; } = default!;

        [Inject] DialogService DialogService { get; set; } = default!;
        #endregion

        #region [Variables]
        public List<PriorityResponseDto> priorities = new List<PriorityResponseDto>();
        public PriorityResponseDto? selectPriority = null;
        public bool process = false;
        public bool showForm = false;
        private string? _searchText = string.Empty;
        private DataListingResponseModel _listing = new();
        public int _orderColumn, _orderDirection, _pageSize = 10;
        #endregion
        protected override async Task OnInitializedAsync()
        {
            await GetPriority();
        }

        public async Task GetPriority(bool hasLoader = true)
        {
            try
            {
                process = true;
                var request = new ListingFilterDto
                {
                    PageIndex = _listing.PageNumber > 0 ? _listing.PageNumber : 1,
                    PageSize = _pageSize,
                    OrderColumn = _orderColumn,
                    OrderDirection = _orderDirection
                };

                if (!string.IsNullOrWhiteSpace(_searchText))
                    request.Search = _searchText;

                var result = await MasterService.GetPriorities(request);
                if (result.IsSuccess)
                {
                    if (result.HasDataAvailable())
                    {
                        var data = result.Data!;
                        _listing.FilterRecords = data.Items.Count();
                        _listing.HasNext = data.HasNext;
                        _listing.HasPrevious = data.HasPrevious;
                        _listing.TotalRecords = data.TotalCount;
                        _listing.FilterRecords = _listing.FilterRecords == _pageSize ? _listing.PageNumber * _listing.FilterRecords : (_listing.PageNumber - 1) * _pageSize + _listing.FilterRecords;
                        priorities = data.Items;
                    }
                }
                else CustomNotification.ShowNotification(NotificationSeverity.Error, result.Message);
            }
            catch (Exception ex)
            {
                CustomNotification.ShowNotification(NotificationSeverity.Error, ex.Message);
            }
            finally
            {
                process = false;
                StateHasChanged();
            }
        }

        private async void ChangePageIndex(bool hasNext)
        {
            try
            {
                var maxPageNumber = _listing.TotalRecords / _pageSize;
                maxPageNumber = _listing.TotalRecords % _pageSize == 0 ? maxPageNumber : ++maxPageNumber;
                _listing.PageNumber = hasNext ? (_listing.HasNext ? _listing.PageNumber + 1 : _listing.PageNumber) : (_listing.HasPrevious ? _listing.PageNumber - 1 : _listing.PageNumber);
                if (_listing.PageNumber > maxPageNumber)
                    _listing.PageNumber -= 1;
                else if (_listing.PageNumber == 0)
                    _listing.PageNumber = 1;
                else if (_listing.PageNumber <= maxPageNumber && _listing.PageNumber > 0)
                    await GetPriority(false);
            }
            catch { }
        }

        public async void UpdatePageSize(ChangeEventArgs e)
        {
            _pageSize = Convert.ToInt32(e.Value);
            await GetPriority();
        }

        public async Task OnSort(Sorting sort, int column)
        {
            _orderColumn = (int)column;
            _orderDirection = (int)sort;
            await GetPriority();
        }

        public void ShowCreateForm()
        {
            showForm = true;
        }
        public void CancelForm()
        {
            showForm = false;
        }
        public async Task OnFormSave()
        {
            showForm = false;
            await GetPriority();
        }

        private async Task DeletePriority(int id, bool confirm)
        {
            var result = await DialogService.Confirm(Resource.PRIORITY_DELETE_CONFIRM_TITLE, Resource.PRIORITY_DELETE_CONFIRM_MESSAGE);
            if (result.HasValue && result.Value) // User clicked "Yes"
            {
                var response = await MasterService.DeletePriority(id);
                if (response.IsSuccess)
                {
                    CustomNotification.ShowNotification(NotificationSeverity.Success, Resource.PRIORITY_DELETE_SUCCESS_MESSAGE);
                    await GetPriority();
                }
                else CustomNotification.ShowNotification(NotificationSeverity.Error, response.Message);
            }
        }

        public void EditPriority(PriorityResponseDto Priority)
        {
            selectPriority = Priority;
            showForm = true;
        }

        public async void UpdateStatus(PriorityResponseDto Priority)
        {
            try
            {
                var result = await DialogService.Confirm(Resource.PRIORITY_STATUS_CAHNGE_MESSAGE, Resource.PRIORITY_STATUS_CAHNGE_TITLE);

                if (result.HasValue && result.Value) // User clicked "Yes"
                {
                    var response = await MasterService.UpdatePriorityStatus(Priority.Id, !Priority.IsActive);
                    if (response.IsSuccess)
                    {
                        CustomNotification.ShowNotification(NotificationSeverity.Success,
                            Priority.IsActive ? Resource.PRIORITY_DEACTIVE_STATUS_MESSAGE.Replace("##PRIORITY_NAME##", Priority.Title) :
                             Resource.PRIORITY_ACTIVE_STATUS_MESSAGE.Replace("##PRIORITY_NAME##", Priority.Title)
                            );
                        await GetPriority();
                    }
                    else CustomNotification.ShowNotification(NotificationSeverity.Error, response.Message);
                }
            }
            catch (Exception ex)
            {
                CustomNotification.ShowNotification(NotificationSeverity.Error, ex.Message);
            }
        }
    }
}
