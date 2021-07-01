using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PerpetualGuardian.Data;
using PerpetualGuardian.Dto;
using PerpetualGuardian.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PerpetualGuardian.Services.Implement
{
    public class FileInformationService : IFileInformationService
    {
        private readonly MyDbContext myDbContext;

        private readonly string uploadDir;

        private readonly IConfiguration configuration;

        public FileInformationService(MyDbContext myDbContext, IConfiguration configuration)
        {
            this.myDbContext = myDbContext;
            this.configuration = configuration;
            this.uploadDir = Path.Combine(this.configuration["UploadDir"]);
        }

        public async Task<FileInformationDto> AddFileInformationAsync(FileInformationDto fileInformationDto)
        {
            if (fileInformationDto != null)
            {
                if (!Directory.Exists(uploadDir))   //create upload folder
                {
                    Directory.CreateDirectory(uploadDir);
                }

                FileInformation fileInformation = new FileInformation()
                {
                    FileName = fileInformationDto.FileName,
                    UploadDataTime = DateTime.Now
                };
                this.myDbContext.Add(fileInformation);   //save file information to database

                this.myDbContext.SaveChanges();

                if (fileInformationDto.FileData!=null&& fileInformationDto.FileData.Length > 0)  //store file data
                {
                    string filePath = fileInformation.GetUploadFilePath(this.uploadDir);
                    using (FileStream fs = File.Create(filePath))
                    {
                        await fs.WriteAsync(fileInformationDto.FileData, 0, fileInformationDto.FileData.Length);
                    }
                }

                return fileInformation.ToFileInformationDto();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> AddFileInformationListAsync(List<FileInformationDto> fileInformationDtoes)
        {
            bool bReturn = false;
            if (fileInformationDtoes != null)
            {
                foreach (var fileInformation in fileInformationDtoes)
                {
                    await AddFileInformationAsync(fileInformation);
                }
                bReturn = true;
            }
            return bReturn;
        }

        public async Task<List<FileInformationDto>> GetAllFilesAsync()
        {
            return await this.myDbContext.FileInformationSet
                                            .OrderBy(x => x.Id)
                                            .Reverse()
                                            .Select(x => x.ToFileInformationDto())
                                            .ToListAsync();
        }

        public async Task<FileInformationDto> GetFileAsync(int id)
        {
            FileInformationDto fileInformationDto = null;

            FileInformation fileInformation = await this.myDbContext.FileInformationSet.FindAsync(id);

            if (fileInformation != null)
            {
                fileInformationDto = fileInformation.ToFileInformationDto();  //get file information from database

                string filePath = fileInformation.GetUploadFilePath(this.uploadDir);

                if (File.Exists(filePath))   //get file data
                {
                    fileInformationDto.FileData = await File.ReadAllBytesAsync(filePath);
                }
            }

            return fileInformationDto;
        }
    }
}
