using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace ServerTCP.S3
{
    internal class ObjectS3
    {
        private string _accessKey = "DOQ61L40I9903B2VJDNO";
        private string _secretKey = "jeNggPSKpz1DlONP8ni31ASWEiaTomBhl5mR0FfC";

        public ObjectS3()
        {
            AmazonS3Config configsS3 = new AmazonS3Config
            {
                ServiceURL = @"https://s3.timeweb.cloud"
            };

            AmazonS3Client client = new AmazonS3Client(
                _accessKey,
                _secretKey,
                configsS3
            );


        }

    }
}
