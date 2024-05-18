using Microsoft.AspNetCore.Mvc;

namespace Carpediem.Controllers.Utils
{
    public class ControllerResponse
    {
        public string Message { get; set; }
        public object[] Data { get; set; }
    }
}