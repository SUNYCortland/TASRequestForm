using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Data.Repositories.Nh.Transformers
{
    public class GetCoursesTransformer : IResultTransformer
    {
        public static readonly string Sql = 
            @"SELECT
                SSBSECT_CRN crn,
                SSBSECT_SUBJ_CODE sub,
                SSBSECT_CRSE_NUMB num,
                SSBSECT_SEQ_NUMB sec,
                NVL(SSBSECT_CRSE_TITLE,SCBCRSE_TITLE) title,
                SPRIDEN_FIRST_NAME,
                SPRIDEN_LAST_NAME,
                SPRIDEN_PIDM
            FROM
                SCBCRSE,
                SSBSECT,
                SFRSTCR,
                SPRIDEN,
                SIRASGN
            WHERE SCBCRSE_SUBJ_CODE = SSBSECT_SUBJ_CODE
            AND SCBCRSE_CRSE_NUMB = SSBSECT_CRSE_NUMB
            AND SCBCRSE_EFF_TERM = (
                SELECT MAX(SCBCRSE_EFF_TERM)
                FROM SCBCRSE
                WHERE
                    SCBCRSE_SUBJ_CODE = SSBSECT_SUBJ_CODE
                AND SCBCRSE_CRSE_NUMB = SSBSECT_CRSE_NUMB
                AND SCBCRSE_EFF_TERM <= SSBSECT_TERM_CODE
            )
            AND SSBSECT_TERM_CODE = SFRSTCR_TERM_CODE
            AND SSBSECT_CRN = SFRSTCR_CRN
            AND SIRASGN_CRN = SFRSTCR_CRN
            AND SPRIDEN_PIDM = SIRASGN_PIDM
            AND SPRIDEN_CHANGE_IND IS NULL
            AND SIRASGN_TERM_CODE = SFRSTCR_TERM_CODE
            AND SIRASGN_PRIMARY_IND = 'Y'
            AND SFRSTCR_PIDM = :Pidm
            AND SFRSTCR_TERM_CODE = (SELECT cortfunct.f_get_current_term() as currentTerm FROM dual)";

        public static readonly GetCoursesTransformer Transformer = new GetCoursesTransformer();

        private GetCoursesTransformer() { }

        public IList TransformList(IList collection)
        {
            return collection;
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            int? iProfessorPidm = null;
            string strCRN = String.Empty;
            string strSubject = String.Empty;
            string strNumber = String.Empty;
            string strSection = String.Empty;
            string strTitle = String.Empty;
            string strProfessorFirstName = String.Empty;
            string strProfessorLastName = String.Empty;

            if (tuple[0] != null)
                strCRN = tuple[0].ToString();

            if (tuple[1] != null)
                strSubject = tuple[1].ToString();

            if (tuple[2] != null)
                strNumber = tuple[2].ToString();

            if (tuple[3] != null)
                strSection = tuple[3].ToString();

            if (tuple[4] != null)
                strTitle = tuple[4].ToString();

            if (tuple[5] != null)
                strProfessorFirstName = tuple[5].ToString();

            if (tuple[6] != null)
                strProfessorLastName = tuple[6].ToString();

            if (tuple[7] != null)
                iProfessorPidm = int.Parse(tuple[7].ToString());

            return new Course
            {
                CRN = strCRN,
                Subject = strSubject,
                Number = strNumber,
                Section = strSection,
                Title = strTitle,
                ProfessorFirstName = strProfessorFirstName,
                ProfessorLastName = strProfessorLastName,
                ProfessorPidm = iProfessorPidm.Value
            };
        }
    }
}
