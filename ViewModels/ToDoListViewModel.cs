using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1WebCore21Test2.ViewModels
{
    [Serializable]
    public class ToDoListViewModel
    {
        //[Key]
        public int Id { get; set; }

        //[MaxLength(10)]
        //[Required]
        public string Name { get; set; }

        //[MaxLength(50)]
        //[Required]
        public string ToDo { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
