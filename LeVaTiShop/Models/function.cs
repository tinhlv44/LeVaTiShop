using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace LeVaTiShop.Models
{
    public class function
    {
        dtDataContext dt = new dtDataContext();
        public void analysisImage(string s, out List<string> sColor, out List<string> sImage)
        {
            sColor = new List<string>();
            foreach (string i in SplitString(s))
            {
                sColor.Add("#" + i);
            }
            sImage = new List<string>();
            foreach (string i in SplitString(s))
            {
                sImage.Add(i + ".jpg");
            }
        }
        public void amountProduct(string s, out List<string> amount)
        {
            amount = new List<string>();
            foreach (string i in SplitString(s))
            {
                amount.Add(i);
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
        public List<string> SplitString(string s)
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
        public Product getProduct(int id)
        {
            var res = dt.Products.SingleOrDefault(p => p.idProduct == id);
            return res;
        }
        public void SendMail(string email, string Subject, string Body)
        {
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("2124801030225@student.tdmu.edu.vn", "v@ntinhGG45"),
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
        public string getState(int i, out string state, out string hexColor)
        {
            string[] listState = { "Chờ duyệt", "Đang xử lí", "Đang giao hàng", "Hoàn thành", "Hủy" };
            string[] listHexColor = { "#333333", "#FFD700", "#006400", "#000080", "#FF0000" };
            state = listState[i];
            hexColor = listHexColor[i];
            return state;
        }
        public string takeMessage(string message)
        {
            return message.IndexOf('_')!=-1?message.Substring(message.IndexOf('_')+1):message;
        }

        public string GenerateRandomPassword()
        {
            const int requiredUniqueChars = 4;

            string password = Membership.GeneratePassword(10, requiredUniqueChars);
            return password;
        }
        

        private const string Salt = "h%t";
        public static string HashPassword(string password)
        {
            string saltedPassword = password + Salt;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(saltedPassword);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string saltedPassword = password + Salt;
            string hashedInput = HashPassword(saltedPassword);
            return hashedInput == hashedPassword;
        }
    }
}