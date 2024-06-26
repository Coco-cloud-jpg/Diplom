﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Screenshot : BaseEntity
    {
        public Screenshot()
        {
            Comments = new HashSet<Comment>();
        }
        public Guid RecorderId { get; set; }
        public DateTime DateCreated { get; set; }
        public string StorePath { get; set; }
        public AlertState Mark { get; set; }

        public RecorderRegistration Recorder { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

    public enum AlertState
    {
        None,
        InternalWarning,
        SubmittedViolation
    }
}
