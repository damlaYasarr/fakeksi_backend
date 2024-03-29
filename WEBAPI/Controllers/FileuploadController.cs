﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using WEBAPI.Data;
using WEBAPI.Models;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileuploadController : Controller
    {
        //private readonly DataContext _context;
        public static IWebHostEnvironment? _webHostEnvironment;
        public FileuploadController(IWebHostEnvironment webHostEnvironment)
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
        [HttpPost("imagedelete")]
        public string DeleteImage(string imageName)
        {
            try
            {
                string path = _webHostEnvironment.WebRootPath + "\\uploads\\" + imageName;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return "Image deleted successfully.";
                }
                else
                {
                    return "Image not found.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
}
