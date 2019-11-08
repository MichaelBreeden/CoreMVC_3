using CoreMVC_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// A View model may not have all the information needed, so you use a ViewModel,
// basically a composite model.Also known as Data Transfer Objects (DTO).

namespace CoreMVC_3.ViewModels
{
    public class HomeDetailsViewModel
    {
        public Employee Employee { get; set; }
        public string PageTitle { get; set; }
    }
}
