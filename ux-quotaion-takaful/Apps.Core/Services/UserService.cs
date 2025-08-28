using Apps.Core.Interfaces;
using AutoMapper;
using DataAccess.EFCore.Interfaces;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EFCore.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Apps.Core.DTOS;
using Apps.Core.Models.Users;
using Apps.Core.Models.RoleAccess;
using System.Net.Http.Headers;
using Apps.Core.Models;
using Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Core.Services
{
    public class UserService : EFRepository, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        public UserService(ApplicationContext context, IRepository repository, IMapper mapper) : base(context)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public UserService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }

        public async Task<User> GetUserById(string userid)
        {
            var data = await context.Users.FindAsync(userid);
            return data;
        }
                
	}
}
