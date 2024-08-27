using Microsoft.AspNetCore.Mvc;

namespace _20240827_MVCEmptyApp01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Help()
        {
            return View();
        }
    }
}

/////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers
{
    public class HelpController : ControllerBase
    {
        // /Help 또는 /Help/{id} 경로로 요청을 처리한다.
        [Route("/Help/{id?}")]
        public int Help(int? id)
        {
            if (id.HasValue)
            {
                // id가 전달된 경우, 해당 ID를 반환한다.
                return id.Value;
            }
            else
            {
                // id가 전달되지 않은 경우, 기본값으로 0을 반환한다.
                return 0;
            }
        }
    }
}
