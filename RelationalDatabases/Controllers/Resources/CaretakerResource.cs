using System.ComponentModel.DataAnnotations;

namespace RelationalDatabases.Controllers.Resources
{
    public class CaretakerResource
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string phone { get; set; }

        #region Relations
        [Required]
        public AddressResource address { get; set;}
        #endregion
    }
}