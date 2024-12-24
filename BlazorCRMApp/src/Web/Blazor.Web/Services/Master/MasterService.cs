using Shared.Lib.Dto;

namespace Blazor.Web.Services
{
    public interface IMasterService
    {
        #region [Designation]
        Task<BaseApiResponseDto<PageResponseViewModel<DesignationResponseDto>>> GetDesignations(ListingFilterDto model);
        Task<BaseApiResponseDto<DesignationResponseDto>> GetDesignationById(int id);
        Task<BaseApiResponseDto<object>> ManageDesignation(DesignationRequestDto model);
        Task<BaseApiResponseDto<bool>> DeleteDesignation(int id);
        Task<BaseApiResponseDto<bool>> UpdateDesignationStatus(int id, bool status);
        #endregion

        #region [Priority]
        Task<BaseApiResponseDto<PageResponseViewModel<PriorityResponseDto>>> GetPriorities(ListingFilterDto model);
        Task<BaseApiResponseDto<PriorityResponseDto>> GetPriorityById(int id);
        Task<BaseApiResponseDto<object>> ManagePriority(PriorityRequestDto model);
        Task<BaseApiResponseDto<bool>> DeletePriority(int id);
        Task<BaseApiResponseDto<bool>> UpdatePriorityStatus(int id, bool status);
        #endregion

        #region[Country State City]
        Task<BaseApiResponseDto<List<SelectListDto>>> GetCountries();
        Task<BaseApiResponseDto<List<SelectListDto>>> GetStates(int countryId);
        Task<BaseApiResponseDto<List<SelectListDto>>> GetCities(int stateId);
        #endregion
    }
    public class MasterService : IMasterService
    {
        private IHttpService httpService;

        public MasterService(HttpClient _httpClient, IHttpService _httpService)
        {
            httpService = _httpService;
            httpService.Client = _httpClient;
        }

        #region [Designation]
        public async Task<BaseApiResponseDto<PageResponseViewModel<DesignationResponseDto>>> GetDesignations(ListingFilterDto model)
        {
            return await httpService.PostAsync<PageResponseViewModel<DesignationResponseDto>>($"Master/GetDesignations", model);
        }
        public async Task<BaseApiResponseDto<DesignationResponseDto>> GetDesignationById(int id)
        {
            return await httpService.GetAsync<DesignationResponseDto>($"Master/GetDesignationById?id={id}");
        }
        public async Task<BaseApiResponseDto<object>> ManageDesignation(DesignationRequestDto model)
        {
            return await httpService.PostAsync<object>("Master/ManageDesignation", model);
        }

        public async Task<BaseApiResponseDto<bool>> DeleteDesignation(int id)
        {
            return await httpService.DeleteAsync<bool>($"Master/DeleteDesignation", id);
        }

        public async Task<BaseApiResponseDto<bool>> UpdateDesignationStatus(int id, bool status)
        {
            return await httpService.GetAsync<bool>($"Master/UpdateDesignationStatus?id={id}&status={status}");
        }
        #endregion

        #region [Priority]
        public async Task<BaseApiResponseDto<PageResponseViewModel<PriorityResponseDto>>> GetPriorities(ListingFilterDto model)
        {
            return await httpService.PostAsync<PageResponseViewModel<PriorityResponseDto>>($"Master/GetPriorities", model);
        }
        public async Task<BaseApiResponseDto<PriorityResponseDto>> GetPriorityById(int id)
        {
            return await httpService.GetAsync<PriorityResponseDto>($"Master/GetPriorityById?id={id}");
        }
        public async Task<BaseApiResponseDto<object>> ManagePriority(PriorityRequestDto model)
        {
            return await httpService.PostAsync<object>("Master/ManagePriority", model);
        }

        public async Task<BaseApiResponseDto<bool>> DeletePriority(int id)
        {
            return await httpService.DeleteAsync<bool>($"Master/DeletePriority", id);
        }

        public async Task<BaseApiResponseDto<bool>> UpdatePriorityStatus(int id, bool status)
        {
            return await httpService.GetAsync<bool>($"Master/UpdatePriorityStatus?id={id}&status={status}");
        }
        #endregion

        #region[Country State City]
        public async Task<BaseApiResponseDto<List<SelectListDto>>> GetCountries()
        {
            return await httpService.GetAsync<List<SelectListDto>>($"Master/GetCountries");
        }

        public async Task<BaseApiResponseDto<List<SelectListDto>>> GetStates(int countryId)
        {
            return await httpService.GetAsync<List<SelectListDto>>($"Master/GetStates?countryId={countryId}");
        }

        public async Task<BaseApiResponseDto<List<SelectListDto>>> GetCities(int stateId)
        {
            return await httpService.GetAsync<List<SelectListDto>>($"Master/GetCities?stateId={stateId}");
        }
        #endregion
    }
}
