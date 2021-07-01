using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerpetualGuardian.Dto;
using PerpetualGuardian.Services;
using PerpetualGuardian.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PerpetualGuardian.Controllers
{
    [Route("FileInformation")]
    [ApiController]
    public class FileInformationController : ControllerBase
    {
        private readonly IFileInformationService fileInformationService;

        public FileInformationController(IFileInformationService fileInformationService)
        {
            this.fileInformationService = fileInformationService;
        }

        /// <summary>
        /// Upload file end point
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                /**
                 Generate fileInformationDto list
                 */
                IEnumerable<FileInformationDto> fileInformationDtoes = files.Select(x =>
                    {
                        byte[] inputData = new byte[x.Length];
                        x.OpenReadStream().Read(inputData);
                        return new FileInformationDto()
                        {
                            FileName = x.FileName,
                            FileData = inputData
                        };
                    }
                );

                await this.fileInformationService.AddFileInformationListAsync(fileInformationDtoes.ToList());

                return Ok();
            }
            return BadRequest(new { message = "Please upload files." });
        }

        /// <summary>
        /// Download file end point
        /// </summary>
        /// <param name="id">file id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Download(int id)
        {
            FileInformationDto fileInformationDto = await this.fileInformationService.GetFileAsync(id);

            if (fileInformationDto == null)
                return NotFound();

            string extensionName = Path.GetExtension(fileInformationDto.FileName);

            var contentype = FileContentType.GetMimeType(extensionName);

            return File(fileInformationDto.FileData, contentype, fileInformationDto.FileName);
        }

        /// <summary>
        /// Get all file end point
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<List<FileInformationDto>> AllFiles()
        {
            List<FileInformationDto> list = await this.fileInformationService.GetAllFilesAsync();
            return list;
        }
    }
}