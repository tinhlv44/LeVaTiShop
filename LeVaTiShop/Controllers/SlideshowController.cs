using LeVaTiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace LeVaTiShop.Controllers
{
    public class SlideshowController : Controller
    {
        dtDataContext dt = new dtDataContext();
        // Action để lấy dữ liệu slide từ trang web https://www.thegioididong.com/
        public ActionResult Index()
        {
           /* // Sử dụng thư viện HttpClient để tải nội dung trang web
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync("https://www.thegioididong.com/").Result;

            // Sử dụng thư viện HtmlAgilityPack để phân tích cú pháp HTML
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // Tìm các phần tử HTML chứa thông tin slide
            var slideElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'item')]");

            // Duyệt qua các phần tử slide và lấy thông tin
            var slides = new List<Slide>();
            foreach (var slideElement in slideElements)
            {
                var imageUrl = slideElement.SelectSingleNode(".//img")?.GetAttributeValue("src", "");
                var title = slideElement.SelectSingleNode(".//h3")?.InnerText.Trim();
                var description = slideElement.SelectSingleNode(".//p")?.InnerText.Trim();

                var slide = new Slide
                {
                    ImageUrl = imageUrl,
                    Title = title,
                    Description = description
                };
                slides.Add(slide);
            }
*/
            return View();
        }
    }
}