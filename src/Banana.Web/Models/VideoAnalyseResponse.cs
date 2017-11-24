﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Models
{
    public class VideoAnalyseResponse
    {
        public List<VideoInfo> Data { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public string Version { get; set; }


    }

    public class VideoInfo
    {
        public VideoInfo()
        {
            Part = new List<PartInfo>();
        }


        public string Name { get; set; }

        public List<PartInfo> Part { get; set; }

        public int PartCount { get; set; }

    }

    public class PartInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Remark { get; set; }
    }
}
