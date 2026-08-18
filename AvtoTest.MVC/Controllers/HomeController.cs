using AvtoTest.Data.Entities;
using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.MVC.Models;
using AvtoTest.Service.Services.Interfeces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AvtoTest.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService homeService;
        private readonly UserManager<CustomUser> _userManager;
        public HomeController(IHomeService homeService, UserManager<CustomUser> userManager)
        {
            this.homeService = homeService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult CheckingView()
        {
            return View();
        }
        public async Task<IActionResult> EntryPage()
        {
            if (!(await CheckLogin()))
            {
                var visitor = new AnonymousUser();
                var getJson = Request.Cookies["AnonymousUser"];

                if (getJson != null)
                    visitor = JsonConvert.DeserializeObject<AnonymousUser>(getJson);

                var connection = HttpContext.Connection;
                var ipAddress = connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();


                if (visitor is null)
                {
                    visitor = new AnonymousUser()
                    {
                        Id = Guid.NewGuid(),
                        TestCount = visitor.TestCount, //faqat test yechilganida o'zgaradi
                        IpAddress = ipAddress,
                        UserAgent = userAgent,
                        BrowseType = ParseBrwserType(userAgent),
                        DeviceType = ParseDeviceType(userAgent),
                        OperatingSystem = ParseOperatingSystem(userAgent),
                        CreateAt = DateTime.UtcNow, // bu ham nomalum
                        LastVisited = DateTime.UtcNow, // 
                        LastTestAt = null
                    };
                }
                else
                {
                    visitor.LastVisited = DateTime.UtcNow;
                }
                var json = JsonConvert.SerializeObject(visitor);
                Response.Cookies.Append("AnonymousUser", json);
            }
            return View();
        }
        private string ParseBrwserType(string? userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Unknown";
            if (userAgent.Contains("Edg/")) return "Edge";
            if (userAgent.Contains("OPR/") || userAgent.Contains("Opera/")) return "Opera";
            if (userAgent.Contains("Chrome/") && !userAgent.Contains("Edg/")) return "Chrome";
            if (userAgent.Contains("Safari/")) return "Safari";
            if (userAgent.Contains("Firefox/")) return "Firefox";
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident/")) return "Internet Explore";
            return "Other";
        }
        private string ParseDeviceType(string? userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";

            var ua = userAgent.ToLower();

            if (ua.Contains("mobile") ||
                ua.Contains("android") && !ua.Contains("tablet") ||
                ua.Contains("iphone") ||
                ua.Contains("ipod"))
            {
                return "Mobile";
            }

            if (ua.Contains("tablet") || ua.Contains("ipad")) return "Tablet";

            if (ua.Contains("windows") ||
                ua.Contains("mac") ||
                ua.Contains("linux") ||
                ua.Contains("ubuntu") ||
                ua.Contains("cros") ||  // Chrome OS
                ua.Contains("x11"))     // Unix/Linux
            {
                return "Desktop";
            }

            return "Other";
        }
        private string ParseOperatingSystem(string? userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";

            var ua = userAgent.ToLower();

            if (ua.Contains("windows nt 10.0")) return "Windows 10";
            if (ua.Contains("windows nt 6.3")) return "Windows 8.1";
            if (ua.Contains("windows nt 6.2")) return "Windows 8";
            if (ua.Contains("windows nt 6.1")) return "Windows 7";
            if (ua.Contains("windows nt 6.0")) return "Windows Vista";
            if (ua.Contains("windows nt 5.1")) return "Windows XP";
            if (ua.Contains("windows nt")) return "Windows";
            if (ua.Contains("windows phone")) return "Windows Phone";

            if (ua.Contains("mac os x 10_15")) return "macOS Catalina";
            if (ua.Contains("mac os x 10_14")) return "macOS Mojave";
            if (ua.Contains("mac os x 10_13")) return "macOS High Sierra";
            if (ua.Contains("mac os x 10_12")) return "macOS Sierra";
            if (ua.Contains("mac os x 10_11")) return "OS X El Capitan";
            if (ua.Contains("mac os x")) return "macOS";
            if (ua.Contains("macintosh")) return "Macintosh";

            if (ua.Contains("iphone") || ua.Contains("ipad"))
            {
                if (ua.Contains("os 17")) return "iOS 17";
                if (ua.Contains("os 16")) return "iOS 16";
                if (ua.Contains("os 15")) return "iOS 15";
                if (ua.Contains("os 14")) return "iOS 14";
                if (ua.Contains("os 13")) return "iOS 13";
                return "iOS";
            }

            if (ua.Contains("android 14")) return "Android 14";
            if (ua.Contains("android 13")) return "Android 13";
            if (ua.Contains("android 12")) return "Android 12";
            if (ua.Contains("android 11")) return "Android 11";
            if (ua.Contains("android 10")) return "Android 10";
            if (ua.Contains("android")) return "Android";

            if (ua.Contains("ubuntu")) return "Ubuntu";
            if (ua.Contains("debian")) return "Debian";
            if (ua.Contains("fedora")) return "Fedora";
            if (ua.Contains("linux")) return "Linux";

            if (ua.Contains("cros")) return "Chrome OS";

            return "Other";
        }
        private async Task<CustomUser> GetUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return user!;
        }
        private async Task<bool> CheckLogin()
        {
            if (await GetUser() is null) return false;
            else return true;
        }
    }
}
