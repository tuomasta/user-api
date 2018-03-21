using System.Web.Http;
using UserApi.Repos;
using System.Threading.Tasks;
using UserApi.Messaging;
using System;

namespace UserApi.Controllers
{
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        IUserRepo _userRepo;
        IMessageBus _messageBus;

        public UsersController(IUserRepo userRepo, IMessageBus messageBus)
        {
            _userRepo = userRepo;
            _messageBus = messageBus;
        }

        // GET api/values
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await _userRepo.GetUsersAsync().ConfigureAwait(false));
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAsync(string email)
        {
            var user = await _userRepo.GetUserAsync(u => u.Email.Equals(email)).ConfigureAwait(false);
            return user != null ? (IHttpActionResult)Ok(user) : NotFound();
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            var user = await _userRepo.GetUserAsync(u => u.Id.Equals(id)).ConfigureAwait(false);
            return user != null ? (IHttpActionResult) Ok(user) : NotFound();
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody]User user)
        {
            // TODO better validation
            if (string.IsNullOrWhiteSpace(user?.Email) || string.IsNullOrWhiteSpace(user?.Password)) {
                return BadRequest("Required user data is missing!");
            }

            var document = await _userRepo.CreateUserAsync(user).ConfigureAwait(false);

            await _messageBus.SendEventAsync(new UserCreatedEvent()
            {
                UserId = document.Id,
                Email = document.Email,
                Password = document.Password,
            }).ConfigureAwait(false);

            return Created($"api/v1/users/{document.Id}", document);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException("TODO: implement!");
        }
    }
}
