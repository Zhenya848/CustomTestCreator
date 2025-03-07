using CSharpFunctionalExtensions;
using CustomTestCreator.SharedKernel;

namespace PetProject.Core.ValueObjects
{
    public record FilePath
    {
        public string FullPath { get; }

        private FilePath(string path) =>
            FullPath = path;

        public static Result<FilePath, Error> Create(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var validExtensions = new List<string>() { ".png", ".jpg", ".jpeg", ".ico" };

            if (validExtensions.Any(e => e == extension) == false)
                return Errors.General.ValueIsInvalid("File extension");

            return new FilePath(Guid.NewGuid() + extension);
        }
    }
}
