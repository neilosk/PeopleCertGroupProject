using FinalProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Dtos
{
    public class CertificateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public static CertificateDto FromEntity(Certificate certificate)
        {
            return new CertificateDto
            {
                Id = certificate.Id,
                Title = certificate.Title
            };
        }
    }
}
