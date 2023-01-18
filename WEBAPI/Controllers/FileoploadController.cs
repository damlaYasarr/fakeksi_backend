using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Data;
using WEBAPI.Models;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileoploadController : Controller
    {
        private readonly DataContext _context;
        public static IWebHostEnvironment _webHostEnvironment;
        public FileoploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;   
        }
        public FileoploadController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("imageupload")]
        public string Post([FromForm]FileUpload u)
        {
            try
            {
                if (u.image.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);

                    }
                    using (FileStream filestream = System.IO.File.Create(path + u.image.FileName))
                    {
                       
                        u.image.CopyTo(filestream);
                        //_context.Files.Add(u.image);
                        filestream.Flush();
                        return "Uploaded";
                    }
                }
                else
                {
                    return "Not upload";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
                
            }

        }
        [HttpGet("getphoto")]
        public string Getpath()
        {

        }
    }
}
