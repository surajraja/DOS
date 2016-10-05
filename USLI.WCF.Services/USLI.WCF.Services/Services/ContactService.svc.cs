using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using USLI.CommonLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.ServiceModel.Activation;

namespace USLI.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class ContactService : IContactService
    {

        public void AddStaffToTeam(int teamId, int staffId)
        {
            using (Database db = new Database())
            {
                string sql = "dbo.usp_INS_UserDepartment";
                db.AddInParameter("DeptID", System.Data.SqlDbType.Int, teamId);
                db.AddInParameter("UserID", System.Data.SqlDbType.Int, staffId);

                db.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, sql);
            }
        }

        public void AddWebStaffToTeam(int teamId, int staffId)
        {
            using (Database db = new Database())
            {
                string sql = "dbo.usp_INS_WebUserDepartment";
                db.AddInParameter("DeptID", System.Data.SqlDbType.Int, teamId);
                db.AddInParameter("UserID", System.Data.SqlDbType.Int, staffId);

                db.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, sql);
            }
        }

        public void DeleteSchool(int edId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("DELETE FROM dbo.USLIEducation WHERE EducationID = {0}", Convertion.ToString(edId));

            using (Database db = new Database())
            {
                db.ExecuteNonQuery(sb.ToString());
            }
        }

        public void DeleteStaffFromTeam(int teamId, int staffId)
        {
            using (Database db = new Database())
            {
                string sql = "dbo.usp_DEL_UserDepartment";
                db.AddInParameter("DeptID", System.Data.SqlDbType.Int, teamId);
                db.AddInParameter("UserID", System.Data.SqlDbType.Int, staffId);

                db.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, sql);
            }
        }

        public void DeleteUserFifties(int userid, int agentid)
        {
            string sql = "dbo.usp_DEL_User50";

            using (Database db = new Database())
            {
                db.AddInParameter("UserID", SqlDbType.Int, userid);
                db.AddInParameter("AgentID", SqlDbType.Int, agentid);
                db.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, sql);
            }
        }

        public void DeleteUSLICert(int id)
        {
            string sql = "DELETE FROM dbo.USLICertifications WHERE CertID = " + Convertion.ToString(id);

            using (Database db = new Database())
            {
                db.ExecuteNonQuery(sql);
            }
        }

        public void DeleteUSLITitle(int id)
        {
            string sql = "DELETE FROM dbo.USLITitle WHERE TitleID = " + Convertion.ToString(id);

            using (Database db = new Database())
            {
                db.ExecuteNonQuery(sql);
            }
        }

        public void DeleteWebStaffFromTeam(int teamId, int staffId)
        {
            using (Database db = new Database())
            {
                string sql = "dbo.usp_DEL_WebUserDepartment";
                db.AddInParameter("DeptID", System.Data.SqlDbType.Int, teamId);
                db.AddInParameter("UserID", System.Data.SqlDbType.Int, staffId);

                db.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, sql);
            }
        }

        public ObservableCollection<USLICertification> GetCerts()
        {
            ObservableCollection<USLICertification> Certs = new ObservableCollection<USLICertification>();

            using (Database db = new Database())
            {
                string sql = "SELECT  CertID, CertName FROM dbo.USLICertifications ORDER BY CertName";
                SqlDataReader dr = db.ExecuteReader(sql);

                while (dr.Read())
                {
                    Certs.Add(new USLICertification
                    {
                        CertID = Convertion.ToInt(dr["CertID"]),
                        CertName = Convertion.ToString(dr["CertName"]),
                        isChecked = false
                    });
                }
                dr.Close();
            }

            return Certs;
        }

        public ObservableCollection<GenericEntity> GetEducationType()
        {
            ObservableCollection<GenericEntity> items = new ObservableCollection<GenericEntity>();
            string sql = "SELECT   EducationTypeID, EducationType FROM dbo.USLIEducationType ORDER BY EducationType";

            using (Database db = new Database())
            {
                SqlDataReader dr = db.ExecuteReader(sql);

                while (dr.Read())
                {
                    items.Add(new GenericEntity
                    {
                        EntityID = Convertion.ToInt(dr["EducationTypeID"]),
                        EntityName = Convertion.ToString(dr["EducationType"])
                    });
                }
                dr.Close();
            }

            return items;
        }

        public ObservableCollection<Conferenceroom> GetRooms()
        {
            string s = AppDomain.CurrentDomain.BaseDirectory + "ConferenceRoom.xml";
            XDocument doc = XDocument.Load(s);

            var q = from i in doc.Descendants("room")
                    select new Conferenceroom
                    {
                        Name = (string)i.Attribute("name"),
                        Phone = (string)i.Attribute("phone"),
                        Floor = (int)i.Attribute("floor"),
                        x = (double)i.Attribute("x"),
                        y = (double)i.Attribute("y")
                    };

            ObservableCollection<Conferenceroom> Rooms = new ObservableCollection<Conferenceroom>();

            foreach (Conferenceroom tm in q)
            {
                Rooms.Add(tm);
            }

            return Rooms;
        }

        public ObservableCollection<USLIEducation> GetSchools()
        {
            ObservableCollection<USLIEducation> Schools = new ObservableCollection<USLIEducation>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  USLIEducation.EducationID, USLIEducation.EducationName, USLIEducation.EducationTypeID, USLIEducationType.EducationType ");
            sb.Append("FROM USLIEducation INNER JOIN ");
            sb.Append("USLIEducationType ON USLIEducation.EducationTypeID = USLIEducationType.EducationTypeID ");
            sb.Append("ORDER BY USLIEducation.EducationName ");

            using (Database db = new Database())
            {
                SqlDataReader dr = db.ExecuteReader(sb.ToString());

                while (dr.Read())
                {
                    Schools.Add(new USLIEducation
                    {
                        EducationID = Convertion.ToInt(dr["EducationID"]),
                        EducationName = Convertion.ToString(dr["EducationName"]),
                        EducationTypeID = Convertion.ToInt(dr["EducationTypeID"]),
                        EducationTypeName = Convertion.ToString(dr["EducationType"]),
                        isChecked = false
                    });
                }
                dr.Close();
            }

            return Schools;
        }

        public USLIUserAddition GetStaffAdditions(int staffId)
        {
            USLIUserAddition adtn = new USLIUserAddition();
            adtn.UserTitles = new ObservableCollection<USLITitle>();
            adtn.UserCerts = new ObservableCollection<USLICertification>();
            adtn.UserTeams = new ObservableCollection<USLITeam>();
            adtn.UserEducation = new ObservableCollection<USLIEducation>();
            adtn.UserUrl = string.Empty;

            using (Database db = new Database())
            {
                string sql = "dbo.usp_SEL_StaffAddition";
                db.AddInParameter("UserID", System.Data.SqlDbType.Int, staffId);
                SqlDataReader dr = db.ExecuteReader(System.Data.CommandType.StoredProcedure, sql);

                while (dr.Read())  // UserTitle
                {
                    adtn.UserTitles.Add(new USLITitle
                    {
                        TitleID = Convertion.ToInt(dr["TitleID"]),
                        TitleName = Convertion.ToString(dr["TitleName"])
                    });
                }

                dr.NextResult();
                while (dr.Read())  // UserCertifications
                {
                    adtn.UserCerts.Add(new USLICertification
                    {
                        CertID = Convertion.ToInt(dr["CertID"]),
                        CertName = Convertion.ToString(dr["CertName"])
                    });
                }

                dr.NextResult();
                while (dr.Read())  // UserDepartment
                {
                    adtn.UserTeams.Add(new USLITeam
                    {
                        TeamID = Convertion.ToInt(dr["DeptID"]),
                        TeamName = Convertion.ToString(dr["DeptName"])
                    });
                }

                dr.NextResult();
                while (dr.Read())  // UserExtendedInfo
                {
                    adtn.Floor = Convertion.ToInt(dr["FloorID"]);
                    adtn.WorkStation = Convertion.ToInt(dr["Workstation"]);
                    adtn.UserUrl = Convertion.ToString(dr["userlink"]);
                }

                dr.NextResult();
                while (dr.Read())  // UserExtendedInfo
                {
                    adtn.UserEducation.Add(new USLIEducation
                    {
                        EducationID = Convertion.ToInt(dr["EducationID"]),
                        EducationName = Convertion.ToString(dr["EducationName"])
                    });
                }

                dr.Close();
            }

            return adtn;
        }

        public ObservableCollection<USState> GetStates()
        {
            DataSet ds = Common.GetState();

            ObservableCollection<USState> states = new ObservableCollection<USState>();

            states.Add(new USState
            {
                StateID = 0,
                Name = string.Empty,
                ShortName = "Select a State"
            });

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                states.Add(new USState
                {
                    StateID = Convertion.ToInt(dr["StateID"]),
                    Name = Convertion.ToString(dr["LongState"]),
                    ShortName = Convertion.ToString(dr["State"])
                });
            }

            ds.Dispose();

            states.Add(new USState
            {
                StateID = 99,
                Name = string.Empty,
                ShortName = "All State"
            });

            return states;
        }

        public ObservableCollection<USLITeam> GetTeams()
        {
            ObservableCollection<USLITeam> Teams = new ObservableCollection<USLITeam>();

            using (Database db = new Database())
            {
                string sql = "SELECT  DeptID, DeptName FROM  dbo.USLIDepartment WHERE  (DeptID > 0 AND DeptID <> 53) ORDER BY DeptName";
                SqlDataReader dr = db.ExecuteReader(sql);

                while (dr.Read())
                {
                    Teams.Add(new USLITeam
                    {
                        TeamID = Convertion.ToInt(dr["DeptID"]),
                        TeamName = Convertion.ToString(dr["DeptName"]),
                        isChecked = false
                    });
                }
                dr.Close();
            }

            return Teams;
        }

        public ObservableCollection<USLIUser> GetTeamStaffs(int teamId)
        {
            ObservableCollection<USLIUser> staffs = new ObservableCollection<USLIUser>();

            using (Database db = new Database())
            {
                string sql = "dbo.usp_SEL_DepartUsers";
                db.AddInParameter("DepartmentID", System.Data.SqlDbType.Int, teamId);

                SqlDataReader dr = db.ExecuteReader(System.Data.CommandType.StoredProcedure, sql);

                string sImage = string.Empty;

                while (dr.Read())
                {
                    sImage = @"http://customers.usli.com/i/USLIteam/" + Convertion.ToString(dr["UserImage"]) + ".jpg";

                    staffs.Add(new USLIUser
                    {
                        UserID = Convertion.ToInt(dr["UserID"]),
                        FirstName = Convertion.ToString(dr["FirstName"]),
                        LastName = Convertion.ToString(dr["LastName"]),
                        Email = Convertion.ToString(dr["Email"]),
                        PhoneNumber = Convertion.ToString(dr["PhoneNumber"]),
                        PhoneExtension = Convertion.ToString(dr["PhoneExtension"]),
                        UserImage = sImage,
                        Title = Convertion.ToString(dr["TitleName"]),
                        Certification = Convertion.ToString(dr["Cert"]),
                        Department = Convertion.ToString(dr["Dept"]),
                        School = Convertion.ToString(dr["School"])                        
                    });
                }
                dr.Close();
            }

            return staffs;
        }

        public ObservableCollection<USLITitle> GetTitles()
        {
            ObservableCollection<USLITitle> titles = new ObservableCollection<USLITitle>();
            using (Database db = new Database())
            {
                string sql = "SELECT  TitleID, TitleName FROM dbo.USLITitle ORDER BY TitleName";
                SqlDataReader dr = db.ExecuteReader(sql);

                while (dr.Read())
                {
                    titles.Add(new USLITitle
                    {
                        TitleID = Convertion.ToInt(dr["TitleID"]),
                        TitleName = Convertion.ToString(dr["TitleName"]),
                        isChecked = false
                    });
                }
                dr.Close();
            }

            return titles;
        }

        public USLIUser GetUser(int userID, int workstationid)
        {
            USLIUser user = new USLIUser();

            using (Database db = new Database())
            {
                string sql = "dbo.usp_SEL_CompanyUserDetail";
                db.AddInParameter("UserID", System.Data.SqlDbType.Int, userID);

                SqlDataReader dr = db.ExecuteReader(System.Data.CommandType.StoredProcedure, sql);

                while (dr.Read())
                {
                    user.UserID = userID;
                    user.FirstName = Convertion.ToString(dr["FirstName"]);
                    user.LastName = Convertion.ToString(dr["LastName"]);
                    user.Email = Convertion.ToString(dr["Email"]);
                    user.PhoneNumber = Convertion.ToString(dr["PhoneNumber"]);
                    user.PhoneExtension = Convertion.ToString(dr["PhoneExtension"]);
                    user.UserImage = Convertion.ToString(dr["UserImage"]);
                    user.Title = Convertion.ToString(dr["TitleName"]);
                    user.Certification = Convertion.ToString(dr["Cert"]);
                    user.Department = Convertion.ToString(dr["Dept"]);
                    user.School = Convertion.ToString(dr["School"]);
                    user.StartDate = Convertion.ToShortDateTime(Convertion.ToString(dr["StartDate"]));
                }

                if (workstationid > 0 && user.UserID > 0)
                {
                    dr.NextResult();
                    while (dr.Read())
                    {
                        user.UserFloor = Convertion.ToInt(dr["FloorID"]);
                        user.LocationX = Convertion.ToDouble(dr["x"]);
                        user.LocationY = Convertion.ToDouble(dr["y"]);
                    }
                }
                dr.Close();
            }

            return user;

        }

        public List<User50> GetUserFifties()
        {
            List<User50> userfifties = new List<User50>();

            int iStatus = 14;

            using (Database db = new Database())
            {
                SqlDataReader dr = db.ExecuteReader(CommandType.StoredProcedure, "dbo.usp_SEL_User50");

                while (dr.Read())
                {
                    iStatus = Convertion.ToInt(dr["Status"]);
                    userfifties.Add(new User50
                    {
                        UserID = Convertion.ToInt(dr["UserID"]),
                        FirstName = Convertion.ToString(dr["FirstName"]),
                        LastName = Convertion.ToString(dr["LastName"]),
                        CustID = Convertion.ToInt(dr["AgentID"]),
                        HireDate = Convertion.ToShortDateTime(Convertion.ToString(dr["HireDate"])),
                        City = Convertion.ToString(dr["City"]),
                        State = Convertion.ToString(dr["State"]),
                        CustName = Convertion.ToString(dr["AGENT"]),
                        Status = iStatus,
                        StatusDesc = GetStatusDesc(iStatus)

                    });
                }
                dr.Close();

            }

            return userfifties;
        }

        public Customer50ViewModel GetUserFiftyPickerView()
        {
            Customer50ViewModel userfifties = new Customer50ViewModel();
            userfifties.Customers = new List<GenericEntity>();
            userfifties.States = new List<GenericEntity>();

            using (Database db = new Database())
            {
                SqlDataReader dr = Common.GetActiveCustomerList();

                while (dr.Read())
                {
                    userfifties.Customers.Add(new GenericEntity
                    {
                        EntityID = Convertion.ToInt(dr["AgentID"]),
                        EntityName = Convertion.ToString(dr["AGENT"])
                    });
                }
                dr.Close();

                DataSet ds = Common.GetState();
                foreach (DataRow rw in ds.Tables[0].Rows)
                {
                    userfifties.States.Add(new GenericEntity
                    {
                        EntityID = Convertion.ToInt(rw["StateID"]),
                        EntityName = Convertion.ToString(rw["State"])
                    });
                }
                ds.Dispose();
            }

            userfifties.Status = new List<GenericEntity>();

            userfifties.Status.Add(new GenericEntity
            {
                EntityID = 14,
                EntityName = "Active"
            });
            userfifties.Status.Add(new GenericEntity
            {
                EntityID = 5,
                EntityName = "Cancelled"
            });

            return userfifties;
        }

        public ObservableCollection<USLIUsers> GetUsers()
        {
            ObservableCollection<USLIUsers> users = new ObservableCollection<USLIUsers>();
            using (Database db = new Database())
            {

                SqlDataReader dr = db.ExecuteReader(System.Data.CommandType.StoredProcedure, "dbo.usp_SEL_UserWorkStation");

                while (dr.Read())
                {
                    users.Add(new USLIUsers
                    {
                        UserID = Convertion.ToInt(dr["UserID"]),
                        UserName = Convertion.ToString(dr["FirstName"]) + " " + Convertion.ToString(dr["LastName"]),
                        StationID = Convertion.ToInt(dr["Workstation"]),
                        WorkstationID = Convertion.ToInt(dr["WorkstationID"]),
                        FloorID = FloorName(Convertion.ToInt(dr["FloorID"])),
                        Extention = Convertion.ToString(dr["PhoneExtension"])
                    });
                }
                dr.Close();
            }

            return users;

        }

        public USLIUser GetWebRegionalContact(int userID)
        {
            USLIUser staff = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT UserID, COALESCE(FirstName, '') + ' ' + COALESCE(LastName, '') AS UserName, ");
            sb.Append("dbo.Titlelist(UserID) AS TitleName, Email, ");
            sb.Append("PhoneNumber, PhoneExtension, UserImage ");
            sb.Append("FROM  dbo.UserMaster ");
            sb.AppendFormat("WHERE UserID = {0}", Convertion.ToString(userID));

            using (Database db = new Database())
            {
                SqlDataReader dr = db.ExecuteReader(sb.ToString());

                string sImage = string.Empty;

                while (dr.Read())
                {
                    sImage = @"http://customers.usli.com/i/USLIteam/" + Convertion.ToString(dr["UserImage"]) + ".jpg";

                    staff = new USLIUser
                    {
                        UserID = Convertion.ToInt(dr["UserID"]),
                        FirstName = Convertion.ToString(dr["UserName"]),
                        Email = Convertion.ToString(dr["Email"]),
                        PhoneNumber = Convertion.ToString(dr["PhoneNumber"]),
                        PhoneExtension = Convertion.ToString(dr["PhoneExtension"]),
                        UserImage = sImage,
                        Title = Convertion.ToString(dr["TitleName"])
                    };
                }
                dr.Close();
            }


            return staff;
        }

        public ObservableCollection<USLIUser> GetWebRegionalContacts(int teamId, string statecode, int isUA)
        {
            ObservableCollection<USLIUser> staffs = new ObservableCollection<USLIUser>();

            using (Database db = new Database())
            {
                string sql = "dbo.usp_SEL_GetUnderwritersForAState";
                db.AddInParameter("StateCode", SqlDbType.VarChar, statecode);
                db.AddInParameter("DeptID", SqlDbType.SmallInt, teamId);
                db.AddInParameter("isUA", SqlDbType.SmallInt, isUA);

                SqlDataReader dr = db.ExecuteReader(System.Data.CommandType.StoredProcedure, sql);


                string sImage = string.Empty;

                while (dr.Read())
                {
                    sImage = @"http://customers.usli.com/i/USLIteam/" + Convertion.ToString(dr["UserImage"]) + ".jpg";

                    staffs.Add(new USLIUser
                    {
                        UserID = Convertion.ToInt(dr["UserID"]),
                        FirstName = Convertion.ToString(dr["UserName"]),
                        Email = Convertion.ToString(dr["Email"]),
                        PhoneNumber = Convertion.ToString(dr["PhoneNumber"]),
                        PhoneExtension = Convertion.ToString(dr["PhoneExtension"]),
                        UserImage = sImage,
                        Title = Convertion.ToString(dr["TitleName"]),
                        UserUrl = Convertion.ToString(dr["userUrl"])
                    });
                }
                dr.Close();
            }
            return staffs;
        }

        public ObservableCollection<USLITeam> GetWebRegionalTeams()
        {
            ObservableCollection<USLITeam> Teams = new ObservableCollection<USLITeam>();
            using (Database db = new Database())
            {
                string sql = "dbo.usp_SEL_GetWebTeam";

                SqlDataReader dr = db.ExecuteReader(System.Data.CommandType.StoredProcedure, sql);

                if (dr.HasRows) Teams.Add(new USLITeam
                {
                    TeamID = 0,
                    TeamName = "Select a Department",
                    isChecked = false
                });

                while (dr.Read())
                {
                    Teams.Add(new USLITeam
                    {
                        TeamID = Convertion.ToInt(dr["DeptID"]),
                        TeamName = Convertion.ToString(dr["DeptName"]),
                        isChecked = false
                    });
                }
                dr.Close();
            }

            return Teams;
        }

        public ObservableCollection<USLITeam> GetWebTeams()
        {
            string s = AppDomain.CurrentDomain.BaseDirectory + "Team.xml";
            XDocument doc = XDocument.Load(s);

            var q = from i in doc.Descendants("team")
                    select new USLITeam
                    {
                        TeamID = (int)i.Attribute("id"),
                        TeamName = (string)i.Attribute("name"),
                    };

            ObservableCollection<USLITeam> Teams = new ObservableCollection<USLITeam>();

            foreach (USLITeam tm in q)
            {
                Teams.Add(tm);
            }

            return Teams;
        }

        public ObservableCollection<USLIUser> GetWebTeamStaffs(int teamId)
        {
            ObservableCollection<USLIUser> staffs = new ObservableCollection<USLIUser>();
            using (Database db = new Database())
            {
                string sql = "dbo.usp_SEL_GetWebTeamStaffs";
                db.AddInParameter("DeptID", System.Data.SqlDbType.Int, teamId);

                SqlDataReader dr = db.ExecuteReader(System.Data.CommandType.StoredProcedure, sql);

                string sImage = string.Empty;

                while (dr.Read())
                {
                    sImage = @"http://customers.usli.com/i/USLIteam/" + Convertion.ToString(dr["UserImage"]) + ".jpg";

                    staffs.Add(new USLIUser
                    {
                        UserID = Convertion.ToInt(dr["UserID"]),
                        FirstName = Convertion.ToString(dr["FirstName"]),
                        LastName = Convertion.ToString(dr["LastName"]),
                        Email = Convertion.ToString(dr["Email"]),
                        PhoneNumber = Convertion.ToString(dr["PhoneNumber"]),
                        PhoneExtension = Convertion.ToString(dr["PhoneExtension"]),
                        UserImage = sImage,
                        Title = Convertion.ToString(dr["TitleName"]),
                        Certification = Convertion.ToString(dr["Cert"]),
                        Department = Convertion.ToString(dr["Dept"]),
                        School = Convertion.ToString(dr["School"]),
                        StartDate = Convertion.ToShortDateTime(Convertion.ToString(dr["StartDate"]))
                    });
                }
                dr.Close();
            }

            return staffs;
        }

        public void SaveStaff(USLIUser staff, USLIUserAddition stfAdition)
        {
            try
            {
                using (Database db = new Database())
                {
                    string sql = "dbo.usp_SEL_USLIWorkStationID";

                    int wrkId = -1;
                    if (stfAdition.Floor > 0)
                    {
                        db.AddInParameter("FloorID", System.Data.SqlDbType.Int, stfAdition.Floor);
                        db.AddInParameter("Workstation", System.Data.SqlDbType.VarChar, stfAdition.WorkStation);
                        wrkId = Convertion.ToInt(db.ExecuteScalar(System.Data.CommandType.StoredProcedure, sql));
                        db.ClearParameters();
                    }

                    string sUserID = Convertion.ToString(staff.UserID);

                    StringBuilder sb = new StringBuilder();
                    string sCert = string.Empty;

                    int seqN = 1;
                    if (stfAdition.UserCerts.Count > 0)
                    {
                        sb.Append("<ROOT>");
                        foreach (USLICertification ct in stfAdition.UserCerts)
                        {
                            sb.AppendFormat("<User UserID=\"{0}\" CertID=\"{1}\" Sequence=\"{2}\" />", sUserID, Convertion.ToString(ct.CertID), Convertion.ToString(seqN));
                            seqN++;
                        }
                        sb.Append("</ROOT>");
                        sCert = sb.ToString();
                    }

                    sb = new StringBuilder();
                    string sTitle = string.Empty;
                    seqN = 1;

                    if (stfAdition.UserTitles.Count > 0)
                    {
                        sb.Append("<ROOT>");
                        foreach (USLITitle tl in stfAdition.UserTitles)
                        {
                            sb.AppendFormat("<User UserID=\"{0}\" TitleID=\"{1}\" Sequence=\"{2}\" />", sUserID, Convertion.ToString(tl.TitleID), Convertion.ToString(seqN));
                            seqN++;
                        }
                        sb.Append("</ROOT>");
                        sTitle = sb.ToString();

                    }

                    sb = new StringBuilder();
                    string sDept = string.Empty;
                    seqN = 1;

                    if (stfAdition.UserTeams.Count > 0)
                    {
                        sb.Append("<ROOT>");
                        foreach (USLITeam tm in stfAdition.UserTeams)
                        {
                            sb.AppendFormat("<User UserID=\"{0}\" DeptID=\"{1}\" Sequence=\"{2}\" />", sUserID, Convertion.ToString(tm.TeamID), Convertion.ToString(seqN));
                            seqN++;
                        }
                        sb.Append("</ROOT>");
                        sDept = sb.ToString();
                    }

                    sb = new StringBuilder();
                    string sSchool = string.Empty;
                    seqN = 1;

                    if (stfAdition.UserEducation.Count > 0)
                    {
                        sb.Append("<ROOT>");
                        foreach (USLIEducation tm in stfAdition.UserEducation)
                        {
                            sb.AppendFormat("<User UserID=\"{0}\" EducationID=\"{1}\" Sequence=\"{2}\" />", sUserID, Convertion.ToString(tm.EducationID), Convertion.ToString(seqN));
                            seqN++;
                        }
                        sb.Append("</ROOT>");
                        sSchool = sb.ToString();
                    }

                    sql = "dbo.usp_UPD_User";
                    db.AddInParameter("UserID", System.Data.SqlDbType.Int, staff.UserID);
                    db.AddInParameter("FirstName", System.Data.SqlDbType.VarChar, staff.FirstName);
                    db.AddInParameter("LastName", System.Data.SqlDbType.VarChar, staff.LastName);
                    db.AddInParameter("Email", System.Data.SqlDbType.VarChar, staff.Email);
                    db.AddInParameter("PhoneNumber", System.Data.SqlDbType.VarChar, staff.PhoneNumber);
                    db.AddInParameter("PhoneExtension", System.Data.SqlDbType.VarChar, staff.PhoneExtension);
                    db.AddInParameter("UserImage", System.Data.SqlDbType.VarChar, staff.UserImage);
                    db.AddInParameter("WorkstationID", System.Data.SqlDbType.Int, wrkId);
                    db.AddInParameter("UserUrl", System.Data.SqlDbType.VarChar, stfAdition.UserUrl);

                    db.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, sql);

                    sql = "dbo.usp_UPD_UserAddition";
                    db.ClearParameters();
                    db.AddInParameter("UserID", System.Data.SqlDbType.Int, staff.UserID);
                    db.AddInParameter("TitleDoc", System.Data.SqlDbType.VarChar, sTitle);
                    db.AddInParameter("DeptDoc", System.Data.SqlDbType.VarChar, sDept);
                    db.AddInParameter("CertDoc", System.Data.SqlDbType.VarChar, sCert);
                    db.AddInParameter("SchoolDoc", System.Data.SqlDbType.VarChar, sSchool);

                    db.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, sql);
                }
            }
            catch (Exception ex)
            {
                Common.LogError("0", "USLI.WCF.Services.SaveStaff: \r\n" + ex.ToString());
            }
            
        }

        public void SaveUserFifties(User50 user)
        {
            using (Database db = new Database())
            {
                db.AddInParameter("UserID", SqlDbType.Int, user.UserID);
                db.AddInParameter("AgentID", SqlDbType.Int, user.CustID);
                db.AddInParameter("FirstName", SqlDbType.VarChar, user.FirstName);
                db.AddInParameter("LastName", SqlDbType.VarChar, user.LastName);
                db.AddInParameter("City", SqlDbType.VarChar, user.City);
                db.AddInParameter("State", SqlDbType.VarChar, user.State);
                db.AddInParameter("Status", SqlDbType.Int, user.Status);
                db.AddInParameter("HireDate", SqlDbType.DateTime, user.HireDate);

                db.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.usp_SAV_User50");
            }
        }

        public void SaveUSLICert(int id, string name)
        {
            string sql = "dbo.usp_SAV_USLICertifications";

            try
            {
                using (Database db = new Database())
                {
                    db.AddInParameter("CerID", SqlDbType.Int, id);
                    db.AddInParameter("CertName", SqlDbType.VarChar, name);

                    db.ExecuteNonQuery(CommandType.StoredProcedure, sql);
                }
            }
            catch (Exception ex)
            {
                Common.LogError("0", "USLI.WCF.Services.SaveUSLICert: \r\n" + ex.ToString());
            }
            
        }

        public void SaveUSLIEducation(USLIEducation ed)
        {
            string sql = "dbo.usp_SAV_USLIEducation";

            try
            {
                using (Database db = new Database())
                {
                    db.AddInParameter("EducationName", SqlDbType.VarChar, ed.EducationName);
                    db.AddInParameter("EducationTypeID", SqlDbType.Int, ed.EducationTypeID);

                    if (ed.EducationID > 0)
                    {
                        db.AddInParameter("EducationID", SqlDbType.Int, ed.EducationID);
                    }

                    db.ExecuteNonQuery(CommandType.StoredProcedure, sql);
                }
            }
            catch (Exception ex)
            {
                Common.LogError("0", "USLI.WCF.Services.SaveUSLIEducation: \r\n" + ex.ToString());
            }
           
        }

        public void SaveUSLITitle(int id, string name)
        {
            string sql = "dbo.usp_SAV_USLITitle";

            try
            {
                using (Database db = new Database())
                {
                    db.AddInParameter("TitleID", SqlDbType.Int, id);
                    db.AddInParameter("TitleName", SqlDbType.VarChar, name);

                    db.ExecuteNonQuery(CommandType.StoredProcedure, sql);
                }
            }
            catch (Exception ex)
            {
                Common.LogError("0", "USLI.WCF.Services.SaveUSLITitle: \r\n" + ex.ToString());
            }
            
        }

        public bool SaveWebRegionalContact(int DeptID, string state, string ListSelected, string isUA)
        {
            try
            {
                string s = string.Empty;
                StringBuilder sb = new StringBuilder();
                int iError = 0;

                string[] sel = ListSelected.Split(new char[] { '|' });
                for (int i = 0; i < sel.Length; i++)
                {
                    sb.AppendFormat("<User StateCode=\"{0}\" DeptID=\"{1}\" SeqNo=\"{2}\" UserID=\"{3}\" Notes=\"{4}\" />", state, Convertion.ToString(DeptID), Convertion.ToString(i), sel[i], isUA);
                }

                if (sb.Length > 0) s = "<ROOT>" + sb.ToString() + "</ROOT>";

                using (Database db = new Database())
                {
                    db.AddInParameter("State", SqlDbType.VarChar, state);
                    db.AddInParameter("DeptID", SqlDbType.SmallInt, DeptID);
                    db.AddInParameter("doc", SqlDbType.VarChar, s);
                    db.AddInParameter("isUA", SqlDbType.SmallInt, isUA);

                    SqlDataReader rd = db.ExecuteReader(CommandType.StoredProcedure, "dbo.usp_UPD_USLIStateUnderwriter");
                    while (rd.Read()){
                        iError = Convertion.ToInt(rd[0]);
                    }
                    rd.Close();
                }

                return iError == 0;
            }
            catch (Exception e)
            {
                Common.LogError("0", "USLIContactManager: /n/l" + e.ToString());
                return false;
            }
        }

        public ObservableCollection<USLIUsers> SearchUsers(string search)
        {
            ObservableCollection<USLIUsers> allusers = GetUsers();

            ObservableCollection<USLIUsers> users = new ObservableCollection<USLIUsers>();

            if (Convertion.ToInt(search) > 0)
            {
                foreach (USLIUsers uer in allusers)
                {
                    if (Convertion.ToString(uer.StationID).ToUpper().StartsWith(search.ToUpper())) users.Add(uer);
                }

            }
            else
            {
                foreach (USLIUsers uer in allusers)
                {
                    if (uer.UserName.ToUpper().IndexOf(search.ToUpper()) > -1) users.Add(uer);
                }
            }

            return users;
        }

        public ObservableCollection<USLIContactType> GetContactTypes()
        {
            string s = AppDomain.CurrentDomain.BaseDirectory + "ContactTypes.xml";
            XDocument doc = XDocument.Load(s);

            var q = from i in doc.Descendants("contacttype")
                    select new USLIContactType
                    {
                        ContactTypeID = (int)i.Attribute("id"),
                        ContactTypeName = (string)i.Attribute("name"),
                    };

            ObservableCollection<USLIContactType> types = new ObservableCollection<USLIContactType>();

            foreach (USLIContactType tp in q)
            {
                types.Add(tp);
            }

            return types;
        }

        public ObservableCollection<USLITeam> GetContactDeparts()
        {
            string s = AppDomain.CurrentDomain.BaseDirectory + "Departs.xml";
            XDocument doc = XDocument.Load(s);

            var q = from i in doc.Descendants("team")
                    select new USLITeam
                    {
                        TeamID = (int)i.Attribute("id"),
                        TeamName = (string)i.Attribute("name"),
                    };

            ObservableCollection<USLITeam> teams = new ObservableCollection<USLITeam>();

            foreach (USLITeam tp in q)
            {
                teams.Add(tp);
            }

            return teams;
        }

        public USLIContactView GetContactView(int productid, int usertype, int catlog, int prodtypeid)
        {
            USLIContactView returnclass = new USLIContactView();
            returnclass.AsignmentDetails = new ObservableCollection<USLIContactAsignment>();

            using (Database db = new Database())
            {
                db.AddInParameter("ProductLineID", SqlDbType.TinyInt, productid);
                db.AddInParameter("UserType", SqlDbType.TinyInt, usertype);
                db.AddInParameter("CatLog", SqlDbType.TinyInt, catlog);
                db.AddInParameter("ProductTypeID", SqlDbType.TinyInt, prodtypeid);

                SqlDataReader dr = db.ExecuteReader(CommandType.StoredProcedure, "dbo.usp_SEL_AsgndContactsForCust");

                while (dr.Read())
                {
                    USLIContactAsignment asgmt = new USLIContactAsignment();
                    asgmt.UserID = Convertion.ToInt(dr["UserID"]);
                    asgmt.UserName = Convertion.ToString(dr["UserName"]);
                    asgmt.Entity1ID = Convertion.ToInt(dr["EntityID"]);
                    asgmt.Entity1Name = Convertion.ToString(dr["EntityName"]);


                    if (catlog == 3)
                    {
                        asgmt.Entity2ID = Convertion.ToInt(dr["Entity2ID"]);
                        asgmt.Entity2Name = Convertion.ToString(dr["Entity2Name"]);
                    }

                    returnclass.AsignmentDetails.Add(asgmt);
                }

                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        returnclass.DefaultContactName = Convertion.ToString(dr["UserName"]);
                    }
                }

                dr.Close();
            }
            
            return returnclass;
        }

        public bool SaveDefaultContact(USLIContactIDs contids)
        {
            bool b = true;

            try
            {
                using (Database db = new Database())
                {
                    db.AddInParameter("ProductLineID", SqlDbType.TinyInt, contids.DeptID);
                    db.AddInParameter("UserID", SqlDbType.Int, contids.ContactID);
                    db.AddInParameter("Type", SqlDbType.TinyInt, contids.ContactTypeID);

                    db.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.usp_SAV_ProductLine_User");
                }
            }
            catch (Exception e)
            {
                Common.LogError("0", "ContactMaster - SaveDefaultContact \r\n " + e.Message);
                b = false;
            }

            return b;
        }

        public bool DeleteDefaultContact(USLIContactIDs contids)
        {
            bool b = true;

            try
            {
                using (Database db = new Database())
                {
                    db.AddInParameter("ProductLineID", SqlDbType.TinyInt, contids.DeptID);
                    db.AddInParameter("Type", SqlDbType.TinyInt, contids.ContactTypeID);

                    db.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.usp_Del_ProductLine_User");
                }
            }
            catch (Exception e)
            {
                Common.LogError("0", "ContactMaster - DeleteDefaultContact \r\n " + e.Message);
                b = false;
            }

            return b;
        }

        public void SaveAssignments(string ContactList)
        {
            JArray jsn = new JArray { JObject.Parse(ContactList) as JObject };
            var q = from assignment in jsn
                    select new
                    {
                        DeptID = (int)assignment["DeptID"],
                        ContactTypeID = (int)assignment["ContactTypeID"],
                        ContactId = (int)assignment["ContactID"],
                        Catlog = (int)assignment["Catlog"],
                        Entity1ID = assignment["Entity1ID"].ToArray(),
                        Entity2ID = assignment["Entity2ID"].ToArray()
                    };

            foreach (var ass in q)
            {
                using (Database db = new Database())
                {
                    foreach (int e2 in ass.Entity2ID)
                    {
                        foreach (int e1 in ass.Entity1ID)
                        {
                            db.ClearParameters();
                            db.AddInParameter("DeptID", SqlDbType.TinyInt, ass.DeptID);
                            db.AddInParameter("UserType", SqlDbType.TinyInt, ass.ContactTypeID);
                            db.AddInParameter("UserID", SqlDbType.Int, ass.ContactId);
                            db.AddInParameter("CatLogID", SqlDbType.TinyInt, ass.Catlog);
                            db.AddInParameter("EntityID", SqlDbType.Int, e1);
                            db.AddInParameter("Entity2ID", SqlDbType.Int, e2);

                            try
                            {

                                db.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.usp_SAV_ContactAssignment");
                            }
                            catch (Exception ex) { string s = ex.Message; }
                        }
                    }
                }
            }
        }

        public void DeleteAssignments(string ContactList)
        {
            JArray jsn = new JArray { JObject.Parse(ContactList) as JObject };
            var q = from assignment in jsn
                    select new
                    {
                        DeptID = (int)assignment["DeptID"],
                        ContactTypeID = (int)assignment["ContactTypeID"],
                        Catlog = (int)assignment["Catlog"],
                        Entity = assignment["Entity"].ToArray()
                    };

            foreach (var ass in q)
            {
                using (Database db = new Database())
                {
                    foreach (string s in ass.Entity)
                    {
                        string[] entites = s.Split(':');
                        db.ClearParameters();
                        db.AddInParameter("DeptID", SqlDbType.TinyInt, ass.DeptID);
                        db.AddInParameter("UserType", SqlDbType.TinyInt, ass.ContactTypeID);
                        db.AddInParameter("UserID", SqlDbType.Int, entites[0]);
                        db.AddInParameter("CatLogID", SqlDbType.TinyInt, ass.Catlog);
                        db.AddInParameter("EntityID", SqlDbType.Int, entites[1]);
                        db.AddInParameter("Entity2ID", SqlDbType.Int, entites[2]);

                        try
                        {
                            db.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.usp_DEL_ContactAssignment");
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        public void DeleteContactAssignments(string ContactList)
        {
            JArray jsn = JArray.Parse(ContactList) as JArray;
            var q = from assignment in jsn
                    select new
                    {
                        DeptID = (int)assignment["DeptID"],
                        ContactTypeID = (int)assignment["ContactTypeID"],
                        Catlog = (int)assignment["Catlog"],
                        ContactID = (int)assignment["ContactID"],
                        Entity1ID = (int)assignment["Entity1ID"],
                        Entity2ID = (int)assignment["Entity2ID"]
                    };

            using (Database db = new Database())
            {
                foreach (var ass in q)
                {
                    db.ClearParameters();
                    db.AddInParameter("DeptID", SqlDbType.TinyInt, ass.DeptID);
                    db.AddInParameter("UserType", SqlDbType.TinyInt, ass.ContactTypeID);
                    db.AddInParameter("UserID", SqlDbType.Int, ass.ContactID);
                    db.AddInParameter("CatLogID", SqlDbType.TinyInt, ass.Catlog);
                    db.AddInParameter("EntityID", SqlDbType.Int, ass.Entity1ID);
                    db.AddInParameter("Entity2ID", SqlDbType.Int, ass.Entity2ID);

                    try
                    {
                        db.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.usp_DEL_ContactAssignment");
                    }
                    catch (Exception) { }
                }
            }
        }

        public USLIContactMainPikerView GetContactMainPikerView(int catlogid, int tmid)
        {
            USLIContactMainPikerView vw = new USLIContactMainPikerView();

            using (Database db = new Database())
            {
                db.AddInParameter("CatLogID", SqlDbType.TinyInt, catlogid);
                SqlDataReader dr = db.ExecuteReader(CommandType.StoredProcedure, "dbo.usp_SEL_ContactMainPickerView");
                if (dr.HasRows)
                {
                    vw.ContactList = new ObservableCollection<USLIContactType>();
                    while (dr.Read())
                    {
                        vw.ContactList.Add(new USLIContactType
                        {
                            ContactTypeID = Convertion.ToInt(dr["ENTITYID"]),
                            ContactTypeName = Convertion.ToString(dr["ENTITYName"])
                        });
                    }
                }

                if (dr.NextResult())
                {
                    if (dr.HasRows)
                    {
                        vw.EntityListOne = new ObservableCollection<USLITeam>();
                        while (dr.Read())
                        {
                            vw.EntityListOne.Add(new USLITeam
                            {
                                TeamID = Convertion.ToInt(dr["ENTITYID"]),
                                TeamName = Convertion.ToString(dr["ENTITYName"]),
                                TeamState = Convertion.ToString(dr["State"]),
                                isChecked = false
                            });
                        }
                    }

                }

                if (dr.NextResult())
                {
                    if (dr.HasRows)
                    {
                        if (catlogid == 0)
                        {
                            vw.BState = new ObservableCollection<string>();
                            while (dr.Read())
                            {
                                vw.BState.Add(Convertion.ToString(dr["State"]));
                            }
                        }
                        if (catlogid == 3)
                        {
                            if (tmid == 7||tmid == 1)
                            {
                                vw.EntityListTwo = new ObservableCollection<USLITeam>();
                                while (dr.Read())
                                {
                                    vw.EntityListTwo.Add(new USLITeam
                                    {
                                        TeamID = Convertion.ToInt(dr["ENTITYID"]),
                                        TeamName = Convertion.ToString(dr["ENTITYName"])

                                    });
                                }
                            }
                            else
                            {
                                vw.EntityListTwo = GetProfessionalProducts(tmid);
                            }
                        }
                    }
                }

                dr.Close();
            }

            
            return vw;
        }

        public USLIContactAsignmentDetailView GetContactDetailView(int userid)
        {
            USLIContactAsignmentDetailView returnclass = new USLIContactAsignmentDetailView();
            returnclass.CustomerList = new ObservableCollection<USLIContactAsignmentDetail>();
            returnclass.StateList = new ObservableCollection<USLIContactAsignmentDetail>();
            returnclass.ProductList = new ObservableCollection<USLIContactAsignmentDetail>();
            returnclass.CustProdList = new ObservableCollection<USLIContactAsignmentDetail>();

            using (Database db = new Database())
            {
                db.AddInParameter("UserID", SqlDbType.Int, userid);

                SqlDataReader dr = db.ExecuteReader(CommandType.StoredProcedure, "dbo.usp_SEL_ContactAssignmentDetail");

                while (dr.Read())
                {
                    USLIContactAsignmentDetail asgmt = new USLIContactAsignmentDetail();
                    asgmt.Entity1ID = Convertion.ToInt(dr["Entity1ID"]);
                    asgmt.Entity1Name = Convertion.ToString(dr["Entity1Name"]);
                    asgmt.ProductLineID = Convertion.ToInt(dr["ProductLineID"]);
                    asgmt.ProductLineName = Convertion.ToString(dr["DeptName"]);
                    asgmt.UserTypeID = Convertion.ToInt(dr["UserType"]);
                    asgmt.UserTypeName = Convertion.ToString(dr["UserTypeDesc"]);

                    returnclass.CustomerList.Add(asgmt);
                }

                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        USLIContactAsignmentDetail asgmt = new USLIContactAsignmentDetail();
                        asgmt.Entity1ID = Convertion.ToInt(dr["Entity1ID"]);
                        asgmt.Entity1Name = Convertion.ToString(dr["Entity1Name"]);
                        asgmt.ProductLineID = Convertion.ToInt(dr["ProductLineID"]);
                        asgmt.ProductLineName = Convertion.ToString(dr["DeptName"]);
                        asgmt.UserTypeID = Convertion.ToInt(dr["UserType"]);
                        asgmt.UserTypeName = Convertion.ToString(dr["UserTypeDesc"]);

                        returnclass.ProductList.Add(asgmt);
                    }
                }

                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        USLIContactAsignmentDetail asgmt = new USLIContactAsignmentDetail();
                        asgmt.Entity1ID = Convertion.ToInt(dr["Entity1ID"]);
                        asgmt.Entity1Name = Convertion.ToString(dr["Entity1Name"]);
                        asgmt.ProductLineID = Convertion.ToInt(dr["ProductLineID"]);
                        asgmt.ProductLineName = Convertion.ToString(dr["DeptName"]);
                        asgmt.UserTypeID = Convertion.ToInt(dr["UserType"]);
                        asgmt.UserTypeName = Convertion.ToString(dr["UserTypeDesc"]);

                        returnclass.StateList.Add(asgmt);
                    }
                }

                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        USLIContactAsignmentDetail asgmt = new USLIContactAsignmentDetail();
                        asgmt.Entity1ID = Convertion.ToInt(dr["Entity1ID"]);
                        asgmt.Entity1Name = Convertion.ToString(dr["Entity1Name"]);
                        asgmt.Entity2ID = Convertion.ToInt(dr["Entity2ID"]);
                        asgmt.Entity2Name = Convertion.ToString(dr["Entity2Name"]);
                        asgmt.ProductLineID = Convertion.ToInt(dr["ProductLineID"]);
                        asgmt.ProductLineName = Convertion.ToString(dr["DeptName"]);
                        asgmt.UserTypeID = Convertion.ToInt(dr["UserType"]);
                        asgmt.UserTypeName = Convertion.ToString(dr["UserTypeDesc"]);

                        returnclass.CustProdList.Add(asgmt);
                    }
                }

                dr.Close();
            }
            
            
            return returnclass;
        }

        public void ReplaceContactAssignments(int source, int target)
        {
            using (Database db = new Database())
            {
                db.AddInParameter("UserIDSource", SqlDbType.Int, source);
                db.AddInParameter("UserIDTarget", SqlDbType.Int, target);

                try
                {
                    db.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.usp_SAV_ContactAssignmentReplace");
                }
                catch (Exception) { }

            }
        }

        public ObservableCollection<USLITeam> GetProducts()
        {
            string s = AppDomain.CurrentDomain.BaseDirectory + "ProfessionalProds.xml";
            XDocument doc = XDocument.Load(s);

            var q = from i in doc.Descendants("prod")
                    select new USLITeam
                    {
                        TeamID = (int)i.Attribute("id"),
                        TeamName = (string)i.Attribute("name"),
                    };

            ObservableCollection<USLITeam> teams = new ObservableCollection<USLITeam>();

            foreach (USLITeam tp in q)
            {
                teams.Add(tp);
            }

            return teams;
        }

        public string CreateExcelReport(string columndata, ObservableCollection<USLIContactAsignment> astList, int catlog)
        {

            string filePath = Common.GetAppSetting("ReportDir");

            FileService.ClearOldFiles(filePath, DateTime.Now.AddMinutes(-30));

            string filename = System.Guid.NewGuid().ToString() + ".xml";
            filePath += filename;

            string XMLContent = GetExcel(columndata, astList, catlog);


            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(XMLContent);
            }

            string fileURL = Common.GetAppSetting("ReportURL") + filename;

            return fileURL;

        }

        private ObservableCollection<USLITeam> GetProfessionalProducts(int tmid)
        {
            string s = AppDomain.CurrentDomain.BaseDirectory + "ProfessionalProds.xml";
            XDocument doc = XDocument.Load(s);

            var q = from i in doc.Descendants("prod")
                    where (int)i.Attribute("teamid") == tmid
                    orderby (string)i.Attribute("name")
                    select new USLITeam
                    {
                        TeamID = (int)i.Attribute("id"),
                        TeamName = (string)i.Attribute("name")
                    };

            ObservableCollection<USLITeam> teams = new ObservableCollection<USLITeam>();

            foreach (USLITeam tp in q)
            {
                teams.Add(tp);
            }

            return teams;
        }

        private string FloorName(int floorid)
        {
            string s = "N/A";

            switch (floorid)
            {
                case 1:
                    s = "East 1";
                    break;
                case 2:
                    s = "East 2";
                    break;
                case 3:
                    s = "East 3";
                    break;
                case 4:
                    s = "West 1";
                    break;
                case 5:
                    s = "West 2";
                    break;
                case 6:
                    s = "West 3";
                    break;
                case 7:
                    s = "Austin";
                    break;
                case 8:
                    s = "Mission Viejo";
                    break;
                case 9:
                    s = "San Ramon";
                    break;
                case 10:
                    s = "Gateway";
                    break;
                case 11:
                    s = "Chicago";
                    break;
                case 12:
                case 14:
                case 15:
                    s = "1170-1";
                    break;
                case 13:
                    s = "Oakbrook";
                    break;
                case 16:
                    s = "1170-2";
                    break;
                default:
                    s = "N/A";
                    break;
            }
            return s;
        }

        private string GetStatusDesc(int statusid)
        {
            string s = "Active";

            if (statusid != 14) s = "Cancelled";

            return s;
        }

        private string GetExcel(string columndata, ObservableCollection<USLIContactAsignment> astList, int catlog)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ExcelFirstPart());
            sb.Append(columndata);
            sb.Append("</Row> \r\n");

            foreach (USLIContactAsignment ast in astList)
            {
                sb.Append("<Row ss:AutoFitHeight=\"0\">");

                sb.AppendFormat("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>", DoEncode(ast.UserName));
                sb.AppendFormat("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>", DoEncode(ast.Entity1Name));
                if (catlog > 2) sb.AppendFormat("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>", DoEncode(ast.Entity2Name));

                sb.Append("</Row> \r\n");
            }

            sb.Append(ExcelLastPart());

            return sb.ToString();
        }
        private string ExcelFirstPart()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\"?> \r\n");
            sb.Append("<?mso-application progid=\"Excel.Sheet\"?> \r\n");
            sb.Append("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" \r\n ");
            sb.Append("xmlns:o=\"urn:schemas-microsoft-com:office:office\" \r\n ");
            sb.Append("xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \r\n ");
            sb.Append("xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" \r\n ");
            sb.Append("xmlns:html=\"http://www.w3.org/TR/REC-html40\"> \r\n");
            sb.Append("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"> \r\n ");
            sb.Append("<Author></Author> \r\n");
            sb.Append("<LastAuthor></LastAuthor> \r\n");
            sb.AppendFormat("<Created>{0}</Created> \r\n", DateTime.Now.ToLongDateString());
            sb.Append("<Version>12.00</Version> \r\n");
            sb.Append("</DocumentProperties> \r\n");
            sb.Append("<ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\"> \r\n");
            sb.Append("<WindowHeight>12015</WindowHeight> \r\n");
            sb.Append("<WindowWidth>24855</WindowWidth> \r\n");
            sb.Append("<WindowTopX>240</WindowTopX> \r\n");
            sb.Append("<WindowTopY>150</WindowTopY> \r\n");
            sb.Append("<ProtectStructure>False</ProtectStructure> \r\n");
            sb.Append("<ProtectWindows>False</ProtectWindows> \r\n");
            sb.Append("</ExcelWorkbook> \r\n");
            sb.Append("<Styles> \r\n");
            sb.Append("<Style ss:ID=\"Default\" ss:Name=\"Normal\"> \r\n");
            sb.Append("<Alignment ss:Vertical=\"Bottom\"/> \r\n");
            sb.Append("<Borders/> \r\n");
            sb.Append("<Font ss:FontName=\"Calibri\" x:Family=\"Swiss\" ss:Size=\"11\" ss:Color=\"#000000\"/> \r\n");
            sb.Append("<Interior/> \r\n");
            sb.Append("<NumberFormat/> \r\n");
            sb.Append("<Protection/> \r\n");
            sb.Append("</Style> \r\n");
            sb.Append("<Style ss:ID=\"s62\"> \r\n");
            sb.Append("<Font ss:FontName=\"Calibri\" x:Family=\"Swiss\" ss:Size=\"11\" ss:Color=\"#000000\" ss:Bold=\"1\" /> \r\n");
            sb.Append("</Style></Styles> \r\n");
            sb.Append("<Worksheet ss:Name=\"Sheet1\"> \r\n");
            sb.Append("<Table x:FullColumns=\"1\" x:FullRows=\"1\" ss:DefaultRowHeight=\"15\"> \r\n ");
            sb.Append("<Column ss:AutoFitWidth=\"0\" ss:Width=\"140\"/> \r\n");
            sb.Append("<Column ss:AutoFitWidth=\"0\" ss:Width=\"200\"/> \r\n");
            sb.Append("<Row ss:AutoFitHeight=\"0\"> \r\n");

            return sb.ToString();
        }

        private string ExcelLastPart()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("</Table> \r\n");
            sb.Append("<WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\"> \r\n");
            sb.Append("<PageSetup> \r\n");
            sb.Append("<Header x:Margin=\"0.3\"/> \r\n");
            sb.Append("<Footer x:Margin=\"0.3\"/> \r\n");
            sb.Append("<PageMargins x:Bottom=\"0.75\" x:Left=\"0.7\" x:Right=\"0.7\" x:Top=\"0.75\"/> \r\n");
            sb.Append("</PageSetup> \r\n");
            sb.Append("<Unsynced/> \r\n");
            sb.Append("<Print> \r\n");
            sb.Append("<ValidPrinterInfo/> \r\n");
            sb.Append("<HorizontalResolution>600</HorizontalResolution> \r\n");
            sb.Append("<VerticalResolution>600</VerticalResolution> \r\n");
            sb.Append("</Print> \r\n");
            sb.Append("<Selected/> \r\n");
            sb.Append("<Panes> \r\n");
            sb.Append("<Pane> \r\n");
            sb.Append("<Number>3</Number> \r\n");
            sb.Append("<ActiveRow>1</ActiveRow> \r\n");
            sb.Append("</Pane> \r\n");
            sb.Append("</Panes> \r\n");
            sb.Append("<ProtectObjects>False</ProtectObjects> \r\n");
            sb.Append("<ProtectScenarios>False</ProtectScenarios> \r\n");
            sb.Append("</WorksheetOptions> \r\n");
            sb.Append("</Worksheet> \r\n");
            sb.Append("</Workbook>");

            return sb.ToString();
        }

        private string DoEncode(string sInput)
        {
            if (sInput.Length > 0)
            {
                return HttpUtility.HtmlEncode(sInput);
            }
            else
            {
                return string.Empty;
            }
        }


        public string ExcelReport(string columndata, int iDeptid, int iUserType, int icatlog, int iProdType)
        {
            USLIContactView CTView = GetContactView(iDeptid, iUserType, icatlog, iProdType);
            
            string filePath = Common.GetAppSetting("ReportDir");

            FileService.ClearOldFiles(filePath, DateTime.Now.AddMinutes(-30));

            string filename = System.Guid.NewGuid().ToString() + ".xml";
            filePath += filename;

            string XMLContent = GetExcel(columndata, CTView.AsignmentDetails, icatlog);

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(XMLContent);
            }

            string fileURL = Common.GetAppSetting("ReportURL") + filename;

            return fileURL;
        }
    }
}
