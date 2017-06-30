using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Services;

namespace TASRequestForm.Core.Extensions
{
    public static class EmailTemplateExtensions
    {
        public static string ToFriendlyString(this EmailTemplate me)
        {
            switch (me)
            {
                case EmailTemplate.StudentReminder:
                    return "StudentReminder";

                case EmailTemplate.InstructorReceiptReminder:
                    return "InstructorReceiptReminder";

                case EmailTemplate.InstructorReminder:
                    return "InstructorReminder";

                case EmailTemplate.InstructorApproval:
                    return "InstructorApproval";

                case EmailTemplate.InstructorDenied:
                    return "InstructorDenied";

                case EmailTemplate.InstructorReceipt:
                    return "InstructorReceipt";

                case EmailTemplate.StudentReceipt:
                    return "StudentReceipt";

                case EmailTemplate.TASApproval:
                    return "TASApproval";

                case EmailTemplate.TASDenied:
                    return "TASDenied";

                case EmailTemplate.InstructorBypass:
                    return "InstructorBypass";

                default:
                    throw new ArgumentException("Invalid EmailTemplate.");
            }
        }
    }
}
