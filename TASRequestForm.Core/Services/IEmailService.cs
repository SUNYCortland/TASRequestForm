using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Services
{
    public interface IEmailService
    {
        void SendEmail<T>(EmailTemplate template, T model, string emailAddress, string subject);
    }

    public enum EmailTemplate
    {
        InstructorReceiptReminder,
        InstructorReminder,
        InstructorApproval,
        InstructorDenied,
        InstructorReceipt,
        StudentReceipt,
        StudentReminder,
        TASApproval,
        TASDenied,
        InstructorBypass
    }
}
