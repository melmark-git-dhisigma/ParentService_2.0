using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.UI.WebControls;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IParentService" in both code and config file together.
[ServiceContract]
public interface IParentService
{
    [OperationContract]
    int Login(LoginDetails model);

    [OperationContract]
    int ChangePassword(LoginDetails model, int ParentID);

    [OperationContract]
    int GetStudentId(int ParentId);

     [OperationContract]
    int GetSchoolId(int ParentId);

     [OperationContract]
     string GetParentName(int ParentId);

     [OperationContract]
     string getCountry(int ParentId);

     [OperationContract]
     string getState(int ParentId);

     [OperationContract]
     string GetImageUrl(int ParentId);

    [OperationContract]
    string DownloadDoc(int documentId, int StudentId, string type = "Document");

    [OperationContract]
    ParentDetails GetParentDetails(int StudentID);

    [OperationContract]
    ParentDetailsPA GetParentDetailsPA(int StudentId);

    [OperationContract]
    //List<ProgressDetails> CLientsReport(int SchoolID, int StudentID);
    ProgressDetails CLientsReport(int SchoolID, int StudentID, int IEPID);



    [OperationContract]
    List<ReportDetails> GetProgressReport(int StudentId, int SchoolId);

    [OperationContract]
    IEnumerable<IEPDetails> GetIEPLists(int StudentId, int SchoolId, int pageIndex, int pageSize, string keyword, int filter);

    [OperationContract]
    IEnumerable<DocDetails> GetBinaryDoc(int StudentId, int SchoolId, int DocumentId);

    [OperationContract]
    int GetTotalRecordIEP(int filter, string keyword, int StudentId);

    [OperationContract]
    IEnumerable<Activities> GetActivities(int StudentId, int pageIndex, int pageSize, string keyword, int filter);

    [OperationContract]
    IEnumerable<ActivitiesList> GetActivitiesList(int ActivityId, string AType);
        
    [OperationContract]
    int GetTotalActivityCount(int StudentId, string keyword, int filter);

    [OperationContract]
    IEnumerable<ListItem> GetAllStatusForDDL(string Status);

    [OperationContract]
    IEnumerable<Documents> GetDocuments(int StudentId, int SchoolId, int pageIndex, int pageSize, string keyword, int filter);

    [OperationContract]
    int GetTotalDocumentCount(int StudentId, string keyword, int filter);

    [OperationContract]
    int FileUpload(int SchoolId, int StudentId, string DocName, string ContentType, string DocType, string other, string DocPath, int ParentId, byte[] data);

    [OperationContract]
    IEnumerable<Concents> GetConcents(int StudentId, int SchoolId, int pageIndex, int pageSize, string keyword, string filter);

    [OperationContract]
    int GetTotalConcentForms(int StudentId, string keyword, int filter);

    [OperationContract]
    int Delete(int id);

    [OperationContract]
    int SignUpdate(byte[] data, int DocumentId);


    [OperationContract]
    IEnumerable<Behaviours> GetItemsForBehaviour(string BehaviourName, int StudentId);
}
[DataContract]
public class ParentDetails
{
    [DataMember]
    public virtual string NickName { get; set; }
    [DataMember]
    public virtual string FirstName { get; set; }
    [DataMember]
    public virtual string LastName { get; set; }
    [DataMember]
    public virtual string MiddleName { get; set; }
    [DataMember]
    public virtual string CreatedOn { get; set; }
    [DataMember]
    public virtual string ImageUrl { get; set; }
    [DataMember]
    public virtual DateTime? DOB { get; set; }
    [DataMember]
    public virtual DateTime? AdmissionDate { get; set; }
    [DataMember]
    public virtual string BirthPlace { get; set; }
    [DataMember]
    public virtual string CitizenShip { get; set; }
    [DataMember]
    public virtual string PrimaryLanguage { get; set; }
    [DataMember]
    public virtual string Gender { get; set; }
    [DataMember]
    public virtual string LegalCompetencyStat { get; set; }
    [DataMember]
    public virtual string GuardianStat { get; set; }
    [DataMember]
    public virtual string OtherStateInvolveStat { get; set; }
    [DataMember]
    public virtual string ParentMaritalStat { get; set; }
    [DataMember]
    public virtual string Race { get; set; }
    [DataMember]
    public virtual decimal? Height { get; set; }
    [DataMember]
    public virtual decimal? Weight { get; set; }
    [DataMember]
    public virtual string HairColor { get; set; }
    [DataMember]
    public virtual string EyeColor { get; set; }
    [DataMember]
    public virtual string DistinguishMarks { get; set; }
    [DataMember]
    public virtual string CaseManagerRes { get; set; }
    [DataMember]
    public virtual string CaseManagerEdu { get; set; }
    [DataMember]
    public virtual string Address1 { get; set; }
    [DataMember]
    public virtual string Address2 { get; set; }
    [DataMember]
    public virtual string Address3 { get; set; }
    [DataMember]
    public virtual DateTime? DateInitEligibleforSpclEducation { get; set; }
    [DataMember]
    public virtual DateTime? DateofMostRecentSpecialEducationEvaluations { get; set; }
    [DataMember]
    public virtual DateTime? DateofNextScheduled3YearEvaluation { get; set; }
    [DataMember]
    public virtual DateTime? CrntIEPStartDate { get; set; }
    [DataMember]
    public virtual DateTime? CrntIEPExpireDate { get; set; }
    [DataMember]
    public virtual DateTime? DischargeDate { get; set; }
    [DataMember]
    public virtual string LocationAfterDischarge { get; set; }
    [DataMember]
    public virtual string MelmarkNewEnglandsFollowUpResponsibilities { get; set; }
    [DataMember]
    public virtual IEnumerable<RelationDetails> Relations { get; set; }
    [DataMember]
    public virtual IEnumerable<InsuranceDetails> InsuranceDetl { get; set; }

    [DataMember]
    public virtual MedicalDetails MedicalInform { get; set; }


    [DataMember]
    public virtual IEnumerable<RefIep> RefIEPInform { get; set; }

    [DataMember]
    public virtual IEnumerable<EmergencySchoolContcts> EmergencySchool { get; set; }
    [DataMember]
    public virtual IEnumerable<SchoolAttended> SchoolsAttended { get; set; }
    public ParentDetails()
    {
        Relations = new List<RelationDetails>();
        InsuranceDetl = new List<InsuranceDetails>();
        EmergencySchool = new List<EmergencySchoolContcts>();
        SchoolsAttended = new List<SchoolAttended>();
        MedicalInform=new MedicalDetails();
    }

}
[DataContract]
public class ParentDetailsPA
{
    [DataMember]
    public virtual string BathRoom { get; set; }
    [DataMember]
    public virtual string FirstName { get; set; }
    [DataMember]
    public virtual string LastName { get; set; }
    [DataMember]
    public virtual string MiddleName { get; set; }
    [DataMember]
    public virtual string CreatedOn { get; set; }
    [DataMember]
    public virtual DateTime? DOB { get; set; }
    [DataMember]
    public virtual DateTime? AdmissionDate { get; set; }
    [DataMember]
    public virtual string ImageUrl { get; set; }
    [DataMember]
    public virtual string OnCampus { get; set; }
    [DataMember]
    public virtual string WhenTransporting { get; set; }
    [DataMember]
    public virtual string OffCampus { get; set; }
    [DataMember]
    public virtual string Gender { get; set; }
    [DataMember]
    public virtual string Pool_Swimming { get; set; }
    [DataMember]
    public virtual string GuardianStat { get; set; }
    [DataMember]
    public virtual string Van { get; set; }
    [DataMember]
    public virtual string CommonAreas { get; set; }
    [DataMember]
    public virtual string Race { get; set; }
    [DataMember]
    public virtual decimal? Height { get; set; }
    [DataMember]
    public virtual decimal? Weight { get; set; }
    [DataMember]
    public virtual string BedroomAwake { get; set; }
    [DataMember]
    public virtual string BedroomAsleep { get; set; }
    [DataMember]
    public virtual string Task_Break { get; set; }
    [DataMember]
    public virtual string TransitionsInside { get; set; }
    [DataMember]
    public virtual string TransitionsUneven { get; set; }
    [DataMember]
    public virtual string Address1 { get; set; }
    [DataMember]
    public virtual string Address2 { get; set; }
    [DataMember]
    public virtual string Address3 { get; set; }
    [DataMember]
    public virtual int? Country { get; set; }
    [DataMember]
    public virtual int? State { get; set; }
    [DataMember]
    public virtual string City { get; set; }
    [DataMember]
    public virtual string Zip { get; set; }
    [DataMember]
    public virtual string PhoneNumber { get; set; }
    [DataMember]
    public virtual string FundingSource { get; set; }
    [DataMember]
    public virtual string RiskofResistance { get; set; }
    [DataMember]
    public virtual string Mobility { get; set; }
    [DataMember]
    public virtual string NeedforExtraHelp { get; set; }
    [DataMember]
    public virtual string ResponseToInstructions { get; set; }
    [DataMember]
    public virtual string Consciousness { get; set; }
    [DataMember]
    public virtual string WakingResponse { get; set; }
    [DataMember]
    public virtual string Allergies { get; set; }
    [DataMember]
    public virtual string Seizures { get; set; }
    [DataMember]
    public virtual string Diet { get; set; }
   
    [DataMember]
    public virtual string Other { get; set; }
    [DataMember]
    public virtual RelationDetails PrimaryCntct { get; set; }
    [DataMember]
    public virtual RelationDetails LegalGrdn1 { get; set; }
    [DataMember]
    public virtual RelationDetails LegalGrdn2 { get; set; }
    [DataMember]
    public virtual RelationDetails SupportCord { get; set; }
    [DataMember]
    public virtual RelationDetails Advocate { get; set; }
    [DataMember]
    public virtual IEnumerable<AdaptiveEquipment> AdaptiveEquip { get; set; }
    [DataMember]
    public virtual IEnumerable<BasicBehaviouralInfo> BasicBehaviouralInfo { get; set; }
    [DataMember]
    public virtual IEnumerable<Diagnoses> Diagnoses { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> LiftingOrTransfers { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> Ambulation { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> Toileting { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> Eating { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> Showering { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> ToothBrushing { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> Dressing { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> SkinCareOrSkinIntegrity { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> Communication { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> PreferredActivities { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> GeneralInformation { get; set; }
    [DataMember]
    public virtual IEnumerable<Behaviours> SuggestedProactiveEnvironmentalProcedures { get; set; }
    public ParentDetailsPA()
    {
        PrimaryCntct = new RelationDetails();
        LegalGrdn1 = new RelationDetails();
        LegalGrdn2 = new RelationDetails();
        SupportCord = new RelationDetails();
        Advocate = new RelationDetails();
        Diagnoses = new List<Diagnoses>();
        AdaptiveEquip = new List<AdaptiveEquipment>();
        BasicBehaviouralInfo = new List<BasicBehaviouralInfo>();
        LiftingOrTransfers = new List<Behaviours>();
        Ambulation = new List<Behaviours>();
        Toileting = new List<Behaviours>();
        Eating = new List<Behaviours>();
        Showering = new List<Behaviours>();
        ToothBrushing = new List<Behaviours>();
        Dressing = new List<Behaviours>();
        SkinCareOrSkinIntegrity = new List<Behaviours>();
        Communication = new List<Behaviours>();
        PreferredActivities = new List<Behaviours>();
        GeneralInformation = new List<Behaviours>();
        SuggestedProactiveEnvironmentalProcedures = new List<Behaviours>();
    }

}
[DataContract]
public class Behaviours
{
    [DataMember]
    public virtual string BehaviorName { get; set; }
    [DataMember]
    public virtual int BehaviourId { get; set; }
}
[DataContract]
public class BasicBehaviouralInfo
{
    [DataMember]
    public virtual string TargetBehaviour { get; set; }
    [DataMember]
    public virtual string Definition { get; set; }
    [DataMember]
    public virtual string Antecedent { get; set; }
    [DataMember]
    public virtual string FCT { get; set; }
    [DataMember]
    public virtual string Consequence { get; set; }
}
[DataContract]
public class AdaptiveEquipment
{
    [DataMember]
    public virtual string Item { get; set; }
    [DataMember]
    public virtual string ScheduleForUse { get; set; }
    [DataMember]
    public virtual string StorageLocation { get; set; }
    [DataMember]
    public virtual string CleaningInstructions { get; set; }
}
[DataContract]
public class Diagnoses
{
    [DataMember]
    public virtual string DiagnosesName { get; set; }
    [DataMember]
    public virtual int DiagnosesId { get; set; }
}
[DataContract]
public class RelationDetails
{
    [DataMember]
    public virtual string Relation { get; set; }
    [DataMember]
    public virtual string FName { get; set; }
    [DataMember]
    public virtual string LName { get; set; }
    [DataMember]
    public virtual string MName { get; set; }
    [DataMember]
    public virtual string PrmryLanguage { get; set; }
    [DataMember]
    public virtual string Address1 { get; set; }
    [DataMember]
    public virtual string Address2 { get; set; }
    [DataMember]
    public virtual string Address3 { get; set; }
    [DataMember]
    public virtual int? State { get; set; }
    [DataMember]
    public virtual string HomePhone { get; set; }
    [DataMember]
    public virtual string OtherPhone { get; set; }
    [DataMember]
    public virtual string WorkPhone { get; set; }
    [DataMember]
    public virtual string CellPhone { get; set; }
    [DataMember]
    public virtual string Email { get; set; }
}
[DataContract]
public class EmergencySchoolContcts
{
    [DataMember]
    public virtual string FName { get; set; }
    [DataMember]
    public virtual string LName { get; set; }
    [DataMember]
    public virtual string MName { get; set; }
    [DataMember]
    public virtual string Title { get; set; }
    [DataMember]
    public virtual string Phone { get; set; }
}
[DataContract]
public class InsuranceDetails
{
    [DataMember]
    public virtual string InsuranceType { get; set; }
    [DataMember]
    public virtual string PolicyNumber { get; set; }
    [DataMember]
    public virtual string PolicyHolder { get; set; }
}
[DataContract]
public class MedicalDetails
{
    [DataMember]
    public virtual string PhFirstName { get; set; }
    [DataMember]
    public virtual string PhLastName { get; set; }
    [DataMember]
    public virtual string Phone { get; set; }
    [DataMember]
    public virtual string Address1 { get; set; }
    [DataMember]
    public virtual string Address2 { get; set; }
    [DataMember]
    public virtual string Address3 { get; set; }
    [DataMember]
    public virtual DateTime? DtExam { get; set; }
    [DataMember]
    public virtual string Diagnosis { get; set; }
    [DataMember]
    public virtual string Allergies { get; set; }
    [DataMember]
    public virtual string CMedications { get; set; }
    [DataMember]
    public virtual string SelfPA { get; set; }
    [DataMember]
    public virtual string SBCharacteristics { get; set; }
    [DataMember]
    public virtual string Capabilities { get; set; }
    [DataMember]
    public virtual string Limitations { get; set; }
    [DataMember]
    public virtual string Preferences { get; set; }
}



[DataContract]
public class RefIep
{
    [DataMember]
    public virtual string Name { get; set; }
    [DataMember]
    public virtual string Title { get; set; }
    [DataMember]
    public virtual string Phone { get; set; }
    [DataMember]
    public virtual string ReferringAgency { get; set; }
    [DataMember]
    public virtual string SourceofTuition { get; set; }
}




[DataContract]
public class AddressDetails
{
    [DataMember]
    public virtual string Address1 { get; set; }
    [DataMember]
    public virtual string Address2 { get; set; }
    [DataMember]
    public virtual string Address3 { get; set; }
    [DataMember]
    public virtual string HomePhone { get; set; }
    [DataMember]
    public virtual string OtherPhone { get; set; }
    [DataMember]
    public virtual string Email { get; set; }
}
[DataContract]
public class SchoolAttended
{
    [DataMember]
    public virtual string Address1 { get; set; }
    [DataMember]
    public virtual string Address2 { get; set; }
    [DataMember]
    public virtual string City { get; set; }
    [DataMember]
    public virtual string State { get; set; }
    [DataMember]
    public virtual string SchoolName { get; set; }
    [DataMember]
    public virtual DateTime? DateFrom { get; set; }
    [DataMember]
    public virtual DateTime? DateTo { get; set; }
    [DataMember]
    public virtual int? ContactSequence { get; set; }
}
[DataContract]
public class IEPDetails
{
    [DataMember]
    public virtual int IEPId { get; set; }
    [DataMember]
    public virtual string IEPYear { get; set; }
    [DataMember]
    public virtual DateTime? StartDate { get; set; }
    [DataMember]
    public virtual DateTime? EndDate { get; set; }
    [DataMember]
    public virtual string Status { get; set; }
}
[DataContract]
public class DocDetails
{
    [DataMember]
    public virtual int DocumentId { get; set; }
    [DataMember]
    public virtual string DocumentName { get; set; }
    [DataMember]
    public virtual string ContentType { get; set; }
    [DataMember]
    public virtual byte[] Data { get; set; }
}
[DataContract]
public class Activities
{
    [DataMember]
    public virtual int ActivityId { get; set; }
    [DataMember]
    public virtual string ActivityName { get; set; }
    [DataMember]
    public virtual string ActivityType { get; set; }
    [DataMember]

    public virtual DateTime? Date { get; set; }
    [DataMember]
    public virtual string Status { get; set; }
}
[DataContract]
public class ActivitiesList
{
    [DataMember]
    public virtual int ActivityId { get; set; }
    [DataMember]
    public virtual string ActivityName { get; set; }
    [DataMember]
    public virtual DateTime? Date { get; set; }
    [DataMember]
    public virtual string Status { get; set; }
    [DataMember]
    public virtual string ActivityType { get; set; }
    [DataMember]
    public virtual string Program { get; set; }
    [DataMember]
    public virtual string PrimaryNurse { get; set; }
    [DataMember]
    public virtual string UnitClerk { get; set; }
    [DataMember]
    public virtual string BehaviourAnalyst { get; set; }
    [DataMember]
    public virtual DateTime? ExpiredOn { get; set; }
    [DataMember]
    public virtual DateTime? CreatedOn { get; set; }
}
[DataContract]
public class Documents
{
    [DataMember]
    public virtual int DocumentId { get; set; }
    [DataMember]
    public virtual string DocumentName { get; set; }
    [DataMember]
    public virtual string DocumentType { get; set; }
    [DataMember]
    public virtual string DocumentPath { get; set; }
    [DataMember]
    public virtual string UserType { get; set; }
    [DataMember]
    public virtual string Types { get; set; }
    [DataMember]
    public virtual string Other { get; set; }
    [DataMember]
    public virtual int UserId { get; set; }

}
[DataContract]
public class Concents
{
    [DataMember]
    public virtual int ConcentId { get; set; }
    [DataMember]
    public virtual string ConcentForm { get; set; }
    [DataMember]
    public virtual DateTime? SignedOn { get; set; }
}
[DataContract]
public class SignDetails
{
    [DataMember]
    public virtual int SignId { get; set; }
    [DataMember]
    public virtual int SignUserId { get; set; }
    [DataMember]
    public virtual string SignedUser { get; set; }
    [DataMember]
    public virtual string UserType { get; set; }
    [DataMember]
    public virtual DateTime? SignedOn { get; set; }
}


[DataContract]
public class ProgressDetails
{
    [DataMember]
    public string SclDistrictName { get; set; }
    [DataMember]
    public string SclDistrictAddress { get; set; }
    [DataMember]
    public string SclDistrictContact { get; set; }
    [DataMember]
    public string StudentName { get; set; }
    [DataMember]
    public DateTime IEPDtFrom { get; set; }
    [DataMember]
    public DateTime IEPDtTo { get; set; }
    [DataMember]
    public DateTime DOB { get; set; }
    [DataMember]
    public int ID { get; set; }

    [DataMember]
    public int stdtIEPId { get; set; }
    [DataMember]
    public virtual GoalData Goalid { get; set; }
    [DataMember]
    public virtual GoalData GoalName { get; set; }
    [DataMember]
    public virtual GoalData LessonplanName { get; set; }
    [DataMember]
    public virtual List<GoalData> GoalDt { get; set; }
    [DataMember]
    public string student { get; set; }
    [DataMember]
    public string schooladdr { get; set; }

    public ProgressDetails()
    {

        GoalDt = new List<GoalData>();
    }
    

}
[DataContract]
public class GoalData
{
    [DataMember]
    public virtual string GoalName { get; set; }
    [DataMember]
    public virtual string LessonplanName { get; set; }
    [DataMember]
    public virtual int Goalid { get; set; }
    [DataMember]
    public virtual int Lessonplanid { get; set; }
    [DataMember]
    public virtual int GoalLPRelId { get; set; }
    [DataMember]
    public virtual int GoalNo { get; set; }
    [DataMember]
    public virtual IEnumerable<GridListPlacement> PlcacementList { get; set; }
    [DataMember]
    public virtual IEnumerable<ReportInfo> RptList { get; set; }
    [DataMember]
    public virtual IEnumerable<ReportDetails> ProgressInfo { get; set; }
    public GoalData()
    {
        PlcacementList = new List<GridListPlacement>();
        RptList = new List<ReportInfo>();
        ProgressInfo = new List<ReportDetails>();

    }
}
[DataContract]
public class ReportInfo
{
    [DataMember]
    public int rptid { get; set; }
    [DataMember]
    public DateTime? rptdate { get; set; }
    [DataMember]
    public string rptinfo { get; set; }
    [DataMember]
    public int goalid { get; set; }
    [DataMember]
    public int stdtIEPId { get; set; }
    [DataMember]
    public bool? VisibleToParent { get; set; }
}
[DataContract]
public class GridListPlacement
{
    [DataMember]
    public virtual string objective1 { get; set; }
    [DataMember]
    public virtual string objective2 { get; set; }
    [DataMember]
    public virtual string objective3 { get; set; }


}
[DataContract]
public class ReportDetails
{
    [DataMember]
    public int iepid { get; set; }
    [DataMember]
    public string rptdate { get; set; }
    [DataMember]
    public DateTime startdate { get; set; }
    [DataMember]
    public DateTime enddate { get; set; }
    [DataMember]
    public virtual string Status { get; set; }
}
[DataContract]
public class LoginDetails
{
     [DataMember]
    public virtual int ParentID { get; set; }
     [DataMember]
    public virtual string UserName { get; set; }
     [DataMember]
    public virtual string Password { get; set; }
     [DataMember]
    public virtual bool? IsValid { get; set; }
     [DataMember]
    public virtual string Message { get; set; }
     [DataMember]
    public virtual string NewPassword { get; set; }
     [DataMember]
    public virtual string ConfirmPassword { get; set; }
}


