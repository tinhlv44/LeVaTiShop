using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace LeVaTiShop.Models
{
    public class function
    {
        dtDataContext dt = new dtDataContext();
        public void analysisImage(string s, out List<string> sColor, out List<string> sImage)
        {
            sColor = new List<string>();
            foreach (string i in getColor(s))
            {
                sColor.Add("#" + i);
            }
            sImage = new List<string>();
            foreach (string i in getColor(s))
            {
                sImage.Add(i + ".jpg");
            }
        }
        public void analysisSpecifications(string input, out List<string> lfield, out List<string> lvalue)
        {
            lfield = new List<string>();
            lvalue = new List<string>();

            // Regex pattern to match the fields and values
            string pattern = @"@\((.*?)\)\{(.*?)\}";
            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string fieldName = match.Groups[1].Value; // Tên trường
                    string value = match.Groups[2].Value; // Giá trị

                    lfield.Add(fieldName);
                    lvalue.Add(value);
                }
            }
        }
        public List<string> getColor(string s)
        {
            List<string> result = new List<string>();
            string[] values = s.Split('#');
            foreach (string value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value);
                }
            }
            return result;
        }
        public int getId(string s)
        {
            var res = dt.Products.SingleOrDefault(p => p.nameProduct == s);
            return res.idProduct;
        }
        public void SendMail(string email, string Subject, string Body)
        {
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("2124801030225@student.tdmu.edu.vn", "v@ntinhGG44"),
                EnableSsl = true
            };

            var message = new MailMessage();
            message.From = new MailAddress("2124801030225@student.tdmu.edu.vn");
            message.ReplyToList.Add("2124801030225@student.tdmu.edu.vn");
            message.To.Add(new MailAddress(email));
            message.Subject = Subject;
            message.Body = Body;
            mail.Send(message);
        }
    }
}