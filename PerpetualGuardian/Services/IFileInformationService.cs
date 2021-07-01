using PerpetualGuardian.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PerpetualGuardian.Services
{
    public interface IFileInformationService
    {
        /// <summary>
        /// Add files
        /// </summary>
        /// <param name="fileInformationDtoes">A data transfer object list</param>
        /// <returns>A file information transfer object</returns>
        Task<bool> AddFileInformationListAsync(List<FileInformationDto> fileInformationDtoes);

        /// <summary>
        /// Add a file
        /// </summary>
        /// <param name="fileInformationDto">A data transfer object</param>
        /// <returns>A file information transfer object</returns>
        Task<FileInformationDto> AddFileInformationAsync(FileInformationDto fileInformationDto);

        /// <summary>
        /// Get a file by it's id
        /// </summary>
        /// <param name="id">File id</param>
        /// <returns>A file information transfer object</returns>
        Task<FileInformationDto> GetFileAsync(int id);

        /// <summary>
        /// Get all files
        /// </summary>
        /// <returns>A list whose each elemment is a file transfer information </returns>
        Task<List<FileInformationDto>> GetAllFilesAsync();

    }
}
