using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessConvert.Models
{
    [Table("Videos")]
    public class Video
    {
        [ExplicitKey]
        public int Id { get; set; }
        public int ProcessingStatus { get; set; }
        public string EmbedUrl { get; set; }
        public string VideoTime { get; set;}
        public string AudioFile { get; set; }
        public string VideoCodec { get; set; }
    }
}
