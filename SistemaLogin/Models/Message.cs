using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace SistemaLogin.Models
{
    public class Message
    {
        public List<MailboxAddress> Recipients { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> recipients,
            string subject,
            int userId,
            string code)
        {
            Recipients = new List<MailboxAddress>();
            Recipients.AddRange(recipients.Select(d => new MailboxAddress(d)));
            Subject = Subject;
            Content = $"http://localhost:5000/Register/Active?UserId={userId}&CodeActive={code}";
        }
    }
}
