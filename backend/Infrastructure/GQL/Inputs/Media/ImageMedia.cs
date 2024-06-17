using System.ComponentModel.DataAnnotations;
using Application.Attributes;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.GQL.Inputs.Media;

public class ImageMedia
{
    [Required(ErrorMessage = "Please select a file.")]
    [DataType(DataType.Upload)]
    [MaxFileSize(5 * 1024 * 1024)]
    [AllowedExtensions(new string[] { ".jpg", ".png", ".gif", ".jpge" })]
    public IFormFile Media { get; set; }
}
