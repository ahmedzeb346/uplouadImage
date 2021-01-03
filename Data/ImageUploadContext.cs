using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImageUpload.Models;

namespace ImageUpload.Data
{
    public class ImageUploadContext : DbContext
    {
        public ImageUploadContext (DbContextOptions<ImageUploadContext> options)
            : base(options)
        {
        }

        public DbSet<ImageUpload.Models.ImageModel> ImageModel { get; set; }
    }
}
