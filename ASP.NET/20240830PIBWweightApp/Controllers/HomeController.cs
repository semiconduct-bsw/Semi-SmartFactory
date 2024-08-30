using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(double height, double weight, string gender)
        {
            // 키를 cm에서 m로 변환
            double heightInMeters = height / 100.0;

            // 성별에 따른 표준 체중 계산
            double standardWeight;
            if (gender == "남성")
            {
                standardWeight = heightInMeters * heightInMeters * 22;
            }
            else // 여성
            {
                standardWeight = heightInMeters * heightInMeters * 21;
            }

            // 표준 체중 대비 백분율(PIBW) 계산
            double pibw = (weight / standardWeight) * 100;
            string result;

            // PIBW 값에 따른 체중 구분
            if (pibw < 90)
            {
                result = "저체중";
            }
            else if (pibw >= 90 && pibw < 110)
            {
                result = "정상 체중";
            }
            else if (pibw >= 110 && pibw < 120)
            {
                result = "과체중";
            }
            else if (pibw >= 120 && pibw < 130)
            {
                result = "경도 비만";
            }
            else if (pibw >= 130 && pibw < 160)
            {
                result = "중정도 비만";
            }
            else
            {
                result = "고도 비만";
            }

            ViewBag.Result = result;
            return View();
        }
    }
}
