using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FinalProjectDotNet.Pages
{
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {

        }
        public void OnPost(string email, string psw, string rpsw)
        {
            if (!IsAllFieldsAreFull(email, psw, rpsw))
            {
                Response.WriteAsync(@"<p style=""color: red; font-size:large"">Please fill all the fields</p>");
                GoBackParagraph();
            }
            else if (!IsValidEmail(email))
            {
                Response.WriteAsync(@"<p style=""color: red; font-size:large"">Wrong mail format</p>");
                GoBackParagraph();
            }
            else if (!IsSamePassword(psw, rpsw))
            {
                Response.WriteAsync(@"<p style=""color: red; font-size:large"">Passwords are not identical</p>");
                GoBackParagraph();
            }
            else if (!IsValidPassword(psw))
            {
                Response.WriteAsync(@"<p style=""color: red; font-size:large"">Password must have at least - 1 upper casel letter ,1 lower case letter, 1 digit, and at least 6 char </p>");
                GoBackParagraph();
            }
            else
            {
                Response.WriteAsync(@"<p style=""color: green; font-size:large"">Registration completed successfully</p>");
                GoBackParagraph();
            }

        }

        public void GoBackParagraph()
        {
            Response.WriteAsync(@"<p style=""color: green; font-size:large"">Please go back and try again</p>");

        }

        private bool IsValidPassword(string psw)
        {
            if (psw.Length < 6)
            {
                return false;
            }
            else if (psw.Any(char.IsUpper))
            {
                return false;
            }
            else if (psw.Any(char.IsLower))
            {
                return false;
            }
            else if (psw.Any(char.IsDigit))
            {
                return false;
            }

            return true;
        }

        private bool IsSamePassword(string psw, string rpsw)
        {
            return psw.Equals(rpsw);
        }

        private bool IsAllFieldsAreFull(string email, string psw, string rpsw)
        {
            return email.Length > 0 && psw.Length > 0 && rpsw.Length > 0;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}