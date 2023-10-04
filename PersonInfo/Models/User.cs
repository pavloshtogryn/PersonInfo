
namespace PersonInfo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age 
        {
            get
            {
                int age = DateTime.Now.Year - DateOfBirth.Year;

                if (DateTime.Now.DayOfYear > DateOfBirth.DayOfYear)
                {
                    return age;
                }
                else
                {
                    return --age;
                }
            }
        }
    }
}
