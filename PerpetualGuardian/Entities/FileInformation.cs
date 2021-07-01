using PerpetualGuardian.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace PerpetualGuardian.Entities
{
    /// <summary>
    /// File information model
    /// </summary>
    public class FileInformation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}", MinimumLength = 1)]
        public string FileName { set; get; }

        public DateTime UploadDataTime { set; get; }

        /// <summary>
        /// Convert to data transfer object
        /// </summary>
        /// <returns>a data transfer object</returns>
        public FileInformationDto ToFileInformationDto()
        {
            return new FileInformationDto()
            {
                Id = this.Id,
                FileName = this.FileName,
                UploadDateTime = this.UploadDataTime.ToString("MM/dd/yyyy h:mm")
            };
        }

        /// <summary>
        /// Generate file upload path
        /// </summary>
        /// <param name="rootPath">root path</param>
        /// <returns>the file upload path</returns>
        public string GetUploadFilePath(string rootPath)
        {
            return Path.Combine(rootPath, $"{this.Id}-{this.UploadDataTime.ToString("yyyyMMddHHmmssfff")}");
        }
    }
}
