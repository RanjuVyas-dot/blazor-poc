using Blazor.API.Data.Entities;
using Blazor.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using Shared.Lib.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blazor.API.Services
{
    public interface IMasterService
    {
        #region [Designation Master]
        List<DesignationMaster> GetDesignations(Expression<Func<DesignationMaster, bool>> expression, int? OrderColumn, int? OrderDirection);
        DesignationMaster? GetDesignationById(int id);
        void AddUpdate(DesignationMaster CategoryModal);
        bool DeleteDesignationById(int id);
        bool UpdateDesignationStatus(int id, bool status);
        bool DesignationNameAvailabiltity(int? id, string name);
        #endregion

        #region [Priority Master]
        List<PriorityMaster> GetPriorities(Expression<Func<PriorityMaster, bool>> expression, int? OrderColumn, int? OrderDirection);
        PriorityMaster? GetPriorityById(int id);
        bool PriorityNameAvailabiltity(int? id, string name);
        void AddUpdatePriority(PriorityMaster PriorityModal);
        bool DeletePriorityById(int id);
        bool UpdatePriorityStatus(int id, bool status);
        #endregion

        #region [Opportunity Quality Manager]
        List<LeadQualityMaster> GetPriorities(Expression<Func<LeadQualityMaster, bool>> expression, int? OrderColumn, int? OrderDirection);
        LeadQualityMaster? GetLeadQualityById(int id);
        bool LeadQualityNameAvailabiltity(int? id, string name);
        void AddUpdateLeadQuality(LeadQualityMaster LeadQualityModal);
        bool DeleteLeadQualityById(int id);
        bool UpdateLeadQualityStatus(int id, bool status);
        #endregion

        #region[Country State City]
        List<CountryMaster> GetCountries();
        List<StateMaster> GetStates(int countryId);
        List<CityMaster> GetCities(int stateId);
        List<UserTimeZones> GetTimeZones();
        #endregion
    }
    public class MasterService : IMasterService
    {
        private readonly ApplicationDbContext _DBContext;

        public MasterService(ApplicationDbContext dbContext)
        {
            _DBContext = dbContext;
        }

        #region [Designation Master]

        public List<DesignationMaster> GetDesignations(Expression<Func<DesignationMaster, bool>> expression, int? OrderColumn, int? OrderDirection)
        {
            try
            {
                IQueryable<DesignationMaster> query = _DBContext.DesignationMaster;
                if (OrderColumn.HasValue)
                {
                    switch (OrderColumn.Value)
                    {
                        case 0:
                            query = query.OrderByDescending(m => m.Id);
                            break;
                        case 1:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.Name) : query.OrderByDescending(m => m.Name);
                            break;
                        case 2:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.Colour) : query.OrderByDescending(m => m.Colour);
                            break;
                        case 3:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.ModifiedDate) : query.OrderByDescending(m => m.ModifiedDate);
                            break;
                        case 4:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.IsActive) : query.OrderByDescending(m => m.IsActive);
                            break;
                    }
                }

                return query.Where(expression).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DesignationMaster? GetDesignationById(int id)
        {
            try
            {
                return _DBContext.DesignationMaster.FirstOrDefault(m => m.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DesignationNameAvailabiltity(int? id, string name)
        {
            try
            {
                bool available = true;
                if (id != null && id > 0)
                    available = _DBContext.DesignationMaster.Any(m => m.Name.ToLower().Trim() == name.ToLower().Trim() && m.Id != id && m.IsDeleted != true);
                else
                    available = _DBContext.DesignationMaster.Any(m => m.Name.ToLower().Trim() == name.ToLower().Trim() && m.IsDeleted == true);

                return available;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddUpdate(DesignationMaster designationModal)
        {
            try
            {
                var designation = _DBContext.DesignationMaster.FirstOrDefault(m => m.Id == designationModal.Id);
                if (designation != null)
                {
                    designation.Name = designationModal.Name;
                    designation.IsActive = designationModal.IsActive;
                    designation.ModifiedDate = DateTime.Now;
                    _DBContext.DesignationMaster.Update(designation);
                }
                else
                {
                    designationModal.CreatedDate = DateTime.Now;
                    designationModal.ModifiedDate = DateTime.Now;
                    designationModal.IsDeleted = false;
                    _DBContext.DesignationMaster.Add(designationModal);
                }
                _DBContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteDesignationById(int id)
        {
            try
            {
                var designation = _DBContext.DesignationMaster.FirstOrDefault(m => m.Id == id);
                if (designation != null)
                {
                    designation.IsDeleted = true;
                    designation.ModifiedDate = DateTime.Now;
                    _DBContext.DesignationMaster.Update(designation);
                    _DBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateDesignationStatus(int id, bool status)
        {
            try
            {
                var designation = _DBContext.DesignationMaster.FirstOrDefault(m => m.Id == id);
                if (designation != null)
                {
                    designation.IsActive = status;
                    designation.ModifiedDate = DateTime.Now;
                    _DBContext.DesignationMaster.Update(designation);
                    _DBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [Priority Master]

        public List<PriorityMaster> GetPriorities(Expression<Func<PriorityMaster, bool>> expression, int? OrderColumn, int? OrderDirection)
        {
            try
            {
                IQueryable<PriorityMaster> query = _DBContext.PriorityMaster;
                if (OrderColumn.HasValue)
                {
                    switch (OrderColumn.Value)
                    {
                        case 0:
                            query = query.OrderByDescending(m => m.Id);
                            break;
                        case 1:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.Title) : query.OrderByDescending(m => m.Title);
                            break;
                        case 2:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.Color) : query.OrderByDescending(m => m.Color);
                            break;
                        case 3:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.ModifiedDate) : query.OrderByDescending(m => m.ModifiedDate);
                            break;
                        case 4:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.IsActive) : query.OrderByDescending(m => m.IsActive);
                            break;
                    }
                }

                return query.Where(expression).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PriorityMaster? GetPriorityById(int id)
        {
            try
            {
                return _DBContext.PriorityMaster.FirstOrDefault(m => m.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool PriorityNameAvailabiltity(int? id, string name)
        {
            try
            {
                bool available = true;
                if (id != null && id > 0)
                    available = _DBContext.PriorityMaster.Any(m => m.Title.ToLower().Trim() == name.ToLower().Trim() && m.Id != id && m.IsDeleted != true);
                else
                    available = _DBContext.PriorityMaster.Any(m => m.Title.ToLower().Trim() == name.ToLower().Trim() && m.IsDeleted == true);

                return available;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddUpdatePriority(PriorityMaster PriorityModal)
        {
            try
            {
                var Priority = _DBContext.PriorityMaster.FirstOrDefault(m => m.Id == PriorityModal.Id);
                if (Priority != null)
                {
                    Priority.Title = PriorityModal.Title;
                    Priority.IsActive = PriorityModal.IsActive;
                    Priority.ModifiedDate = DateTime.Now;
                    _DBContext.PriorityMaster.Update(Priority);
                }
                else
                {
                    PriorityModal.CreatedDate = DateTime.Now;
                    PriorityModal.ModifiedDate = DateTime.Now;
                    PriorityModal.IsDeleted = false;
                    _DBContext.PriorityMaster.Add(PriorityModal);
                }
                _DBContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeletePriorityById(int id)
        {
            try
            {
                var Priority = _DBContext.PriorityMaster.FirstOrDefault(m => m.Id == id);
                if (Priority != null)
                {
                    Priority.IsDeleted = true;
                    Priority.ModifiedDate = DateTime.Now;
                    _DBContext.PriorityMaster.Update(Priority);
                    _DBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePriorityStatus(int id, bool status)
        {
            try
            {
                var Priority = _DBContext.PriorityMaster.FirstOrDefault(m => m.Id == id);
                if (Priority != null)
                {
                    Priority.IsActive = status;
                    Priority.ModifiedDate = DateTime.Now;
                    _DBContext.PriorityMaster.Update(Priority);
                    _DBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [Opportunity Quality Manager]

        public List<LeadQualityMaster> GetPriorities(Expression<Func<LeadQualityMaster, bool>> expression, int? OrderColumn, int? OrderDirection)
        {
            try
            {
                IQueryable<LeadQualityMaster> query = _DBContext.LeadQualityMaster;
                if (OrderColumn.HasValue)
                {
                    switch (OrderColumn.Value)
                    {
                        case 0:
                            query = query.OrderByDescending(m => m.Id);
                            break;
                        case 1:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.LeadQuality) : query.OrderByDescending(m => m.LeadQuality);
                            break;
                        case 2:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.ModifiedDate) : query.OrderByDescending(m => m.ModifiedDate);
                            break;
                        case 3:
                            query = OrderDirection!.Value == (int)Sorting.ASCENDING ? query.OrderBy(m => m.IsActive) : query.OrderByDescending(m => m.IsActive);
                            break;
                    }
                }

                return query.Where(expression).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LeadQualityMaster? GetLeadQualityById(int id)
        {
            try
            {
                return _DBContext.LeadQualityMaster.FirstOrDefault(m => m.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool LeadQualityNameAvailabiltity(int? id, string name)
        {
            try
            {
                bool available = true;
                if (id != null && id > 0)
                    available = _DBContext.LeadQualityMaster.Any(m => m.LeadQuality.ToLower().Trim() == name.ToLower().Trim() && m.Id != id && m.IsDeleted != true);
                else
                    available = _DBContext.LeadQualityMaster.Any(m => m.LeadQuality.ToLower().Trim() == name.ToLower().Trim() && m.IsDeleted == true);

                return available;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddUpdateLeadQuality(LeadQualityMaster LeadQualityModal)
        {
            try
            {
                var LeadQuality = _DBContext.LeadQualityMaster.FirstOrDefault(m => m.Id == LeadQualityModal.Id);
                if (LeadQuality != null)
                {
                    LeadQuality.LeadQuality = LeadQualityModal.LeadQuality;
                    LeadQuality.IsActive = LeadQualityModal.IsActive;
                    LeadQuality.ModifiedDate = DateTime.Now;
                    _DBContext.LeadQualityMaster.Update(LeadQuality);
                }
                else
                {
                    LeadQualityModal.CreatedDate = DateTime.Now;
                    LeadQualityModal.ModifiedDate = DateTime.Now;
                    LeadQualityModal.IsDeleted = false;
                    _DBContext.LeadQualityMaster.Add(LeadQualityModal);
                }
                _DBContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteLeadQualityById(int id)
        {
            try
            {
                var LeadQuality = _DBContext.LeadQualityMaster.FirstOrDefault(m => m.Id == id);
                if (LeadQuality != null)
                {
                    LeadQuality.IsDeleted = true;
                    LeadQuality.ModifiedDate = DateTime.Now;
                    _DBContext.LeadQualityMaster.Update(LeadQuality);
                    _DBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateLeadQualityStatus(int id, bool status)
        {
            try
            {
                var LeadQuality = _DBContext.LeadQualityMaster.FirstOrDefault(m => m.Id == id);
                if (LeadQuality != null)
                {
                    LeadQuality.IsActive = status;
                    LeadQuality.ModifiedDate = DateTime.Now;
                    _DBContext.LeadQualityMaster.Update(LeadQuality);
                    _DBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region[Country State City]
        public List<CountryMaster> GetCountries()
        {
            try
            {
                return _DBContext.CountryMaster.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StateMaster> GetStates(int countryId)
        {
            try
            {
                return _DBContext.StateMaster.Where(m => m.CountryId == countryId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CityMaster> GetCities(int stateId)
        {
            try
            {
                return _DBContext.CityMaster.Where(m => m.StateId == stateId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserTimeZones> GetTimeZones()
        {
            try
            {
                return _DBContext.UserTimeZones.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
