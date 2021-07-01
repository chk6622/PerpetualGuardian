using PerpetualGuardian.Dto;
using PerpetualGuardian.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace PerpetualGuardianTest
{
    public class FileInformationServiceTest : BaseTest
    {
        private IFileInformationService fileInformationService;
        public FileInformationServiceTest(ProgramInitFixture programInitFixture) : base(programInitFixture)
        {
            this.fileInformationService = Provider.GetService(typeof(IFileInformationService)) as IFileInformationService;
        }

        [Theory]
        [InlineData("File1.txt")]
        [InlineData("File2.doc")]
        [InlineData("File3.jpg")]
        public async void ShouldStoreFileInformationIntoDatabase(string fileName)
        {
            FileInformationDto fileInformationDto = await this.fileInformationService.AddFileInformationAsync(new FileInformationDto(){
                FileName = fileName,
            });
            Assert.Equal(fileName, fileInformationDto.FileName);
            Assert.NotEqual(0, fileInformationDto.Id);
            Assert.NotNull(fileInformationDto.UploadDateTime);
        }

        [Fact]
        public async void ShouldStoreFileInformationListIntoDatabase()
        {
            List<FileInformationDto> list = new List<FileInformationDto>()
            {
                new FileInformationDto()
                {
                    FileName = "File4.txt"
                },
                new FileInformationDto()
                {
                    FileName = "File5.txt"
                },
                new FileInformationDto()
                {
                    FileName = "File6.txt"
                },
            };
            bool result = await this.fileInformationService.AddFileInformationListAsync(list);
            Assert.True(result);
        }

        [Fact]
        public async void ShouldReturnNullIfFileInformationIsNull()
        {
            FileInformationDto fileInformationDto = await this.fileInformationService.AddFileInformationAsync(null);
            Assert.Null(fileInformationDto);
        }

        [Theory]
        [InlineData("File7.txt")]
        [InlineData("File8.doc")]
        [InlineData("File9.jpg")]
        public async void ShouldGetFileInformationFromDatabase(string fileName)
        { 
            FileInformationDto fileInformationDto1 = await this.fileInformationService.AddFileInformationAsync(new FileInformationDto()
            {
                FileName = fileName,
            });

            FileInformationDto fileInformationDto2 = await this.fileInformationService.GetFileAsync(fileInformationDto1.Id);

            Assert.Equal(fileInformationDto1.Id, fileInformationDto2.Id);
            Assert.Equal(fileInformationDto1.FileName, fileInformationDto2.FileName);
            Assert.Equal(fileInformationDto1.UploadDateTime, fileInformationDto2.UploadDateTime);
        }

        [Fact]
        public async void ShouldGetAllFileInformationFromDatabase()
        {
            await this.fileInformationService.AddFileInformationAsync(new FileInformationDto()
            {
                FileName = "File10.txt",
            });
            await this.fileInformationService.AddFileInformationAsync(new FileInformationDto()
            {
                FileName = "File11.doc",
            });
            await this.fileInformationService.AddFileInformationAsync(new FileInformationDto()
            {
                FileName = "File12.jpg",
            });

            List<FileInformationDto> results = await this.fileInformationService.GetAllFilesAsync();

            Assert.True(results.Where(x => x.FileName == "File10.txt").Any());
            Assert.True(results.Where(x => x.FileName == "File11.doc").Any());
            Assert.True(results.Where(x => x.FileName == "File12.jpg").Any());
        }

        [Fact]
        public async void ShouldUploadFile()
        {
            string filePath = Path.Combine("testFile","IMGP0055.JPG");
            if (File.Exists(filePath))
            {
                byte[] inputFileData = File.ReadAllBytes(filePath);
                FileInformationDto fileInformationDto = await this.fileInformationService.AddFileInformationAsync(new FileInformationDto()
                {
                    FileName = "IMGP0055.JPG",
                    FileData = inputFileData
                });

                FileInformationDto fileInformationDto1 = await this.fileInformationService.GetFileAsync(fileInformationDto.Id);
                Assert.NotNull(fileInformationDto1.FileData);
                Assert.Equal(inputFileData, fileInformationDto1.FileData);
            }
        }

    }
}
