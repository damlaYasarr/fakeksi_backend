using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
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
        [HttpPost("uploadimagesecond")]
        public async Task<ActionResult> Uploadimg([FromForm] FileUpload u)
        {
            bool Result = false;
            try
            {
                var _uploadedfiles = Request.Form.Files; 
                foreach(IFormFile source in _uploadedfiles)
                {
                    string Filename = source.FileName;
                    string FilePath = Getpath(Filename);
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    string imagepath = FilePath + u.image.FileName;
                    if (System.IO.File.Exists(FilePath))
                    {
                        System.IO.File.Delete(imagepath);
                    }

                    using (FileStream filestream = System.IO.File.Create(imagepath + u.image.FileName))
                    {
                        await source.CopyToAsync(filestream);
                        Result = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(Result);

        }
        [NonAction]
        public string Getpath(string filename)
        {
            return _webHostEnvironment.WebRootPath + "\\uploads\\"+filename;
        }
     
    }
}
