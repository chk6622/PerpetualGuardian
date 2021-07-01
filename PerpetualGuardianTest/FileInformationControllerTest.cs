using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PerpetualGuardian;
using PerpetualGuardian.Dto;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace PerpetualGuardianTest
{
    public class FileInformationControllerTest : BaseTest
    {
        private readonly HttpClient httpClient = null;
        public FileInformationControllerTest(ProgramInitFixture programInitFixture) : base(programInitFixture)
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<Startup>());
            httpClient = server.CreateClient();
        }

        [Fact]
        public async void ShouldUploadFiles()
        {
            string fileName1 = "IMGP0055.JPG";
            string fileName2 = "IMGP0056.JPG";
            string fileName3 = "IMGP0057.JPG";
            string filePath1 = Path.Combine("testFile", fileName1);
            string filePath2 = Path.Combine("testFile", fileName2);
            string filePath3 = Path.Combine("testFile", fileName3);

            var form = new MultipartFormDataContent();
            if (File.Exists(filePath1))
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath1));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "files", fileName1);  
            }
            if (File.Exists(filePath2))
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath2));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "files", fileName2);
            }
            if (File.Exists(filePath3))
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath3));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "files", fileName3);
            }

            var response = await httpClient.PostAsync($"/FileInformation", form);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void ShouldGetAllFiles()
        {
            string fileName1 = "IMGP0055.JPG";
            string fileName2 = "IMGP0056.JPG";
            string fileName3 = "IMGP0057.JPG";
            string filePath1 = Path.Combine("testFile", fileName1);
            string filePath2 = Path.Combine("testFile", fileName2);
            string filePath3 = Path.Combine("testFile", fileName3);

            var form = new MultipartFormDataContent();
            if (File.Exists(filePath1))
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath1));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "files", fileName1);
            }
            if (File.Exists(filePath2))
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath2));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "files", fileName2);
            }
            if (File.Exists(filePath3))
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath3));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "files", fileName3);
            }

            await httpClient.PostAsync("/FileInformation", form);

            HttpResponseMessage response = await httpClient.GetAsync("/FileInformation/All");

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            List<FileInformationDto> files = JsonConvert.DeserializeObject<List<FileInformationDto>>(json);

            Assert.True(files.Where(x => x.FileName == fileName1).Any());
            Assert.True(files.Where(x => x.FileName == fileName2).Any());
            Assert.True(files.Where(x => x.FileName == fileName3).Any());
        }

        [Fact]
        public async void ShouldDownloadFile()
        {
            string fileName = "IMGP0055.JPG";
            
            string filePath = Path.Combine("testFile", fileName);

            byte[] fileData = null;

            var form = new MultipartFormDataContent();
            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                var fileContent = new ByteArrayContent(fileData);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "files", fileName);
            }

            await httpClient.PostAsync("/FileInformation", form);

            HttpResponseMessage response = await httpClient.GetAsync("/FileInformation/All");

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            List<FileInformationDto> files = JsonConvert.DeserializeObject<List<FileInformationDto>>(json);

            FileInformationDto fileInformationDto = files.Where(x => x.FileName == fileName).FirstOrDefault();

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"/FileInformation/{fileInformationDto.Id}");

            httpResponseMessage.EnsureSuccessStatusCode();

            byte[] responseData = await httpResponseMessage.Content.ReadAsByteArrayAsync();

            Assert.Equal(fileData, responseData);
        }
    }
}
