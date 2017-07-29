using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using JobLanes.WcfService.ModelBuilder;
using JobLanes.WcfService.Models;
using Model.JobLanes.Dto;
using Model.JobLanes.ViewModel;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Model.JobLanes.Entity;
using NHibernate;
using Services.Joblanes.Exceptions;

namespace JobLanes.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WebAdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WebAdminService.svc or WebAdminService.svc.cs at the Solution Explorer and start debugging.
    public class WebAdminService : ConnectionBase, IWebAdminService
    {
        #region Propertise & Object Initialization
        private readonly IRegionService _regionSevice;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IOrganizationTypeService _organizationTypeService;
        private readonly ICompanyTypeService _companyTypeService;
        private readonly IJobTypeService _jobTypeService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly IJobseekerService _jobseekerService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;

        public WebAdminService()
        {
            ISession session = NhSessionFactory.OpenSession();
            _regionSevice = new RegionService(session);
            _countryService = new CountryService(session);
            _stateService = new StateService(session);
            _cityService = new CityService(session);
            _organizationTypeService = new OrganizationTypeService(session);
            _companyTypeService = new CompanyTypeService(session);
            _jobTypeService = new JobTypeService(session);
            _jobCategoryService = new JobCategoryService(session);
            _paymentTypeService = new PaymentTypeService(session);
            _jobseekerService = new JobSeekerService(session);
            _companyService = new CompanyService(session);
            _userService = new UserService(session);
        }
        #endregion

        #region Common Helper
        #endregion

        #region Region

        public RegionDto GetRegionById(long id)
        {
            try
            {
                RegionDto data = _regionSevice.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegionRowCount(string name, int status)
        {
            try
            {
                return _regionSevice.RegionRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RegionDto> LoadRegion(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                List<RegionDto> dataList = _regionSevice.LoadRegion(start, length, orderBy, orderDir, name, status);
                List<RegionDto> returnDataList = dataList;

                //foreach (var data in dataList)
                //{
                //    returnDataList.Add(new RegionDto() { Name = data.Name, Status = data.Status });
                //}
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<RegionDto> LoadRegionByStatus(int status)
        {
            try
            {
                List<RegionDto> dataList = _regionSevice.LoadRegion(status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveRegion(RegionViewModel regionvm)
        {
            try
            {
                _regionSevice.Save(regionvm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public void DeleteRegion(long id)
        {
            try
            {
                _regionSevice.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        #endregion

        #region Country
        public void SaveCountry(CountryViewModel countryvm)
        {
            try
            {
                _countryService.Save(countryvm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public void DeleteCountry(long id)
        {
            try
            {
                _countryService.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public List<CountryDto> LoadCountry(int start, int length, string orderBy, string orderDir, string name, string shortName, string callingCode, long region, int status)
        {
            try
            {
                List<CountryDto> dataList = _countryService.LoadCountry(start, length, orderBy, orderDir, name, shortName, callingCode, region, status);

                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CountryDto> LoadCountryByCriteria(int? status = null, long? region = null)
        {
            try
            {
                List<CountryDto> dataList = _countryService.LoadCountry(status, region);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CountryDto GetCountryById(long id)
        {
            try
            {
                CountryDto data = _countryService.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CountryRowCount(string name, string shortName, string callingCode, long region, int status)
        {
            try
            {
                return _countryService.CountryRowCount(name, shortName, callingCode, region, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region City
        public void SaveCity(CityViewModel cityvm)
        {
            try
            {
                _cityService.Save(cityvm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public void DeleteCity(long id)
        {
            try
            {
                _cityService.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public List<CityDto> LoadCity(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, long state, int status)
        {
            try
            {
                List<CityDto> dataList = _cityService.LoadCity(start, length, orderBy, orderDir, name, shortName, region, country, state, status);

                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CityDto> LoadCityByCriteria(long? region = null, long? country = null, long? state = null, int? status = null)
        {
            try
            {
                List<CityDto> dataList = _cityService.LoadCity(region, country, state, status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CityDto GetCityById(long id)
        {
            try
            {
                CityDto data = _cityService.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CityRowCount(string name, string shortName, long region, long country, long state, int status)
        {
            try
            {
                return _cityService.CityRowCount(name, shortName, region, country, state, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region State
        public void SaveState(StateViewModel statevm)
        {
            try
            {
                _stateService.Save(statevm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public void DeleteState(long id)
        {
            try
            {
                _stateService.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public List<StateDto> LoadState(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, int status)
        {
            try
            {
                List<StateDto> dataList = _stateService.LoadState(start, length, orderBy, orderDir, name, shortName, region, country, status);

                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StateDto> LoadStateByCriteria(long? region = null, long? country = null, int? status = null)
        {
            try
            {
                List<StateDto> dataList = _stateService.LoadState(region, country, status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StateDto GetStateById(long id)
        {
            try
            {
                StateDto data = _stateService.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int StateRowCount(string name, string shortName, long region, long country, int status)
        {
            try
            {
                return _stateService.StateRowCount(name, shortName, region, country, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Organization Type
        public void SaveOrganizationType(OrganizationTypeViewModel organizationTypeVm)
        {
            try
            {
                _organizationTypeService.Save(organizationTypeVm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }

        }

        public void DeleteOrganizationType(long id)
        {
            try
            {
                _organizationTypeService.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public List<OrganizationTypeDto> LoadOrganizationType(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                List<OrganizationTypeDto> dataList = _organizationTypeService.LoadOrganizationType(start, length, orderBy, orderDir, name, status);

                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<OrganizationTypeDto> LoadOrganizationTypeByStatus(int status)
        {
            try
            {
                List<OrganizationTypeDto> dataList = _organizationTypeService.LoadOrganizationType(status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OrganizationTypeDto GetOrganizationTypeById(long id)
        {
            try
            {
                OrganizationTypeDto data = _organizationTypeService.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int OrganizationTypeRowCount(string name, int status)
        {
            try
            {
                return _organizationTypeService.OrganizationTypeRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Company Type
        public void SaveCompanyType(CompanyTypeViewModel companyTypeVm)
        {
            try
            {
                _companyTypeService.Save(companyTypeVm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }

        }

        public void DeleteCompanyType(long id)
        {
            try
            {
                _companyTypeService.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public List<CompanyTypeDto> LoadCompanyType(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                List<CompanyTypeDto> dataList = _companyTypeService.LoadCompanyType(start, length, orderBy, orderDir, name, status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CompanyTypeDto> LoadCompanyTypeByStatus(int status)
        {
            try
            {
                List<CompanyTypeDto> dataList = _companyTypeService.LoadCompanyType(status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CompanyTypeDto GetCompanyTypeById(long id)
        {
            try
            {
                CompanyTypeDto data = _companyTypeService.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CompanyTypeRowCount(string name, int status)
        {
            try
            {
                return _companyTypeService.CompanyTypeRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Job Type
        public void SaveJobType(JobTypeViewModel jobTypeVm)
        {
            try
            {
                _jobTypeService.Save(jobTypeVm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }

        }

        public void DeleteJobType(long id)
        {
            try
            {
                _jobTypeService.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public List<JobTypeDto> LoadJobType(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                List<JobTypeDto> dataList = _jobTypeService.LoadJobType(start, length, orderBy, orderDir, name, status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<JobTypeDto> LoadJobTypeByStatus(int status)
        {
            try
            {
                List<JobTypeDto> dataList = _jobTypeService.LoadJobType(status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobTypeDto GetJobTypeById(long id)
        {
            try
            {
                JobTypeDto data = _jobTypeService.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int JobTypeRowCount(string name, int status)
        {
            try
            {
                return _jobTypeService.JobTypeRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Job Category
        public void SaveJobCategory(JobCategoryViewModel jobCategoryVm)
        {
            try
            {
                _jobCategoryService.Save(jobCategoryVm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }

        }

        public void DeleteJobCategory(long id)
        {
            try
            {
                _jobCategoryService.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public List<JobCategoryDto> LoadJobCategory(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                List<JobCategoryDto> dataList = _jobCategoryService.LoadJobCategory(start, length, orderBy, orderDir, name, status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<JobCategoryDto> LoadJobCategoryByStatus(int status)
        {
            try
            {
                List<JobCategoryDto> dataList = _jobCategoryService.LoadJobCategory(status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobCategoryDto GetJobCategoryById(long id)
        {
            try
            {
                JobCategoryDto data = _jobCategoryService.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int JobCategoryRowCount(string name, int status)
        {
            try
            {
                return _jobCategoryService.JobCategoryRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Payment Type
        public void SavePaymentType(PaymentTypeViewModel paymentTypeVm)
        {
            try
            {
                _paymentTypeService.Save(paymentTypeVm);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }

        }

        public void DeletePaymentType(long id)
        {
            try
            {
                _paymentTypeService.Delete(id);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public List<PaymentTypeDto> LoadPaymentType(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                List<PaymentTypeDto> dataList = _paymentTypeService.LoadPaymentType(start, length, orderBy, orderDir, name, status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<PaymentTypeDto> LoadPaymentTypeByStatus(int status)
        {
            try
            {
                List<PaymentTypeDto> dataList = _paymentTypeService.LoadPaymentType(status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PaymentTypeDto GetPaymentById(long id)
        {
            try
            {
                PaymentTypeDto data = _paymentTypeService.GetById(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int PaymentTypeRowCount(string name, int status)
        {
            try
            {
                return _paymentTypeService.PaymentTypeRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion


        #region JobSeeker

        public List<JobSeekerDto> LoadOnlyJobSeeker(int? status = null)
        {
            try
            {
                List<JobSeekerDto> dataList = _jobseekerService.LoadOnlyJobSeeker(status);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobSeekerDto> LoadJobSeekerForWebAdmin(int start, int length, string orderBy, string orderDir, int? status = null,
            string firstName = "", string lastName = "", string contactMobile = "", string contactEmail = "", string zip = "",
            long? region = null, long? country = null, long? state = null, long? city = null)
        {
            try
            {
                List<JobSeekerDto> dataList = _jobseekerService.LoadJobSeekerForWebAdmin(start, length, orderBy, orderDir, status, firstName, lastName, contactMobile, contactEmail, zip,
                region, country, state, city);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int JobSeekerRowCount(int? status = null, string firstName = null, string lastName = "", string contactMobile = "",
            string contactEmail = "", string zip = "", long? region = null, long? country = null, long? state = null,
            long? city = null)
        {
            try
            {
                return _jobseekerService.JobSeekerRowCount(status, firstName, lastName, contactMobile, contactEmail, zip,
                region, country, state, city);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerDto GetJobSeekerById(long jobSeeker, int status)
        {
            try
            {
                return _jobseekerService.GetJobSeekerById(jobSeeker, status);
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerDto GetJobSeekerByAspId(string aspNetUser, int status)
        {
            try
            {
                return _jobseekerService.GetJobSeekerByAspId(aspNetUser, status);
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        #endregion

        #region Company

        #endregion

        #region WebAdmin
        #endregion

        #region UserProfile

        public UserProfileDto GetUserProfileDtoByAspId(string aspNetUser)
        {
            try
            {
                return _userService.GetByAspNetUserId(aspNetUser);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }

        public void BlockOrUnblockUser(string aspNetUser, bool isBlock)
        {
            try
            {
                _userService.BlockOrUnblockUser(aspNetUser, isBlock);
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
        }
        #endregion

    }
}
