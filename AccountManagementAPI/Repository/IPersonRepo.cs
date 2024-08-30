using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountManagementAPI.Model;
namespace AccountManagementAPI.Repository
{
    public interface IPersonRepo
    {

        public List<Person> GetPeople();
        public Person? GetPersonById(int id);
        
    }
}
