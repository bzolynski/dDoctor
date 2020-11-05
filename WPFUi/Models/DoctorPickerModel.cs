namespace WPFUi.Models
{
    public class DoctorPickerModel
    {
        public int Id { get; set; }
        public string FullName => $"{ LastName } { FirstName }";
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NPWZ { get; set; }
    }
}
