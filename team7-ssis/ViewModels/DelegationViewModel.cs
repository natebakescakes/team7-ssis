﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class DelegationViewModel
    {
        public string Recipient { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class DelegationMobileViewModel
    {
        public int DelegationId { get; set; }
        public string Recipient { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
    }

    public class DelegationSubmitViewModel
    {
        public string RecipientEmail { get; set; }
        public string HeadEmail { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class CancelDelegationViewModel
    {
        public int DelegationId { get; set; }
        public string HeadEmail { get; set; }
    }
}