namespace PerpetualGuardian.Dto
{
    /// <summary>
    /// File information data transfer object
    /// </summary>
    public class FileInformationDto
    {
        public int Id { set; get; }

        public string FileName { set; get; }

        public string UploadDateTime { set; get; }

        public byte[] FileData { set; get; }

    }
}
