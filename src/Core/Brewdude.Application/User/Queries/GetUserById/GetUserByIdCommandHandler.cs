using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Application.Security;
using Brewdude.Application.User.Models;
using Brewdude.Persistence;
using MediatR;

namespace Brewdude.Application.User.Queries.GetUserById
{
    public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand, UserViewModel>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public GetUserByIdCommandHandler(BrewdudeDbContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<UserViewModel> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            
            if (user == null)
                throw new UserNotFoundException($"User [{request.UserId}] does not exist");
            
            // Generate a token for immediate use
//            var token = _tokenService.CreateToken(user);
//            if (string.IsNullOrWhiteSpace(token))
//                throw new UserCreationException("Token generation failed during user retrieval");
            
            var userViewModel = _mapper.Map<UserViewModel>(user);
//            userViewModel.Token = token;

            return userViewModel;
        }
    }
}