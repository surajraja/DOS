using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace USLI.WCF.Services
{
    [ServiceContract]
    public interface IContactService
    {
        [OperationContract]
        void AddStaffToTeam(int teamId, int staffId);
        [OperationContract]
        string CreateExcelReport(string columndata, ObservableCollection<USLIContactAsignment> astList, int catlog);
        [OperationContract]
        string ExcelReport(string columndata, int iDeptid, int iUserType, int icatlog, int iProdType);
        [OperationContract]
        void AddWebStaffToTeam(int teamId, int staffId);
        [OperationContract]
        void DeleteSchool(int edId);
        [OperationContract]
        void DeleteStaffFromTeam(int teamId, int staffId);
        [OperationContract]
        void DeleteUserFifties(int userid, int agentid);
        [OperationContract]
        void DeleteUSLICert(int id);
        [OperationContract]
        void DeleteUSLITitle(int id);
        [OperationContract]
        void DeleteWebStaffFromTeam(int teamId, int staffId);
        [OperationContract]
        ObservableCollection<USLICertification> GetCerts();
        [OperationContract]
        ObservableCollection<GenericEntity> GetEducationType();
        [OperationContract]
        ObservableCollection<Conferenceroom> GetRooms();
        [OperationContract]
        ObservableCollection<USLIEducation> GetSchools();
        [OperationContract]
        USLIUserAddition GetStaffAdditions(int staffId);
        [OperationContract]
        ObservableCollection<USState> GetStates();
        [OperationContract]
        ObservableCollection<USLITeam> GetTeams();
        [OperationContract]
        ObservableCollection<USLIUser> GetTeamStaffs(int teamId);
        [OperationContract]
        ObservableCollection<USLITitle> GetTitles();
        [OperationContract]
        USLIUser GetUser(int userID, int workstationid);
        [OperationContract]
        System.Collections.Generic.List<User50> GetUserFifties();
        [OperationContract]
        Customer50ViewModel GetUserFiftyPickerView();
        [OperationContract]
        ObservableCollection<USLIUsers> GetUsers();
        [OperationContract]
        USLIUser GetWebRegionalContact(int userID);
        [OperationContract]
        ObservableCollection<USLIUser> GetWebRegionalContacts(int teamId, string statecode, int isUA);
        [OperationContract]
        ObservableCollection<USLITeam> GetWebRegionalTeams();
        [OperationContract]
        ObservableCollection<USLITeam> GetWebTeams();
        [OperationContract]
        ObservableCollection<USLIUser> GetWebTeamStaffs(int teamId);
        [OperationContract]
        void SaveStaff(USLIUser staff, USLIUserAddition stfAdition);
        [OperationContract]
        void SaveUserFifties(User50 user);
        [OperationContract]
        void SaveUSLICert(int id, string name);
        [OperationContract]
        void SaveUSLIEducation(USLIEducation ed);
        [OperationContract]
        void SaveUSLITitle(int id, string name);
        [OperationContract]
        bool SaveWebRegionalContact(int DeptID, string state, string ListSelected, string isUA);
        [OperationContract]
        ObservableCollection<USLIUsers> SearchUsers(string search);
        [OperationContract]
        ObservableCollection<USLIContactType> GetContactTypes();
        [OperationContract]
        ObservableCollection<USLITeam> GetContactDeparts();
        [OperationContract]
        USLIContactView GetContactView(int productid, int usertype, int catlog, int prodtypeid);
        [OperationContract]
        bool SaveDefaultContact(USLIContactIDs contids);
        [OperationContract]
        bool DeleteDefaultContact(USLIContactIDs contids);
        [OperationContract]
        void SaveAssignments(string ContactList);
        [OperationContract]
        void DeleteAssignments(string ContactList);
        [OperationContract]
        void DeleteContactAssignments(string ContactList);
        [OperationContract]
        USLIContactMainPikerView GetContactMainPikerView(int catlogid, int tmid);
        [OperationContract]
        USLIContactAsignmentDetailView GetContactDetailView(int userid);
        [OperationContract]
        void ReplaceContactAssignments(int source, int target);
        [OperationContract]
        ObservableCollection<USLITeam> GetProducts();
        
    }


}
