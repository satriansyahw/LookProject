using System;
using System.Collections.Generic;
using System.Text;

namespace GenHelper
{
    public class Settings
    {
        public string CloudAccountName = @"ddigudang";
        public string CloudAccountKey = @"+sXdBMuZS3hhWGezMNzmjq+48z5R4tIt73nquF0/QSegcPWYjHu5jqgjBnq2NkGUVw0vv/XP353lxdvhnG1oHQ==";
        public string CloudContainerName = @"starter";
        public string NetworkSharing = @"\\PEB-PC09RRX7\TestSharing";
        public int UploadType = 1;// 0=cloud,1= network share,2=ftp
        public string FTPUserName = "pebpapua";
        public string FTPPassword = "p4pu4t34m";
        public string FTPAddress = "ftp://172.16.104.5/";

    }
}
