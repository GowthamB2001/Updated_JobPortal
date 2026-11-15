using JobPortal.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Infrastructure
{
   
        public interface IEmailService
        {
            Task SendEmailAsync(
                EmailMessage message);
        }
    }

