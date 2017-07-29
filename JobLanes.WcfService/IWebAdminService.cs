using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using JobLanes.WcfService.Models;
using Model.JobLanes.Dto;
using Model.JobLanes.ViewModel;

namespace JobLanes.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWebAdminService" in both code and config file together.
    [ServiceContract]
    public interface IWebAdminService
    {
        #region Region
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveRegion(RegionViewModel regionvm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteRegion(long id);

        [OperationContract]
        List<RegionDto> LoadRegion(int start, int length, string orderBy, string orderDir, string name, int status);

        [OperationContract]
        List<RegionDto> LoadRegionByStatus(int status);

        [OperationContract]
        RegionDto GetRegionById(long id);

        [OperationContract]
        int RegionRowCount(string name, int status);

        #endregion

        #region Country
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveCountry(CountryViewModel countryvm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteCountry(long id);

        [OperationContract]
        List<CountryDto> LoadCountry(int start, int length, string orderBy, string orderDir, string name, string shortName, string callingCode, long region, int status);

        [OperationContract]
        List<CountryDto> LoadCountryByCriteria(int? status = null, long? region = null);

        [OperationContract]
        CountryDto GetCountryById(long id);

        [OperationContract]
        int CountryRowCount(string name, string shortName, string callingCode, long region, int status);

        #endregion

        #region State
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveState(StateViewModel statevm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteState(long id);

        [OperationContract]
        List<StateDto> LoadState(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, int status);

        [OperationContract]
        List<StateDto> LoadStateByCriteria(long? region = null, long? country = null, int? status = null);

        [OperationContract]
        StateDto GetStateById(long id);

        [OperationContract]
        int StateRowCount(string name, string shortName, long region, long country, int status);

        #endregion

        #region City
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveCity(CityViewModel cityvm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteCity(long id);

        [OperationContract]
        List<CityDto> LoadCity(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, long state, int status);

        [OperationContract]
        List<CityDto> LoadCityByCriteria(long? region = null, long? country = null, long? state = null, int? status = null);

        [OperationContract]
        CityDto GetCityById(long id);

        [OperationContract]
        int CityRowCount(string name, string shortName, long region, long country, long state, int status);

        #endregion

        #region Organization Type
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveOrganizationType(OrganizationTypeViewModel organizationTypeVm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteOrganizationType(long id);

        [OperationContract]
        List<OrganizationTypeDto> LoadOrganizationType(int start, int length, string orderBy, string orderDir, string name, int status);

        [OperationContract]
        List<OrganizationTypeDto> LoadOrganizationTypeByStatus(int status);

        [OperationContract]
        OrganizationTypeDto GetOrganizationTypeById(long id);

        [OperationContract]
        int OrganizationTypeRowCount(string name, int status);
        #endregion

        #region Company Type
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveCompanyType(CompanyTypeViewModel companyTypeVm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteCompanyType(long id);

        [OperationContract]
        List<CompanyTypeDto> LoadCompanyType(int start, int length, string orderBy, string orderDir, string name, int status);

        [OperationContract]
        List<CompanyTypeDto> LoadCompanyTypeByStatus(int status);

        [OperationContract]
        CompanyTypeDto GetCompanyTypeById(long id);

        [OperationContract]
        int CompanyTypeRowCount(string name, int status);
        #endregion

        #region Job Type
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveJobType(JobTypeViewModel jobTypeVm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteJobType(long id);

        [OperationContract]
        List<JobTypeDto> LoadJobType(int start, int length, string orderBy, string orderDir, string name, int status);

        [OperationContract]
        List<JobTypeDto> LoadJobTypeByStatus(int status);

        [OperationContract]
        JobTypeDto GetJobTypeById(long id);

        [OperationContract]
        int JobTypeRowCount(string name, int status);
        #endregion

        #region Job Category
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveJobCategory(JobCategoryViewModel jobCategoryVm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteJobCategory(long id);

        [OperationContract]
        List<JobCategoryDto> LoadJobCategory(int start, int length, string orderBy, string orderDir, string name, int status);

        [OperationContract]
        List<JobCategoryDto> LoadJobCategoryByStatus(int status);

        [OperationContract]
        JobCategoryDto GetJobCategoryById(long id);

        [OperationContract]
        int JobCategoryRowCount(string name, int status);
        #endregion

        #region Payment Type
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SavePaymentType(PaymentTypeViewModel paymentTypeVm);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeletePaymentType(long id);

        [OperationContract]
        List<PaymentTypeDto> LoadPaymentType(int start, int length, string orderBy, string orderDir, string name, int status);

        [OperationContract]
        List<PaymentTypeDto> LoadPaymentTypeByStatus(int status);

        [OperationContract]
        PaymentTypeDto GetPaymentById(long id);

        [OperationContract]
        int PaymentTypeRowCount(string name, int status);
        #endregion

        #region JobSeeker
        [OperationContract]
        List<JobSeekerDto> LoadOnlyJobSeeker(int? status = null);

        [OperationContract]
        List<JobSeekerDto> LoadJobSeekerForWebAdmin(int start, int length, string orderBy, string orderDir,
           int? status = null, string firstName = "", string lastName = "", string contactMobile = "", string contactEmail = "",
           string zip = "", long? region = null, long? country = null, long? state = null, long? city = null);

        [OperationContract]
        int JobSeekerRowCount(int? status = null, string firstName = null, string lastName = "", string contactMobile = "", string contactEmail = "",
            string zip = "", long? region = null, long? country = null, long? state = null, long? city = null);

        [OperationContract]
        JobSeekerDto GetJobSeekerById(long jobSeeker, int status);

        [OperationContract]
        JobSeekerDto GetJobSeekerByAspId(string aspNetUser, int status);
        #endregion

        #region Company

        #endregion

        #region WebAdmin
        #endregion

        #region UserProfile
        [OperationContract]
        UserProfileDto GetUserProfileDtoByAspId(string aspNetUser);

        [OperationContract]
        void BlockOrUnblockUser(string aspNetUser, bool isBlock);

        #endregion
    }
}
