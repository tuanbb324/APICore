using Microsoft.AspNetCore.Mvc;
using Serilog;
using ACBSChatbotConnector.Models.Response;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Services;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;



namespace ACBSChatbotConnector.Controllers
{
    [Route("api")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileUploadService _fileService;
        public FileController(IFileUploadService fileService)
        {
            _fileService = fileService;

        }
        [Authorize]
        [HttpPost]
        [Route("files/uploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                if (httpRequest.ContentType == null)
                {
                    var _resError = httpRequest.ContentType.ErrorRespond<string>(611, "File not found.");
                    return StatusCode(611, _resError);
                }
                List<FileUpload> lst = new List<FileUpload>();
                string fileNotAccept = string.Empty;
                string _fileNameNotAccept = string.Empty;
                foreach (IFormFile postedFile in httpRequest.Form.Files)
                {
                    FileUpload _file = _fileService.UpFile(postedFile, ref _fileNameNotAccept);
                    if (_file != null)
                    {
                        lst.Add(_file);
                    }
                    else
                    {
                        fileNotAccept += _fileNameNotAccept + ", ";
                        _fileNameNotAccept = string.Empty;
                        var _resError = fileNotAccept.ErrorRespond<string>(615, "File Not Accept");
                        return StatusCode(615, _resError);
                    }
                }
                var _successResponse = lst.SuccessRespond<List<FileUpload>>();
                return Ok(_successResponse);
            }
            catch (Exception ex)
            {
                Log.Error($"ViewFileAsync {ex}");
                var _errorResponse = default(FileUpload).InternalServerError<FileUpload>();
                return StatusCode(500, _errorResponse);
            }
        }
        /*[Authorize]
        [HttpGet]
        [Route("files/crop/{Date}/{FileName}")]
        public IActionResult CropImage([FromRoute] string Date, [FromRoute] string FileName, [FromQuery] int w, int h, string p = "CENTER")
        {
            try
            {
                string staticFolder = Config.Config.RootFolder + @"static\" + Date + @"\";
                string _pathImage = string.Empty;               
                FileStreamResult _res;
                string _checkNull = null;

                if (w <= 0 || h <= 0)
                {
                    var _resError = _checkNull.ErrorRespond<string>(400, "Wight and Hight must >=0.");
                    return StatusCode(400, _resError);
                }

                _res = CropFile(staticFolder, FileName, w, h, p);
                if (_res == null)
                {
                    var _resError = _checkNull.ErrorRespond<string>(400, "CropFile Error.");
                    return StatusCode(400, _resError);
                }
                var _successResponse = "Sucessfull".SuccessRespond<string>();
                return Ok(_successResponse);
            }
            catch (Exception ex)
            {
                Log.Error($"CropImage {ex}");
                var _errorResponse = default(string).InternalServerError<string>();
                return StatusCode(500, _errorResponse);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("files/resize/{Date}/{FileName}")]
        public IActionResult ResizeImage([FromRoute] string Date, [FromRoute] string FileName, [FromQuery] int w, int h, string p = "CENTER")
        {
            try
            {
                string staticFolder = Config.Config.RootFolder + @"static\" + Date + @"\";
                string _pathImage = string.Empty;
                FileStreamResult _res;
                string _checkNull = null;

                if (w <= 0 || h <= 0)
                {
                    var _resError = _checkNull.ErrorRespond<string>(400, "Wight and Hight must >=0.");
                    return StatusCode(400, _resError);
                }

                _res = ResizeFile(staticFolder, FileName, w, p);
                if (_res == null)
                {
                    var _resError = _checkNull.ErrorRespond<string>(400, "CropFile Error.");
                    return StatusCode(400, _resError);
                }

                var _successResponse = "Sucessfull".SuccessRespond<string>();
                return Ok(_successResponse);
            }
            catch (Exception ex)
            {
                Log.Error($"ResizeImage {ex}");
                var _errorResponse = default(string).InternalServerError<string>();
                return StatusCode(500, _errorResponse);
            }
        }*/
        [HttpGet]
        [Route("files/view/{Date}/{FileName}")]
        public IActionResult LoadImage([FromRoute] string Date, [FromRoute] string FileName)
        {
            try
            {
                string _checkNull = null;
                string staticFolder = Config.Config.RootFolder + @"static\" + Date + @"\";
                string path = staticFolder + FileName;
                if (!Directory.Exists(path))
                {
                    var _resError = _checkNull.ErrorRespond<string>(611, "File not found.");
                    return StatusCode(611, _resError);
                }

                FileStream stream = System.IO.File.Open(path, FileMode.Open);

                return File(stream, "image/png");
            }
            catch (Exception ex)
            {
                Log.Error($"LoadImage {ex}");
                var _errorResponse = default(FileUpload).InternalServerError<FileUpload>();
                return StatusCode(500, _errorResponse);
            }
        }
        /*        private FileStreamResult CropFile(string staticFolder, string fileName, int w, int h, string pos)
                {
                    try
                    {
                        string path = staticFolder + fileName;
                        string fName = Path.GetFileNameWithoutExtension(path);
                        string _pathImage = Path.Combine(staticFolder, "crop_" + fName + w + "x" + h + "px.png");

                        if (System.IO.File.Exists(_pathImage))
                        {
                            FileStream crop = System.IO.File.Open(_pathImage, FileMode.Open);
                            return File(crop, "image/png");
                        }

                        using (FileStream stream = System.IO.File.Open(path, FileMode.Open))
                        {
                            using (Image image = Image.Load(stream))
                            {
                                //image.Clone(ctx => ctx.Crop(w, h)).Save(_path);
                                var options = new ResizeOptions
                                {
                                    Size = new Size(w, h),
                                    Mode = ResizeMode.Crop,
                                    Position = AnchorPositionMode.Center
                                };

                                if (pos.ToUpper() != "CENTER")
                                {
                                    options.Position = AnchorPositionMode.Top;
                                }

                                image.Clone(x => x.Resize(options)).Save(_pathImage);
                            }
                        }

                        FileStream _crop = System.IO.File.Open(_pathImage, FileMode.Open);

                        return File(_crop, "image/png");
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"CropSizeImage {ex}");
                        return null;
                    }
                }
                private FileStreamResult ResizeFile(string staticFolder, string fileName, int w, string mode)
                {
                    try
                    {
                        string path = staticFolder + fileName;
                        string fName = Path.GetFileNameWithoutExtension(path);
                        string _pathImage = Path.Combine(staticFolder, "resize_" + fName + "w" + w + "px.png");

                        if (System.IO.File.Exists(_pathImage))
                        {
                            FileStream crop = System.IO.File.Open(_pathImage, FileMode.Open);
                            return File(crop, "image/png");
                        }

                        using (FileStream stream = System.IO.File.Open(path, FileMode.Open))
                        {
                            using (Image image = Image.Load(stream))
                            {
                                var options = new ResizeOptions
                                {
                                    Size = new Size() { Width = w },
                                    Mode = ResizeMode.Max,
                                };

                                if (mode.ToUpper() == "BOX")
                                {
                                    options.Mode = ResizeMode.BoxPad;
                                }

                                image.Clone(x => x.Resize(options)).Save(_pathImage);
                            }
                        }
                        FileStream _resize = System.IO.File.Open(_pathImage, FileMode.Open);
                        return File(_resize, "image/png");
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"ResizeImage {ex}");
                        return null;
                    }
                }*/
    }
}
