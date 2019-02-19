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
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        [Required]
        [Display(Name = "使用者名稱：")]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "代辦事項（ToDo）：")]
        public string ToDo { get; set; }

        [Display(Name = "開始日期：")]
        public string StartDate { get; set; }

        [Display(Name = "結束日期：")]
        public string EndDate { get; set; }
    }
}
