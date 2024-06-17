using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Configs;
using Application.Utils.Service;
using GraphQL.AspNet.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.api.Controllers.v1.GQL
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [GraphRoute("clients")]
    public class VenderController : BaseGraphController
    {
        public VenderController(IMediator _mediator, BackendApplicationConfig _configuration) : base(_mediator, _configuration)
        {
        }

        
    }
}