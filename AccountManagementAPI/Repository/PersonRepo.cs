using Microsoft.AspNetCore.Mvc;
using AccountManagementAPI.Model;

namespace AccountManagementAPI.Repository
{
    public class PersonRepo
    {
        private readonly AccountDBContext _dbContext;
        public PersonRepo(AccountDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public List<Person> GetPeople()
        {
            return _dbContext.People.ToList();
        }

        [HttpGet("{id}")]
        public Person? GetPersonById(int id)
        {
            return _dbContext.People.Where(person => person.PersonId == id).FirstOrDefault();
        }

    }
}
