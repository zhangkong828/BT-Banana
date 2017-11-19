using Banana.Common;
using Newtonsoft.Json;
using System;

namespace Banana.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = @"{
          'name': '终结者1-5',
          'type': 'mp4',
          'files': [
            {
              'size': 2639942647,
              'name': 'Z结者5：C世纪.韩版.Terminator.Genisys.2015.HD720P.X264.AAC.English&Mandarin.CHS-ENG.Mp4Ba.mp4'
            },
            {
              'size': 313282,
              'name': '_____padding_file_3_如果您看到此文件，请升级到BitComet(比特彗星)0.85或以上版本____'
            },
            {
              'size': 1775974462,
              'name': 'T4.rmvb'
            },
            {
              'size': 1848413,
              'name': '_____padding_file_2_如果您看到此文件，请升级到BitComet(比特彗星)0.85或以上版本____'
            },
            {
              'size': 1229179811,
              'name': 'T3.rmvb'
            },
            {
              'size': 614905,
              'name': '_____padding_file_1_如果您看到此文件，请升级到BitComet(比特彗星)0.85或以上版本____'
            },
            {
              'size': 2159451655,
              'name': 'T2.rmvb'
            },
            {
              'size': 986524,
              'name': '_____padding_file_0_如果您看到此文件，请升级到BitComet(比特彗星)0.85或以上版本____'
            },
            {
              'size': 1209070180,
              'name': 'T1.rmvb'
            }
          ],
          'size': 9017381879,
          'infohash': 'ea10ca7c876b5b53d2b509a23f7a1e2dfb280467',
          'createtime': '2017-10-7 21:02:45',
          'tag': [
            '终结',
            '者',
            '1',
            '5'
          ]
        }";

            var a = JsonConvert.DeserializeObject<MagnetUrl>(json);

            Console.ReadKey();
        }
    }
}
