using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;

namespace USLI.WCF.Services
{
    
    [DataContract]
    public class Conferenceroom
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public int Floor { get; set; }

        [DataMember]
        public double x { get; set; }

        [DataMember]
        public double y { get; set; }
    }

    [DataContract]
    public class USLIUsers
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int StationID { get; set; }

        [DataMember]
        public int WorkstationID { get; set; }

        [DataMember]
        public string FloorID { get; set; }

        [DataMember]
        public string Extention { get; set; }

    }

    [DataContract]
    public class USLIUserAddition
    {
        [DataMember]
        public ObservableCollection<USLITeam> UserTeams { get; set; }

        [DataMember]
        public ObservableCollection<USLITitle> UserTitles { get; set; }

        [DataMember]
        public ObservableCollection<USLICertification> UserCerts { get; set; }

        [DataMember]
        public ObservableCollection<USLIEducation> UserEducation { get; set; }

        [DataMember]
        public int WorkStation { get; set; }

        [DataMember]
        public int Floor { get; set; }

        [DataMember]
        public string UserUrl { get; set; }
    }

    [DataContract]
    public class USState
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public int StateID { get; set; }
    }

    [DataContract]
    public class User50
    {
        [DataMember]
        public int CustID { get; set; }

        [DataMember]
        public string CustName { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string HireDate { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string StatusDesc { get; set; }

    }

    [DataContract]
    public class GenericEntity
    {
        [DataMember]
        public int EntityID { get; set; }

        [DataMember]
        public string EntityName { get; set; }
    }

    [DataContract]
    public class Customer50ViewModel
    {
        [DataMember]
        public List<GenericEntity> Customers { get; set; }

        [DataMember]
        public List<GenericEntity> States { get; set; }

        [DataMember]
        public List<GenericEntity> Status { get; set; }

    }

    [DataContract]
    public class USLIEducation
    {
        [DataMember]
        public int EducationID { get; set; }

        [DataMember]
        public string EducationName { get; set; }

        [DataMember]
        public int EducationTypeID { get; set; }

        [DataMember]
        public string EducationTypeName { get; set; }

        [DataMember]
        public bool isChecked { get; set; }
    }

    [DataContract]
    public class USLIContactType
    {
        [DataMember]
        public int ContactTypeID { get; set; }

        [DataMember]
        public string ContactTypeName { get; set; }

    }

    [DataContract]
    public class USLIContactView
    {
        [DataMember]
        public ObservableCollection<USLIContactAsignment> AsignmentDetails { get; set; }

        [DataMember]
        public string DefaultContactName { get; set; }

    }

    [DataContract]
    public class USLIContactAsignment
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int Entity1ID { get; set; }

        [DataMember]
        public int Entity2ID { get; set; }

        [DataMember]
        public string Entity1Name { get; set; }

        [DataMember]
        public string Entity2Name { get; set; }

    }

    [DataContract]
    public class USLIContactIDs
    {
        [DataMember]
        public int ContactID { get; set; }
        [DataMember]
        public int ContactTypeID { get; set; }
        [DataMember]
        public int DeptID { get; set; }
        [DataMember]
        public int EntityID { get; set; }
        [DataMember]
        public int Entity2ID { get; set; }
        [DataMember]
        public int CatlogID { get; set; }
    }

    [DataContract]
    public class USLIContactAsignmentDetailView
    {
        [DataMember]
        public ObservableCollection<USLIContactAsignmentDetail> CustomerList { get; set; }

        [DataMember]
        public ObservableCollection<USLIContactAsignmentDetail> StateList { get; set; }

        [DataMember]
        public ObservableCollection<USLIContactAsignmentDetail> ProductList { get; set; }

        [DataMember]
        public ObservableCollection<USLIContactAsignmentDetail> CustProdList { get; set; }

    }

    [DataContract]
    public class USLIContactAsignmentDetail
    {
        [DataMember]
        public int Entity1ID { get; set; }

        [DataMember]
        public string Entity1Name { get; set; }

        [DataMember]
        public int Entity2ID { get; set; }

        [DataMember]
        public string Entity2Name { get; set; }

        [DataMember]
        public int ProductLineID { get; set; }

        [DataMember]
        public string ProductLineName { get; set; }

        [DataMember]
        public int UserTypeID { get; set; }

        [DataMember]
        public string UserTypeName { get; set; }

    }

    [DataContract]
    public class USLIContactMainPikerView
    {
        [DataMember]
        public ObservableCollection<USLIContactType> ContactList { get; set; }

        [DataMember]
        public ObservableCollection<USLITeam> EntityListOne { get; set; }

        [DataMember]
        public ObservableCollection<USLITeam> EntityListTwo { get; set; }

        [DataMember]
        public ObservableCollection<string> BState { get; set; }

    }

    [Serializable()]
    [DataContractAttribute(IsReference = true)]
    public partial class USLIUser : EntityObject
    {
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.Int32 UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                if (_UserID != value)
                {
                    OnUserIDChanging(value);
                    ReportPropertyChanging("UserID");
                    _UserID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("UserID");
                    OnUserIDChanged();
                }
            }
        }
        private global::System.Int32 _UserID;
        partial void OnUserIDChanging(global::System.Int32 value);
        partial void OnUserIDChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                OnUserNameChanging(value);
                ReportPropertyChanging("UserName");
                _UserName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("UserName");
                OnUserNameChanged();
            }
        }
        private global::System.String _UserName;
        partial void OnUserNameChanging(global::System.String value);
        partial void OnUserNameChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.String LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                OnLastNameChanging(value);
                ReportPropertyChanging("LastName");
                _LastName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("LastName");
                OnLastNameChanged();
            }
        }
        private global::System.String _LastName;
        partial void OnLastNameChanging(global::System.String value);
        partial void OnLastNameChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.String FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                OnFirstNameChanging(value);
                ReportPropertyChanging("FirstName");
                _FirstName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("FirstName");
                OnFirstNameChanged();
            }
        }
        private global::System.String _FirstName;
        partial void OnFirstNameChanging(global::System.String value);
        partial void OnFirstNameChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                OnEmailChanging(value);
                ReportPropertyChanging("Email");
                _Email = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Email");
                OnEmailChanged();
            }
        }
        private global::System.String _Email;
        partial void OnEmailChanging(global::System.String value);
        partial void OnEmailChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                OnPhoneNumberChanging(value);
                ReportPropertyChanging("PhoneNumber");
                _PhoneNumber = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("PhoneNumber");
                OnPhoneNumberChanged();
            }
        }
        private global::System.String _PhoneNumber;
        partial void OnPhoneNumberChanging(global::System.String value);
        partial void OnPhoneNumberChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String PhoneExtension
        {
            get
            {
                return _PhoneExtension;
            }
            set
            {
                OnPhoneExtensionChanging(value);
                ReportPropertyChanging("PhoneExtension");
                _PhoneExtension = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("PhoneExtension");
                OnPhoneExtensionChanged();
            }
        }
        private global::System.String _PhoneExtension;
        partial void OnPhoneExtensionChanging(global::System.String value);
        partial void OnPhoneExtensionChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String UserImage
        {
            get
            {
                return _UserImage;
            }
            set
            {
                OnUserImageChanging(value);
                ReportPropertyChanging("UserImage");
                _UserImage = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("UserImage");
                OnUserImageChanged();
            }
        }
        private global::System.String _UserImage;
        partial void OnUserImageChanging(global::System.String value);
        partial void OnUserImageChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                OnTitleChanging(value);
                ReportPropertyChanging("Title");
                _Title = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Title");
                OnTitleChanged();
            }
        }
        private global::System.String _Title;
        partial void OnTitleChanging(global::System.String value);
        partial void OnTitleChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String Certification
        {
            get
            {
                return _Certification;
            }
            set
            {
                OnCertificationChanging(value);
                ReportPropertyChanging("Certification");
                _Certification = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Certification");
                OnCertificationChanged();
            }
        }
        private global::System.String _Certification;
        partial void OnCertificationChanging(global::System.String value);
        partial void OnCertificationChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String Department
        {
            get
            {
                return _Department;
            }
            set
            {
                OnDepartmentChanging(value);
                ReportPropertyChanging("Department");
                _Department = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Department");
                OnDepartmentChanged();
            }
        }
        private global::System.String _Department;
        partial void OnDepartmentChanging(global::System.String value);
        partial void OnDepartmentChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.Int32 UserFloor
        {
            get
            {
                return _UserFloor;
            }
            set
            {
                if (_UserFloor != value)
                {
                    OnUserFloorChanging(value);
                    ReportPropertyChanging("UserFloor");
                    _UserFloor = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("UserFloor");
                    OnUserFloorChanged();
                }
            }
        }
        private global::System.Int32 _UserFloor;
        partial void OnUserFloorChanging(global::System.Int32 value);
        partial void OnUserFloorChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.Double LocationX
        {
            get
            {
                return _LocationX;
            }
            set
            {
                if (_LocationX != value)
                {
                    OnLocationXChanging(value);
                    ReportPropertyChanging("LocationX");
                    _LocationX = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("LocationX");
                    OnLocationXChanged();
                }
            }
        }
        private global::System.Double _LocationX;
        partial void OnLocationXChanging(global::System.Double value);
        partial void OnLocationXChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.Double LocationY
        {
            get
            {
                return _LocationY;
            }
            set
            {
                if (_LocationY != value)
                {
                    OnLocationYChanging(value);
                    ReportPropertyChanging("LocationY");
                    _LocationY = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("LocationY");
                    OnLocationYChanged();
                }
            }
        }
        private global::System.Double _LocationY;
        partial void OnLocationYChanging(global::System.Double value);
        partial void OnLocationYChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String School
        {
            get
            {
                return _School;
            }
            set
            {
                OnSchoolChanging(value);
                ReportPropertyChanging("School");
                _School = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("School");
                OnSchoolChanged();
            }
        }
        private global::System.String _School;
        partial void OnSchoolChanging(global::System.String value);
        partial void OnSchoolChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String UserUrl
        {
            get
            {
                return _UserUrl;
            }
            set
            {
                OnUserUrlChanging(value);
                ReportPropertyChanging("Certification");
                _UserUrl = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("UserUrl");
                OnUserUrlChanged();
            }
        }
        private global::System.String _UserUrl;
        partial void OnUserUrlChanging(global::System.String value);
        partial void OnUserUrlChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                OnStartDateChanging(value);
                ReportPropertyChanging("Certification");
                _StartDate = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("StartDate");
                OnStartDateChanged();
            }
        }
        private global::System.String _StartDate;
        partial void OnStartDateChanging(global::System.String value);
        partial void OnStartDateChanged();
        
    }

    [Serializable()]
    [DataContractAttribute(IsReference = true)]
    public partial class USLITeam : EntityObject
    {
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.Int32 TeamID
        {
            get
            {
                return _TeamID;
            }
            set
            {
                if (_TeamID != value)
                {
                    OnTeamIDChanging(value);
                    ReportPropertyChanging("TeamID");
                    _TeamID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("TeamID");
                    OnTeamIDChanged();
                }
            }
        }
        private global::System.Int32 _TeamID;
        partial void OnTeamIDChanging(global::System.Int32 value);
        partial void OnTeamIDChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String TeamName
        {
            get
            {
                return _TeamName;
            }
            set
            {
                OnTeamNameChanging(value);
                ReportPropertyChanging("TeamName");
                _TeamName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("TeamName");
                OnTeamNameChanged();
            }
        }
        private global::System.String _TeamName;
        partial void OnTeamNameChanging(global::System.String value);
        partial void OnTeamNameChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String TeamState
        {
            get
            {
                return _TeamState;
            }
            set
            {
                OnTeamStateChanging(value);
                ReportPropertyChanging("TeamState");
                _TeamState = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("TeamState");
                OnTeamStateChanged();
            }
        }
        private global::System.String _TeamState;
        partial void OnTeamStateChanging(global::System.String value);
        partial void OnTeamStateChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> isChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                OnisCheckedChanging(value);
                ReportPropertyChanging("isChecked");
                _isChecked = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("isChecked");
                OnisCheckedChanged();
            }
        }
        private Nullable<global::System.Boolean> _isChecked;
        partial void OnisCheckedChanging(Nullable<global::System.Boolean> value);
        partial void OnisCheckedChanged();


    }

    [Serializable()]
    [DataContractAttribute(IsReference = true)]
    public partial class USLICertification : EntityObject
    {
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.Int32 CertID
        {
            get
            {
                return _CertID;
            }
            set
            {
                if (_CertID != value)
                {
                    OnCertIDChanging(value);
                    ReportPropertyChanging("CertID");
                    _CertID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("CertID");
                    OnCertIDChanged();
                }
            }
        }
        private global::System.Int32 _CertID;
        partial void OnCertIDChanging(global::System.Int32 value);
        partial void OnCertIDChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String CertName
        {
            get
            {
                return _CertName;
            }
            set
            {
                OnCertNameChanging(value);
                ReportPropertyChanging("CertName");
                _CertName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("CertName");
                OnCertNameChanged();
            }
        }
        private global::System.String _CertName;
        partial void OnCertNameChanging(global::System.String value);
        partial void OnCertNameChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> isChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                OnisCheckedChanging(value);
                ReportPropertyChanging("isChecked");
                _isChecked = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("isChecked");
                OnisCheckedChanged();
            }
        }
        private Nullable<global::System.Boolean> _isChecked;
        partial void OnisCheckedChanging(Nullable<global::System.Boolean> value);
        partial void OnisCheckedChanged();
    }

    [Serializable()]
    [DataContractAttribute(IsReference = true)]
    public partial class USLITitle : EntityObject
    {
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.Int32 TitleID
        {
            get
            {
                return _TitleID;
            }
            set
            {
                if (_TitleID != value)
                {
                    OnTitleIDChanging(value);
                    ReportPropertyChanging("TitleID");
                    _TitleID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("TitleID");
                    OnTitleIDChanged();
                }
            }
        }
        private global::System.Int32 _TitleID;
        partial void OnTitleIDChanging(global::System.Int32 value);
        partial void OnTitleIDChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String TitleName
        {
            get
            {
                return _TitleName;
            }
            set
            {
                OnTitleNameChanging(value);
                ReportPropertyChanging("TitleName");
                _TitleName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("TitleName");
                OnTitleNameChanged();
            }
        }
        private global::System.String _TitleName;
        partial void OnTitleNameChanging(global::System.String value);
        partial void OnTitleNameChanged();

        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> isChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                OnisCheckedChanging(value);
                ReportPropertyChanging("isChecked");
                _isChecked = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("isChecked");
                OnisCheckedChanged();
            }
        }
        private Nullable<global::System.Boolean> _isChecked;
        partial void OnisCheckedChanging(Nullable<global::System.Boolean> value);
        partial void OnisCheckedChanged();
    }
    
}