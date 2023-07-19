using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using System.Data.Entity.Design;
using ParentDb;
using System.Web.UI.WebControls;
using System.Data.Objects.SqlClient;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ParentService" in code, svc and config file together.
public class ParentService : IParentService
{
    Melmark_ParentEntities ParentEntity = null;

    public int Login(LoginDetails model)
    {
        Parent pt = new Parent();
        ParentEntity = new Melmark_ParentEntities();
        int ParentId = 0;
        pt = ParentEntity.Parents.Where(parentObject => parentObject.Username == model.UserName && parentObject.Password == model.Password).SingleOrDefault();
        if (pt != null)
        {
            string database_username = pt.Username;
            string input_username = model.UserName;

            string database_Password = pt.Password;
            string input_Password = model.Password;

            //this will perform a case sensitive comparison
            if (input_username.Equals(database_username, StringComparison.Ordinal) && input_Password.Equals(database_Password, StringComparison.Ordinal))

                ParentId = pt.ParentID;

        }
        return ParentId;
    }

    public int ChangePassword(LoginDetails model, int ParentID)
    {
        Parent pt = new Parent();
        ParentEntity = new Melmark_ParentEntities();
        int result = 0;
        pt = ParentEntity.Parents.Where(parentObject => parentObject.ParentID == ParentID && parentObject.Password == model.Password).SingleOrDefault();
        if (pt != null)
        {
            pt.Password = model.NewPassword;
            result = ParentEntity.SaveChanges();
            return result;
        }
        else return -1;
    }
    public int GetStudentId(int ParentId)
    {
        StudentParentRel stud = new StudentParentRel();
        ParentEntity = new Melmark_ParentEntities();
        stud = ParentEntity.StudentParentRels.Where(studObj => studObj.ParentID == ParentId).SingleOrDefault();
        if (stud != null)
            return stud.StudentPersonalId;
        else return 0;
    }
    public int GetSchoolId(int ParentId)
    {

        StudentPersonal schoolinfo = new StudentPersonal();
        ParentEntity = new Melmark_ParentEntities();
        int SchoolId = 0;
        int Studentid = 0;
        Studentid = GetStudentId(ParentId);
        schoolinfo = ParentEntity.StudentPersonals.Where(studObj => studObj.StudentPersonalId == Studentid).SingleOrDefault();

        if (schoolinfo != null)
        {
            SchoolId = Convert.ToInt32(schoolinfo.SchoolId);
            return SchoolId;
        }
        else return 1;
    }

    public string GetParentName(int ParentId)
    {
        User usr = new User();
        ParentEntity = new Melmark_ParentEntities();
        string ParentName = "";
        usr = ParentEntity.Users.Where(usrr => usrr.UserId == ParentId).SingleOrDefault();
        if (usr != null)
            ParentName = usr.UserLName + " , " + usr.UserFName;

        return ParentName;

    }

    public string getCountry(int countreyID)
    {
        string country = "";
        try
        {
            ParentEntity = new Melmark_ParentEntities();
            country = ParentEntity.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupId == countreyID).Select(objLookup => objLookup.LookupName).Single();
        }
        catch (Exception eX)
        {

        }
        return country;
    }
    public string getState(int stateID)
    {
        string state = "";
        try
        {
            ParentEntity = new Melmark_ParentEntities();
            state = ParentEntity.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == stateID).Select(objLookup => objLookup.LookupName).Single();
        }
        catch (Exception eX)
        {

        }
        return state;
    }



    public string GetImageUrl(int ParentId)
    {
        StudentParentRel stud = new StudentParentRel();
        ParentEntity = new Melmark_ParentEntities();
        string studid = "";
        try
        {
            studid = ParentEntity.StudentPersonals.Where(studObj => studObj.StudentPersonalId == ParentId).Select(studObj => studObj.ImageUrl).SingleOrDefault().ToString();

        }
        catch { }
        return studid;
    }

    public string DownloadDoc(int documentId, int StudentId, string type = "Document")
    {
        ParentEntity = new Melmark_ParentEntities();
        Document ObjDoc = new Document();
        Visitation ObjVisit = new Visitation();
        string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Files/Documents".ToString()).Replace('\\', '/');


        // dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
        //string dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileLocation"].ToString();
        //var attachmenttable = dbobj.Appattachments.Single(x => x.Id == id && x.RefObjectid == userSessionObj.id);
        //var userdoc = dbobj.AppAttachedFiles.Single(x => x.AttachmentId == attachmenttable.Id);
        if (type == "Document")
        {
            ObjDoc = ParentEntity.Documents.Where(objDocument => objDocument.DocumentId == documentId && objDocument.StudentPersonalId == StudentId).SingleOrDefault();
            if (ObjDoc != null)
            {
                var documentPath = dirpath + ObjDoc.DocumentId + "-" + ObjDoc.DocumentPath;
                return documentPath.Replace('\\', '/');
            }
        }
        else if (type == "Visitation")
        {
            ObjVisit = ParentEntity.Visitations.Where(objDocument => objDocument.VisitationId == documentId && objDocument.StudentPersonalId == StudentId).SingleOrDefault();
            int visitationid = ObjVisit.VisitationId;
            ObjDoc = ParentEntity.Documents.Where(objDocument => objDocument.DocumentId == visitationid && objDocument.StudentPersonalId == StudentId).SingleOrDefault();
            if (ObjVisit != null)
            {
                var documentPath = dirpath + ObjDoc.DocumentId + "-" + ObjDoc.DocumentPath;
                return documentPath.Replace('\\', '/');
            }
        }
        else if (type == "IEP")
        {

        }
        return "Failed";
    }

    public ParentDetails GetParentDetails(int StudentId)
    {

        //IList<ParentDetails> ParentDetails = new List<ParentDetails>();

        ParentEntity = new Melmark_ParentEntities();
        //Parent pt = new Parent();
        //pt = ParentEntity.Parents.Where(parentObj => parentObj.ParentID == ParentId).SingleOrDefault();

        //ParentDetails userInfo = new ParentDetails();
        ParentDetails parentInfo = new ParentDetails();

        ParentDetails parentInfolist = new ParentDetails();
        parentInfolist = (from objStudent in ParentEntity.StudentPersonals
                          join objStdtAddrRel in ParentEntity.StudentAddresRels on objStudent.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                          join objAddress in ParentEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                          where (objStudent.StudentPersonalId == StudentId & objStdtAddrRel.ContactSequence == 0)

                          select new ParentDetails
                          {
                              NickName = objStudent.NickName,
                              FirstName = objStudent.FirstName,
                              LastName = objStudent.LastName,
                              MiddleName = objStudent.MiddleName,
                              DOB = objStudent.BirthDate,
                              ImageUrl = objStudent.ImageUrl,
                              BirthPlace = objStudent.PlaceOfBirth,
                              AdmissionDate = objStudent.AdmissionDate,
                              CitizenShip = objStudent.CountryOfCitizenship,
                              PrimaryLanguage = objStudent.PrimaryLanguage,
                              Gender = objStudent.Gender,
                              LegalCompetencyStat = objStudent.LegalCompetencyStatus,
                              GuardianStat = objStudent.GuardianShip,
                              OtherStateInvolveStat = objStudent.OtherStateAgenciesInvolvedWithStudent,
                              ParentMaritalStat = objStudent.MaritalStatusofBothParents,
                              Height = objStudent.Height,
                              Weight = objStudent.Weight,
                              HairColor = objStudent.HairColor,
                              EyeColor = objStudent.EyeColor,
                              DistinguishMarks = objStudent.DistingushingMarks,
                              CaseManagerRes = objStudent.CaseManagerResidential,
                              CaseManagerEdu = objStudent.CaseManagerEducational,
                              Address1 = objAddress.ApartmentType,
                              Address2 = objAddress.StreetName,
                              Address3 = objAddress.City,
                              DateInitEligibleforSpclEducation = objStudent.DateInitiallyEligibleforSpecialEducation,
                              DateofMostRecentSpecialEducationEvaluations = objStudent.DateofMostRecentSpecialEducationEvaluations,
                              DateofNextScheduled3YearEvaluation = objStudent.DateofNextScheduled3YearEvaluation,
                              CrntIEPStartDate = objStudent.CurrentIEPStartDate,
                              CrntIEPExpireDate = objStudent.CurrentIEPExpirationDate,
                              DischargeDate = objStudent.DischargeDate,
                              LocationAfterDischarge = objStudent.LocationAfterDischarge,
                              MelmarkNewEnglandsFollowUpResponsibilities = objStudent.MelmarkNewEnglandsFollowUpResponsibilities
                          }
                                               ).SingleOrDefault();
        if (parentInfolist != null)
        {
            parentInfolist.Race = (from objStudent in ParentEntity.StudentPersonals
                                   join objLookup in ParentEntity.LookUps
                                       on objStudent.RaceId equals objLookup.LookupId
                                   where (objStudent.StudentPersonalId == StudentId)
                                   select objLookup.LookupName).SingleOrDefault();

            LookUp lk = ParentEntity.LookUps.Where(objlk => objlk.LookupName == "Primary" && objlk.LookupType == "Physician").SingleOrDefault();
           var tempdata = (from objmedical in ParentEntity.MedicalAndInsurances
                            join objaddr in ParentEntity.AddressLists
                            on objmedical.AddressId equals objaddr.AddressId
                            where (objmedical.StudentPersonalId == StudentId&&objmedical.PhysicianId==lk.LookupId)
                          
                            select new MedicalDetails
                            {
                                PhFirstName = objmedical.FirstName,
                                PhLastName = objmedical.LastName,
                                Phone = objmedical.OfficePhone,
                                Address1 = objaddr.ApartmentType,
                                Address2 = objaddr.StreetName,
                                Address3 = objaddr.City,
                                DtExam = objmedical.DateOfLastPhysicalExam,
                                Diagnosis = objmedical.MedicalConditionsDiagnosis,
                                Allergies = objmedical.Allergies,
                                CMedications = objmedical.CurrentMedications,
                                SelfPA = objmedical.SelfPreservationAbility,
                                SBCharacteristics = objmedical.SignificantBehaviorCharacteristics,
                                Capabilities = objmedical.Capabilities,
                                Limitations = objmedical.Limitations,
                                Preferences = objmedical.Preferances
                            }
                                               ).ToList(); 
            if(tempdata.Count>1)
            {
              tempdata = (from objmedical in ParentEntity.MedicalAndInsurances
                                join objaddr in ParentEntity.AddressLists
                                on objmedical.AddressId equals objaddr.AddressId
                                where (objmedical.StudentPersonalId == StudentId && objmedical.PhysicianId == lk.LookupId)
                                orderby objmedical.DateOfLastPhysicalExam descending
                                select new MedicalDetails
                                {
                                    PhFirstName = objmedical.FirstName,
                                    PhLastName = objmedical.LastName,
                                    Phone = objmedical.OfficePhone,
                                    Address1 = objaddr.ApartmentType,
                                    Address2 = objaddr.StreetName,
                                    Address3 = objaddr.City,
                                    DtExam = objmedical.DateOfLastPhysicalExam,
                                    Diagnosis = objmedical.MedicalConditionsDiagnosis,
                                    Allergies = objmedical.Allergies,
                                    CMedications = objmedical.CurrentMedications,
                                    SelfPA = objmedical.SelfPreservationAbility,
                                    SBCharacteristics = objmedical.SignificantBehaviorCharacteristics,
                                    Capabilities = objmedical.Capabilities,
                                    Limitations = objmedical.Limitations,
                                    Preferences = objmedical.Preferances
                                }
                                                ).Take(1).ToList(); 
            }

            if (tempdata.Count==0)
            {
            tempdata = (from objmedical in ParentEntity.MedicalAndInsurances
                            join objaddr in ParentEntity.AddressLists
                            on objmedical.AddressId equals objaddr.AddressId
                            where (objmedical.StudentPersonalId == StudentId)
                            orderby objmedical.DateOfLastPhysicalExam descending
                            select new MedicalDetails
                            {
                                PhFirstName = objmedical.FirstName,
                                PhLastName = objmedical.LastName,
                                Phone = objmedical.OfficePhone,
                                Address1 = objaddr.ApartmentType,
                                Address2 = objaddr.StreetName,
                                Address3 = objaddr.City,
                                DtExam = objmedical.DateOfLastPhysicalExam,
                                Diagnosis = objmedical.MedicalConditionsDiagnosis,
                                Allergies = objmedical.Allergies,
                                CMedications = objmedical.CurrentMedications,
                                SelfPA = objmedical.SelfPreservationAbility,
                                SBCharacteristics = objmedical.SignificantBehaviorCharacteristics,
                                Capabilities = objmedical.Capabilities,
                                Limitations = objmedical.Limitations,
                                Preferences = objmedical.Preferances
                            }
                                               ).Take(1).ToList(); 
            }
            parentInfolist.MedicalInform = tempdata.Count > 0 ? tempdata.First() : null;
            // parentInfolist.MedicalInform = parentInfolist.MedicalInform.OrderByDescending(x => x.DtExam).ToList().Select(y;
            //parentInfolist.Race = race;
            parentInfolist.InsuranceDetl = (from objInsurance in ParentEntity.Insurances
                                            where (objInsurance.StudentPersonalId == StudentId)

                                            select new InsuranceDetails
                                            {
                                                InsuranceType = objInsurance.InsuranceType,
                                                PolicyNumber = objInsurance.PolicyNumber,
                                                PolicyHolder = objInsurance.PolicyHolder
                                            }).ToList();


            parentInfolist.RefIEPInform = (from objRef in ParentEntity.StudentPersonals
                                           where (objRef.StudentPersonalId == StudentId)

                                           select new RefIep
                                           {
                                               Name = objRef.IEPReferralFullName,
                                               Title = objRef.IEPReferralTitle,
                                               Phone = objRef.IEPReferralPhone,
                                               ReferringAgency = objRef.IEPReferralReferrinAgency,
                                               SourceofTuition = objRef.IEPReferralSourceofTuition,
                                           }).ToList();





            parentInfolist.Relations = (from objStudent in ParentEntity.StudentPersonals
                                        join objContactPersnl in ParentEntity.ContactPersonals on objStudent.StudentPersonalId equals objContactPersnl.StudentPersonalId
                                        join objStdtAddrRel in ParentEntity.StudentAddresRels on objContactPersnl.ContactPersonalId equals objStdtAddrRel.ContactPersonalId
                                        join objAddr in ParentEntity.AddressLists on objStdtAddrRel.AddressId equals objAddr.AddressId
                                        join objStdtContactRel in ParentEntity.StudentContactRelationships on objContactPersnl.ContactPersonalId equals objStdtContactRel.ContactPersonalId
                                        join objLookUp in ParentEntity.LookUps on objStdtContactRel.RelationshipId equals objLookUp.LookupId
                                        where (objStudent.StudentPersonalId == StudentId & objStdtAddrRel.ContactSequence == 1)
                                        /*
                                         *& objAddr.AddressType > 0 & 
                                        objContactPersnl.Status == 1 & objLookUp.LookupName == "Emergency Contact" & objLookUp.LookupType == "Relationship")
                                         */
                                        select new RelationDetails
                                        {
                                            FName = objContactPersnl.FirstName,
                                            LName = objContactPersnl.LastName,
                                            MName = objContactPersnl.MiddleName,
                                            PrmryLanguage = objContactPersnl.PrimaryLanguage,
                                            Relation = objLookUp.LookupName,
                                            Address1 = objAddr.ApartmentType,
                                            Address2 = objAddr.StreetName,
                                            Address3 = objAddr.City,
                                            HomePhone = objAddr.Phone,
                                            OtherPhone = objAddr.OtherPhone,
                                            Email = objAddr.PrimaryEmail
                                        }).ToList();

            parentInfolist.EmergencySchool = (from objEmrgncy in ParentEntity.EmergencyContactSchools
                                              where (objEmrgncy.StudentPersonalId == StudentId)
                                              select new EmergencySchoolContcts
                                              {
                                                  FName = objEmrgncy.FirstName,
                                                  LName = objEmrgncy.LastName,
                                                  Title = objEmrgncy.Title,
                                                  Phone = objEmrgncy.Phone
                                              }).ToList();

            parentInfolist.SchoolsAttended = (from objSchlAttend in ParentEntity.SchoolsAttendeds
                                              where (objSchlAttend.StudentPersonalId == StudentId)
                                              select new SchoolAttended
                                              {
                                                  SchoolName = objSchlAttend.SchoolName,
                                                  Address1 = objSchlAttend.Address1,
                                                  Address2 = objSchlAttend.Address2,
                                                  City = objSchlAttend.City,
                                                  State = objSchlAttend.State,
                                                  DateFrom = objSchlAttend.DateFrom,
                                                  DateTo = objSchlAttend.DateTo,
                                                  ContactSequence = objSchlAttend.SequenceId
                                              }).OrderByDescending(schlAttend => schlAttend.ContactSequence).ToList();


        }
        return parentInfolist;
    }
    public ParentDetailsPA GetParentDetailsPA(int StudentId)
    {



        //IList<ParentDetails> ParentDetails = new List<ParentDetails>();

        ParentEntity = new Melmark_ParentEntities();
        //Parent pt = new Parent();
        //pt = ParentEntity.Parents.Where(parentObj => parentObj.ParentID == ParentId).SingleOrDefault();

        //ParentDetails userInfo = new ParentDetails();
        ParentDetailsPA parentInfo = new ParentDetailsPA();

        ParentDetailsPA parentInfolistPA = new ParentDetailsPA();
        parentInfolistPA = (from objStudent in ParentEntity.StudentPersonals
                            join objStdPA in ParentEntity.StudentPersonalPAs on objStudent.StudentPersonalId equals objStdPA.StudentPersonalId
                            join objStdtAddrRel in ParentEntity.StudentAddresRels on objStudent.StudentPersonalId equals objStdtAddrRel.StudentPersonalId
                            join objAddress in ParentEntity.AddressLists on objStdtAddrRel.AddressId equals objAddress.AddressId
                            where (objStudent.StudentPersonalId == StudentId & objStdtAddrRel.ContactSequence == 0)

                            select new ParentDetailsPA
                            {

                                FirstName = objStudent.FirstName,
                                LastName = objStudent.LastName,
                                MiddleName = objStudent.MiddleName,
                                DOB = objStudent.BirthDate,
                                AdmissionDate = objStudent.AdmissionDate,
                                Gender = objStudent.Gender,
                                Address1 = objAddress.ApartmentType,
                                Address2 = objAddress.StreetName,
                                Country = objAddress.CountryId,
                                State = objAddress.StateProvince,
                                City = objAddress.City,
                                Zip = objAddress.PostalCode,
                                PhoneNumber = objAddress.Phone,
                                BathRoom = objStdPA.Bathroom,
                                OnCampus = objStdPA.OnCampus,
                                WhenTransporting = objStdPA.WhenTranspoting,
                                OffCampus = objStdPA.OffCampus,
                                Pool_Swimming = objStdPA.PoolOrSwimming,
                                Van = objStdPA.Van,
                                CommonAreas = objStdPA.ho_CommonAres,
                                BedroomAsleep = objStdPA.ho_BedroomAsleep,
                                BedroomAwake = objStdPA.ho_BedroomAwake,
                                Task_Break = objStdPA.dy_TaskOrBreak,
                                TransitionsInside = objStdPA.dy_TransitionInside,
                                TransitionsUneven = objStdPA.dy_TransitionUnevenGround,
                                RiskofResistance = objStdPA.RiskOfResistance,
                                Mobility = objStdPA.Mobility,
                                NeedforExtraHelp = objStdPA.NeedForExtraHelp,
                                ResponseToInstructions = objStdPA.ResponseToInstruction,
                                Consciousness = objStdPA.Consciousness,
                                WakingResponse = objStdPA.WalkingResponses,
                                Allergies = objStdPA.Allergies,
                                Seizures = objStdPA.Seizures,
                                Diet = objStdPA.Diet,
                                FundingSource = objStdPA.FundingSource,
                                Other = objStdPA.Other
                            }).SingleOrDefault();






        try
        {
            if (parentInfolistPA != null)
            {

                try
                {
                   
                    var PrimaryCntctTemp = (from objStudent in ParentEntity.StudentPersonals
                                                     join objStdPA in ParentEntity.StudentPersonalPAs on objStudent.StudentPersonalId equals objStdPA.StudentPersonalId
                                                     join objContactPersnl in ParentEntity.ContactPersonals on objStudent.StudentPersonalId equals objContactPersnl.StudentPersonalId
                                                     join objStdtAddrRel in ParentEntity.StudentAddresRels on objContactPersnl.ContactPersonalId equals objStdtAddrRel.ContactPersonalId 
                                                     join objAddr in ParentEntity.AddressLists on objStdtAddrRel.AddressId equals objAddr.AddressId
                                                     join objStdtContactRel in ParentEntity.StudentContactRelationships on objContactPersnl.ContactPersonalId equals objStdtContactRel.ContactPersonalId
                                                     join objLookUp in ParentEntity.LookUps on objStdtContactRel.RelationshipId equals objLookUp.LookupId
                                                     where (objStudent.StudentPersonalId == StudentId && objLookUp.LookupName == "Primary Contact" && objLookUp.LookupType == "RelationShip" && objContactPersnl.Status == 1)
                                                     orderby objStdtAddrRel.ContactSequence
                                                     select new RelationDetails
                                                     {
                                                         FName = objContactPersnl.FirstName,
                                                         LName = objContactPersnl.LastName,
                                                         MName = objContactPersnl.MiddleName,
                                                         Relation = objLookUp.LookupName,
                                                         Address1 = objAddr.ApartmentType,
                                                         Address2 = objAddr.StreetName,
                                                         Address3 = objAddr.City,
                                                         HomePhone = objAddr.Phone,
                                                         CellPhone = objAddr.Mobile,
                                                         WorkPhone = objAddr.OtherPhone,
                                                         Email = objAddr.PrimaryEmail
                                                     }).ToList();
                    parentInfolistPA.PrimaryCntct = PrimaryCntctTemp.First();
                    foreach (var item in PrimaryCntctTemp)
                     {
                         if (item.Address1 != null && item.Address2!=null)
                         {
                             parentInfolistPA.PrimaryCntct = item;
                             break;
                         }
                     }
               

              

                }
                catch
                {
                }




                try
                {
                  
                          var LegalGrdn1Temp= (from objStudent in ParentEntity.StudentPersonals
                                                   join objStdPA in ParentEntity.StudentPersonalPAs on objStudent.StudentPersonalId equals objStdPA.StudentPersonalId
                                                   join objContactPersnl in ParentEntity.ContactPersonals on objStudent.StudentPersonalId equals objContactPersnl.StudentPersonalId
                                                   join objStdtAddrRel in ParentEntity.StudentAddresRels on objContactPersnl.ContactPersonalId equals objStdtAddrRel.ContactPersonalId
                                                   join objAddr in ParentEntity.AddressLists on objStdtAddrRel.AddressId equals objAddr.AddressId
                                                   join objStdtContactRel in ParentEntity.StudentContactRelationships on objContactPersnl.ContactPersonalId equals objStdtContactRel.ContactPersonalId
                                                   join objLookUp in ParentEntity.LookUps on objStdtContactRel.RelationshipId equals objLookUp.LookupId
                                                   where (objStudent.StudentPersonalId == StudentId && objLookUp.LookupName == "Legal Guardian 1" && objLookUp.LookupType == "RelationShip" && objContactPersnl.Status==1)
                                                    orderby objStdtAddrRel.ContactSequence
                                                   select new RelationDetails
                                                   {
                                                       FName = objContactPersnl.FirstName,
                                                       LName = objContactPersnl.LastName,
                                                       MName = objContactPersnl.MiddleName,
                                                       Relation = objLookUp.LookupName,

                                                       Address1 = objAddr.ApartmentType,
                                                       Address2 = objAddr.StreetName,
                                                       Address3 = objAddr.City,
                                                       HomePhone = objAddr.Phone,
                                                       CellPhone = objAddr.Mobile,
                                                       Email = objAddr.PrimaryEmail,
                                                       WorkPhone = objAddr.OtherPhone
                                                   }).ToList();

                          parentInfolistPA.LegalGrdn1 = LegalGrdn1Temp.First();
                          foreach (var item in LegalGrdn1Temp)
                          {
                              if (item.Address1 != null && item.Address2 != null)
                              {
                                  parentInfolistPA.LegalGrdn1 = item;
                                  break;
                              }
                          }
                }
                catch { }
                try
                {
                    var LegalGrdn2Temp= (from objStudent in ParentEntity.StudentPersonals
                                                   join objStdPA in ParentEntity.StudentPersonalPAs on objStudent.StudentPersonalId equals objStdPA.StudentPersonalId
                                                   join objContactPersnl in ParentEntity.ContactPersonals on objStudent.StudentPersonalId equals objContactPersnl.StudentPersonalId
                                                   join objStdtAddrRel in ParentEntity.StudentAddresRels on objContactPersnl.ContactPersonalId equals objStdtAddrRel.ContactPersonalId
                                                   join objAddr in ParentEntity.AddressLists on objStdtAddrRel.AddressId equals objAddr.AddressId
                                                   join objStdtContactRel in ParentEntity.StudentContactRelationships on objContactPersnl.ContactPersonalId equals objStdtContactRel.ContactPersonalId
                                                   join objLookUp in ParentEntity.LookUps on objStdtContactRel.RelationshipId equals objLookUp.LookupId
                                                   where (objStudent.StudentPersonalId == StudentId && objLookUp.LookupName == "Legal Guardian 2" && objLookUp.LookupType == "RelationShip" && objContactPersnl.Status==1)
                                                   orderby objStdtAddrRel.ContactSequence
                                                   select new RelationDetails
                                                   {
                                                       FName = objContactPersnl.FirstName,
                                                       LName = objContactPersnl.LastName,
                                                       MName = objContactPersnl.MiddleName,
                                                       Relation = objLookUp.LookupName,

                                                       Address1 = objAddr.ApartmentType,
                                                       Address2 = objAddr.StreetName,
                                                       Address3 = objAddr.City,
                                                       HomePhone = objAddr.Phone,
                                                       CellPhone = objAddr.Mobile,
                                                       Email = objAddr.PrimaryEmail,
                                                       WorkPhone = objAddr.OtherPhone
                                                   }).ToList();

                    parentInfolistPA.LegalGrdn2 = LegalGrdn2Temp.First();
                    foreach (var item in LegalGrdn2Temp)
                          {
                              if (item.Address1 != null && item.Address2 != null)
                              {
                                  parentInfolistPA.LegalGrdn2 = item;
                                  break;
                              }
                          }
                 
                }
                catch { }
                                            
                try
                {
                   var SupportCordTemp= (from objStudent in ParentEntity.StudentPersonals
                                                    join objStdPA in ParentEntity.StudentPersonalPAs on objStudent.StudentPersonalId equals objStdPA.StudentPersonalId
                                                    join objContactPersnl in ParentEntity.ContactPersonals on objStudent.StudentPersonalId equals objContactPersnl.StudentPersonalId
                                                    join objStdtAddrRel in ParentEntity.StudentAddresRels on objContactPersnl.ContactPersonalId equals objStdtAddrRel.ContactPersonalId
                                                    join objAddr in ParentEntity.AddressLists on objStdtAddrRel.AddressId equals objAddr.AddressId
                                                    join objStdtContactRel in ParentEntity.StudentContactRelationships on objContactPersnl.ContactPersonalId equals objStdtContactRel.ContactPersonalId
                                                    join objLookUp in ParentEntity.LookUps on objStdtContactRel.RelationshipId equals objLookUp.LookupId
                                                    where (objStudent.StudentPersonalId == StudentId && objLookUp.LookupName == "Support Coordinator" && objLookUp.LookupType == "RelationShip" && objContactPersnl.Status==1)
                                                    orderby objStdtAddrRel.ContactSequence
                                                    select new RelationDetails
                                                    {
                                                        FName = objContactPersnl.FirstName,
                                                        LName = objContactPersnl.LastName,
                                                        MName = objContactPersnl.MiddleName,
                                                        Relation = objLookUp.LookupName,

                                                        Address1 = objAddr.ApartmentType,
                                                        Address2 = objAddr.StreetName,
                                                        Address3 = objAddr.City,
                                                        HomePhone = objAddr.Phone,
                                                        WorkPhone = objAddr.OtherPhone,
                                                        CellPhone = objAddr.Mobile,
                                                        Email = objAddr.PrimaryEmail
                                                    }).ToList();

                   parentInfolistPA.SupportCord = SupportCordTemp.First();
                   foreach (var item in SupportCordTemp)
                          {
                              if (item.Address1 != null && item.Address2 != null)
                              {
                                  parentInfolistPA.SupportCord = item;
                                  break;
                              }
                          }
                 
                     
                }
                catch { }
                try
                {
                  var AdvocateTemp= (from objStudent in ParentEntity.StudentPersonals
                                                 join objStdPA in ParentEntity.StudentPersonalPAs on objStudent.StudentPersonalId equals objStdPA.StudentPersonalId
                                                 join objContactPersnl in ParentEntity.ContactPersonals on objStudent.StudentPersonalId equals objContactPersnl.StudentPersonalId
                                                 join objStdtAddrRel in ParentEntity.StudentAddresRels on objContactPersnl.ContactPersonalId equals objStdtAddrRel.ContactPersonalId
                                                 join objAddr in ParentEntity.AddressLists on objStdtAddrRel.AddressId equals objAddr.AddressId
                                                 join objStdtContactRel in ParentEntity.StudentContactRelationships on objContactPersnl.ContactPersonalId equals objStdtContactRel.ContactPersonalId
                                                 join objLookUp in ParentEntity.LookUps on objStdtContactRel.RelationshipId equals objLookUp.LookupId
                                                 where (objStudent.StudentPersonalId == StudentId && objLookUp.LookupName == "Advocate" && objLookUp.LookupType == "RelationShip" && objContactPersnl.Status==1)
                                                 orderby objStdtAddrRel.ContactSequence
                                                 select new RelationDetails
                                                 {
                                                     FName = objContactPersnl.FirstName,
                                                     LName = objContactPersnl.LastName,
                                                     MName = objContactPersnl.MiddleName,
                                                     Relation = objLookUp.LookupName,

                                                     Address1 = objAddr.ApartmentType,
                                                     Address2 = objAddr.StreetName,
                                                     Address3 = objAddr.City,
                                                     HomePhone = objAddr.Phone,
                                                     WorkPhone = objAddr.OtherPhone,
                                                     CellPhone = objAddr.Mobile,
                                                     Email = objAddr.PrimaryEmail
                                                 }).ToList();

                  parentInfolistPA.Advocate = AdvocateTemp.First();
                  foreach (var item in AdvocateTemp)
                          {
                              if (item.Address1 != null && item.Address2 != null)
                              {
                                  parentInfolistPA.Advocate = item;
                                  break;
                              }
                          }
                     
                }
                catch { }


                try
                {
                    parentInfolistPA.Diagnoses = (from objDiag in ParentEntity.DiaganosesPAs
                                                  join objStd in ParentEntity.StudentPersonals on objDiag.StudentPersonalId equals objStd.StudentPersonalId
                                                  join objStdPA in ParentEntity.StudentPersonalPAs on objStd.StudentPersonalId equals objStdPA.StudentPersonalId
                                                  where (objStd.StudentPersonalId == StudentId)
                                                  select new Diagnoses
                                                  {
                                                      DiagnosesId = objDiag.DiaganosePAId,
                                                      DiagnosesName = objDiag.Diaganoses
                                                  }).ToList();


                }
                catch { }


                try
                {
                    parentInfolistPA.AdaptiveEquip = (from objAdaptive in ParentEntity.AdaptiveEquipments
                                                      join objStd in ParentEntity.StudentPersonals on objAdaptive.StudentPersonalId equals objStd.StudentPersonalId
                                                      join objStdPA in ParentEntity.StudentPersonalPAs on objStd.StudentPersonalId equals objStdPA.StudentPersonalId
                                                      where (objStd.StudentPersonalId == StudentId)
                                                      select new AdaptiveEquipment
                                                      {
                                                          Item = objAdaptive.Item,
                                                          ScheduleForUse = objAdaptive.ScheduleForUse,
                                                          StorageLocation = objAdaptive.StorageLocation,
                                                          CleaningInstructions = objAdaptive.CleaningInstruction
                                                      }).ToList();
                }


                catch { }
                try
                {
                    parentInfolistPA.BasicBehaviouralInfo = (from objBasic in ParentEntity.BasicBehavioralInformations
                                                             join objStd in ParentEntity.StudentPersonals on objBasic.StudentPersonalId equals objStd.StudentPersonalId
                                                             join objStdPA in ParentEntity.StudentPersonalPAs on objStd.StudentPersonalId equals objStdPA.StudentPersonalId
                                                             where (objStd.StudentPersonalId == StudentId)
                                                             select new BasicBehaviouralInfo
                                                             {
                                                                 TargetBehaviour = objBasic.TargetBehavior,
                                                                 Definition = objBasic.Definition,
                                                                 Antecedent = objBasic.Antecedent,
                                                                 FCT = objBasic.FCT,
                                                                 Consequence = objBasic.Consequence
                                                             }).ToList();

                }
                catch
                {

                }


                try
                {
                    parentInfolistPA.LiftingOrTransfers = GetItemsForBehaviour("LIFTING / TRANSFERS", StudentId);
                    parentInfolistPA.Ambulation = GetItemsForBehaviour("AMBULATION", StudentId);
                    parentInfolistPA.Toileting = GetItemsForBehaviour("TOILETING", StudentId);
                    parentInfolistPA.Eating = GetItemsForBehaviour("EATING", StudentId);
                    parentInfolistPA.Showering = GetItemsForBehaviour("SHOWERING", StudentId);
                    parentInfolistPA.ToothBrushing = GetItemsForBehaviour("TOOTHBRUSHING", StudentId);
                    parentInfolistPA.Dressing = GetItemsForBehaviour("DRESSING", StudentId);
                    parentInfolistPA.SkinCareOrSkinIntegrity = GetItemsForBehaviour("SKIN CARE/SKIN INTEGRITY", StudentId);
                    parentInfolistPA.Communication = GetItemsForBehaviour("COMMUNICATION", StudentId);
                    parentInfolistPA.PreferredActivities = GetItemsForBehaviour("PREFERRED ACTIVITIES", StudentId);
                    parentInfolistPA.GeneralInformation = GetItemsForBehaviour("GENERAL INFORMATION", StudentId);
                    parentInfolistPA.SuggestedProactiveEnvironmentalProcedures = GetItemsForBehaviour("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES", StudentId);

                }
                catch
                {
                }
            }
        }
        catch
        {
        }
        return parentInfolistPA;
    }
    public IEnumerable<Behaviours> GetItemsForBehaviour(string BehaviourName, int StudentId)
    {
        IEnumerable<Behaviours> behaviourLists = new List<Behaviours>();
        ParentEntity = new Melmark_ParentEntities();

        behaviourLists = (from objBehaviour in ParentEntity.BehavioursPAs
                          join objBehaviourLookup in ParentEntity.BehaveLookups on objBehaviour.ParentId equals objBehaviourLookup.BehaviouralId
                          join objStd in ParentEntity.StudentPersonals on objBehaviour.StudentPersonalId equals objStd.StudentPersonalId
                          join objStdPA in ParentEntity.StudentPersonalPAs on objStd.StudentPersonalId equals objStdPA.StudentPersonalId
                          where (objStd.StudentPersonalId == StudentId & objBehaviourLookup.BehaviouralName == BehaviourName)
                          select new Behaviours
                          {
                              BehaviorName = objBehaviour.BehaviourName,
                              BehaviourId = objBehaviourLookup.BehaviouralId
                          }).ToList();

        return behaviourLists;
    }


    public IEnumerable<DocDetails> GetBinaryDoc(int StudentId, int SchoolId, int DocumentId)
    {
        ParentEntity = new Melmark_ParentEntities();
        IEnumerable<DocDetails> IEPBinaryDoc = new List<DocDetails>();

        IEPBinaryDoc = (from objBin in ParentEntity.binaryFiles
                        where (objBin.StudentId == StudentId && objBin.SchoolId == SchoolId && objBin.BinaryId == DocumentId)
                        select new DocDetails
                        {
                            DocumentId = objBin.DocId.HasValue ? objBin.DocId.Value : 0,
                            DocumentName = objBin.DocumentName,
                            ContentType = objBin.ContentType,
                            Data = objBin.Data,
                        }).ToList();


        return IEPBinaryDoc;
    }


    public IEnumerable<IEPDetails> GetIEPLists(int StudentId, int SchoolId, int pageIndex, int pageSize, string keyword = "", int filter = 0)
    {
        ParentEntity = new Melmark_ParentEntities();
        IEnumerable<IEPDetails> IEPinfoList = new List<IEPDetails>();

        if (SchoolId == 1)
        {
            IEPinfoList = (from objIEP in ParentEntity.StdtIEPs
                           join objLookup in ParentEntity.LookUps on objIEP.StatusId equals objLookup.LookupId
                           join objBin in ParentEntity.binaryFiles on objIEP.StdtIEPId equals objBin.IEPId
                           join objAsmntYr in ParentEntity.AsmntYears on objIEP.AsmntYearId equals objAsmntYr.AsmntYearId
                           where (objBin.StudentId == StudentId && objBin.ModuleName == "IEP" && objBin.type == "BW" && objBin.Varified==true && objBin.AllowParent==true)
                           select new IEPDetails
                           {
                               IEPId = (int)objBin.BinaryId,
                               IEPYear = objAsmntYr.AsmntYearDesc,
                               StartDate = objIEP.EffStartDate,
                               EndDate = objIEP.EffEndDate,
                               Status = objLookup.LookupName,
                           }).ToList();

        }
        else
        {
            IEPinfoList = (from objIEP in ParentEntity.StdtIEP_PE
                           join objLookup in ParentEntity.LookUps on objIEP.StatusId equals objLookup.LookupId
                           join objBin in ParentEntity.binaryFiles on objIEP.StdtIEP_PEId equals objBin.IEPId
                           join objAsmntYr in ParentEntity.AsmntYears on objIEP.AsmntYearId equals objAsmntYr.AsmntYearId
                           where (objBin.StudentId == StudentId && objBin.ModuleName == "IEP" && objBin.type == "BW" && objBin.Varified == true && objBin.AllowParent==true)
                           select new IEPDetails
                           {
                               IEPId = (int)objBin.BinaryId,
                               IEPYear = objAsmntYr.AsmntYearDesc,
                               StartDate = objIEP.EffStartDate,
                               EndDate = objIEP.EffEndDate,
                               Status = objLookup.LookupName,
                           }).ToList();

        }




        return IEPinfoList;
    }
    public int GetTotalRecordIEP(int filter, string keyword, int StudentId)
    {
        ParentEntity = new Melmark_ParentEntities();
        int count = 0;
        if (filter != 0)
        {

            // Determine the total number of products being paged through (needed to compute PageCount) without filter
            count = this.ParentEntity.StdtIEPs.Join(this.ParentEntity.LookUps, v => v.StatusId, l => l.LookupId, (v, l) => new
            {
                StudentId = v.StudentId,
                YearId = v.AsmntYearId,
                Lookupname = l.LookupName,
                Lookupid = l.LookupId
            }).Join(this.ParentEntity.AsmntYears, iep => iep.YearId, yr => yr.AsmntYearId, (iep, yr) => new
            {
                StudentId = iep.StudentId,
                IEPYear = yr.AsmntYearDesc,
                Lookupname = iep.Lookupname,
                Lookupid = iep.Lookupid
            }).Where(cond => cond.Lookupid == filter && cond.StudentId == StudentId && cond.IEPYear.StartsWith(keyword)).Count();
        }
        else
        {

            // Determine the total number of products being paged through (needed to compute PageCount) without filter
            count = this.ParentEntity.StdtIEPs.Join(this.ParentEntity.LookUps, v => v.StatusId, l => l.LookupId, (v, l) => new
            {
                StudentId = v.StudentId,
                YearId = v.AsmntYearId
            }).Join(this.ParentEntity.AsmntYears, iep => iep.YearId, yr => yr.AsmntYearId, (iep, yr) => new
            {
                StudentId = iep.StudentId,
                IEPYear = yr.AsmntYearDesc
            }).Where(cond => cond.StudentId == StudentId && cond.IEPYear.StartsWith(keyword)).Count();
        }


        return count;

    }
    public IEnumerable<Activities> GetActivities(int StudentId, int pageIndex, int pageSize, string keyword, int filter)
    {
        ParentEntity = new Melmark_ParentEntities();
        IEnumerable<Activities> ActivityInfo = new List<Activities>();
        IEnumerable<Activities> Events = new List<Activities>();
        IEnumerable<Activities> Placements = new List<Activities>();
        if (filter != 0)
        {
            ActivityInfo = (from objActivities in ParentEntity.Visitations
                            join objLookup in ParentEntity.LookUps on objActivities.VisitationStatus equals objLookup.LookupId
                            where (objActivities.StudentPersonalId == StudentId && objActivities.VisitationName.StartsWith(keyword) && objActivities.VisitationStatus == filter && objActivities.Status == 1)
                            select new Activities
                            {
                                ActivityId = objActivities.VisitationId,
                                ActivityName = objActivities.VisitationName,
                                Date = objActivities.VisitationDate,
                                Status = objLookup.LookupName,
                                ActivityType = "Visitation/Trip"
                            });

            Events = (from objActivities in ParentEntity.Events
                      join objLookup in ParentEntity.LookUps on objActivities.EventStatus equals objLookup.LookupId
                      where (objActivities.StudentPersonalId == StudentId && objActivities.EventsName.StartsWith(keyword) && objActivities.EventStatus == filter && objActivities.Status == 1)
                      select new Activities
                      {
                          ActivityId = objActivities.EventId,
                          ActivityName = objActivities.EventsName,
                          Date = objActivities.EventDate,
                          Status = objLookup.LookupName,
                          ActivityType = "EventLogs"
                      });
            ActivityInfo = ActivityInfo.Concat(Events);
            Placements = (from objActivities in ParentEntity.Placements
                          join objLookup in ParentEntity.LookUps on objActivities.Status equals objLookup.LookupId
                          join objLkup in ParentEntity.LookUps on objActivities.PlacementType equals objLkup.LookupId
                          where (objActivities.StudentPersonalId == StudentId &&  objActivities.Status == filter && objActivities.Status == 1)
                          select new Activities
                          {
                              ActivityId = objActivities.PlacementId,
                              ActivityName = objLkup.LookupName,
                              Date = objActivities.StartDate,
                              Status = objLookup.LookupName,
                              ActivityType = "Placement"
                          });
            ActivityInfo = ActivityInfo.Concat(Placements);
        }
        else
        {
            ActivityInfo = (from objActivities in ParentEntity.Visitations
                            join objLookup in ParentEntity.LookUps on objActivities.VisitationStatus equals objLookup.LookupId
                            where (objActivities.StudentPersonalId == StudentId && objActivities.VisitationName.StartsWith(keyword) && objActivities.Status == 1)
                            select new Activities
                            {
                                ActivityId = objActivities.VisitationId,
                                ActivityName = objActivities.VisitationName,
                                Date = objActivities.VisitationDate,
                                Status = objLookup.LookupName,
                                ActivityType = "Visitation/Trip"
                            });
            Events = (from objActivities in ParentEntity.Events
                      join objLookup in ParentEntity.LookUps on objActivities.EventStatus equals objLookup.LookupId
                      where (objActivities.StudentPersonalId == StudentId && objActivities.EventsName.StartsWith(keyword) && objActivities.Status == 1)
                      select new Activities
                      {
                          ActivityId = objActivities.EventId,
                          ActivityName = objActivities.EventsName,
                          Date = objActivities.EventDate,
                          Status = objLookup.LookupName,
                          ActivityType = "EventLogs"
                      });

            ActivityInfo = ActivityInfo.Concat(Events);
            Placements = (from objActivities in ParentEntity.Placements
                          join objLookup in ParentEntity.LookUps on objActivities.Status equals objLookup.LookupId
                          join objLkup in ParentEntity.LookUps on objActivities.PlacementType equals objLkup.LookupId
                          where (objActivities.StudentPersonalId == StudentId && objActivities.Status == 1)
                          select new Activities
                          {
                              ActivityId = objActivities.PlacementId,
                              ActivityName = objLkup.LookupName,
                              Date = objActivities.StartDate,
                              Status = objLookup.LookupName,
                              ActivityType = "Placement"
                          });
            ActivityInfo = ActivityInfo.Concat(Placements);
        }

        return ActivityInfo;
    }
    public int GetTotalActivityCount(int StudentId, string keyword, int filter)
    {
        ParentEntity = new Melmark_ParentEntities();
        int count = 0;
        if (filter != 0)
        {

            // Determine the total number of products being paged through (needed to compute PageCount) with filter
            count = this.ParentEntity.Visitations.Join(this.ParentEntity.LookUps, v => v.VisitationStatus, l => l.LookupId, (v, l) => new
            {
                VisitationStatus = v.VisitationStatus,
                Lookupid = l.LookupId,
                Visitationname = v.VisitationName,
                StudentId = v.StudentPersonalId,
                LookupName = l.LookupName
            }).Where(cond => cond.Lookupid == filter && cond.StudentId == StudentId && cond.Visitationname.StartsWith(keyword)).Count();
        }
        else
        {

            // Determine the total number of products being paged through (needed to compute PageCount) without filter
            count = this.ParentEntity.Visitations.Join(this.ParentEntity.LookUps, v => v.VisitationStatus, l => l.LookupId, (v, l) => new
            {
                VisitationStatus = v.VisitationStatus,
                Lookupid = l.LookupId,
                Visitationname = v.VisitationName,
                StudentId = v.StudentPersonalId,
                LookupName = l.LookupName
            }).Where(cond => cond.StudentId == StudentId && cond.Visitationname.StartsWith(keyword)).Count();
        }

        return count;
    }
    public IEnumerable<ActivitiesList> GetActivitiesList(int ActivityId, string AType)
    {
        ParentEntity = new Melmark_ParentEntities();
        IEnumerable<ActivitiesList> ActivityListInfo = new List<ActivitiesList>();
        IEnumerable<ActivitiesList> Events = new List<ActivitiesList>();
        IEnumerable<ActivitiesList> Placements = new List<ActivitiesList>();

        if (AType == "Visitation/Trip")
        {
            ActivityListInfo = (from objActivities in ParentEntity.Visitations
                                join objLookup in ParentEntity.LookUps on objActivities.VisitationStatus equals objLookup.LookupId
                                join objLookup1 in ParentEntity.LookUps on objActivities.VisittaionType equals objLookup1.LookupId
                                where (objActivities.VisitationId == ActivityId)
                                select new ActivitiesList
                                {
                                    ActivityId = objActivities.VisitationId,
                                    ActivityName = objActivities.VisitationName,
                                    Date = objActivities.VisitationDate,
                                    Status = objLookup.LookupName,
                                    ActivityType = objLookup1.LookupName,
                                    ExpiredOn = objActivities.ExpiredOn,
                                    CreatedOn = objActivities.CreatedOn

                                }).ToList();
        }
        else if (AType == "EventLogs")
        {
            ActivityListInfo = new List<ActivitiesList>();
            Events = (from objActivities in ParentEntity.Events
                      join objLookup in ParentEntity.LookUps on objActivities.EventStatus equals objLookup.LookupId

                      where (objActivities.EventId == ActivityId)
                      select new ActivitiesList
                      {
                          ActivityId = objActivities.EventId,
                          ActivityName = objActivities.EventsName,
                          Date = objActivities.EventDate,
                          Status = objLookup.LookupName,
                          ActivityType = objActivities.EventType,
                          ExpiredOn = objActivities.ExpiredOn,
                          CreatedOn = objActivities.CreatedOn

                      }).ToList();
            ActivityListInfo = ActivityListInfo.Concat(Events);
        }
        else if (AType == "Placement")
        {
            ActivityListInfo = new List<ActivitiesList>();
            Placements = (from objActivities in ParentEntity.Placements
                          //join objLookup in ParentEntity.LookUps on objActivities.Status equals objLookup.LookupId
                          join objpr in ParentEntity.Users on objActivities.PrimaryNurse equals objpr.UserId into objpr_f
                          from objprNull in objpr_f.DefaultIfEmpty()
                          join objunit in ParentEntity.Users on objActivities.UnitClerk equals objunit.UserId into objunit_f
                          from objunitNull in objunit_f.DefaultIfEmpty()
                          join objbehav in ParentEntity.Users on objActivities.BehaviorAnalyst equals objbehav.UserId into objbeh_f
                          from objbehavNull in objbeh_f.DefaultIfEmpty()
                          join objdept in ParentEntity.LookUps on objActivities.Department equals objdept.LookupId
                          join objprog in ParentEntity.LookUps on objActivities.PlacementType equals objprog.LookupId
                          where (objActivities.PlacementId == ActivityId)
                          select new ActivitiesList
                          {
                              ActivityId = objActivities.PlacementId,
                              // ActivityName = objActivities.EventsName,
                              Program = objprog.LookupName,
                              PrimaryNurse = objprNull.UserLName + "," + objprNull.UserFName,
                              UnitClerk = objunitNull.UserLName + "," + objunitNull.UserFName,
                              BehaviourAnalyst = objbehavNull.UserLName + "," + objbehavNull.UserFName,
                              Date = objActivities.StartDate,
                             // Status = objLookup.LookupName,
                              ActivityType = objdept.LookupName,
                              ExpiredOn = objActivities.EndDate,
                              CreatedOn = objActivities.StartDate

                          });
            ActivityListInfo = ActivityListInfo.Concat(Placements);
        
        }
       

        return ActivityListInfo;
    }
    public IEnumerable<ListItem> GetAllStatusForDDL(string Status)
    {
        ParentEntity = new Melmark_ParentEntities();

        IEnumerable<ListItem> lists = (from objLookups in ParentEntity.LookUps
                                       where (objLookups.LookupType == Status && objLookups.LookupName != "Consent" && objLookups.LookupName != "Visitation")
                                       select new ListItem
                                       {
                                           Text = objLookups.LookupName,
                                           Value = SqlFunctions.StringConvert((double)objLookups.LookupId).Trim()
                                       });
        return lists;
    }
    public IEnumerable<Documents> GetDocuments(int StudentId, int SchoolId, int pageIndex, int pageSize, string keyword, int filter)
    {
        ParentEntity = new Melmark_ParentEntities();
        List<Documents> DocmntInfoList = new List<Documents>();

        if (keyword == null) keyword = "";

        try
        {
            if (filter != 0)
            {
                int LookId = 0;
                if (filter != 0)
                {
                    LookId = Convert.ToInt16(filter);
                }
                var Look = from lk in ParentEntity.LookUps
                           where lk.LookupId == LookId
                           select lk;
                var lkName = Look.FirstOrDefault().LookupName;


                DocmntInfoList = (from objDoc in ParentEntity.binaryFiles
                                  where (objDoc.StudentId == StudentId && objDoc.Varified == true && objDoc.SchoolId == SchoolId && objDoc.ModuleName == lkName && objDoc.ModuleName != "Consent" && objDoc.ModuleName != "IEP" && objDoc.DocumentName.StartsWith(keyword) && objDoc.type != "Referal")
                                  select new Documents
                                  {
                                      DocumentId = objDoc.BinaryId,
                                      DocumentName = objDoc.DocumentName,
                                      DocumentType = objDoc.ModuleName,
                                      DocumentPath = "",
                                      UserType = "",
                                      Types = objDoc.type,
                                      UserId = (int)objDoc.CreatedBy

                                  }).ToList();

            }
            else
            {
                DocmntInfoList = (from objDoc in ParentEntity.binaryFiles
                                  where (objDoc.StudentId == StudentId && objDoc.Varified == true && objDoc.SchoolId == SchoolId && objDoc.ModuleName != "Consent" && objDoc.ModuleName != "IEP" && objDoc.DocumentName.StartsWith(keyword) && objDoc.type != "Referal")
                                  select new Documents
                                  {
                                      DocumentId = objDoc.BinaryId,
                                      DocumentName = objDoc.DocumentName,
                                      DocumentType = objDoc.ModuleName,
                                      DocumentPath = "",
                                      UserType = "",
                                      Types = objDoc.type,
                                      UserId = (int)objDoc.CreatedBy
                                  }).ToList();
            }

            if (DocmntInfoList != null)
            {
                foreach (var item in DocmntInfoList)
                {
                    if (item.Types != "Parent")
                    {
                        var data = (from objuser in ParentEntity.Users
                                    join objurg in ParentEntity.UserRoleGroups on objuser.UserId equals objurg.UserId
                                    join objrg in ParentEntity.RoleGroups on objurg.RoleGroupId equals objrg.RoleGroupId
                                    join objrole in ParentEntity.Roles on objrg.RoleId equals objrole.RoleId
                                    where (objuser.UserId == item.UserId)
                                    select new Documents
                                    {
                                        UserType = objrole.RoleDesc
                                    }).SingleOrDefault();
                        item.UserType = data.UserType;

                    }
                    else if (item.Types == "Parent")
                    {
                        item.UserType = "Parent";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
        }
        return DocmntInfoList;
    }
    public int GetTotalDocumentCount(int StudentId, string keyword, int filter)
    {
        ParentEntity = new Melmark_ParentEntities();
        int count = 0;
        if (filter != 0)
        {

            // Determine the total number of products being paged through (needed to compute PageCount) with filter
            count = this.ParentEntity.Documents.Join(this.ParentEntity.LookUps, v => v.DocumentType, l => l.LookupId, (v, l) => new
            {
                DocumentType = v.DocumentType,
                Lookupid = l.LookupId,
                DocumentName = v.DocumentName,
                StudentId = v.StudentPersonalId,
                LookupName = l.LookupName,
                Status = v.Status
            }).Where(cond => cond.Lookupid == filter && cond.Status == true && cond.LookupName != "Consent" && cond.LookupName != "Visitation" && cond.StudentId == StudentId && cond.DocumentName.StartsWith(keyword)).Count();
        }
        else
        {

            // Determine the total number of products being paged through (needed to compute PageCount) without filter
            count = this.ParentEntity.Documents.Join(this.ParentEntity.LookUps, v => v.DocumentType, l => l.LookupId, (v, l) => new
            {
                DocumentType = v.DocumentType,
                Lookupid = l.LookupId,
                DocumentName = v.DocumentName,
                StudentId = v.StudentPersonalId,
                LookupName = l.LookupName,
                Status = v.Status
            }).Where(cond => cond.StudentId == StudentId && cond.Status == true && cond.LookupName != "Consent" && cond.LookupName != "Visitation" && cond.DocumentName.StartsWith(keyword)).Count();
        }

        return count;
    }

    public int FileUpload(int SchoolId, int StudentId, string DocName, string ContentType, string DocType, string other, string DocPath, int ParentId, byte[] data)
    {
        int rtrn = -1;
        int rtrnval = -1;
        ParentEntity = new Melmark_ParentEntities();
        binaryFile binfile = new binaryFile();
        int LookId = 0;
        if (DocType != null)
        {
            LookId = Convert.ToInt16(DocType);
        }



        var Look = from lk in ParentEntity.LookUps
                   where lk.LookupId == LookId
                   select lk;

        var lkName = Look.FirstOrDefault();

        Melmark_ParentEntities dbobj = new Melmark_ParentEntities();



        Document tblDoc = new Document();
        tblDoc.DocumentName = DocName;
        tblDoc.DocumentType = LookId;
        tblDoc.Other = other;
        tblDoc.SchoolId = SchoolId;
        tblDoc.StudentPersonalId = StudentId;
        tblDoc.Status = true;
        tblDoc.UserType = "Parent";
        tblDoc.CreatedBy = ParentId;
        tblDoc.CreatedOn = System.DateTime.Now;
        dbobj.Documents.Add(tblDoc);
        dbobj.SaveChanges();
        rtrnval = tblDoc.DocumentId;

        binfile.DocumentName = DocName;
        binfile.ContentType = ContentType;
        binfile.ModuleName = lkName.LookupName;
        binfile.SchoolId = SchoolId;
        binfile.StudentId = StudentId;
        binfile.DocId = rtrnval;
        binfile.Data = data;
        binfile.Varified = true;
        binfile.type = "Parent";
        // binfile.ModuleName = DocName;
        binfile.CreatedBy = ParentId;
        binfile.CreatedOn = DateTime.Now;
        ParentEntity.binaryFiles.Add(binfile);
        ParentEntity.SaveChanges();
        rtrn = binfile.BinaryId;
        return rtrn;






    }



    public IEnumerable<Concents> GetConcents(int StudentId, int SchoolId, int pageIndex, int pageSize, string keyword, string filter)
    {
        ParentEntity = new Melmark_ParentEntities();
        IEnumerable<Concents> ConcentInfoList = new List<Concents>();
        if (filter == null)
            filter = "Consent";
        if (keyword == null) keyword = "";
        ConcentInfoList = (from objDoc in ParentEntity.binaryFiles
                           where (objDoc.StudentId == StudentId && objDoc.SchoolId == SchoolId && objDoc.ModuleName == filter && objDoc.DocumentName.StartsWith(keyword) && objDoc.type != "Referal")
                           select new Concents
                           {
                               ConcentForm = objDoc.DocumentName,
                               ConcentId = objDoc.BinaryId
                           });

        return ConcentInfoList;
    }
    public int GetTotalConcentForms(int StudentId, string keyword, int filter)
    {
        ParentEntity = new Melmark_ParentEntities();
        int count = 0;
        if (filter != 0)
        {

            // Determine the total number of products being paged through (needed to compute PageCount) with filter
            count = this.ParentEntity.Documents.Join(this.ParentEntity.LookUps, v => v.DocumentType, l => l.LookupId, (v, l) => new
            {
                DocumentType = v.DocumentType,
                Lookupid = l.LookupId,
                DocumentName = v.DocumentName,
                StudentId = v.StudentPersonalId,
                LookupName = l.LookupName,
                Status = v.Status
            }).Where(cond => cond.Lookupid == filter && cond.Status == true && cond.LookupName == "Consent" && cond.StudentId == StudentId && cond.DocumentName.StartsWith(keyword)).Count();
        }
        else
        {

            // Determine the total number of products being paged through (needed to compute PageCount) without filter
            count = this.ParentEntity.Documents.Join(this.ParentEntity.LookUps, v => v.DocumentType, l => l.LookupId, (v, l) => new
            {
                DocumentType = v.DocumentType,
                Lookupid = l.LookupId,
                DocumentName = v.DocumentName,
                StudentId = v.StudentPersonalId,
                LookupName = l.LookupName,
                Status = v.Status
            }).Where(cond => cond.StudentId == StudentId && cond.Status == true && cond.LookupName == "Consent" && cond.DocumentName.StartsWith(keyword)).Count();
        }

        return count;
    }
    public int Delete(int id)
    {
        binaryFile doc = new binaryFile();
        ParentEntity = new Melmark_ParentEntities();
        int result = 0;
        doc = ParentEntity.binaryFiles.Where(docobj => docobj.BinaryId == id).SingleOrDefault();
        if (doc != null)
        {
            doc.Varified = false;
            result = ParentEntity.SaveChanges();
            return result;
        }
        else return -1;
    }
    public int SignUpdate(byte[] data, int DocumentId)
    {
        binaryFile doc = new binaryFile();
        ParentEntity = new Melmark_ParentEntities();
        int result = 0;
        doc = ParentEntity.binaryFiles.Where(docobj => docobj.BinaryId == DocumentId).SingleOrDefault();
        if (doc != null)
        {
            doc.Data = data;
            result = ParentEntity.SaveChanges();
            return result;
        }
        else return -1;
    }
    public ProgressDetails CLientsReport(int SchoolID, int StudentID, int IEPID)
    {
        Melmark_ParentEntities RPCobj = new Melmark_ParentEntities();
        List<ProgressDetails> retunmodel = new List<ProgressDetails>();
        ProgressDetails objProgress = new ProgressDetails();


        // DbModel.StdtIEP objIEP = new DbModel.StdtIEP();


        int asmtYr = RPCobj.AsmntYears.Where(objasmtyrs => objasmtyrs.CurrentInd == "A").Select(objasmtyrs => objasmtyrs.AsmntYearId).SingleOrDefault();

        int statusID = RPCobj.LookUps.Where(objlookup => objlookup.LookupType == "IEP Status" && objlookup.LookupName == "Approved").Select(objlookup => objlookup.LookupId).SingleOrDefault();

        int iepID;
        try
        {
            if (IEPID == 0)
            {
                var iepId = (from objIeps in RPCobj.StdtIEPs
                             join objStudents in RPCobj.Students on objIeps.StudentId equals objStudents.StudentId
                             join objAsmtYear in RPCobj.AsmntYears on objIeps.AsmntYearId equals objAsmtYear.AsmntYearId
                             join objLookup in RPCobj.LookUps on objIeps.StatusId equals objLookup.LookupId
                             where (objIeps.StatusId == statusID && objIeps.AsmntYearId == asmtYr && objIeps.StudentId == StudentID && objIeps.SchoolId == SchoolID)
                             select new
                             {
                                 iepIds = objIeps.StdtIEPId,
                             }).ToList().First();

                string id = iepId.iepIds.ToString();
                iepID = Convert.ToInt32(id);
                int IEPId = iepID;
            }
            else
            {
                iepID = IEPID;
            }
            var iepdate = (from objiep in RPCobj.StdtIEPs
                           where (objiep.StdtIEPId == iepID)
                           select new
                           {
                               startdt = objiep.EffStartDate,
                               enddt = objiep.EffEndDate

                           }).ToList().First();

            var tempresult = RPCobj.StdtGoals.Where(objgoal => objgoal.StdtIEPId == iepID && objgoal.StudentId == StudentID).Select(objgoal => objgoal.GoalId).Distinct().Distinct().ToList();

            foreach (var item in tempresult)
            {

                int GoalId = int.Parse(item.ToString());
                var LessonPlanIdVar = RPCobj.StdtLessonPlans.Where(objlsn => objlsn.StudentId == StudentID && objlsn.ActiveInd == "A" && objlsn.GoalId == GoalId && objlsn.StdtIEPId == iepID).Select(objlsn => objlsn.LessonPlanId).Distinct().ToList();
                int GoalLPRelId = 0;




                var GNo = RPCobj.StdtGoals.Where(obgoal => obgoal.GoalId == GoalId && obgoal.StudentId == StudentID && obgoal.StdtIEPId == iepID).Select(objgoal => objgoal.IEPGoalNo).First();
                int GoalNos = 0;
                if (GNo != 0) GoalNos = Convert.ToInt32(GNo);
                var goalname = RPCobj.Goals.Where(obgoal => obgoal.GoalId == GoalId).Select(objgoal => objgoal.GoalName).First();


                foreach (var lesson in LessonPlanIdVar)
                {
                    int LessonPlanId = int.Parse(lesson.ToString());
                    if (LessonPlanId != 0 && GoalId != 0)
                    {
                        GoalLPRelId = RPCobj.GoalLPRels.Where(LGRel => LGRel.GoalId == GoalId && LGRel.LessonPlanId == (int)LessonPlanId).Select(objgoal => objgoal.GoalLPRelId).First();
                    }

                    var lsnplanName = RPCobj.LessonPlans.Where(objlsn => objlsn.LessonPlanId == LessonPlanId).Select(objlsn => objlsn.LessonPlanName).First();
                    GoalData obj = new GoalData
                       {
                           Goalid = GoalId,
                           GoalName = goalname,
                           Lessonplanid = LessonPlanId,
                           LessonplanName = lsnplanName,
                           GoalNo = GoalNos,
                           GoalLPRelId = GoalLPRelId
                       };




                    obj.PlcacementList = (from objgoals in RPCobj.StdtGoals
                                          join objlesson in RPCobj.StdtLessonPlans on objgoals.StudentId equals objlesson.StudentId
                                          where (objlesson.LessonPlanId == LessonPlanId && objlesson.StudentId == StudentID && objlesson.GoalId == GoalId && objlesson.StdtIEPId == iepID)
                                          select new GridListPlacement
                                              {
                                                  objective1 = objlesson.Objective1,
                                                  objective2 = objlesson.Objective2,
                                                  objective3 = objlesson.Objective3,
                                              }).Distinct().ToList();
                    // obj.PlcacementList = singleData;

                    var rptdata = RPCobj.Progress_Report.Where(objrpt => objrpt.GoalId == GoalLPRelId && objrpt.StdtIEPId == iepID).ToList();
                    obj.RptList = (from x in rptdata
                                   select new ReportInfo
                                     {
                                         rptid = x.Report_Id,
                                         rptdate = x.Report_Date,
                                         rptinfo = x.Report_Info,
                                         goalid = x.GoalId,
                                         VisibleToParent=x.AllowVisible

                                     }).ToList();
                    objProgress.GoalDt.Add(obj);

                }
            }
            var student = (from studs in RPCobj.Students
                           join schools in RPCobj.Schools on studs.SchoolId equals schools.SchoolId
                           where (studs.StudentId == StudentID)
                           select new
                           {
                               studid = studs.StudentId,
                               studfname = studs.StudentFname,
                               studlname = studs.StudentLname,
                               studdob = studs.DOB,
                               schooladdid = schools.AddressId,
                               schoolname = schools.SchoolName,
                               schoolcnt = schools.DistContact,
                               schoolph = schools.DistPhone,
                           }).ToList().First();

            objProgress.DOB = Convert.ToDateTime(student.studdob);
            objProgress.StudentName = student.studlname + "," + student.studfname;
            objProgress.ID = Convert.ToInt32(student.studid);
            objProgress.IEPDtFrom = Convert.ToDateTime(iepdate.startdt);
            objProgress.IEPDtTo = Convert.ToDateTime(iepdate.enddt);

            objProgress.stdtIEPId = iepID;

            var schooladdr = (from objaddr in RPCobj.Addresses
                              join schools in RPCobj.Schools on objaddr.AddressId equals schools.AddressId
                              where (objaddr.AddressId == schools.AddressId)
                              select new
                              {
                                  addln1 = objaddr.AddressLine1,
                                  addln2 = objaddr.AddressLine2,
                                  addcity = objaddr.City,
                                  addstate = objaddr.State,
                                  zip = objaddr.Zip,


                              }).ToList().First();

            objProgress.SclDistrictName = student.schoolname;
            objProgress.SclDistrictAddress = schooladdr.addln1 + "," + schooladdr.addln2 + "," + schooladdr.addcity + "," + schooladdr.addstate + "," + schooladdr.zip;
            objProgress.SclDistrictContact = student.schoolcnt + "/" + student.schoolph;
            // retunmodel.Add(objProgress);
            //return objProgress;
        }
        catch (Exception ex)
        {

        }
        return objProgress;
    }


    public List<ReportDetails> GetProgressReport(int StudentId, int SchoolId)
    {
        ParentEntity = new Melmark_ParentEntities();
        List<ReportDetails> ProgressInfo = new List<ReportDetails>();

        ProgressInfo = (from objiep in ParentEntity.StdtIEPs
                        join objLookup in ParentEntity.LookUps on objiep.StatusId equals objLookup.LookupId
                        where (objiep.StudentId == StudentId && objiep.SchoolId == SchoolId && objLookup.LookupType == "IEP Status" &&
                        objLookup.LookupType == "IEP Status" && (objLookup.LookupName == "Expired" || objLookup.LookupName == "Approved"))
                        select new ReportDetails
                        {
                            iepid = objiep.StdtIEPId,

                            startdate = (DateTime)objiep.EffStartDate,
                            enddate = (DateTime)objiep.EffEndDate,
                            Status = objLookup.LookupName
                        }).ToList();



        //foreach (var item in ProgressInfo)
        //{
        //    if (item.iepid != 0)
        //    {
        //        var data = (from par in ParentEntity.Progress_Report
        //                    where (par.StdtIEPId == item.iepid)
        //                    select new ReportDetails
        //                    {
        //                        rptdate= SqlFunctions.DateName("day",par.Report_Date) + "/" + SqlFunctions.DateName("month",par.Report_Date) + "/" +  SqlFunctions.DateName("year",par.Report_Date)
        //                    }).SingleOrDefault();

        //        if (data != null)
        //        {
        //            item.rptdate = data.rptdate;
        //        }
        //        else
        //        {
        //            item.rptdate = "";
        //        }

        //    }

        //}

        return ProgressInfo;
    }


}

